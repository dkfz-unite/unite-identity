namespace Unite.Identity.Web.Models;

public class AddWorkerModel
{
    private string _name;
    private string _description;

    public string Name { get => _name?.Trim().ToLower(); set => _name = value; }
    public string Description { get => _description?.Trim(); set => _description = value; }
}

public class AddWorkerTokenModel
{
    public int? ExpiryMinutes { get; set; }
    public int? ExpiryHours { get; set; }
    public int? ExpiryDays { get; set; }
}
