namespace OutlookBlazorTestApp2.Data.Models.Supporting

{
    public class YActivityModel
    {
        public string YSUBJECT { get; set; }
        public string YHTMLBODY { get; set; }
        public string YTEXTBODY { get; set; }
        public string BPCCT { get; set; }         // Contact code (BPCCT)
        public DateTime YSENTDATE { get; set; }
        public string YDIR { get; set; } = "IN";  // IN / OUT
        public string YOWNER { get; set; }        // owner / rep
        public string YMESSAGEID { get; set; }    // Outlook message id
    }

    public class SageCreateResponse
    {
        public bool Success { get; set; }
        public string ActivityId { get; set; } = "";
        public string Raw { get; set; } = "";
    }

    public class SageAttachmentResponse
    {
        public bool Success { get; set; }
        public string AttachmentId { get; set; } = "";
        public string Raw { get; set; } = "";
    }
}










