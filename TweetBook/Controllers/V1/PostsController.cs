﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Domain;

namespace TweetBook.Controllers.V1
{ 
    [ApiController]
    public class PostsController : ControllerBase
    {
        private List<Post>? _posts;

        public PostsController()
        {
            _posts = new List<Post>();
            for (int i = 0; i < 5; i++)
            {
                _posts.Add(new Post { Id = Guid.NewGuid().ToString() });
            }
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_posts);
        }

    }
}