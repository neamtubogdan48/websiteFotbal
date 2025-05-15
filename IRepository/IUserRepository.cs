// IRepository/IUserRepository.cs
using Microsoft.AspNetCore.Identity;
using mvc.Models;

namespace mvc.IRepository;

public interface IUserRepository
{
    Task<IEnumerable<Users>> GetAllUsersAsync();
    Task<Users?> GetUserByEmailAsync(string normalizedEmail);
    Task<Users?> GetUserByIdAsync(string id);
    Task AddUserAsync(Users user);
    Task<bool> UserExistsAsync(string id);
    Task UpdateUserAsync(Users user);
    Task DeleteUserAsync(Users user);
    Task AddUserRoleAsync(IdentityUserRole<string> userRole);
    Task UpdateUserRoleAsync(Users user, string newRole);
    Task AddUserRoleAsync(string userId, string roleId);
    Task<Users?> GetUserWithSubscriptionAsync(string userId);
}

