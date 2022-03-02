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

        public async Task<Flower?> GetFlowerByIdAsync(Guid flowerId)
        {
            return await _dataContext.Flowers.SingleOrDefaultAsync(f => f.Id == flowerId);
        }

        public async Task<bool> DeleteFlowerAsync(Guid flowerId)
        {
            var flower = await GetFlowerByIdAsync(flowerId);

            if (flower == null)
                return false;

            _dataContext.Flowers.Remove(flower);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;

        }

    }
}

