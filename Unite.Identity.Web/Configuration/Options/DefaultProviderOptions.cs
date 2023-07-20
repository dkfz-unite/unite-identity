namespace Unite.Identity.Web.Configuration.Options;

public class DefaultProviderOptions
{
    public bool Active => true;

    public string Label
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_DEFAULT_LABEL");

            if (Active && string.IsNullOrWhiteSpace(option))
                throw new ArgumentException("'UNITE_DEFAULT_LABEL' environment variable has to be set");

            return option?.Trim();
        }
    }

    public int? Priority
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_DEFAULT_PRIORITY");

            if (string.IsNullOrWhiteSpace(option))
                return null;

            if (!int.TryParse(option, out var value))
                throw new ArgumentException("'UNITE_DEFAULT_PRIORITY' environment variable has to be set to a positive integer number");

            return value;
        }
    }
}
