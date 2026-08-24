namespace Unite.Identity.Web.Configuration.Options;

public class RetentionOptions
{
    /// <summary>
    /// User data retention period in months.
    /// Account is deleted after this period of inactivity.
    /// Defaults to 3 months.
    /// </summary>
    public byte Period
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_RETENTION_PERIOD");

            if (string.IsNullOrWhiteSpace(option))
                return 3;

            if (!byte.TryParse(option, out var value))
                throw new ArgumentException("'UNITE_RETENTION_PERIOD' environment variable has to be set to a positive integer number");

            return value;
        }
    }
}
