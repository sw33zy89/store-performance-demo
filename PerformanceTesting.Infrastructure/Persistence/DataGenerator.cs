using PerformanceTesting.Core;
using System;

namespace PerformanceTesting.Infrastructure.Persistence
{
    public class DataGenerator
    {
        public static Store GenerateRandomStore(Address address)
        {
            Random random = new Random();

            List<string> storeNames = new List<string> { "Ace", "Walmart", "Meijer", "Home Depot" };
            string name = storeNames.ElementAt(random.Next(storeNames.Count - 1));

            return Store.Create(name, address).Value;
        }
        public static Customer GenerateRandomCustomer()
        {
            Random random = new Random();
            List<string> firstNames = new List<string> { "John", "Jacob", "Eric", "Jack", "Jamie", "Jenny", "George" };
            string firstName = firstNames.ElementAt(random.Next(firstNames.Count - 1));

            List<string> lastNames = new List<string> { "Smith", "Miller", "Sawyer", "McCarthy", "McClain" };
            string lastName = lastNames.ElementAt(random.Next(lastNames.Count - 1));

            PersonName personName = PersonName.Create(firstName, lastName).Value;
            Customer customer = Customer.Create(personName).Value;

            return customer;
        }

        public static Address GenerateRandomAddress()
        {
            Random random = new Random();
            List<string> streets = new List<string> { "Main", "Woodward", "Gratiot", "Old Mill", "Elm", "Oak Park", "Cadieux", "Lakeshore" };
            string street = streets.ElementAt(random.Next(streets.Count - 1));

            List<string> address2Prefixes = new List<string> { null, "PO Box ", "Apt. " };
            string? address2 = address2Prefixes.ElementAt(random.Next(address2Prefixes.Count - 1));
            if (address2 != null)
            {
                address2 += random.Next(9999).ToString();
            }

            List<string> cities = new List<string> { "Chicago", "Detroit", "Atlanta", "New York", "Los Angeles" };
            string city = cities.ElementAt(random.Next(cities.Count - 1));

            List<string> states = new List<string> { "MI", "IL", "GA", "NY", "CA" };
            string state = states.ElementAt(random.Next(states.Count - 1));

            double latitude = ((double)random.Next(26, 48)) + random.NextDouble();
            double longitude = ((double)random.Next(100, 123)) + random.NextDouble();
            GeoCoordinates coords = GeoCoordinates.Create(latitude, longitude).Value;

            return Address.Create(street, address2, city, state, "USA", coords).Value;
        }

        public static List<StoreVisit> GenerateVisits(Customer customer, List<Store> stores)
        {
            List<StoreVisit> visits = new List<StoreVisit>();

            int storeCount = stores.Count;
            Random random = new Random();
            int numVisits = random.Next(1, 10);
            for (int v = 0; v < numVisits; v++)
            {
                Store store = stores[random.Next(storeCount - 1)];
                DateTime visitTime = DateTime.Now
                    .AddDays(random.Next(180))
                    .AddHours(random.Next(24))
                    .AddMinutes(random.Next(60))
                    .AddSeconds(random.Next(60))
                ;
                visits.Add(customer.VisitStore(store, visitTime).Value);
            }

            return visits;
        }
    }
}
