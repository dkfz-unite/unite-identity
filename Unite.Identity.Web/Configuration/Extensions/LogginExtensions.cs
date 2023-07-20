namespace Unite.Identity.Web.Configuration.Extensions;

public static class LogginExtensions
{
    public static void LogException(this ILogger logger, Exception exception)
    {
        logger.LogError(exception, exception.Message);

        if (exception.InnerException != null)
        {
            logger.LogException(exception.InnerException);
        }
    }
}
