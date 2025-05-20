using AIIntegratedCRM.Models.Entities;
using System.Threading.Tasks;

namespace AIIntegratedCRM.Services.Interfaces
{
    /// <summary>
    /// Defines methods for interacting with the Gemini API.
    /// </summary>
    public interface IAIService
    {
        /// <summary>
        /// Generates a brief summary for a given customer using Gemini.
        /// </summary>
        /// <param name="customer">The Customer entity.</param>
        /// <returns>A string containing Gemini’s generated text.</returns>
        Task<string> GenerateCustomerSummaryAsync(Customer customer);
    }
}
