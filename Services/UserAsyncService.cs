using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.Data;
using mvc.IRepository;

namespace mvc.Services
{
    public class UserAsyncService : IUserAsyncService
    {
        private readonly IUserAsyncRepository _userRepository;
        private readonly AppDbContext _context;

        public UserAsyncService(IUserAsyncRepository userRepository, AppDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<Users?> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task UpdateUserAccountTypeAsync(string userId, string accountType)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                user.accountType = accountType;
                await _userRepository.UpdateUserAsync(user);
            }
        }

        public async Task<Users?> GetUserWithSubscriptionAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.subscription) // Include the subscription details
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.subscription == null)
            {
                user.subscription = new Subscription
                {
                    type = "N/A",
                    expireDate = DateTime.MinValue // Default values for no subscription
                };
            }

            return user;
        }
    }
}
