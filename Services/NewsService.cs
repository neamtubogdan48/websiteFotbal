using mvc.Models;
using mvc.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mvc.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<News>> GetAllNewsAsync()
        {
            return await _newsRepository.GetAllAsync();
        }

        public async Task<News?> GetNewsByIdAsync(int id)
        {
            return await _newsRepository.GetByIdAsync(id);
        }

        public async Task AddNewsAsync(News news)
        {
            await _newsRepository.AddAsync(news);
        }

        public async Task UpdateNewsAsync(News news)
        {
            await _newsRepository.UpdateAsync(news);
        }

        public async Task DeleteNewsAsync(int id)
        {
            await _newsRepository.DeleteAsync(id);
        }
    }
}
