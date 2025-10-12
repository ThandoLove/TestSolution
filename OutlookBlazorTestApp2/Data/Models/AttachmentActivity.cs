
namespace OutlookBlazorTestApp2.Data.Models
{
    public class AttachmentActivity
    {
        public int Id { get; set; }
        public string FileName { get; set; } = "";
        public long FileSize { get; set; }
        public int ActivityId { get; set; }
        public Activity? Activity { get; set; }
    }
}

