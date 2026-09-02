namespace Unite.Identity.Web.Models;

public class ChangePasswordModel
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordRepeat { get; set; }
}

public class ResetPasswordRequestModel
{
    public string Email { get; set; }
}

public class ResetPasswordConfirmationModel
{
    public string Token { get; set; }
    public string Password { get; set; }
    public string PasswordRepeat { get; set; }
}
