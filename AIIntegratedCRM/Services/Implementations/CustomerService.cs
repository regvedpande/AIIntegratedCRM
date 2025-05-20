using AIIntegratedCRM.Models.Entities;
using AIIntegratedCRM.Repositories.Interfaces;
using AIIntegratedCRM.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIIntegratedCRM.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            // Business rule: always set CreatedAt to UTC now on creation
            customer.CreatedAt = System.DateTime.UtcNow;
            await _repo.AddAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _repo.UpdateAsync(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            return await _repo.ExistsAsync(id);
        }
    }
}
