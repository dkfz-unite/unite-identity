using Unite.Identity.Data.Entities.Enums;

namespace Unite.Identity.Web.Models;

public class AddUserModel
{
    private string _email;

    public int? ProviderId { get; set; }
    public string Email { get => _email?.Trim().ToLower(); set => _email = value; }
    public Permission[] Permissions { get; set; }
}


public class EditUserModel
{
    public int? ProviderId { get; set; }
    public Permission[] Permissions { get; set; }
}


public class CheckUserModel
{
    private string _email;

    public int? ProviderId { get; set; }
    public string Email { get => _email?.Trim().ToLower(); set => _email = value; }
}
