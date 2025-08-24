using Modules.Users.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Domain
{
    public interface IUsersRepository
    {
        Task<UserGetDto?> GetUserByIdAsync(Guid id, CancellationToken token);
        Task<UserGetDto?> GetUserByEmailAsync(string email, CancellationToken token);
        Task<UserGetDto?> GetUserByUsernameAsync(string username, CancellationToken token);
        Task<bool> UpdateUserLevelAsync(User user, string level);
        Task<Guid> RegisterUserAsync(AddUserDto user, CancellationToken token);
        Task<bool> DeleteUserAsync(Guid id, CancellationToken token);
        Task<User> GetUserForLoginAsync(string email, CancellationToken token);
    }
}
