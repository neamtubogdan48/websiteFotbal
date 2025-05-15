using mvc.Models;
using mvc.Data;
using Microsoft.EntityFrameworkCore;

namespace mvc.IRepository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _context;
        public PlayerRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Player>> GetAllAsync() => await _context.Player.ToListAsync();
        public async Task<Player?> GetByIdAsync(int id)
        {
            return await _context.Player.AsNoTracking().FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task AddAsync(Player player)
        {
            _context.Player.Add(player);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Player player)
        {
            _context.Player.Update(player);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Player player)
        {
            _context.Player.Remove(player);
            await _context.SaveChangesAsync();
        }
    }
}
