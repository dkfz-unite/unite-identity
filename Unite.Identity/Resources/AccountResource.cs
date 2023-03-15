namespace Unite.Identity.Resources;

public class AccountResource
{
    public string Email { get; set; }

    public string[] Permissions { get; set; }
    public string[] Devices { get; set; }
}
