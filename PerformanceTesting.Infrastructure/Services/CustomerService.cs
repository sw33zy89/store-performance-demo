using Microsoft.EntityFrameworkCore;
using PerformanceTesting.Core;
using PerformanceTesting.Core.Services;
using PerformanceTesting.Infrastructure.Persistence;

namespace PerformanceTesting.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext Db;
        public CustomerService(AppDbContext dbContext)
        {
            Db = dbContext;
        }

        public Customer? GetCustomer(int id, bool useSplitQuery = false)
        {
            return Db.Customers
                .Where(c => c.Id == id)
                .IncludeDependencies(useSplitQuery)
                .FirstOrDefault();
        }

        public async Task<Customer?> GetCustomerAsync(int id, bool useSplitQuery = false, CancellationToken cancellationToken = default)
        {
            return await Db.Customers
                .Where(c => c.Id == id)
                .IncludeDependencies(useSplitQuery)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public List<Customer> GetCustomers(List<int> ids, bool useSplitQuery = false)
        {
            return Db.Customers
                .Where(c => ids.Contains(c.Id))
                .IncludeDependencies(useSplitQuery)
                .ToList();
        }

        public async Task<List<Customer>> GetCustomersAsync(List<int> ids, bool useSplitQuery = false, CancellationToken cancellationToken = default)
        {
            return await Db.Customers
                .Where(c => ids.Contains(c.Id))
                .IncludeDependencies(useSplitQuery)
                .ToListAsync(cancellationToken);
        }

        public Customer Insert(Customer customer)
        {
            Db.Customers.Add(customer);
            Db.SaveChanges();

            return customer;
        }

        public async Task<Customer> InsertAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            await Db.Customers.AddAsync(customer);
            await Db.SaveChangesAsync(cancellationToken);

            return customer;
        }
    }

    public static partial class IQueryableExtensions
    {
        public static IQueryable<Customer> IncludeDependencies(this IQueryable<Customer> query, bool useSplitQuery)
        {
            if (useSplitQuery)
                query = query.AsSplitQuery();

            query = query
                .Include(c => c.Addresses)
                .Include(c => c.StoreVisits)
                .ThenInclude(c => c.Store);

            return query;
        }
    }
}
