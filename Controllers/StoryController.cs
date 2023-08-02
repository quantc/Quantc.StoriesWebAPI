using Microsoft.AspNetCore.Mvc;
using Quantc.StoriesWebAPI.Model;
using Quantc.StoriesWebAPI.Services;

namespace Quantc.StoriesWebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly StoryService _storyService;

        public StoryController(StoryService storyService)
        {
            _storyService = storyService;
        }

        [MapToApiVersion("1")]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any,
            VaryByQueryKeys = new string[] { "count" })]
        [HttpGet]
        [Route("best")]
        public async Task<ActionResult<IEnumerable<StoryModel>>> Get(int count)
        {
            var result = await _storyService.GetBestStoriesAsync(count);

            return Ok(result);
        }

        [MapToApiVersion("2")]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any,
            VaryByQueryKeys = new string[] { "count" })]
        [HttpGet]
        [Route("best")]
        public async Task<ActionResult<IEnumerable<StoryModel>>> GetV2(int count = 3)
        {
            var result = await _storyService.GetBestStoriesAsyncV2(count);

            return Ok(result);
        }
    }
}