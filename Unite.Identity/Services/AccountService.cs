using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;
using Unite.Identity.Helpers;

namespace Unite.Identity.Services;

public class AccountService
{
    private readonly IdentityDbContext _dbContext;
    private readonly UserService _userService;
    private readonly UserDataService _userDataService;
    private readonly ProviderService _providerService;


    public AccountService(
        IdentityDbContext dbContext,
        UserService userService,
        UserDataService userDataService,
        ProviderService providerService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _userDataService = userDataService;
        _providerService = providerService;
    }


    /// <summary>
    /// Returns user with specified email and provider.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="provider">User provider.</param>
    /// <returns>Found user or null if user is not in access list or not registered.</returns>
    public User Get(string email, string provider)
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
    public User AddPrivate(string email, string password)
    {
        var passwordHash = PasswordHelper.GetPasswordHash(password);

        var entity = GetUser(email, Providers.Default, false);
        if (entity == null)
            return null;

        entity.Password = passwordHash;
        entity.IsActive = true;

        _dbContext.Update(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    /// <summary>
    /// Registers user with specified email and password.
    /// Possible only for 'Default' identity provider.
    /// Possible only for users that are not registered yet.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>Created user or null if user already registered.</returns>
    public User AddPublic(string email, string password)
    {
        var passwordHash = PasswordHelper.GetPasswordHash(password);

        var entity = GetUser(email, Providers.Default);
        if (entity != null)
            return null;

        var provider = GetProvider(Providers.Default);

        entity = _userService.Add(email, provider.Id, true, false);

        entity.Password = passwordHash;

        _dbContext.Update(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    /// <summary>
    /// Deletes user with specified email and provider.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="provider">User provider.</param>
    /// <returns>True if user was deleted. False otherwise.</returns>
    public bool Delete(string email, string provider)
    {
        var entity = GetUser(email, provider, true);
        if (entity == null)
            return false;

        _userDataService.DeleteAnalysesForUser(entity.Email);
        _userDataService.DeleteDatasetsForUser(entity.Email);
        _userService.Delete(entity);
        return true;
    }

    /// <summary>
    /// Deletes all users that are inactive for specified retention period.
    /// </summary>
    /// <param name="retentionPeriod">Retention period in days.</param>
    public void DeleteInactive(int retentionPeriod)
    {
        var entities = _userService.GetAll(user => user.LastActive < DateTime.UtcNow.AddDays(-retentionPeriod));

        foreach (var entity in entities)
        {
            // Call this method as it should remove underlying data as well.
            Delete(entity.Email, entity.Provider.Name);
        }
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
        var oldPasswordHash = PasswordHelper.GetPasswordHash(oldPassword);
        var newPasswordHash = PasswordHelper.GetPasswordHash(newPassword);

        var entity = GetUser(email, Providers.Default, true);

        if (entity == null)
            return null;

        if (entity.Password != oldPasswordHash)
            return null;

        entity.Password = newPasswordHash;

        _dbContext.Update(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    /// <summary>
    /// Requests password reset token.
    /// Possible only for 'Default' identity provider.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <returns>Password reset token or null if user is not found.</returns>
    public string RequestPasswordReset(string email)
    {
        var entity = GetUser(email, Providers.Default, true);
        if (entity == null)
            return null;

        var token = Guid.NewGuid().ToString();
        entity.PasswordToken = PasswordHelper.GetPasswordHash(token);
        entity.PasswordTokenExpires = DateTime.UtcNow.AddMinutes(30);

        _dbContext.Update(entity);
        _dbContext.SaveChanges();
        
        return token;
    }

    /// <summary>
    /// Confirms password reset using the provided token and sets the new password.
    /// Possible only for 'Default' identity provider.
    /// </summary>
    /// <param name="token">Password reset token.</param>
    /// <param name="password">New password.</param>
    /// <returns>Updated user or null if token is invalid or expired.</returns>
    public User ConfirmPasswordReset(string token, string password)
    {
        var tokenHash = PasswordHelper.GetPasswordHash(token);
        var passwordHash = PasswordHelper.GetPasswordHash(password);

         var entity = GetUserByToken(tokenHash);
         if (entity == null)
             return null;

        if (entity.PasswordTokenExpires > DateTime.UtcNow)
        {
            entity.PasswordToken = null;
            entity.PasswordTokenExpires = null;

            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return null;
        }
        else
        {
            entity.Password = passwordHash;
            entity.PasswordToken = null;
            entity.PasswordTokenExpires = null;

            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return entity;
        }
    }


    private Provider GetProvider(string name)
    {
        return _providerService.Get(entity => entity.Name == name && entity.IsActive == true);
    }

    private User GetUser(string email, string provider)
    {
        return _userService.Get(entity => 
            entity.Provider.Name == provider && 
            entity.Email == email
        );
    }

    private User GetUser(string email, string provider, bool isActive)
    {
        return _userService.Get(entity => 
            entity.Provider.Name == provider && 
            entity.Email == email && 
            entity.IsActive == isActive
        );
    }

    private User GetUserByToken(string tokenHash)
    {
       return _userService.Get(entity =>
            entity.PasswordToken == tokenHash
        );
    }
}
