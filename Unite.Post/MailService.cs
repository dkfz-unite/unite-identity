using Unite.Post.Configuration.Options;
using Unite.Post.Mails;

namespace Unite.Post;

public class MailService
{
    private readonly MailClient _client;

    public MailService(ISmtpOptions options)
    {
        _client = new MailClient(options);
    }

    public void SendPasswordResetMail(string recipient, PasswordReset mail)
    {
        _client.Send(recipient, "Password Reset", mail);
    }
}
