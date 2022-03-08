﻿using Microsoft.EntityFrameworkCore;
using TweetBook.Data;
using TweetBook.Domain;


namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Post>> GetPostsAsync(GetAllPostsFilter? filter = null, PaginationFilter? paginationFilter = null)
        {
            var queryable = _dataContext.Posts.AsQueryable();

            queryable = AddFiltersOnQuery(filter, queryable);

            if (paginationFilter == null)
                return await queryable.Include(p => p.Tags).ToListAsync();

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Include(p => p.Tags).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.Include(p => p.Tags).SingleOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<bool> CreatePostAsync(Post post)
        { 
            post.Tags?.ForEach(t => t.TagName = t.TagName.ToLower());
            await AddNewTags(post);

            await _dataContext.Posts.AddAsync(post);

            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            postToUpdate.Tags?.ForEach(t => t.TagName = t.TagName.ToLower());
            await AddNewTags(postToUpdate);

            _dataContext.Posts.Update(postToUpdate);
            
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if (post == null)
                return false;

            _dataContext.Posts.Remove(post);

            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0; 
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(post => post.Id == postId);

            if (post == null)
                return false;

            if (post.UserId != userId)
                return false;

            return true;
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            tag.Name = tag.Name.ToLower();

            var existingTag = await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(t => t.Name == tag.Name);
            if (existingTag != null)
                return true;

            await _dataContext.Tags.AddAsync(tag);  
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<Tag?> GetTagByNameAsync(string tagName)
        {
            return await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(tag => tag.Name == tagName.ToLower());
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _dataContext.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteTagAsync(string tagName)
        {
            var tag = await GetTagByNameAsync(tagName);

            if (tag == null)
                return false;

            var postTags = await _dataContext.PostTags.Where(postTag => postTag.TagName == tagName.ToLower()).ToListAsync();
            _dataContext.PostTags.RemoveRange(postTags);

            _dataContext.Tags.Remove(tag);

            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        private async Task AddNewTags(Post post)
        {
            foreach (var tag in post.Tags)
            { 
                var existingTag = await _dataContext.Tags.SingleOrDefaultAsync(t => t.Name == tag.TagName);

                if (existingTag != null)
                    continue;

                await _dataContext.Tags.AddAsync(new Tag
                {
                    Name = tag.TagName,
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = post.UserId
                });
            }
        }

        private static IQueryable<Post> AddFiltersOnQuery(GetAllPostsFilter? filter, IQueryable<Post> queryable)
        {
            if (!string.IsNullOrEmpty(filter?.UserId))
                queryable = queryable.Where(p => p.UserId == filter.UserId);
            return queryable;
        }
    }
}
