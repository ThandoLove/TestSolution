// ✅ File: Services/EmailAttachmentService.cs
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;

namespace OutlookBlazorTestApp2.Services
{
    // This service sends attachments from Blazor to your AttachmentController
    public class EmailAttachmentService
    {
        private readonly HttpClient _http;

        public EmailAttachmentService(HttpClient http)
        {
            _http = http;
        }

        // Upload attachment to AttachmentController, which forwards to Sage
        public async Task<(bool Success, string SageAttachmentId, string Raw)> UploadAttachmentAsync(
            Guid activityId,
            string filename,
            string base64
        )
        {
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(activityId.ToString()), "activityId");
            form.Add(new StringContent(filename), "fileName");
            form.Add(new StringContent(base64), "fileContentBase64");

            var resp = await _http.PostAsync("/api/attachments/upload", form);
            var txt = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode) return (false, "", txt);

            try
            {
                var doc = JsonDocument.Parse(txt);
                var sageId = doc.RootElement.GetProperty("sageAttachmentId").GetString() ?? "";
                return (true, sageId, txt);
            }
            catch
            {
                return (true, "", txt);
            }
        }
    }
}
