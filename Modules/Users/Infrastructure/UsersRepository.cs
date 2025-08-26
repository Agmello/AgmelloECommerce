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

        public async Task<bool> DeleteUserAsync(Guid id, CancellationToken token)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid user ID", nameof(id));
            m_dbContext.Users.Remove(new User { Id = id });
            return await m_dbContext.SaveChangesAsync(token) > 0;
        }
        public async Task<UserGetDto?> GetUserByIdAsync(Guid id, CancellationToken token)
        {
            return (await m_dbContext.Users.FirstOrDefaultAsync(x => x.Id == id))?.ToDto();
        }

        public async Task<UserGetDto?> GetUserByEmailAsync(string email, CancellationToken token)
        {
            return (await GetUserAsync(email)).ToDto();
        }
        private async Task<User> GetUserAsync(string email)
        {
            return await m_dbContext.Users.FirstOrDefaultAsync(x => x.Email == email) ?? throw new ArgumentException("Invalid email");
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

        public async Task<User> GetUserForLoginAsync(string email, CancellationToken token)
        {
            return await GetUserAsync(email);
        }
    }
}
