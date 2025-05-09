using mvc.Data;
using mvc.Models;

namespace mvc.IRepository
{
    public class UserAsyncRepository : IUserAsyncRepository
    {
        private readonly AppDbContext _context;

        public UserAsyncRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateUserAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

}
