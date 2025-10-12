using OutlookBlazorTestApp2.Data.Models.Supporting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutlookBlazorTestApp2.Data.Models
{
    public class Opportunity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(150)]
        public string? Title { get; set; }

        public decimal Value { get; set; }
        public string? Currency { get; set; }
        public DateTime? CloseDate { get; set; }
        public string? Stage { get; set; }

        // ✅ Foreign key to Contact
        public Guid? ContactId { get; set; }
        [ForeignKey(nameof(ContactId))]
        public Contact? Contact { get; set; }

        // ✅ Tracking fields
        public string? ExternalId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
    }
}

