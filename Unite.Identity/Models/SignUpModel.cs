namespace Unite.Identity.Models;

public class SignUpModel
{
    private string _email;

    public string Email
    {
        get { return _email?.Trim().ToLower(); }
        set { _email = value; }
    }
    public string Password { get; set; }
    public string PasswordRepeat { get; set; }
}
