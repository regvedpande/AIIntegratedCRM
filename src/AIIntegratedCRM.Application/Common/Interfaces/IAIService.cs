namespace AIIntegratedCRM.Application.Common.Interfaces;

public interface IAIService
{
    Task<LeadScoreResult> ScoreLeadAsync(LeadScoreRequest request, CancellationToken cancellationToken = default);
    Task<OpportunityPredictionResult> PredictOpportunityWinAsync(OpportunityPredictionRequest request, CancellationToken cancellationToken = default);
    Task<string> GenerateEmailAsync(EmailGenerationRequest request, CancellationToken cancellationToken = default);
    Task<string> SummarizeMeetingAsync(string transcript, CancellationToken cancellationToken = default);
    Task<string> AnswerNaturalLanguageQueryAsync(string query, string tenantContext, CancellationToken cancellationToken = default);
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
}

public record LeadScoreRequest(
    string Email,
    string? Company,
    string? JobTitle,
    string? Source,
    string? Country,
    int? EmployeeCount,
    int? AnnualRevenue,
    string? Notes);

public record LeadScoreResult(decimal Score, string Reason, string[] KeyFactors);

public record OpportunityPredictionRequest(
    string OpportunityName,
    decimal Amount,
    string Stage,
    int DaysInStage,
    int? TotalActivities,
    string? CompetitorInfo,
    string? AccountIndustry,
    decimal? AccountRevenue);

public record OpportunityPredictionResult(decimal WinProbability, string Reason, string[] Recommendations);

public record EmailGenerationRequest(
    string Tone,
    string Purpose,
    string RecipientName,
    string RecipientCompany,
    string? Context,
    string SenderName);
