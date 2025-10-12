using OutlookBlazorTestApp2.Data.Models.Supporting;
using System;
using System.ComponentModel.DataAnnotations;

namespace OutlookBlazorTestApp2.Data.Models
{
    public class Lead
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); // ✅ consistent with Contact

        [Required, StringLength(100)]
        public string? FirstName { get; set; }

        [Required, StringLength(100)]
        public string? LastName { get; set; }
       
        [Required,StringLength(100)]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Company { get; set; }

        [StringLength(200)]
        public string? ContactName { get; set; }         // ✅ Used in your code
        public string? Title { get; set; }

        // ✅ Additional fields from SageX3Service
        public decimal? EstimatedValue { get; set; }
        public string? Currency { get; set; }
        public DateTime? ExpectedCloseDate { get; set; }
        public string? Source { get; set; }
        public string? Description { get; set; }

        // ✅ Tracking fields
        [StringLength(200)]
        public string? CompanyName { get; set; }
        
        public string? ExternalId { get; set; } // For Sage X3 reference
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        // ✅ Convenience property
        public string FullName => $"{FirstName} {LastName}";
    }
}

