using DocumentLibrary.WebApi.ApiModels.Responses;
using DocumentsLibrary.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DocumentLibrary.WebApi.Common
{
    public class BaseController : ControllerBase
    {
        public IActionResult Result<TData>(OperationResult<TData> result)
        {
            if (result.IsSuccess)
                return Ok(result.Data);
            else if (result.IsNotFound)
                return NotFound(new ApiError(HttpStatusCode.NotFound.ToString(), result.ErrorMessage!));
            else
                return BadRequest(new ApiError(HttpStatusCode.BadRequest.ToString(), result.ErrorMessage!));
        }
    }
}
