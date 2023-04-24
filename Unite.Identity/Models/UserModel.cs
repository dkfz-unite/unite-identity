using Unite.Identity.Data.Entities.Enums;

namespace Unite.Identity.Models;

public class AddUserModel
{
    private string _email;

    public string Email
    {
        get { return _email?.Trim().ToLower(); }
        set { _email = value; }
    }

    public Permission[] Permissions { get; set; }
}

public class EditUserModel
{
    public int? Id { get; set; }

    public Permission[] Permissions { get; set; }
}
