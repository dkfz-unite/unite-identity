using Unite.Cache.Configuration.Options;
using Unite.Cache.Repositories;

namespace Unite.Identity.Repositories;

internal record AnalysisRecord(string Id, string UserId);

internal class AnalysesRepository : CacheRepository<AnalysisRecord>
{
    public override string DatabaseName => "user-data";
    public override string CollectionName => "analyses";

    public AnalysesRepository(IMongoOptions options) : base(options)
    {
    }
}
