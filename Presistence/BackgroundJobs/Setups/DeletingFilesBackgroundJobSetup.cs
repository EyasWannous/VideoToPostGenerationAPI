using Microsoft.Extensions.Options;
using Quartz;

namespace VideoToPostGenerationAPI.Presistence.BackgroundJobs.Setups;

public class DeletingFilesBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        // Create a JobKey for the DeletingFilesBackgroundJob
        var jobKey = JobKey.Create(nameof(DeletingFilesBackgroundJob));

        // Configure the job and its trigger
        options
            .AddJob<DeletingFilesBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule
                            .WithInterval(TimeSpan.FromDays(7))
                            .RepeatForever()
                    )
            );
    }
}
