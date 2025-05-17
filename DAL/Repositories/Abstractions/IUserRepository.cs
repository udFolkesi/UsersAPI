using Core.Entities;

namespace DAL.Repositories.Abstractions
{
    public interface IUserRepository: IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAllActiveAsync();
        Task<User?> GetByLoginAsync(string login);
        Task<User?> GetByLoginAndPasswordAsync(string login, string password);
        Task<IEnumerable<User>> GetOlderThanAsync(int age);
    }
}
