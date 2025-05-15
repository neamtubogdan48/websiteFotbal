using mvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvc.IRepository
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task<Schedule?> GetByIdAsync(int id);
        Task AddAsync(Schedule schedule);
        Task UpdateAsync(Schedule schedule);
        Task DeleteAsync(Schedule schedule);
    }
}
