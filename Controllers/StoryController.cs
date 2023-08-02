using Microsoft.AspNetCore.Mvc;
using Quantc.StoriesWebAPI.Model;
using Quantc.StoriesWebAPI.Services;

namespace Quantc.StoriesWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly StoryService _storyService;

        public StoryController(StoryService storyService)
        {
            _storyService = storyService;
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any,
            VaryByQueryKeys = new string[] { "count" })]
        [HttpGet(Name = "Get Best Stories")]
        public async Task<ActionResult<IEnumerable<StoryModel>>> Get(int count = 3)
        {
            var result = await _storyService.GetBestStoriesAsync(count);

            return Ok(result);
        }
    }
}