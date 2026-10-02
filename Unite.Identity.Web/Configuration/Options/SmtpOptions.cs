using Unite.Essentials.Attributes;
using Unite.Post.Configuration.Options;

namespace Unite.Identity.Web.Configuration.Options;

public class SmtpOptions : ISmtpOptions
{
    public string Host
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_HOST");

            if (string.IsNullOrWhiteSpace(option))
                throw new InvalidOperationException("UNITE_SMTP_HOST environment variable is not set.");

            return option.Trim();
        }
    }

    public int Port
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_PORT");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_SMTP_PORT environment variable is not set.");

            if (!int.TryParse(option, out var port) || port < 1 || port > 65535)
                throw new InvalidOperationException("UNITE_SMTP_PORT environment variable has to be an integer between 1 and 65535.");

            return port;
        }
    }

    public bool EnableSsl
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_SSL_ENABLE");

            if (string.IsNullOrEmpty(option))
                throw new InvalidOperationException("UNITE_SMTP_SSL_ENABLE environment variable is not set.");

            if (!bool.TryParse(option, out var value))
                throw new InvalidOperationException("UNITE_SMTP_SSL_ENABLE environment variable is not a valid boolean.");

            return value;
        }
    }

    public SmtpLoginMethod LoginMethod
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_AUTH_METHOD");

            if (string.IsNullOrWhiteSpace(option))
                throw new InvalidOperationException("UNITE_SMTP_AUTH_METHOD environment variable is not set.");

            try
            {
                return option.FromAliasString<SmtpLoginMethod>();
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("UNITE_SMTP_AUTH_METHOD environment variable has to be set to 'login', 'plain' or 'ntlm'.", exception);
            }
        }
    }

    public string Domain
    {
        get
        {
            if (LoginMethod != SmtpLoginMethod.Ntlm)
                return null;

            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_NTLM_DOMAIN");

            if (string.IsNullOrWhiteSpace(option))
                throw new InvalidOperationException("UNITE_SMTP_NTLM_DOMAIN environment variable is not set.");

            return option.Trim();
        }
    }

    public string User
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_SMTP_USER");

            if (string.IsNullOrWhiteSpace(option))
                throw new InvalidOperationException("UNITE_SMTP_USER environment variable is not set.");

            return option.Trim();
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

            if (string.IsNullOrWhiteSpace(option))
                throw new InvalidOperationException("UNITE_SMTP_FROM environment variable is not set.");

            return option.Trim();
        }
    }
}
