namespace Unite.Identity.Web.Models;

public class AddProviderModel 
{
    private string _name;
    private string _label;

    public string Name { get => _name?.Trim() ; set => _name = value; }
    public string Label { get => _label?.Trim(); set => _label = value; }
    public bool? IsActive { get; set; }
    public int? Priority { get; set; }
}

public class EditProviderModel
{
    private string _name;
    private string _label;

    public string Name { get => _name?.Trim() ; set => _name = value; }
    public string Label { get => _label?.Trim(); set => _label = value; }
    public bool? IsActive { get; set; }
    public int? Priority { get; set; }
}
