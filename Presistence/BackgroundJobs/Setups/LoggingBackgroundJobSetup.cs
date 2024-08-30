using Microsoft.Extensions.Options;
using Quartz;

namespace VideoToPostGenerationAPI.Presistence.BackgroundJobs.Setups;

/// <summary>
/// Configures the Quartz job for logging background tasks.
/// </summary>
public class LoggingBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    /// <summary>
    /// Configures the Quartz options for scheduling the <see cref="LoggingBackgroundJob"/>.
    /// </summary>
    /// <param name="options">The Quartz options to configure.</param>
    public void Configure(QuartzOptions options)
    {
        // Create a JobKey for the LoggingBackgroundJob
        var jobKey = JobKey.Create(nameof(LoggingBackgroundJob));

        // Configure the job and its trigger
        options
            .AddJob<LoggingBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule
                            .WithIntervalInSeconds(10)
                            .RepeatForever()
                    )
            );
    }
}
