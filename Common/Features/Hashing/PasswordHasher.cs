using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Features.Hashing
{
    internal interface IPasswordHasher<TClass>
    {
        string HashPassword(TClass obj, string password);
        bool VerifyHashedPassword(TClass obj, string hashedPassword, string providedPassword);
    }

    public abstract class CommonPasswordHasher<TClass> : IPasswordHasher<TClass> where TClass : class 
    {
        public string HashPassword(TClass obj, string password)
        {
            var hasher = new PasswordHasher<TClass>();
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

            var hashedPassword = hasher.HashPassword(obj, password);
            return hashedPassword;
        }

        public bool VerifyHashedPassword(TClass obj, string hashedPassword, string providedPassword)
        {
            var hasher = new PasswordHasher<TClass>();
            var match = hasher.VerifyHashedPassword(obj, hashedPassword, providedPassword);

            return match != PasswordVerificationResult.Failed;
        }
    }
}
