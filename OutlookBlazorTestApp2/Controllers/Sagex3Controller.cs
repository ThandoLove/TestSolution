using Microsoft.AspNetCore.Mvc;
using OutlookBlazorTestApp2.Data.Models;
using OutlookBlazorTestApp2.services;
using OutlookBlazorTestApp2.Controllers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace OutlookBlazorTestApp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SageX3Controller : BaseController
    {
        private readonly ISageX3Service _sage;
        private readonly ILogger<SageX3Controller> _logger;

        public SageX3Controller(ISageX3Service sage, ILogger<SageX3Controller> logger)
        {
            _sage = sage;
            _logger = logger;
        }

        [HttpPost("create-lead")]
        public async Task<IActionResult> CreateLead([FromBody] CreateLeadRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _sage.CreateLeadAsync(request);
            return OkResult(result);
        }

        [HttpPost("create-contact")]
        public async Task<IActionResult> CreateContact([FromBody] CreateContactRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _sage.CreateContactAsync(request);
            return OkResult(result);
        }

        [HttpPost("create-opportunity")]
        public async Task<IActionResult> CreateOpportunity([FromBody] CreateOpportunityRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiError("Request cannot be null"));
            }

            var result = await _sage.CreateOpportunityAsync(request);
            return OkResult(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new ApiError("Query cannot be null or empty"));
            }

            var result = await _sage.SearchEntitiesAsync(query);
            return OkResult(result);
        }
    }
}

