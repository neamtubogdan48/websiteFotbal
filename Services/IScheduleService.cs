using mvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvc.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task<Schedule?> GetByIdAsync(int id);
        Task AddAsync(Schedule schedule);
        Task UpdateAsync(Schedule schedule);
        Task DeleteAsync(Schedule schedule);
    }
}
