using System.Net;
using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
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
        using var client = CreateClient();

        var message = new MailMessage(_smtpOptions.Sender, recipient)
        {
            Subject = subject,
            Body = RenderTemplate(template, data)
        };

        client.Send(message);
    }

    public void Send<T>(string recipient, string subject, T mail) where T : class
    {
        var template = GetTemplate(typeof(T));
        var data = GetData(mail);
        Send(recipient, subject, template, data);
    }


    private SmtpClient CreateClient()
    {
        return new SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
        {
            EnableSsl = _smtpOptions.EnableSsl,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpOptions.User, _smtpOptions.Password, _smtpOptions.Domain)
        };
    }

    public static string GetTemplate(Type type)
    {
        using var stream = type.Assembly.GetManifestResourceStream($"{type.FullName}.html");

        if (stream == null)
            throw new InvalidOperationException($"Template for {type.FullName} not found.");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    public static  Dictionary<string, string> GetData<T>(T data) where T : class
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
