using Unite.Identity.Data.Entities.Enums;

namespace Unite.Identity.Web.Models;

public class AddTokenModel
{
    private string _name;
    private string _description;
    public string Name { get => _name?.Trim().ToLower(); set => _name = value; }
    public string Description { get => _description?.Trim(); set => _description = value; }
    public string Key { get; set; }
    public bool Revoked { get; set; }

    public EpiryDateModel ExpiryDate { get; set; }
    public Permission[] Permissions { get; set; }
}

public class EditTokenModel
{
    public EpiryDateModel ExpiryDate { get; set; }
    public Permission[] Permissions { get; set; }
}

public class EpiryDateModel
{
    public int? ExpiryMinutes { get; set; }
    public int? ExpiryHours { get; set; }
    public int? ExpiryDays { get; set; }
}
