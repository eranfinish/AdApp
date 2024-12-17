using AdApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;


namespace AdApp.Core.Services.Auth
{
    public interface IAuthService
    {
        List<User> GetAllUsers();
        ValidationResult ValidateUser(User user);
        Task<User> RegisterAsync(User user);
        Task<User?> Login(string email, string password);
        bool DeleteUser(string userName);
        bool AuthorizeUser(string userName, string password);
    }
}
