using System;
namespace Unite.Identity.Models;

public class AddProviderModel
{
    public string Name { get; set; }
    public string Label { get; set; }
    public bool IsActive { get; set; }
    public int? Priority { get; set; }
}

public class EditProviderModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public bool IsActive { get; set; }
    public int? Priority { get; set; }
}
