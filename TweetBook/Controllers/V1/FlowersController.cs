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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Poster")]
    public class FlowersController : ControllerBase
    {
        private readonly IFlowerService _flowerService;

        public FlowersController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        [HttpGet(ApiRoutes.Flowers.GetAll)]
        public async Task<IActionResult> GetAll()
        { 
            return Ok(await _flowerService.GetFlowersAsync());
        }

        [HttpPost(ApiRoutes.Flowers.Create)]
        public async Task<IActionResult> Create([FromBody] UpsertFlowerRequest flowerRequest)
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

        [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Flowers.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid flowerId)
        {
            var deleted = await _flowerService.DeleteFlowerAsync(flowerId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
