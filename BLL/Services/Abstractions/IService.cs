using Core.Entities;

namespace BLL.Services.Abstractions
{
    public interface IService
    {
        Task<User?> Authenticate(string login, string password);
        Task<List<User>> GetAllActive();
        Task<User?> GetByLogin(string login);
        Task<User?> GetByLoginAndPassword(string login, string password);
        Task<List<User>> GetOlderThan(int age);
        Task<bool> Create(User user, string creatorLogin);
        Task<bool> Update(User updated, string modifierLogin);
        Task<bool> UpdateLogin(string oldLogin, string newLogin, string byLogin);
        Task<bool> UpdatePassword(string login, string newPassword, string byLogin);
        Task<bool> SoftDelete(string login, string byLogin);
        Task<bool> Restore(string login);
    }
}
