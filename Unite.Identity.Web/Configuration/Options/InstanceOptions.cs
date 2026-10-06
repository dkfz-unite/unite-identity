namespace Unite.Identity.Web.Configuration.Options;

public class InstanceOptions
{
    /// <summary>
    /// The host name of the instance.
    /// </summary>
    public string Host
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_INSTANCE_HOST");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_INSTANCE_HOST environment variable is not set.");

            return option;
        }
    }

    /// <summary>
    /// Whether the instance is public or private.
    /// Public instances allows any user to register and login, while private instance has access list.
    /// Defaults to false (private).
    /// </summary>
    public bool Public
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_INSTANCE_PUBLIC");

            if (string.IsNullOrWhiteSpace(option))
                return false;

            if (!bool.TryParse(option, out var value))
                throw new ArgumentException("'UNITE_INSTANCE_PUBLIC' environment variable has to be set to 'true' or 'false'");

            return value;
        }
    }

    /// <summary>
    /// Password reset token lifetime in minutes. Defaults to 30.
    /// </summary>
    public int ResetTokenLifetime
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_RESET_TTL");

            if (string.IsNullOrWhiteSpace(option))
                return 30;

            if (!int.TryParse(option, out var minutes) || minutes <= 0)
                throw new ArgumentException("'UNITE_RESET_TTL' environment variable has to be set to a positive integer number of minutes");

            return minutes;
        }
    }
}
