using Microsoft.AspNetCore.Mvc;
using Quantc.StoriesWebAPI.Model;

namespace Quantc.StoriesWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly ILogger<StoryController> _logger;

        public StoryController(ILogger<StoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Get Best Stories")]
        public ActionResult<IEnumerable<StoryModel>> Get()
        {
            var model = new StoryModel();

            return Ok(new StoryModel[] { model });
        }
    }
}