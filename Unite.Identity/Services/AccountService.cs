using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;
using Unite.Identity.Helpers;

namespace Unite.Identity.Services;

public class AccountService
{
    private readonly IdentityDbContext _dbContext;
    private readonly UserService _userService;
    private readonly ProviderService _providerService;

    public AccountService(IdentityDbContext dbContext, UserService userService, ProviderService providerService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _providerService = providerService;
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
    /// Possible only for users that are in access list and not registered yet.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>Created user or null if user is not in access list or already registered.</returns>
    public User CreatePrivateAccount(string email, string password)
    {
        var passwordHash = PasswordHelpers.GetPasswordHash(password);

        var user = GetUser(email, Providers.Default, false);

        if (user != null)
        {
            user.Password = passwordHash;
            user.IsActive = true;

            _dbContext.Update(user);
            _dbContext.SaveChanges();

            return user;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Registers user with specified email and password.
    /// Possible only for 'Default' identity provider.
    /// Possible only for users that are not registered yet.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>Created user or null if user already registered.</returns>
    public User CreatePublicAccount(string email, string password)
    {
        var passwordHash = PasswordHelpers.GetPasswordHash(password);

        var user = GetUser(email, Providers.Default);

        if (user == null)
        {
            var provider = GetProvider(Providers.Default);

            user = _userService.Add(email, provider.Id, true, false);

            user.Password = passwordHash;

            _dbContext.Update(user);
            _dbContext.SaveChanges();

            return user;
        }
        else
        {
            return null;
        }
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
    public User ChangePassword(string email, string newPassword, string oldPassword)
    {
        var oldPasswordHash = PasswordHelpers.GetPasswordHash(oldPassword);
        var newPasswordHash = PasswordHelpers.GetPasswordHash(newPassword);

        var user = GetUser(email, Providers.Default, true);

        if (user == null)
            return null;

        if (user.Password != oldPasswordHash)
            return null;

        user.Password = newPasswordHash;

        _dbContext.Update(user);
        _dbContext.SaveChanges();

        return user;
    }


    private Provider GetProvider(string name)
    {
        return _providerService.GetProvider(entity => entity.Name == name && entity.IsActive == true);
    }

    private User GetUser(string email, string provider)
    {
        return _userService.GetUser(user => 
            user.Provider.Name == provider && 
            user.Email == email
        );
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
