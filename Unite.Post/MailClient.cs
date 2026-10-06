using System.Net;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Unite.Post.Configuration.Options;

namespace Unite.Post;

public class MailClient
{
    private readonly ISmtpOptions _smtpOptions;
    private static readonly Regex _templateVariable = new(@"\{\{([^{}]+)\}\}", RegexOptions.Compiled);


    public MailClient(ISmtpOptions smtpOptions)
    {
        _smtpOptions = smtpOptions;
    }


    public void Send(string recipient, string subject, string template, IReadOnlyDictionary<string, string> data)
    {
        using var message = new MimeMessage();
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = RenderTemplate(template, data) };
        message.From.Add(MailboxAddress.Parse(_smtpOptions.Sender));
        message.To.Add(MailboxAddress.Parse(recipient));

        using var client = new SmtpClient();
        client.Connect(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

        Login(client);
        client.Send(message);
        client.Disconnect(true);
    }

    public void Send<T>(string recipient, string subject, T mail) where T : class
    {
        var template = GetTemplate(typeof(T));
        var data = GetData(mail);
        Send(recipient, subject, template, data);
    }


    private void Login(SmtpClient client)
    {
        var method = _smtpOptions.LoginMethod;

        SaslMechanism mechanism = method switch
        {
            SmtpLoginMethod.Login => new SaslMechanismLogin(_smtpOptions.User, _smtpOptions.Password),
            SmtpLoginMethod.Plain => new SaslMechanismPlain(_smtpOptions.User, _smtpOptions.Password),
            SmtpLoginMethod.Ntlm => new SaslMechanismNtlm(new NetworkCredential(_smtpOptions.User, _smtpOptions.Password, _smtpOptions.Domain)) { AllowChannelBinding = true },
            _ => throw new InvalidOperationException($"Unsupported SMTP login method '{method}'.")
        };

        if (!client.AuthenticationMechanisms.Contains(mechanism.MechanismName))
            throw new InvalidOperationException($"SMTP server does not advertise '{mechanism.MechanismName}' authentication.");

        client.Authenticate(mechanism);
    }

    public static string GetTemplate(Type type)
    {
        using var stream = type.Assembly.GetManifestResourceStream($"{type.FullName}.html");

        if (stream == null)
            throw new InvalidOperationException($"Template for {type.FullName} not found.");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    public static Dictionary<string, string> GetData<T>(T data) where T : class
    {
        var dictionary = new Dictionary<string, string>();

        var properties = typeof(T).GetProperties();
        
        foreach (var property in properties)
        {
            var attribute = property
                .GetCustomAttributes(typeof(JsonPropertyNameAttribute), false)
                .FirstOrDefault() as JsonPropertyNameAttribute;

            if (attribute != null)
            {
                var value = property.GetValue(data)?.ToString() ?? string.Empty;    
                dictionary[attribute.Name] = value;
            }
        }

        return dictionary;
    }

    public static string RenderTemplate(string template, IReadOnlyDictionary<string, string> data)
    {
        return _templateVariable.Replace(template, match =>
        {
            var key = match.Groups[1].Value;

            if (!data.TryGetValue(key, out var value))
                throw new InvalidOperationException($"Template variable '{key}' was not provided.");

            return value;
        });
    }
}
