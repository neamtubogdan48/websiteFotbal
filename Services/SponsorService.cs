using mvc.Models;
using mvc.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvc.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorService;
        public SponsorService(ISponsorRepository sponsorService) => _sponsorService = sponsorService;

        public Task<IEnumerable<Sponsor>> GetAllAsync() => _sponsorService.GetAllAsync();
        public Task<Sponsor?> GetByIdAsync(int id) => _sponsorService.GetByIdAsync(id);
        public Task AddAsync(Sponsor sponsor) => _sponsorService.AddAsync(sponsor);
        public Task UpdateAsync(Sponsor sponsor) => _sponsorService.UpdateAsync(sponsor);
        public Task DeleteAsync(Sponsor sponsor) => _sponsorService.DeleteAsync(sponsor);
    }
}
