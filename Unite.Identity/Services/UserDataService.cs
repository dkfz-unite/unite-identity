using Unite.Cache.Configuration.Options;
using Unite.Data.Context;
using Unite.Identity.Repositories;

namespace Unite.Identity.Services;

public class UserDataService
{
    private readonly DomainDbContext _dbContext;
    private readonly AnalysesRepository _analysesRepository;
    private readonly DatasetsRepository _datasetsRepository;

    public UserDataService(IMongoOptions mongoOptions, DomainDbContext dbContext)
    {
        _dbContext = dbContext;
        _analysesRepository = new AnalysesRepository(mongoOptions);
        _datasetsRepository = new DatasetsRepository(mongoOptions);
    }

    public void DeleteDatasetsForUser(string id)
    {
        var entries = _datasetsRepository.Where(entry => string.Equals(entry.Document.UserId, id));
        var entryIds = entries.Select(entry => entry.Document.Id).ToArray();
        
        _datasetsRepository.DeleteMany(entryIds);
    }

    public void DeleteAnalysesForUser(string id)
    {
        var enties = _analysesRepository.Where(entry => string.Equals(entry.Document.UserId, id));
        var entryIds = enties.Select(entry => entry.Document.Id).ToArray();
        var documentIds = enties.Select(entry => entry.Document.Id).ToArray();
        var tasks = _dbContext.Set<Unite.Data.Entities.Tasks.Task>()
            .Where(task => task.AnalysisTypeId != null && documentIds.Contains(task.Target))
            .ToArray();

        _analysesRepository.DeleteMany(entryIds);
        _dbContext.Set<Unite.Data.Entities.Tasks.Task>().RemoveRange(tasks);
        _dbContext.SaveChanges();
    }
}
