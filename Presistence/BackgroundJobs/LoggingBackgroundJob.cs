﻿using Quartz;

namespace VideoToPostGenerationAPI.Presistence.BackgroundJobs;

[DisallowConcurrentExecution]
public class LoggingBackgroundJob : IJob
{
    private readonly ILogger<LoggingBackgroundJob> _logger;

    public LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{UtcNow}", DateTime.UtcNow);
        return Task.CompletedTask;
    }
}
