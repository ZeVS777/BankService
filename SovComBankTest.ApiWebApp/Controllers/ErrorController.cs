using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SovComBankTest.ApiWebApp.Models;

namespace SovComBankTest.ApiWebApp.Controllers
{
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorController: ControllerBase
    {
        [Route("{statusCode:int?}")]
        public IActionResult Get(int? statusCode = null)
        {
            if (statusCode.HasValue && statusCode != StatusCodes.Status500InternalServerError)
                return StatusCode(statusCode.Value,
                    statusCode switch
                    {
                        StatusCodes.Status404NotFound => new ApiResult("Resource not found."),
                        StatusCodes.Status401Unauthorized => new ApiResult("Provide ApiId."),
                        StatusCodes.Status403Forbidden => new ApiResult("You have no rights for requested resources."),
                        StatusCodes.Status429TooManyRequests => new ApiResult("Too much phone numbers, should be less or equal to 128 per day."),
                        _ => new ApiResult("Request failed.")
                    });

            // Попробовать получить детали возникшей ошибки
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature?>();
            if (exceptionFeature == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResult("Error."));
            
            var routeWhereExceptionOccurred = exceptionFeature.Path;
            var exceptionThatOccurred = exceptionFeature.Error;

            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult<ErrorFeatures>
            (new ErrorFeatures(exceptionThatOccurred, routeWhereExceptionOccurred),
                "Error."));
        }
    }
}