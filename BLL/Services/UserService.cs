using BLL.Services.Abstractions;
using BLL.Validation;
using Core.Entities;
using Core.Response;
using Core.Response.Abstractions;
using DAL.Repositories;
using DAL.Repositories.Abstractions;
using FluentValidation;
using UsersAPI.Controllers;

namespace BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _userValidator;

        public UserService(IUserRepository repository, IValidator<User> validator)
        {
            _userRepository = repository;
            _userValidator = validator;
        }

        // Create
        public async Task<IResult> Create(User user, string creator)
        {
            var validationResult = await _userValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
                return new Result(false, validationResult.Errors.First().ErrorMessage);

            if (await _userRepository.GetByLoginAsync(user.Login) is not null)
                return new Result(false, "Login already in use");

            user.CreatedOn = DateTime.UtcNow;
            user.CreatedBy = creator;
            user.ModifiedOn = user.CreatedOn;
            user.ModifiedBy = creator;

            await _userRepository.AddAsync(user);
            return new Result(true, null);
        }

        // Update
        public async Task<IResult> UpdateUserInfoAsync(string login, UpdateUserDto userDto, string modifiedBy)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null)
                return new Result (false, "User not found");

            if (user.RevokedOn != null)
                return new Result (false, "User deleted");

            user.Name = userDto.Name;
            user.Gender = userDto.Gender;
            user.Birthday = userDto.Birthday;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            await _userRepository.UpdateAsync(user);
            return new Result(true, "Info updated successfully");
        }

        public async Task<string?> ChangePasswordAsync(string login, string newPassword, string modifiedBy)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null)
                return "User not found";

            if (user.RevokedOn != null)
                return "User deleted";

            user.Password = newPassword;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            var validationResult = _userValidator.Validate(user, options =>
            {
                options.IncludeProperties(x => x.Password);
            });

            if (!validationResult.IsValid)
                return string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));

            await _userRepository.UpdateAsync(user);
            return null;
        }

        public async Task<IResult> ChangeLoginAsync(string currentLogin, string newLogin, string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(newLogin) || !newLogin.All(char.IsLetterOrDigit))
                return new Result(false, "Login can contain only Latin letters and digits");

            var user = await _userRepository.GetByLoginAsync(currentLogin);
            if (user == null)
                return new Result(false, "User not found");

            if (user.RevokedOn != null)
                return new Result(false, "User deleted");

            if (await _userRepository.GetByLoginAsync(newLogin) != null)
                return  new Result(false, "This login is already in use");

            user.Login = newLogin;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            await _userRepository.UpdateAsync(user);
            return new Result(true, "Login changed successfully");
        }

        // Request Info
        public async Task<IEnumerable<User>> GetActiveUsersAsync() => await _userRepository.GetAllActiveAsync();

        public async Task<User?> GetByLoginAsync(string login) => await _userRepository.GetByLoginAsync(login);

        public async Task<User?> GetByCredentialsAsync(string login, string password)
            => await _userRepository.GetByLoginAndPasswordAsync(login, password);

        public async Task<IEnumerable<User>> GetOlderThanAsync(int age) 
            => await _userRepository.GetOlderThanAsync(age);

        // Delete
        public async Task<IResult> Delete(string login, string adminLogin)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null) 
                return new Result(false, "User not found");

            await _userRepository.DeleteAsync(user, adminLogin);
            return new Result(true, null);
        }

        // Restore
        public async Task<IResult> RestoreAsync(string login)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null) 
                return new Result(false, "User not found");

            await _userRepository.RestoreAsync(user);
            return new Result(true, "User restored successfully");
        }
    }
}
