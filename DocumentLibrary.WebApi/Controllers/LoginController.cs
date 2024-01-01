using DocumentLibrary.WebApi.ApiModels.Requests;
using DocumentLibrary.WebApi.ApiModels.Responses;
using DocumentLibrary.WebApi.Common;
using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocumentLibrary.WebApi.Controllers
{
    [ApiController]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    [Route("api/[controller]")]
    public class LoginController : BaseController
    {
        private readonly IMediator mediator;

        public LoginController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<GetTokenResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            var command = new GetDocumentUrlQuery()
            {
                Email = login.email,
                Password = login.password
            };

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return Result(result);            

            return Ok(result.Data);

        }
    }
}