namespace Unite.Identity.Web.Models;

public class IdentityModel
{
    private string _email;

    public string Email { get => _email?.Trim().ToLower(); set => _email = value; }
    public string Password { get; set; }
}
