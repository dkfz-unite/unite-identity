namespace Unite.Identity.Models;

public class LoginModel
{
    private string _userLoginId;

    public string Email
    {
        get { return _userLoginId?.Trim().ToLower(); }
        set { _userLoginId = value; }
    }
    public string Password { get; set; }
    public string Client { get; set; }
}
