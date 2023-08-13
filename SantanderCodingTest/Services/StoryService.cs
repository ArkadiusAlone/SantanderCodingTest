using Newtonsoft.Json;
using SantanderCodingTest.DTOs;

namespace SantanderCodingTest.Services
{
    public class StoryService : IStoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StoryService> _logger;

        public StoryService(HttpClient httpClient, ILogger<StoryService> logger)
        {
            _httpClient = httpClient; 
            _logger = logger;   
        }

        public async Task<IEnumerable<Story>> GetTopNBestStories(int n)
        {
            try
            {
                var result = new List<Story>();

                var bestStoryIds = await GetAllStoriesId();

                if (bestStoryIds is null)
                    return new List<Story>();

                var storyDetailsResponse = bestStoryIds.Select(id => _httpClient.GetAsync($"/v0/item/{id}.json")).ToList();

                var bestStoriesInJson = await Task.WhenAll(storyDetailsResponse
                                                 .Where(x => x.Result.IsSuccessStatusCode)
                                                 .Select(async x =>
                                                 {
                                                     var json = await x.Result.Content.ReadAsStringAsync();
                                                     return JsonConvert.DeserializeObject<Story>(json);
                                                 }));

                foreach (var json in bestStoriesInJson)
                {
                    if (json is not null)
                        result.Add(json);
                }

                return result.OrderByDescending(story => story?.Score)
                             .Take(n);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing GetTopNBestStories: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<int>?> GetAllStoriesId()
        {
            try
            {
                var bestStoriesResponse = await _httpClient.GetAsync("/v0/beststories.json");

                if (bestStoriesResponse.IsSuccessStatusCode)
                {
                    var bestStoriesContent = await bestStoriesResponse.Content.ReadAsStringAsync();
                    var bestStoryIds = JsonConvert.DeserializeObject<int[]>(bestStoriesContent);

                    return bestStoryIds?.ToList();
                }

                return new List<int>();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing GetAllStoriesId: {Message}", ex.Message);
                throw;
            }
        }
    }
}
