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
    record GetUserByEmailQuery(string email) : IRequest<UserGetDto>;

    internal class GetUserByEmailQueryHandler(IUsersRepository usersRepository) 
        : UserHandlerBase<GetUserByEmailQuery, UserGetDto>(usersRepository)
    {
        public override async Task<UserGetDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await m_userRepository.GetUserByEmailAsync(request.email, cancellationToken);
            return user;
        }
    }
}
