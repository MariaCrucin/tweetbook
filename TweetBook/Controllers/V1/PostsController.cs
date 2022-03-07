using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Cache;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Requests.Queries;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Extensions;
using TweetBook.Helpers;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PostsController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// Returns all posts for an user, including their tags
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="paginationQuery">Page number and page size</param>
        /// <response code="200">Returns posts</response>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        [ProducesResponseType(typeof(PagedResponse<PostResponse>), 200)]
        public async Task<IActionResult> GetAll([FromQuery]GetAllPostsQuery? query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllPostsFilter>(query);
            var posts = await _postService.GetPostsAsync(filter, pagination);
            var postsResponse = _mapper.Map<List<PostResponse>>(posts);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
                return Ok(new PagedResponse<PostResponse>(postsResponse));

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, postsResponse);
            return Ok(paginationResponse);
        }

        /// <summary>
        /// Returns a post 
        /// </summary>
        /// <param name="postId">The Id of the post</param>
        /// <response code="200">Returns the post</response>
        /// <response code="404">Post was not found</response>
        [HttpGet(ApiRoutes.Posts.Get)]
        [ProducesResponseType(typeof(PostResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [Cached(600)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();

            return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        /// <summary>
        /// Creates a post
        /// </summary>
        /// <param name="postRequest">Post to be created</param>
        /// <response code="201">The uri of created post and the post</response>
        /// <response code="400">Unable to create the post due to validation error</response>
        [HttpPost(ApiRoutes.Posts.Create)]
        [ProducesResponseType(typeof(PostResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var newPostId = Guid.NewGuid();
            var post = new Post 
            { 
                Id = newPostId,
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId(),
                Tags = postRequest.Tags.Select(t => new PostTag { PostId = newPostId, TagName = t }).ToList()
            };

            var created = await _postService.CreatePostAsync(post);
            if (!created)
                return BadRequest(new { error = "Unable to create post" });

            var locationUri = _uriService.GetPostUri(post.Id.ToString());
                 
            return Created(locationUri, new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        /// <summary>
        /// Updates a post
        /// </summary>
        /// <param name="postId">The Id of the post</param>
        /// <param name="request">The post</param>
        /// <response code="201">The post modified</response>
        /// <response code="400">Unable to create the post due to validation error</response>
        /// <response code="404">Post was not found</response>
        /// <returns></returns>
        [HttpPut(ApiRoutes.Posts.Update)]
        [ProducesResponseType(typeof(PostResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new {error = "You do not own this post"}); 

            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();

            post.Name = request.Name;

            var updated = await _postService.UpdatePostAsync(post);

            if (updated)
                return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));

            return NotFound();
        }

        /// <summary>
        /// Delete a post
        /// </summary>
        /// <param name="postId">The Id of the post</param>
        /// <response code="204">Post was deleted</response>
        /// <response code="404">Post was not found</response>
        [HttpDelete(ApiRoutes.Posts.Delete)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { error = "You do not own this post" });

            var deleted = await _postService.DeletePostAsync(postId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

    }
}
