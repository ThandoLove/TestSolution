using Microsoft.AspNetCore.Mvc;
using OutlookBlazorTestApp2.Data.Models;
using OutlookBlazorTestApp2.Services;

namespace OutlookBlazorTestApp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutlookController : BaseController
    {
        private readonly IOutlookService _outlook;
        private readonly ILogger<OutlookController> _logger;

        public OutlookController(IOutlookService outlook, ILogger<OutlookController> logger)
        {
            _outlook = outlook;
            _logger = logger;
        }

        [HttpGet("context")]
        public async Task<IActionResult> GetEmailContext()
        {
            var result = await _outlook.GetEmailContextAsync();
            return OkResult(result);
        }

        [HttpGet("body")]
        public async Task<IActionResult> GetEmailBody([FromQuery] string format = "text")
        {
            var result = await _outlook.GetEmailBodyAsync(format);
            return OkResult(result);
        }

        [HttpGet("attachments")]
        public async Task<IActionResult> GetAttachments()
        {
            var result = await _outlook.GetAttachmentsAsync();
            return OkResult(result);
        }

        [HttpGet("selected-text")]
        public async Task<IActionResult> GetSelectedText()
        {
            var result = await _outlook.GetSelectedTextAsync();
            return OkResult(result);
        }

        [HttpGet("user-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await _outlook.GetUserProfileAsync();
            return OkResult(result);
        }

        [HttpPost("auto-link")]
        public async Task<IActionResult> AutoLink([FromBody] AutoLinkRequest request)
        {
            if (request == null || (string.IsNullOrWhiteSpace(request.Sender) && string.IsNullOrWhiteSpace(request.Content)))
            {
                return BadRequest(new ApiError("Sender or Content must be provided"));
            }

            // This would typically use the CRM service to find matching records
            // For now, we'll return a mock response
            var response = new AutoLinkResponse
            {
                MatchFound = !string.IsNullOrWhiteSpace(request.Sender),
                Message = !string.IsNullOrWhiteSpace(request.Sender) ?
                    "Found matching record" :
                    "No matching record found"
            };

            return Ok(new ApiResponse<AutoLinkResponse> { Data = response });
        }
    }
}