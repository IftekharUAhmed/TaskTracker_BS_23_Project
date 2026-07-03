using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}