using mvc.data.models;
using System.Collections.Generic;
using System.Threading.Tasks;
using mvc.Models;

namespace mvc.IRepository
{
    public interface INewsRepository
    {
        Task<IEnumerable<News>> GetAllAsync();
        Task<News?> GetByIdAsync(int id);
        Task AddAsync(News news);
        Task UpdateAsync(News news);
        Task DeleteAsync(int id);
    }
}
