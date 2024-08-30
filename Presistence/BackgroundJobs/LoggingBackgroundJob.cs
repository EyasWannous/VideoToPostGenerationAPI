using Quartz;

namespace VideoToPostGenerationAPI.Presistence.BackgroundJobs;

/// <summary>
/// A background job for logging the current UTC time.
/// </summary>
[DisallowConcurrentExecution]
public class LoggingBackgroundJob : IJob
{
    private readonly ILogger<LoggingBackgroundJob> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingBackgroundJob"/> class.
    /// </summary>
    /// <param name="logger">The logger instance used for logging information.</param>
    public LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Executes the job to log the current UTC time.
    /// </summary>
    /// <param name="context">The job execution context.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{UtcNow}", DateTime.UtcNow);
        return Task.CompletedTask;
    }
}
