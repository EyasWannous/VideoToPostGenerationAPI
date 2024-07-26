using VideoToPostGenerationAPI.Services;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface IPostService
{
    public Task<string> GetPostAsync();
    public Task<string> GetSomething(string itemName, int quantity);
    public Task<PostService.ScoringItem?> PostScoringItemAsync(PostService.ScoringItem item);
}
