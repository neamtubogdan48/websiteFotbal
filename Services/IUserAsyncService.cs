using mvc.Models;

namespace mvc.Services
{
    public interface IUserAsyncService
    {
        Task<Users?> GetUserByIdAsync(string id);
        Task UpdateUserAccountTypeAsync(string userId, string accountType);
        Task<Users?> GetUserWithSubscriptionAsync(string userId); // Add this method
    }

}
