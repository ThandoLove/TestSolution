using Microsoft.AspNetCore.Mvc;
using OutlookBlazorTestApp2.Data.Models;
using OutlookBlazorTestApp2.services;

[ApiController]
[Route("api/[controller]")]
public class AutoLinkController : ControllerBase
{
    private readonly ICrmService _crmService;

    public AutoLinkController(ICrmService crmService)
    {
        _crmService = crmService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AutoLinkRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Sender) || string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new { error = "Sender and Content cannot be null or empty." });
        }

        var match = await _crmService.FindRecordByEmailOrContent(request.Sender, request.Content);

        if (match != null)
        {
            return Ok(new
            {
                matchFound = true,
                record = match
            });
        }

        return Ok(new { matchFound = false });
    }

    [HttpPost("createrecord")]
    public async Task<IActionResult> CreateRecord([FromBody] AutoLinkRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Sender) || string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new { error = "Sender and Content cannot be null or empty." });
        }

        var newRecord = await _crmService.CreateNewContact(request.Sender, request.Content);
        return Ok(new { record = newRecord });
    }
}
