using mvc.Models;
using mvc.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvc.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository) => _scheduleRepository = scheduleRepository;

        public Task<IEnumerable<Schedule>> GetAllAsync() => _scheduleRepository.GetAllAsync();
        public Task<Schedule?> GetByIdAsync(int id) => _scheduleRepository.GetByIdAsync(id);
        public Task AddAsync(Schedule schedule) => _scheduleRepository.AddAsync(schedule);
        public Task UpdateAsync(Schedule schedule) => _scheduleRepository.UpdateAsync(schedule);
        public Task DeleteAsync(Schedule schedule) => _scheduleRepository.DeleteAsync(schedule);
    }
}
