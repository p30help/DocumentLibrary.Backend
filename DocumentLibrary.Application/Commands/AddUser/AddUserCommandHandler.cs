using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Users;
using DocumentLibrary.Domain.ValueObjects;
using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using MediatR;

namespace DocumentsLibrary.Application.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, OperationResult<AddUserCommandResult>>
    {
        private readonly IUserRepository userRepository;

        public AddUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<OperationResult<AddUserCommandResult>> Handle(AddUserCommand command, CancellationToken cancellationToken)
        {
            var isExisted = await userRepository.ExistUser(command.Email);
            if (isExisted)
                return OperationResult<AddUserCommandResult>.Failure("User already existed");

            var user = AppUser.Create(new EmailField(command.Email), new PasswordField(command.Password));

            await userRepository.AddUser(user);

            return OperationResult<AddUserCommandResult>.Success(new AddUserCommandResult()
            {
                UserId = user.Id
            });
        }

    }
}
