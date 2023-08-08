using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Extensions;

namespace Unite.Identity.Web.Resources;

public class WorkerResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Permissions { get; set; }
    public DateTime? TokenExpiryDate { get; set; }


    public WorkerResource(Worker entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
        TokenExpiryDate = entity.TokenExpiryDate;

        if (entity.WorkerPermissions?.Any() == true)
        {
            Permissions = entity.WorkerPermissions
                .Select(servicePermission => servicePermission.PermissionId.ToDefinitionString())
                .ToArray();
        }
    }
}
