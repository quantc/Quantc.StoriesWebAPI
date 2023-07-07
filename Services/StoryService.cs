using Quantc.StoriesWebAPI.Common;
using Quantc.StoriesWebAPI.Model;

namespace Quantc.StoriesWebAPI.Services
{
    public class StoryService
    {
        private readonly HttpClient _httpClient;

        public StoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<StoryModel>?> GetBestStoriesAsync(int count)
        {
            var storiesIds = await _httpClient.GetFromJsonAsync<IEnumerable<int>>(
                UriSpace.BestStories);

            List<StoryModel> stories = new();
            if (storiesIds == null)
            {
                return null;
            }

            foreach (int storyId in storiesIds)
            {
                var story = await _httpClient.GetFromJsonAsync<StoryModel>(string.Format(
                   UriSpace.SingleStory, storyId));

                if (story != null)
                {
                    stories.Add(story);
                }
            }

            var ordered = stories.OrderByDescending(x => x.Score).Take(count).ToList();
            return ordered;
        }
    }
}
