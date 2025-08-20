using Microsoft.EntityFrameworkCore;
using Modules.Users.Application.Credentials;
using Modules.Users.Application.Dtos;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Infrastructure
{
    public class UsersRepository : IUsersRepository
    {
        UsersDbContext m_dbContext;

        public UsersRepository(UsersDbContext db)
        {
            m_dbContext = db;
        }

        public Task<bool> DeleteUserAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }
        public async Task<UserGetDto?> GetUserByIdAsync(Guid id, CancellationToken token)
        {
            return (await m_dbContext.Users.FirstOrDefaultAsync(x => x.Id == id))?.ToDto();
        }

        public async Task<UserGetDto?> GetUserByEmailAsync(string email, CancellationToken token)
        {
            return (await m_dbContext.Users.FirstOrDefaultAsync(x => x.Email == email))?.ToDto();
        }

        public Task<UserGetDto?> GetUserByUsernameAsync(string username, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> RegisterUserAsync(AddUserDto userDto, CancellationToken token)
        {
            var user = new User(userDto);
            user.Password = new UserPasswordHasher().HashPassword(user, user.Password);
            m_dbContext.Users.Add(user);
            await m_dbContext.SaveChangesAsync(token);
            return user.Id;
        }

        public Task<bool> UpdateUserLevelAsync(User user, string level)
        {
            throw new NotImplementedException();
        }
    }
}
