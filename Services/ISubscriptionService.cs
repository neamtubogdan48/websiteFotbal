using mvc.Models;

namespace mvc.Services
{
    public interface ISubscriptionService
    {
        Task<Subscription?> GetSubscriptionByIdAsync(int id);
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
        Task AddSubscriptionAsync(Subscription subscription);
        Task UpdateSubscriptionAsync(Subscription subscription);
        Task DeleteSubscriptionAsync(int id);
    }
}
