using AIIntegratedCRM.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIIntegratedCRM.Infrastructure.Services;

public class AIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AIService> _logger;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.anthropic.com/v1/messages";
    private const string Model = "claude-sonnet-4-6";

    public AIService(HttpClient httpClient, IConfiguration configuration, ILogger<AIService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = configuration["Anthropic:ApiKey"] ?? throw new InvalidOperationException("Anthropic:ApiKey not configured");
    }

    private async Task<string> CallClaudeAsync(string prompt, int maxTokens = 1024, CancellationToken cancellationToken = default)
    {
        var requestBody = new
        {
            model = Model,
            max_tokens = maxTokens,
            messages = new[] { new { role = "user", content = prompt } }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.PostAsync(BaseUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(responseJson);
        return doc.RootElement.GetProperty("content")[0].GetProperty("text").GetString() ?? string.Empty;
    }

    public async Task<LeadScoreResult> ScoreLeadAsync(LeadScoreRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = $$"""
                You are a CRM lead scoring expert. Score the following lead from 0-100 based on their likelihood to convert.

                Lead Information:
                - Email: {{request.Email}}
                - Company: {{request.Company ?? "Unknown"}}
                - Job Title: {{request.JobTitle ?? "Unknown"}}
                - Source: {{request.Source ?? "Unknown"}}
                - Country: {{request.Country ?? "Unknown"}}
                - Employee Count: {{request.EmployeeCount?.ToString() ?? "Unknown"}}
                - Annual Revenue: {{request.AnnualRevenue?.ToString() ?? "Unknown"}}
                - Notes: {{request.Notes ?? "None"}}

                Respond with ONLY a JSON object in this exact format (no markdown):
                {"score": 75, "reason": "Brief explanation", "keyFactors": ["factor1", "factor2", "factor3"]}
                """;

            var result = await CallClaudeAsync(prompt, 512, cancellationToken);
            var cleaned = result.Trim().TrimStart('`').TrimEnd('`');
            if (cleaned.StartsWith("json")) cleaned = cleaned[4..].Trim();

            using var doc = JsonDocument.Parse(cleaned);
            var score = doc.RootElement.GetProperty("score").GetDecimal();
            var reason = doc.RootElement.GetProperty("reason").GetString() ?? "AI score generated";
            var factors = doc.RootElement.GetProperty("keyFactors").EnumerateArray()
                .Select(f => f.GetString() ?? "").Where(f => !string.IsNullOrEmpty(f)).ToArray();

            return new LeadScoreResult(score, reason, factors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scoring lead");
            return new LeadScoreResult(50, "AI scoring temporarily unavailable", Array.Empty<string>());
        }
    }

    public async Task<OpportunityPredictionResult> PredictOpportunityWinAsync(OpportunityPredictionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = $$"""
                You are a sales analytics expert. Predict the win probability for this opportunity.

                Opportunity:
                - Name: {{request.OpportunityName}}
                - Amount: {{request.Amount:C}}
                - Stage: {{request.Stage}}
                - Days in Current Stage: {{request.DaysInStage}}
                - Total Activities: {{request.TotalActivities ?? 0}}
                - Competitor Info: {{request.CompetitorInfo ?? "None"}}
                - Account Industry: {{request.AccountIndustry ?? "Unknown"}}
                - Account Revenue: {{request.AccountRevenue?.ToString("C") ?? "Unknown"}}

                Respond with ONLY a JSON object (no markdown):
                {"winProbability": 65.5, "reason": "explanation", "recommendations": ["action1", "action2"]}
                """;

            var result = await CallClaudeAsync(prompt, 512, cancellationToken);
            var cleaned = result.Trim().TrimStart('`').TrimEnd('`');
            if (cleaned.StartsWith("json")) cleaned = cleaned[4..].Trim();

            using var doc = JsonDocument.Parse(cleaned);
            var prob = doc.RootElement.GetProperty("winProbability").GetDecimal();
            var reason = doc.RootElement.GetProperty("reason").GetString() ?? "AI prediction generated";
            var recs = doc.RootElement.GetProperty("recommendations").EnumerateArray()
                .Select(r => r.GetString() ?? "").Where(r => !string.IsNullOrEmpty(r)).ToArray();

            return new OpportunityPredictionResult(prob, reason, recs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting opportunity win");
            return new OpportunityPredictionResult(50, "AI prediction temporarily unavailable", Array.Empty<string>());
        }
    }

    public async Task<string> GenerateEmailAsync(EmailGenerationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = $"""
                Generate a {request.Tone} email for the following context:

                Purpose: {request.Purpose}
                Recipient: {request.RecipientName} at {request.RecipientCompany}
                Sender: {request.SenderName}
                Additional Context: {request.Context ?? "None"}

                Write a complete, professional email with subject line and body. Format as:
                Subject: [subject here]

                [email body here]
                """;

            return await CallClaudeAsync(prompt, 1024, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating email");
            return "Error generating email. Please try again.";
        }
    }

    public async Task<string> SummarizeMeetingAsync(string transcript, CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = $"""
                Summarize the following meeting transcript into key points, action items, and decisions made.

                Transcript:
                {transcript}

                Provide a structured summary with:
                1. Key Discussion Points
                2. Decisions Made
                3. Action Items (with owners if mentioned)
                4. Next Steps
                """;

            return await CallClaudeAsync(prompt, 1024, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error summarizing meeting");
            return "Error summarizing meeting. Please try again.";
        }
    }

    public async Task<string> AnswerNaturalLanguageQueryAsync(string query, string tenantContext, CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = $"""
                You are an AI assistant for a CRM system. Answer the following question based on the CRM data context provided.

                CRM Data Context:
                {tenantContext}

                User Question: {query}

                Provide a helpful, concise answer. If you can calculate metrics or provide insights, do so.
                """;

            return await CallClaudeAsync(prompt, 1024, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error answering NL query");
            return "I couldn't process your query at this time. Please try again.";
        }
    }

    public Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        // Semantic search embeddings would use a dedicated embedding model
        // For production, integrate with OpenAI text-embedding-ada-002 or similar
        var rng = new Random(text.GetHashCode());
        var embedding = Enumerable.Range(0, 1536).Select(_ => (float)(rng.NextDouble() * 2 - 1)).ToArray();
        return Task.FromResult(embedding);
    }
}
