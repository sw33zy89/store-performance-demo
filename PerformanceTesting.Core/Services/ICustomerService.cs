using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTesting.Core.Services
{
    public interface ICustomerService
    {
        public Customer? GetCustomer(int id, bool useSplitQuery = false);
        public Task<Customer?> GetCustomerAsync(int id, bool useSplitQuery = false, CancellationToken cancellationToken = default);
        public List<Customer> GetCustomers(List<int> ids, bool useSplitQuery = false);
        public Task<List<Customer>> GetCustomersAsync(List<int> ids, bool useSplitQuery = false, CancellationToken cancellationToken = default);

        public Customer Insert(Customer customer);
        public Task<Customer> InsertAsync(Customer customer, CancellationToken cancellationToken = default);
    }
}
