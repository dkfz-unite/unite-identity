using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;

namespace Unite.Identity.Web.Workers;

public class AccountWorker : BackgroundService
{
    private const int _interval = 24 * 60 * 60 * 1000; // 24 hours

    private readonly RetentionOptions _retentionOptions;
    private readonly SessionService _sessionService;
    private readonly AccountService _accountService;
    private readonly ILogger _logger;


    public AccountWorker(
        RetentionOptions retentionOptions,
        SessionService sessionService,
        AccountService accountService,
        ILogger<AccountWorker> logger)
    {
        _retentionOptions = retentionOptions;
        _sessionService = sessionService;
        _accountService = accountService;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Delay 5 seconds to let the web api start working
        await Task.Delay(5000, cancellationToken);

        _logger.LogInformation("Worker started");

        cancellationToken.Register(() => _logger.LogInformation("Worker stopped"));
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _sessionService.DeleteExpired();
                _accountService.DeleteInactive(_retentionOptions.Period);
            }
            finally
            {
                await Task.Delay(_interval, cancellationToken);
            }
        }
    }
}
