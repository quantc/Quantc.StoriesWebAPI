using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quantc.StoriesWebAPI.Common;
using Quantc.StoriesWebAPI.Model;
using System.Collections.Concurrent;

namespace Quantc.StoriesWebAPI.Services
{
    public class StoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public StoryService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
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
                if (_cache.TryGetValue(storyId, out StoryModel story))
                {
                    if (story != null)
                        stories.Add(story);
                }
                else
                {
                    var response = await _httpClient.GetAsync(string.Format(UriSpace.SingleStory, storyId));
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();
                    story = JsonConvert.DeserializeObject<StoryModel>(responseContent, new UnixDateTimeConverter());
                    if (story != null)
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(600))
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);
                        _cache.Set(storyId, story, cacheEntryOptions);

                        stories.Add(story);
                    }
                }
            }

            var ordered = stories.OrderByDescending(x => x.Score).Take(count).ToList();
            return ordered;
        }

        public async Task<IEnumerable<StoryModel>?> GetBestStoriesAsyncV2(int count)
        {
            var storiesIds = await _httpClient.GetFromJsonAsync<IEnumerable<int>>(
                UriSpace.BestStories);

            ConcurrentBag<StoryModel> stories = new();
            if (storiesIds == null)
            {
                return null;
            }

            await Parallel.ForEachAsync(storiesIds, async (storyId, cancellationToken) =>
            {
                if (_cache.TryGetValue(storyId, out StoryModel story))
                {
                    if (story != null)
                        stories.Add(story);
                }
                else
                {
                    var response = await _httpClient.GetAsync(string.Format(UriSpace.SingleStory, storyId), cancellationToken);
                    var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    response.EnsureSuccessStatusCode();

                    story = JsonConvert.DeserializeObject<StoryModel>(responseContent, new UnixDateTimeConverter());

                    if (story != null)
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(600))
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);
                        _cache.Set(storyId, story, cacheEntryOptions);

                        stories.Add(story);
                    }
                }
            });

            var ordered = stories.OrderByDescending(x => x.Score).Take(count).ToList();
            return ordered;
        }
    }
}
