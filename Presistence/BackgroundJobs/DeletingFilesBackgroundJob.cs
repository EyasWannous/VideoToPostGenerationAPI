using Quartz;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Settings;

namespace VideoToPostGenerationAPI.Presistence.BackgroundJobs;

/// <summary>
/// Background job for deleting old files.
/// </summary>
public class DeletingFilesBackgroundJob : IJob
{
    private readonly ILogger<DeletingFilesBackgroundJob> _logger;
    private readonly IFileService _fileService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletingFilesBackgroundJob"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="fileService">The file service instance.</param>
    public DeletingFilesBackgroundJob(ILogger<DeletingFilesBackgroundJob> logger, IFileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
    }

    /// <summary>
    /// Executes the job to delete old files.
    /// </summary>
    /// <param name="context">The job execution context.</param>
    public async Task Execute(IJobExecutionContext context)
    {
        var files = new List<string>();
        files.AddRange(await _fileService.GetAllFilesAsync(FileSettings.VideosPath));
        files.AddRange(await _fileService.GetAllFilesAsync(FileSettings.AudiosPath));
        files.AddRange(await _fileService.GetAllFilesAsync(FileSettings.ImagesPath));

        var today = DateTime.UtcNow.Date;
        var sevenDaysAgo = today.AddDays(-7);

        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            var creationTime = fileInfo.CreationTimeUtc.Date;

            if (creationTime <= sevenDaysAgo)
            {
                try
                {
                    File.Delete(file);
                    _logger.LogInformation("File Name: {FileName} | Created At: {CreationTime} | Deleted Successfully",
                        fileInfo.Name,
                        creationTime);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting file: {FileName}", fileInfo.Name);
                }
            }
        }
    }
}
