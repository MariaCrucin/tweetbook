using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Extensions;
using TweetBook.Services;

namespace TweetBook.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : ControllerBase
    { 
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public TagsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.TagName,
                CreatorId = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var created = await _postService.CreateTagAsync(newTag);
            if (!created)
                return BadRequest(new { error = "Unable to create tag" });

            var baseUrl = $"{HttpContext.Request.Scheme}: {HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagName}", newTag.Name);
            return Created(locationUri, _mapper.Map<TagResponse>(newTag));
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            var tag = await _postService.GetTagByNameAsync(name);

            if (tag == null)
                return NotFound();

            return Ok(_mapper.Map<TagResponse>(tag));
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _postService.GetAllTagsAsync();

            return Ok(_mapper.Map<List<TagResponse>>(tags));
        }

        [HttpDelete(ApiRoutes.Tags.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string tagName)
        { 
            var deleted = await _postService.DeleteTagAsync(tagName);

            if (deleted)
                return NoContent();

            return NotFound();
        }

    }
}
