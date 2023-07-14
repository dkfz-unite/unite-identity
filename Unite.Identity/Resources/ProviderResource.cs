using Unite.Identity.Data.Entities;

namespace Unite.Identity.Resources;

public class ProviderResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public int? Priority { get; set; }
    public string Title
    {
        get
        {
            return Label ?? Name;
        }
    }

    public ProviderResource(Provider provider)
    {
        Id = provider.Id;
        Name = provider.Name;
        Label = provider.Label;
        Priority = provider.Priority;
    }
}

