using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FlowersController : ControllerBase
    {
        private readonly IFlowerService _flowerService;

        public FlowersController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        [Authorize(Policy = "FlowerViewer")]
        [HttpGet(ApiRoutes.Flowers.GetAll)]
        public async Task<IActionResult> GetAll()
        { 
            return Ok(await _flowerService.GetFlowersAsync());
        }

        [HttpPost(ApiRoutes.Flowers.Create)]
        public async Task<IActionResult> Create([FromBody] CreateFlowerRequest flowerRequest)
        {
            var flower = new Flower
            {
                Name = flowerRequest.Name
            };

            var created = await _flowerService.CreateFlowerAsync(flower);

            if (!created)
                return BadRequest(new { error = "Not saved"});

            return NoContent();
        }
    }
}
