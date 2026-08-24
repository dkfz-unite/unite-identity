namespace Unite.Identity.Web.Configuration.Options;

public class InstanceOptions
{
    /// <summary>
    /// Whether the instance is public or private.
    /// Public instances allows any user to register and login, while private instance has access list.
    /// Defaults to false (private).
    /// </summary>
    public bool Public
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_PUBLIC_INSTANCE");

            if (string.IsNullOrWhiteSpace(option))
                return false;

            if (!bool.TryParse(option, out var value))
                throw new ArgumentException("'UNITE_PUBLIC_INSTANCE' environment variable has to be set to 'true' or 'false'");

            return value;
        }
    }
}
