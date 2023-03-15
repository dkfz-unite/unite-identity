namespace Unite.Identity.Models;

public class SignInModel
{
    private string _email;

    public string Email
    {
        get { return _email?.Trim().ToLower(); }
        set { _email = value; }
    }
    public string Password { get; set; }
    public string Client { get; set; }
}
