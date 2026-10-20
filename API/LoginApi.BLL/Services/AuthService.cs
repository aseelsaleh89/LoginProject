
using LoginApi.DAL.Entities;
using LoginApi.DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LoginApi.BLL.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public bool CreateUser(
        string username,
        string email,
        string password)
    {
        if (_userRepository.GetByUsername(username) is not null)
        {
            return false;
        }

        var user = new User
        {
            Username = username,
            Email = email,
            IsActive = true
        };

        user.PasswordHash =
            _passwordHasher.HashPassword(user, password);

        _userRepository.Add(user);

        return true;
    }

    public bool ValidateCredentials(
        string username,
        string password)
    {
        var user = _userRepository.GetByUsername(username);

        if (user is null ||
            !user.IsActive ||
            string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            return false;
        }

        try
        {
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password);

            return result != PasswordVerificationResult.Failed;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public bool ResetPassword(
        string username,
        string newPassword)
    {
        var user = _userRepository.GetByUsername(username);

        if (user is null)
        {
            return false;
        }

        user.PasswordHash =
            _passwordHasher.HashPassword(user, newPassword);

        _userRepository.Update(user);

        return true;
    }
}
