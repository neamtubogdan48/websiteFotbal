using mvc.Models;
using mvc.Data;
using Microsoft.EntityFrameworkCore;

namespace mvc.IRepository
{
    public class SponsorRepository : ISponsorRepository
    {
        private readonly AppDbContext _context;
        public SponsorRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Sponsor>> GetAllAsync() =>
            await _context.Sponsor.ToListAsync();

        public async Task<Sponsor?> GetByIdAsync(int id) =>
            await _context.Sponsor.AsNoTracking().FirstOrDefaultAsync(s => s.id == id);

        public async Task AddAsync(Sponsor sponsor)
        {
            _context.Sponsor.Add(sponsor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sponsor sponsor)
        {
            _context.Sponsor.Update(sponsor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Sponsor sponsor)
        {
            _context.Sponsor.Remove(sponsor);
            await _context.SaveChangesAsync();
        }
    }
}
