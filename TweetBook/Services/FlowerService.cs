using Microsoft.EntityFrameworkCore;
using TweetBook.Data;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class FlowerService : IFlowerService
    {
        private readonly DataContext _dataContext;

        public FlowerService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateFlowerAsync(Flower flower)
        {
            await _dataContext.Flowers.AddAsync(flower);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<List<Flower>> GetFlowersAsync()
        {
            return await _dataContext.Flowers.ToListAsync();
        }
    }
}
