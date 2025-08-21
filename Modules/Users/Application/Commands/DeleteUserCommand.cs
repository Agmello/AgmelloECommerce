using MediatR;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Application.Commands
{
    internal record DeleteUserCommand(Guid UserId) : IRequest<bool>;
    internal class DeleteUserCommandHandler(IUsersRepository repository) :
        UserHandlerBase<DeleteUserCommand, bool>(repository)
    {
        public override async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await m_userRepository.DeleteUserAsync(request.UserId, cancellationToken);
        }
    }
}
