namespace Unite.Post.Configuration.Options;

public interface ISmtpOptions
{
    string Host { get; }
    int Port { get; }
    bool EnableSsl { get; }
    string Domain { get; }
    string User { get; }
    string Password { get; }
    string Sender { get; }
}
