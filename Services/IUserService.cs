// Services/IUserService.cs
using mvc.Models;

namespace mvc.Services;

public interface IUserService
{
    Task<IEnumerable<Users>> GetAllUsersAsync();
    Task<Users?> GetUserByIdAsync(string id);
    Task AddUserAsync(Users user, string role);
    Task UpdateUserAsync(Users user);
    Task DeleteUserAsync(Users user);
}
