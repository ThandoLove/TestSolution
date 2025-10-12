using OutlookBlazorTestApp2.Data.Models.Supporting;
using System.ComponentModel.DataAnnotations;

    namespace OutlookBlazorTestApp2.Data.Models
    {
        public class Contact
        {
            [Key]
            public Guid Id { get; set; }   // GUID instead of string for consistency

            [Required, StringLength(100)]
            public string FirstName { get; set; } = "";

            [Required, StringLength(100)]
            public string LastName { get; set; } = "";
            public string FullName => $"{FirstName} {LastName}";


            [Required, EmailAddress]
            public string Email { get; set; } = "";

            [StringLength(50)]
            public string Phone { get; set; } = "";

            [StringLength(200)]
            public string Company { get; set; } = "";

            [StringLength(100)]
            public string Title { get; set; } = "";   // ✅ Added

            public Address? PrimaryAddress { get; set; }   // ✅ Added
            public string? ExternalId { get; set; } 
            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        // ✅ Added
            public DateTime? ModifiedDate { get; set; }  // ✅ Added

            public EntityStatus Status { get; set; } = EntityStatus.Active;  // ✅ Added
        }

       
    }