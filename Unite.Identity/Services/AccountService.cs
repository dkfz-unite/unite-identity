using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;
using Unite.Identity.Helpers;

namespace Unite.Identity.Services;

public class AccountService
{
    private readonly IdentityDbContext _dbContext;
    private readonly UserService _userService;


    public AccountService(IdentityDbContext dbContext, UserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    /// <summary>
    /// Returns user with specified email and provider.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="provider">User provider.</param>
    /// <returns>Found user or null if user is not in access list or not registered.</returns>
    public User GetAccount(string email, string provider)
    {
        return GetUser(email, provider, true);
    }

    /// <summary>
    /// Registers user with specified email and password.
    /// Possible only for 'Default' identity provider.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>Created user or null if user is not in access list or already registered.</returns>
    public User CreateAccount(string email, string password)
    {
        var passwordHash = PasswordHelpers.GetPasswordHash(password);

        var user = GetUser(email, Providers.Default, false);

        if (user != null)
        {
            user.Password = passwordHash;
            user.IsActive = true;

            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        return user;
    }

    /// <summary>
    /// Deletes user with specified email and provider.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="provider">User provider.</param>
    /// <returns>True if user was deleted. False otherwise.</returns>
    public bool DeleteAccount(string email, string provider)
    {
        var user = GetUser(email, provider, true);

        if (user != null)
        {
            _userService.Delete(user.Id);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Changes password for user with specified email.
    /// Possible only for 'Default' identity provider.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">New password.</param>
    /// <returns>Updated user or null if user is not in access list or not registered yet.</returns>
    public User ChangePassword(string email, string password)
    {
        var passwordHash = PasswordHelpers.GetPasswordHash(password);

        var user = GetUser(email, Providers.Default, true);

        if (user != null)
        {
            user.Password = passwordHash;

            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        return user;
    }


    private User GetUser(string email, string provider, bool isActive)
    {
        return _userService.GetUser(user => 
            user.Provider.Name == provider && 
            user.Email == email && 
            user.IsActive == isActive
        );
    }
}
