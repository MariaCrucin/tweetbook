using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IFlowerService
    {
        public Task<bool> CreateFlowerAsync(Flower flower);
        public Task<List<Flower>> GetFlowersAsync();
        public Task<Flower?> GetFlowerByIdAsync(Guid flowerId);
        public Task<bool> DeleteFlowerAsync(Guid flowerId);
    }
}
