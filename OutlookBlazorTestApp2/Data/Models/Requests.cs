using OutlookBlazorTestApp2.Data.Models.Supporting;
using System.ComponentModel.DataAnnotations;

namespace OutlookBlazorTestApp2.Data.Models
{
    public class CreateContactRequest
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = "";

        [EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string Phone { get; set; } = "";

        public string Company { get; set; } = "";

        public string Title { get; set; } = "";

        public Address PrimaryAddress { get; set; } = new();
    }

    public class CreateLeadRequest
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = "";

        [EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string Phone { get; set; } = "";

        public string Company { get; set; } = "";

        public string Title { get; set; } = "";

        public decimal EstimatedValue { get; set; }

        public string Currency { get; set; } = "";

        public DateTime ExpectedCloseDate { get; set; }

        public LeadSource Source { get; set; }

        public string Description { get; set; } = "";
    }

    public class CreateOpportunityRequest
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = "";

        public decimal Value { get; set; }

        public string Currency { get; set; } = "";

        public DateTime CloseDate { get; set; }

        public OpportunityStage Stage { get; set; }

        public string ContactId { get; set; } = "";
    }

    public class CreateActivityRequest
    {
        public ActivityType Type { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public PriorityLevel Priority { get; set; }

        public List<EntityReference> RelatedEntities { get; set; } = new();

        public string Location { get; set; } = "";
    }

    public class SearchRequest
    {
        public string Query { get; set; } = "";
        public List<EntityType> EntityTypes { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class AutoLinkRequest
    {
        public string Sender { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
    }

    public class ConvertLeadRequest
    {
        public string ContactId { get; set; } = "";
    }

    public class UpdateContactRequest
    {
        [StringLength(100)]
        public string FirstName { get; set; } = "";

        [StringLength(100)]
        public string LastName { get; set; } = "";

        [EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string Phone { get; set; } = "";

        public string Company { get; set; } = "";

        public string Title { get; set; } = "";

        public Address PrimaryAddress { get; set; } = new();
    }

    public class UpdateLeadRequest
    {
        [StringLength(100)]
        public string FirstName { get; set; } = "";

        [StringLength(100)]
        public string LastName { get; set; } = "";

        [EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string Phone { get; set; } = "";
    }

    public class UpdateOpportunityRequest
    {
        [StringLength(200)]
        public string Name { get; set; } = "";

        public decimal Value { get; set; }

        public string Currency { get; set; } = "USD";

        public DateTime CloseDate { get; set; }

        public OpportunityStage Stage { get; set; }

        public string ContactId { get; set; } = "";
    }
}
