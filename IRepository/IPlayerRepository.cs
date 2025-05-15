using mvc.Models;

namespace mvc.IRepository
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(int id);
        Task AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
    }
}
