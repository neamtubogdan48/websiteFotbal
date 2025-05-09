using Microsoft.EntityFrameworkCore;
using mvc.Data;
using mvc.Models;

namespace mvc.IRepository
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;

        public ContactRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddContactAsync(Contact contact)
        {
            _context.Contact.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contact>> GetContactsByUserIdAsync(string userId)
        {
            return await _context.Contact.Where(c => c.userId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _context.Contact.ToListAsync();
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            return await _context.Contact.FindAsync(id);
        }

        public async Task UpdateAsync(Contact contact)
        {
            _context.Contact.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }
    }

}
