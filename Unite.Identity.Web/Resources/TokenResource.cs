using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Extensions;

namespace Unite.Identity.Web.Resources;

public class TokenResource
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Permissions { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool Revoked { get; set; }


    public TokenResource(Token entity)
    {
        Id = entity.Id;
        Key = entity.Key;
        Name = entity.Name;
        Description = entity.Description;
        ExpiryDate = entity.ExpiryDate;
        Revoked = entity.Revoked;

        if (entity.TokenPermissions?.Any() == true)
        {
            Permissions = entity.TokenPermissions
                .Select(servicePermission => servicePermission.PermissionId.ToDefinitionString())
                .ToArray();
        }
    }
}
