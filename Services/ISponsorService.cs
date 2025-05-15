using mvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvc.Services
{
    public interface ISponsorService
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int id);
        Task AddAsync(Sponsor sponsor);
        Task UpdateAsync(Sponsor sponsor);
        Task DeleteAsync(Sponsor sponsor);
    }
}
