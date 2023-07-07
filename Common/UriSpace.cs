namespace Quantc.StoriesWebAPI.Common
{
    public static class UriSpace
    {
        public static string HackerNewsBase { get; set; } = "https://hacker-news.firebaseio.com/";
        public static string BestStories { get; set; } = "v0/beststories.json";
        public static string SingleStory { get; set; } = "v0/item/{0}.json";
    }
}
