using System.Text.Json.Serialization;

namespace Unite.Post.Mails;

public record PasswordReset
{
    [JsonPropertyName("host")]
    public string Host { get; set; }
    
    [JsonPropertyName("token")]
    public string Token { get; set; }
}
