namespace Unite.Identity.Web.Configuration.Options;

public class RetentionOptions
{
    /// <summary>
    /// User data retention period in days.
    /// Account is deleted after this period of inactivity.
    /// Defaults to 90 days.
    /// </summary>
    public byte Period
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_RETENTION_PERIOD");

            if (string.IsNullOrWhiteSpace(option))
                return 90;

            if (!byte.TryParse(option, out var value))
                throw new ArgumentException("'UNITE_RETENTION_PERIOD' environment variable has to be set to a positive integer number");

            return value;
        }
    }
}
