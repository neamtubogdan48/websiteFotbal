using Microsoft.EntityFrameworkCore;
using mvc.IRepository;
using mvc.Models;

namespace mvc.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task AddContactAsync(string userId, string message)
        {
            var contact = new Contact
            {
                userId = userId,
                message = message
            };
            await _contactRepository.AddContactAsync(contact);
        }

        public async Task<IEnumerable<Contact>> GetUserContactsAsync(string userId)
        {
            return await _contactRepository.GetContactsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _contactRepository.GetAllAsync();
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            return await _contactRepository.GetByIdAsync(id);
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            await _contactRepository.UpdateAsync(contact);
        }

        public async Task DeleteAsync(int id)
        {
            await _contactRepository.DeleteAsync(id);
        }
    }

}
