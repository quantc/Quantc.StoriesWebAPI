namespace Quantc.StoriesWebAPI.Model
{
    public class StoryModel
    {
        public string Title { get; set; }
        public string? URI { get; set; }
        public string? PostedBy { get; set; }
        public DateTime Time { get; set; }
        public int Score { get; set; }
        public int CommentCount { get; set; }
    }
}