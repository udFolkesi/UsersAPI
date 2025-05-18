using Core.Entities;
using DAL.Contexts;
using DAL.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user, string revokedBy)
        {
            user.RevokedOn = DateTime.UtcNow;
            user.RevokedBy = revokedBy;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllActiveAsync()
            => await _context.Users.Where(u => u.RevokedOn == null).OrderBy(u => u.CreatedOn).ToListAsync();

        public async Task<User?> GetByLoginAsync(string login)
            => await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

        public async Task<User?> GetByLoginAndPasswordAsync(string login, string password)
            => await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password && u.RevokedOn == null);

        public async Task<IEnumerable<User>> GetOlderThanAsync(int age)
        {
            var cutoff = DateTime.UtcNow.AddYears(-age);
            return await _context.Users
                .Where(u => u.Birthday != null && u.Birthday <= cutoff)
                .ToListAsync();
        }

        public async Task RestoreAsync(User user)
        {
            user.RevokedOn = null;
            user.RevokedBy = null;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
