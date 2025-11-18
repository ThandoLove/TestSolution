using Microsoft.Extensions.Logging;
using OutlookBlazorTestApp2.Data.Models;
using OutlookBlazorTestApp2.services;

namespace OutlookBlazorTestApp2.Services
{
    // Interface for DI
    public interface IAutoLinkService
    {
        Task<List<MatchResult>> FindMatchesAsync(EmailContext email);
        Task<LinkResult> LinkEmailAsync(LinkRequest request);
    }

    public class AutoLinkService : IAutoLinkService
    {
        private readonly CrmService _crm; // your existing CrmService
        private readonly ILogger<AutoLinkService> _log;

        // Inject the CrmService and logger
        public AutoLinkService(CrmService crm, ILogger<AutoLinkService> log)
        {
            _crm = crm;
            _log = log;
        }

        // 🔹 Step 1: Find potential matches for an email
        public async Task<List<MatchResult>> FindMatchesAsync(EmailContext email)
        {
            var results = new List<MatchResult>();
            if (email == null) return results;

            string fromEmail = email.Sender?.Address?.Trim().ToLowerInvariant() ?? "";

            // 1️⃣ Try exact email match using CrmService
            var exact = await _crm.FindRecordByEmailAsync(fromEmail);
            if (exact.Success && exact.Data != null)
            {
                results.Add(new MatchResult
                {
                    Id = exact.Data.Id.ToString(),
                    Name = exact.Data.Name,
                    EntityType = exact.Data.Type,
                    PrimaryEmail = fromEmail,
                    Score = 1.0 // exact match
                });
            }

            // 2️⃣ Fuzzy match using content/subject/body
            string content = $"{email.Subject} {email.Body} {email.Sender?.Name}";
            var fuzzy = await _crm.FindRecordByContentAsync(content);
            if (fuzzy.Success && fuzzy.Data != null)
            {
                results.Add(new MatchResult
                {
                    Id = fuzzy.Data.Id.ToString(),
                    Name = fuzzy.Data.Name,
                    EntityType = fuzzy.Data.Type,
                    PrimaryEmail = fromEmail,
                    Score = 0.5 // partial/fuzzy match
                });
            }

            // Deduplicate by ID and sort by Score descending
            return results.GroupBy(r => r.Id)
                          .Select(g => g.OrderByDescending(x => x.Score).First())
                          .OrderByDescending(x => x.Score)
                          .ToList();
        }

        // 🔹 Step 2: Link email to a CRM record
        public async Task<LinkResult> LinkEmailAsync(LinkRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.TargetRecordId))
            {
                return new LinkResult
                {
                    Success = false,
                    Message = "Missing target record ID"
                };
            }

            try
            {
                // Use your CrmService to create an activity for this email
                await _crm.LogActivityAsync(new CreateActivityRequest
                {
                    Subject = request.EmailContext.Subject,
                    Description = request.EmailContext.Body,
                    Type = "Email",
                    StartDate = request.EmailContext.ReceivedDate,
                    EndDate = request.EmailContext.ReceivedDate,
                    RelatedEntities = new List<Guid> { Guid.Parse(request.TargetRecordId) }
                });

                return new LinkResult
                {
                    Success = true,
                    Message = "Email successfully linked"
                };
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error linking email to CRM record");
                return new LinkResult
                {
                    Success = false,
                    Message = $"Failed to link email: {ex.Message}"
                };
            }
        }
    }
}
