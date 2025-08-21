using Modules.Users.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Accessibility { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public void Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            UserName = user.UserName;
            Email = user.Email;
            Password = user.Password;
            Accessibility = user.Accessibility;
            LastLoginAt = user.LastLoginAt;
        }

        public UserGetDto ToDto()
        {
            return new UserGetDto
            {
                Id = Id,
                UserName = UserName,
                Email = Email,
                CreatedAt = CreatedAt,
            };
        }
        public User()
        {
        }
        public User(AddUserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));
            UserName = userDto.UserName;
            Email = userDto.Email;
            Password = userDto.Password;
            Accessibility = userDto.Accessibility;
        }
    }
}
