using Unite.Cache.Configuration.Options;
using Unite.Cache.Repositories;

namespace Unite.Identity.Repositories;

internal record DatasetRecord(string Id, string UserId);

internal class DatasetsRepository : CacheRepository<DatasetRecord>
{
    public override string DatabaseName => "user-data";
    public override string CollectionName => "datasets";

    public DatasetsRepository(IMongoOptions options) : base(options)
    {
    }
}
