using DocumentLibrary.Domain.Contracts;
using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;

namespace DocumentsLibrary.Application.Commands.GenerateToken
{
    public class GetTokenCommandHandler : IRequestHandler<GetDocumentUrlQuery, OperationResult<GetTokenResult>>
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenManager tokenManager;

        public GetTokenCommandHandler(IUserRepository userRepository, ITokenManager tokenManager)
        {
            this.userRepository = userRepository;
            this.tokenManager = tokenManager;
        }

        public async Task<OperationResult<GetTokenResult>> Handle(GetDocumentUrlQuery command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserWithPassword(command.Email, command.Password);

            if (user == null)
                return OperationResult<GetTokenResult>.Failure("User or password is incorrect");

            var roles = await userRepository.GetUserRoles(user.Email!);

            if (roles == null || roles.Any() == false)
                return OperationResult<GetTokenResult>.Failure("User or password is incorrect");

            var token = tokenManager.GenerateToken(user, roles);

            return OperationResult<GetTokenResult>.Success(
                new GetTokenResult()
                {
                    AccessToken = token
                });
        }

    }
}
