using mvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvc.Services
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetAllNewsAsync();
        Task<News?> GetNewsByIdAsync(int id);
        Task AddNewsAsync(News news);
        Task UpdateNewsAsync(News news);
        Task DeleteNewsAsync(int id);
    }
}
