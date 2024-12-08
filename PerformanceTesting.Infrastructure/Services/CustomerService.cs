using Microsoft.EntityFrameworkCore;
using PerformanceTesting.Core;
using PerformanceTesting.Core.Services;
using PerformanceTesting.Infrastructure.Persistence;

namespace PerformanceTesting.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        public CustomerService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Customer? GetCustomer(int id, bool useSplitQuery = false)
        {
            using (var Db = _dbContextFactory.CreateDbContext())
            {
                return Db.Customers
                    .Where(c => c.Id == id)
                    .IncludeDependencies(useSplitQuery)
                    .AsNoTracking()
                    .FirstOrDefault();
            }
        }

        public async Task<Customer?> GetCustomerAsync(int id, bool useSplitQuery = false, CancellationToken cancellationToken = default)
        {
            using (var Db = await _dbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                return await Db.Customers
                    .Where(c => c.Id == id)
                    .IncludeDependencies(useSplitQuery)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }

        public List<Customer> GetCustomers(List<int> ids, bool useSplitQuery = false)
        {
            using (var Db = _dbContextFactory.CreateDbContext())
            {
                return Db.Customers
                    .Where(c => ids.Contains(c.Id))
                    .IncludeDependencies(useSplitQuery)
                    .AsNoTracking()
                    .ToList();
            }
        }

        public async Task<List<Customer>> GetCustomersAsync(List<int> ids, bool useSplitQuery = false, CancellationToken cancellationToken = default)
        {
            using (var Db = await _dbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                return await Db.Customers
                    .Where(c => ids.Contains(c.Id))
                    .IncludeDependencies(useSplitQuery)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
        }

        public Customer Insert(Customer customer)
        {
            using (var Db = _dbContextFactory.CreateDbContext())
            {
                Db.Customers.Add(customer);
                Db.SaveChanges();

                return customer;
            }
        }

        public async Task<Customer> InsertAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            using (var Db = await _dbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                await Db.Customers.AddAsync(customer);
                await Db.SaveChangesAsync(cancellationToken);

                return customer;
            }
        }
    }

    public static partial class IQueryableExtensions
    {
        public static IQueryable<Customer> IncludeDependencies(this IQueryable<Customer> query, bool useSplitQuery)
        {
            query = query
                .Include(c => c.Addresses)
                .Include(c => c.StoreVisits)
                .ThenInclude(v => v.Store)
                .ThenInclude(s => s.Address);

            if (useSplitQuery)
                query = query.AsSplitQuery();

            return query;
        }
    }
}
