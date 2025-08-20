using MediatR;
using Modules.Users.Application.Dtos;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Application.Queries
{
    record GetUserByIdQuery(Guid id) : IRequest<UserGetDto>;

    internal class GetUserByIdQueryHandler(IUsersRepository usersRepository) 
        : UserHandlerBase<GetUserByIdQuery, UserGetDto>(usersRepository)
    {
        public override async Task<UserGetDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await m_userRepository.GetUserByIdAsync(request.id, cancellationToken);
            return user;
        }
    }
}
