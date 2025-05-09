using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.Data;

namespace mvc.IRepository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;

        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            await _context.Subscription.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _context.Subscription.ToListAsync();
        }

        public async Task<Subscription?> GetSubscriptionByIdAsync(int id)
        {
            return await _context.Subscription.FindAsync(id);
        }

        public async Task UpdateSubscriptionAsync(Subscription subscription)
        {
            _context.Subscription.Update(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubscriptionAsync(int id)
        {
            var subscription = await _context.Subscription.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscription.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }
    }

}
