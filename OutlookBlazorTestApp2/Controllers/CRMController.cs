using Microsoft.AspNetCore.Mvc;
using OutlookBlazorTestApp2.Data.Models;
using OutlookBlazorTestApp2.services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OutlookBlazorTestApp2.Controllers;


namespace OutlookTestBlazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrmController : BaseController
    {
        private readonly ICrmService _crm;
        private readonly ILogger<CrmController> _logger;

        public CrmController(ICrmService crm, ILogger<CrmController> logger)
        {
            _crm = crm;
            _logger = logger;
        }

        [HttpGet("find-by-email")]
        public async Task<IActionResult> FindByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ApiError("Email cannot be null or empty"));
            }

            var result = await _crm.FindRecordByEmailAsync(email);
            return OkResult(result);
        }

        [HttpGet("find-by-content")]
        public async Task<IActionResult> FindByContent([FromQuery] string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return BadRequest(new ApiError("Content cannot be null or empty"));
            }

            var result = await _crm.FindRecordByContentAsync(content);
            return OkResult(result);
        }

        [HttpPost("create-contact")]
        public async Task<IActionResult> CreateContact([FromBody] CreateContactRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _crm.CreateContactAsync(request);
            return OkResult(result);
        }

        [HttpPost("create-lead")]
        public async Task<IActionResult> CreateLead([FromBody] CreateLeadRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _crm.CreateLeadAsync(request);
            return OkResult(result);
        }

        [HttpPost("log-activity")]
        public async Task<IActionResult> LogActivity([FromBody] CreateActivityRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _crm.LogActivityAsync(request);
            return OkResult(result);
        }

        [HttpGet("activity-timeline/{entityId}")]
        public async Task<IActionResult> GetActivityTimeline(string entityId)
        {
            if (string.IsNullOrWhiteSpace(entityId))
            {
                return BadRequest(new ApiError("Entity ID cannot be null or empty"));
            }

            var result = await _crm.GetActivityTimelineAsync(entityId);
            return OkResult(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _crm.SearchAsync(request);
            return OkResult(result);
        }

        [HttpGet("contact/{id}")]
        public async Task<IActionResult> GetContact(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new ApiError("Contact ID cannot be null or empty"));
            }

            var result = await _crm.GetContactAsync(id);
            return OkResult(result);
        }

        [HttpGet("lead/{id}")]
        public async Task<IActionResult> GetLead(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new ApiError("Lead ID cannot be null or empty"));
            }

            var result = await _crm.GetLeadAsync(id);
            return OkResult(result);
        }

        [HttpPut("contact/{id}")]
        public async Task<IActionResult> UpdateContact(string id, [FromBody] UpdateContactRequest request)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new ApiError("Contact ID cannot be null or empty"));
            }

            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _crm.UpdateContactAsync(id, request);
            return OkResult(result);
        }

        [HttpPut("lead/{id}")]
        public async Task<IActionResult> UpdateLead(string id, [FromBody] UpdateLeadRequest request)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new ApiError("Lead ID cannot be null or empty"));
            }

            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _crm.UpdateLeadAsync(id, request);
            return OkResult(result);
        }

        [HttpPost("convert-lead/{id}")]
        public async Task<IActionResult> ConvertLead(string id, [FromBody] ConvertLeadRequest request)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new ApiError("Lead ID cannot be null or empty"));
            }

            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _crm.ConvertLeadAsync(id, request);
            return OkResult(result);
        }
    }
}