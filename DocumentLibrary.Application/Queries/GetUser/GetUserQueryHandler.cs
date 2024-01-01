using DocumentLibrary.Domain.Contracts;
using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;

namespace DocumentsLibrary.Application.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, OperationResult<GetUserResult>>
    {
        private readonly IUserRepository userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<OperationResult<GetUserResult>> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUser(query.UserId);

            if (user == null)
                return OperationResult<GetUserResult>.NotFound("User not found");

            return OperationResult<GetUserResult>.Success(new GetUserResult()
            {
                UserId = user.Id,
                Email = user.Email!
            });
        }

    }
}
