using Unite.Post.Configuration.Options;

namespace Unite.Identity.Web.Configuration.Extensions;

public static class SmtpOptionsExtensions
{
    public static bool IsConfigured(this ISmtpOptions options)
    {
        return !string.IsNullOrWhiteSpace(options.Host);
    }
}
