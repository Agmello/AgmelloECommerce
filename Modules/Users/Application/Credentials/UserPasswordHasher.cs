using Common.Features.Hashing;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Application.Credentials
{
    public class UserPasswordHasher 
        : CommonPasswordHasher<User>
    {
    }
}
