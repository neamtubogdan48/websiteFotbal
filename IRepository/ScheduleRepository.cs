using mvc.Models;
using mvc.Data;
using Microsoft.EntityFrameworkCore;

namespace mvc.IRepository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly AppDbContext _context;
        public ScheduleRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Schedule>> GetAllAsync() =>
            await _context.Schedule.ToListAsync();

        public async Task<Schedule?> GetByIdAsync(int id) =>
            await _context.Schedule.AsNoTracking().FirstOrDefaultAsync(s => s.id == id);

        public async Task AddAsync(Schedule schedule)
        {
            _context.Schedule.Add(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Schedule schedule)
        {
            _context.Schedule.Update(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Schedule schedule)
        {
            _context.Schedule.Remove(schedule);
            await _context.SaveChangesAsync();
        }
    }
}
