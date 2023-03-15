namespace Unite.Identity.Models;

public class PasswordChangeModel
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordRepeat { get; set; }
}
