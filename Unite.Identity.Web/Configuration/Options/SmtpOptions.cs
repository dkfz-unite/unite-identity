using Unite.Post.Configuration.Options;

namespace Unite.Identity.Web.Configuration.Options;

public class SmtpOptions : ISmtpOptions
{
    public string Host
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_HOST");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_SMTP_HOST environment variable is not set.");

            return option;
        }
    }

    public int Port
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_PORT");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_SMTP_PORT environment variable is not set.");

            if (!int.TryParse(option, out var port))
                throw new InvalidOperationException("UNITE_SMTP_PORT environment variable is not a valid integer.");

            return port;
        }
    }

    public bool EnableSsl
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_TLS_STARTTLS");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_SMTP_TLS_STARTTLS environment variable is not set.");

            if (!bool.TryParse(option, out var enableSsl))
                throw new InvalidOperationException("UNITE_SMTP_TLS_STARTTLS environment variable is not a valid boolean.");

            return enableSsl;
        }
    }

    public string Domain
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_NTLM_DOMAIN");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_SMTP_NTLM_DOMAIN environment variable is not set.");

            return option;
        }
    }

    public string User
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_USER");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_SMTP_USER environment variable is not set.");

            return option;
        }
    }

    public string Password
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_PASSWORD");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_SMTP_PASSWORD environment variable is not set.");

            return option;
        }
    }

    public string Sender
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_FROM");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_SMTP_FROM environment variable is not set.");

            return option;
        }
    }
}
