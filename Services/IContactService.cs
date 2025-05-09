using mvc.Models;

namespace mvc.Services
{
    public interface IContactService
    {
        Task AddContactAsync(string userId, string message);
        Task<IEnumerable<Contact>> GetUserContactsAsync(string userId);
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int id);
        Task UpdateContactAsync(Contact contact);
        Task DeleteAsync(int id);
    }
}
