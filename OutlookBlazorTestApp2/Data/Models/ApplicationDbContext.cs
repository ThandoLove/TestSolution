using Microsoft.EntityFrameworkCore;
using OutlookBlazorTestApp2.Data; // Add this using directive at the top of the fileusing Microsoft.EntityFrameworkCore;


namespace OutlookBlazorTestApp2.Data.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}

