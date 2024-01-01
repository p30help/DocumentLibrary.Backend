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

        //[HttpGet("Test")]
        //public async Task<IActionResult> Test(int dpix, int dpiy, int width, int height)
        //{
        //    //var name = $"x{dpix}_y{dpiy}_w{width}_h{height}";
        //
        //    //GhostscriptSharp.GhostscriptWrapper.GeneratePageThumb(@"c:\testfiles\1.pdf", @$"c:\testfiles\thumb-{name}.jpg", 1 , dpix, dpiy, width, height);
        //
        //    return Ok();
        //}

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

        //[Authorize]
        //[HttpGet("GetCurrentUserInfo")]
        //[ProducesResponseType(typeof(IEnumerable<AddUserCommandResult>), StatusCodes.Status200OK)]        
        //public async Task<IActionResult> GetCurrentUserInfo()
        //{
        //    var query = new GetUserQuery()
        //    {
        //        UserId = User.GetUserId(),
        //    };
        //    var result = await mediator.Send(query);
        //
        //    return Result(result);
        //}

    }
}