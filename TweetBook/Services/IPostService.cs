using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    {
        public Task<List<Post>> GetPostsAsync();
        public Task<Post?> GetPostByIdAsync(Guid postId);
        public Task<bool> CreatePostAsync(Post post);
        public Task<bool> UpdatePostAsync(Post postToUpdate);
        public Task<bool> DeletePostAsync(Guid postId);
        public Task<bool> UserOwnsPostAsync(Guid postId, string userId);

        public Task<bool> CreateTagAsync(Tag tag);
        public Task<Tag?> GetTagByNameAsync(string tagName);
        public Task<List<Tag>> GetAllTagsAsync();
        public Task<bool> DeleteTagAsync(string tagName);
    }
}
