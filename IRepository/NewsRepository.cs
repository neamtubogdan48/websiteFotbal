using mvc.Models;
using mvc.Data;
using Microsoft.EntityFrameworkCore;

namespace mvc.IRepository
{
    public class NewsRepository : INewsRepository
    {
        private readonly AppDbContext _context;

        public NewsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _context.News.ToListAsync();
        }

        public async Task<News?> GetByIdAsync(int id)
        {
            return await _context.News.FindAsync(id);
        }

        public async Task AddAsync(News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(News news)
        {
            var existingNews = await _context.News.FindAsync(news.id);
            if (existingNews == null)
            {
                throw new Exception("News not found.");
            }

            // Update only the necessary fields
            existingNews.title = news.title;
            existingNews.description = news.description;
            existingNews.publishDate = news.publishDate;
            existingNews.photo = news.photo;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }
        }
    }
}
