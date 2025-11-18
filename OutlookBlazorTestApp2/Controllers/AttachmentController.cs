// ✅ File location: Controllers/AttachmentController.cs
// This controller handles attachment uploads from the Outlook Add-In
// It saves files locally (optional) and forwards them to Sage X3 via your SageX3Service.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using OutlookBlazorTestApp2.Services;  // SageX3Service namespace
using System;
using System.IO;
using System.Threading.Tasks;

namespace OutlookBlazorTestApp2.Controllers
{
    // ✅ API controller attribute
    [ApiController]

    // ✅ Base route for this controller
    [Route("api/[controller]")]
    public class AttachmentController : ControllerBase
    {
        // ✅ Inject environment to get app root paths
        private readonly IWebHostEnvironment _env;

        // ✅ Inject your SageX3 service to upload attachments
        private readonly SageX3Service _sage;

        // ✅ Optional: logging
        private readonly ILogger<AttachmentController> _log;

        // ✅ Constructor with DI
        public AttachmentController(IWebHostEnvironment env, SageX3Service sage, ILogger<AttachmentController> log)
        {
            _env = env;
            _sage = sage;
            _log = log;
        }

        // ✅ POST endpoint to upload attachment
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadAttachmentRequest req)
        {
            // ✅ Validate input
            if (req == null || req.activityId == Guid.Empty || string.IsNullOrWhiteSpace(req.fileContentBase64))
                return BadRequest("Missing parameters");

            try
            {
                // ✅ Convert Base64 string to bytes
                var bytes = Convert.FromBase64String(req.fileContentBase64);

                // ✅ Optional: save file locally
                var folder = Path.Combine(_env.ContentRootPath, "UploadedFiles", req.activityId.ToString());
                Directory.CreateDirectory(folder);
                var path = Path.Combine(folder, req.fileName);
                await System.IO.File.WriteAllBytesAsync(path, bytes);

                // ✅ Forward file to Sage X3 via your service
                var sageResp = await _sage.UploadAttachmentAsync(req.activityId.ToString(), req.fileName, bytes);
                if (!sageResp.Success)
                {
                    _log.LogError("Sage upload failed: {Raw}", sageResp.Raw);
                    return StatusCode(502, new
                    {
                        success = false,
                        message = "Failed to upload to Sage",
                        detail = sageResp.Raw
                    });
                }

                // ✅ Return successful result
                return Ok(new
                {
                    success = true,
                    sageAttachmentId = sageResp.AttachmentId,
                    message = "Attachment uploaded successfully"
                });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Attachment upload exception");
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }

    // ✅ Model for incoming attachment requests
    // Can be moved to Models/UploadAttachmentRequest.cs if you prefer
    public class UploadAttachmentRequest
    {
        public Guid activityId { get; set; }         // ID of YActivity / CRM record
        public string fileName { get; set; } = "";   // Original file name
        public string fileContentBase64 { get; set; } = "";  // File content in Base64
    }
}
