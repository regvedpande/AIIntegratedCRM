using AIIntegratedCRM.Models.Entities;
using AIIntegratedCRM.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIIntegratedCRM.Services.Implementations
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpointUrl;

        public AIService(IConfiguration config)
        {
            // Use your provided API key here:
            _apiKey = "";

            // Gemini-2.0-flash endpoint from your curl command:
            _endpointUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

            _httpClient = new HttpClient();
        }

        public async Task<string> GenerateCustomerSummaryAsync(Customer customer)
        {
            // Build a prompt including the customer's key data:
            string promptText = @$"
Write a brief, friendly summary email introduction for this customer:
- Name: {customer.FullName}
- Email: {customer.Email}
- Company: {customer.Company}
- Phone: {customer.Phone}
- Notes: {customer.Notes}
End with a sentence inviting them to reply with any further details.";

            // Construct request payload per Gemini generateContent API
            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = promptText.Trim() }
                        }
                    }
                }
            };

            string jsonPayload = JsonSerializer.Serialize(payload);

            using var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(_endpointUrl, requestContent);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Parse the JSON response to extract the generated text.
                // Gemini’s response typically has: { "candidates": [ { "output": { "parts": [ { "text": "..." } ] } } ] }
                using var doc = JsonDocument.Parse(jsonResponse);
                var root = doc.RootElement;

                if (root.TryGetProperty("candidates", out JsonElement candidates) &&
                    candidates.GetArrayLength() > 0)
                {
                    var first = candidates[0];
                    if (first.TryGetProperty("output", out JsonElement outputElem) &&
                        outputElem.TryGetProperty("parts", out JsonElement partsElem) &&
                        partsElem.GetArrayLength() > 0)
                    {
                        var textElem = partsElem[0].GetProperty("text");
                        return textElem.GetString() ?? "(No text returned)";
                    }
                }

                // Fallback: return full JSON if parsing failed
                return jsonResponse;
            }
            catch (Exception ex)
            {
                // On error, return an error message
                return $"[AI Service Error: {ex.Message}]";
            }
        }
    }
}
