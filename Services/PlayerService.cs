using mvc.Models;
using mvc.IRepository;

namespace mvc.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        public PlayerService(IPlayerRepository playerRepository) => _playerRepository = playerRepository;

        public Task<IEnumerable<Player>> GetAllAsync() => _playerRepository.GetAllAsync();
        public Task<Player?> GetByIdAsync(int id) => _playerRepository.GetByIdAsync(id);
        public Task AddAsync(Player player) => _playerRepository.AddAsync(player);
        public Task UpdateAsync(Player player) => _playerRepository.UpdateAsync(player);
        public Task DeleteAsync(Player player) => _playerRepository.DeleteAsync(player);
    }
}
