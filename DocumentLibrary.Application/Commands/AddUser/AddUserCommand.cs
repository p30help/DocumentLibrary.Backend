using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetListOfDocuments
{
    public class AddUserCommand : IRequest<OperationResult<AddUserCommandResult>>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
