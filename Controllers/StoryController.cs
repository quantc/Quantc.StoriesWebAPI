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

        [HttpGet(Name = "Get Best Stories")]
        public async Task<ActionResult<IEnumerable<StoryModel>>> Get(int count)
        {
            var result = await _storyService.GetBestStoriesAsync(count);

            return Ok(result);
        }
    }
}