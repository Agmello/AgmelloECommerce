using MediatR;
using Modules.Users.Application.Credentials;
using Modules.Users.Application.JWT;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Application.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<string>;

    internal class LoginCommandHandler : UserHandlerBase<LoginCommand, string>
    {
        IUserJwt m_userJwt;
        public LoginCommandHandler(IUsersRepository repository, IUserJwt jwt) : base(repository)
        {
            m_userJwt = jwt;
        }
        public override async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await m_userRepository.GetUserForLoginAsync(request.Email, cancellationToken);

            if (user == null) throw new ArgumentException("Invalid credentials",request.Email);
            var verified = new UserPasswordHasher().VerifyHashedPassword(user,user.Password,request.Password);
            if (verified == false) throw new ArgumentException("Invalid credentials", request.Email);

            // TODO: Add
            //user.LastLoginAt = DateTime.UtcNow;
            //m_userRepository.UpdateAsync();

            return m_userJwt.GenerateToken(user);

        }
    }
}
