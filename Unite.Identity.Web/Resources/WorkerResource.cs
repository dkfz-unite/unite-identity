using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Extensions;

namespace Unite.Identity.Web.Resources;

public class TokenResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Permissions { get; set; }
    public DateTime? TokenExpiryDate { get; set; }
    public string Key { get; set; }
    public bool Revoked { get; set; }


    public TokenResource(Token entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
        TokenExpiryDate = entity.TokenExpiryDate;
        Key = entity.Key;
        Revoked = entity.Revoked;

        if (entity.TokenPermissions?.Any() == true)
        {
            Permissions = entity.TokenPermissions
                .Select(servicePermission => servicePermission.PermissionId.ToDefinitionString())
                .ToArray();
        }
    }
}
