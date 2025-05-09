using mvc.Models;

namespace mvc.IRepository
{
    public interface IUserAsyncRepository
    {
        Task<Users?> GetUserByIdAsync(string id);
        Task UpdateUserAsync(Users user);
    }
}
