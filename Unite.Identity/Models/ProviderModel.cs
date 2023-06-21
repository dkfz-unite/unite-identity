using System;
using Unite.Identity.Data.Entities;

namespace Unite.Identity.Models;

public class ProviderModel
{
    public string Name { get; set; }
    public string Label { get; set; }
    public int? Priority { get; set; }

    public ProviderModel(Provider provider)
    {
        Name = provider.Name;
        Label = provider.Label;
        Priority = provider.Priority;
    }
}

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
