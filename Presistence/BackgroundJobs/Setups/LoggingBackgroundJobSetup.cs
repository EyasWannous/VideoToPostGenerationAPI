using Microsoft.Extensions.Options;
using Quartz;

namespace VideoToPostGenerationAPI.Presistence.BackgroundJobs.Setups;

public class LoggingBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
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
