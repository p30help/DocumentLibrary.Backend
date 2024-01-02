using DocumentLibrary.WebApi.ApiModels.Requests;
using DocumentLibrary.WebApi.Common;
using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocumentLibrary.WebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IMediator mediator;        

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("AddUser")]
        [ProducesResponseType(typeof(IEnumerable<AddUserCommandResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUser(AddUserRequest request)
        {
            var command = new AddUserCommand()
            {
                Email = request.email,
                Password = request.password
            };
            var result = await mediator.Send(command);

            return Result(result);
        }
    }
}