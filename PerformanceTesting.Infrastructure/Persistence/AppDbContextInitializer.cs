using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PerformanceTesting.Core;

namespace PerformanceTesting.Infrastructure.Persistence
{
    public class AppDbContextInitializer
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppDbContextInitializer> _logger;

        public AppDbContextInitializer(AppDbContext context, ILogger<AppDbContextInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            Random random = new Random();
            if (! await _context.Stores.AnyAsync())
            {
                int storeCount = 10000;
                int numCustomers = 10000;
                List<Store> stores = new List<Store>();
                for (int i = 0; i < storeCount; i++)
                {
                    Address storeAddress = DataGenerator.GenerateRandomAddress();
                    _context.Addresses.Add(storeAddress);
                    await _context.SaveChangesAsync();

                    Store store = DataGenerator.GenerateRandomStore(storeAddress);
                    _context.Stores.Add(store);
                    await _context.SaveChangesAsync();

                    stores.Add(store);
                }
                for (int i = 0; i < numCustomers; i++)
                {
                    Address address = DataGenerator.GenerateRandomAddress();
                    _context.Addresses.Add(address);
                    await _context.SaveChangesAsync();

                    Customer customer = DataGenerator.GenerateRandomCustomer();
                    customer.AddAddress(address);
                    _context.Customers.Add(customer);

                    await _context.SaveChangesAsync();

                    List<StoreVisit> visits = DataGenerator.GenerateVisits(customer, stores);
                    await _context.SaveChangesAsync();
                }
            }
        }   
    }
}
