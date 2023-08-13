using SantanderCodingTest.DTOs;

namespace SantanderCodingTest.Services
{
    public interface IStoryService
    {
        Task<IEnumerable<Story>> GetTopNBestStories(int n);
        Task<IEnumerable<int>?> GetAllStoriesId();
    }
}
