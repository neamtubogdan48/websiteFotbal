using mvc.Models;

namespace mvc.IRepository
{
    public interface IContactRepository
    {
        Task AddContactAsync(Contact contact);
        Task<IEnumerable<Contact>> GetContactsByUserIdAsync(string userId);
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact> GetByIdAsync(int id);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(int id);
    }

}
