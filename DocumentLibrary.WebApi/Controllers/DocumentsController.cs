using DocumentLibrary.Domain.Common;
using DocumentLibrary.WebApi.ApiModels.Responses;
using DocumentLibrary.WebApi.Common;
using DocumentLibrary.WebApi.Tools;
using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentLibrary.WebApi.Controllers
{
    [ApiController]    
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class DocumentsController : BaseController
    {
        private readonly IMediator mediator;

        public DocumentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<UploadDocumentResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadDocument(IFormFile file, CancellationToken cancellationToken)
        {
            var command = new UploadDocumentCommand()
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileStream = file.OpenReadStream(),
                UserId = User.GetUserId(),
            };
            var result = await mediator.Send(command, cancellationToken);

            return Result(result);
        }

        [HttpGet("{documentId}")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocument(Guid documentId)
        {
            var query = new GetDocumentStreamQuery()
            {
                DocumentId = documentId,
                UserId = User.GetUserId(),
            };
            var result = await mediator.Send(query);
            
            if (!result.IsSuccess)
                return Result(result);

            return File(result.Data!.FileStream, result.Data!.ContentType, result.Data!.FileName);
        }

        [AllowAnonymous]
        [HttpGet("temp/{encryptedText}")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTempDocument(string encryptedText)
        {
            var query = new GetDocumentStreamByTempUrlQuery()
            {
                EncryptedText = encryptedText,
            };
            var result = await mediator.Send(query);

            if (!result.IsSuccess)
                return Result(result);

            return File(result.Data!.FileStream, result.Data!.ContentType);
        }

        [HttpGet("GetListOfDocuments")]
        [ProducesResponseType(typeof(PagedData<GetListOfDocumentsResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListOfDocuments(int? pageNumber, int? pageSize)
        {
            var query = new GetListOfDocumentsQuery()
            {
                UserId = User.GetUserId(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await mediator.Send(query);

            return Result(result);
        }

        [HttpPut("GenerateTemporaryLink/{documentId}")]
        [ProducesResponseType(typeof(GenerateTemporaryLinkResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateTemporaryLink(Guid documentId, int expirationTime)
        {
            var command = new GenerateTemporaryLinkCommand()
            {
                ExpirationTime = expirationTime,
                DocumentId = documentId,
                UserId = User.GetUserId(),
            };
            var result = await mediator.Send(command);

            return Result(result);
        }
    }
}