namespace OutlookBlazorTestApp2.Data.Models
{
    public abstract class CrmEntity
    {
        public int Id { get; set; }
        public string ExternalId { get; set; } = "";
        public DateTime CreatedOn { get; set; }
    }

    
}
