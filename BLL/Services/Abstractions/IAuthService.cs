using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstractions
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        Task<(bool IsSuccess, string? Error, string? Token)> AuthenticateAsync(string login, string password);
    }
}
