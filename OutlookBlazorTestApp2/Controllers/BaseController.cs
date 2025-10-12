using Microsoft.AspNetCore.Mvc;
using OutlookBlazorTestApp2.Data.Models;

namespace OutlookBlazorTestApp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Standardized success/error response for operations returning data.
        /// </summary>
        protected IActionResult OkResult<T>(OperationResult<T> result)
        {
            if (result == null)
                return StatusCode(500, new ApiError("Null result returned from operation"));

            if (result.Success == false)
            {
                return StatusCode(result.StatusCode != 0 ? result.StatusCode : 400,
                    new ApiError(result.ErrorMessage ?? "Operation failed", result.ValidationErrors));
            }

            return StatusCode(result.StatusCode != 0 ? result.StatusCode : 200,
                new ApiResponse<T>
                {
                    Data = result.Data,
                    Message = "Operation successful",
                    Success = true,
                    Timestamp = DateTime.UtcNow
                });
        }

        /// <summary>
        /// Standardized success/error response for operations without data payload.
        /// </summary>
        protected IActionResult OkResult(OperationResult<object> result)
        {
            if (result == null)
                return StatusCode(500, new ApiError("Null result returned from operation"));

            if (result.Success == false)
            {
                return StatusCode(result.StatusCode != 0 ? result.StatusCode : 400,
                    new ApiError(result.ErrorMessage ?? "Operation failed", result.ValidationErrors));
            }

            return StatusCode(result.StatusCode != 0 ? result.StatusCode : 200,
                new ApiResponse<bool>
                {
                    Data = true,
                    Message = "Operation successful",
                    Success = true,
                    Timestamp = DateTime.UtcNow
                });
        }
    }
}
