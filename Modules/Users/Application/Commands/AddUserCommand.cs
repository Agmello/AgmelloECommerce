using MediatR;
using Modules.Users.Application.Dtos;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Application.Commands
{
    public record AddUserCommand(AddUserDto newUser) : IRequest<Guid>;
    internal class AddUserCommandHandler(IUsersRepository repository) :
        UserHandlerBase<AddUserCommand, Guid>(repository)

    {
        public override async Task<Guid> Handle(AddUserCommand command, CancellationToken cancellationToken)
        {
            return await m_userRepository.RegisterUserAsync(command.newUser, cancellationToken);
        }
    }
}
