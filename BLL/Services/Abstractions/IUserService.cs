using Core.Entities;
using Core.Response.Abstractions;
using UsersAPI.Controllers;

namespace BLL.Services.Abstractions
{
    public interface IUserService
    {
        Task<(bool Success, string? Error)> Create(User user, string creator);
        Task<string?> UpdateUserInfoAsync(string login, UpdateUserDto userDto, string modifiedBy);
        Task<string?> ChangePasswordAsync(string login, string newPassword, string modifiedBy);
        Task<string?> ChangeLoginAsync(string currentLogin, string newLogin, string modifiedBy);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<User?> GetByLoginAsync(string login);
        Task<User?> GetByCredentialsAsync(string login, string password);
        Task<IEnumerable<User>> GetOlderThanAsync(int age);
        Task<(bool Success, string? Error)> Delete(string login, string adminLogin);
        Task<(bool Success, string? Error)> RestoreAsync(string login);
    }
}
