using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;

namespace VideoToPostGenerationAPI.Services;

public class PostService : IPostService
{
    private const string BaseURL = "http://127.0.0.1:8000";
    private readonly HttpClient _client;

    public record ScoringItem
    {
        [JsonPropertyName("year_at_company")]
        public float YearAtCompany { get; set; }

        [JsonPropertyName("employee_satis_faction")]
        public float EmployeeSatisfaction { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; } = string.Empty;

        [JsonPropertyName("salary")]
        public int Salary { get; set; }
    }

    public PostService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri(BaseURL);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GetPostAsync()
    {
        //var request = new HttpRequestMessage(HttpMethod.Get, "/");
        //var response = await _client.SendAsync(request);

        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        if (!response.IsSuccessStatusCode)
            return response.StatusCode.ToString();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSomething(string itemName, int quantity)
    {
        //var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8000/items/issa/15");
        //var response = await _client.SendAsync(request);

        var response = await _client.PostAsync($"/items/{itemName}/{quantity}", null);
        response.EnsureSuccessStatusCode();

        if (!response.IsSuccessStatusCode)
            return response.StatusCode.ToString();


        return await response.Content.ReadAsStringAsync();
    }

    public async Task<ScoringItem?> PostScoringItemAsync(ScoringItem item)
    {
        var json = JsonSerializer.Serialize(item);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/scoringitem", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ScoringItem>(responseContent);
    }
}
