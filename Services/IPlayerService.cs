using mvc.Models;

namespace mvc.Services
{
    public interface IPlayerService
    {
        Task<IEnumerable<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(int id);
        Task AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
    }
}
