using CSharpFunctionalExtensions;

namespace PerformanceTesting.Core
{
    public class Store
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Address Address { get; private set; }

        public IReadOnlyList<StoreVisit> CustomerVisits => _customerVisits;
        private List<StoreVisit> _customerVisits = new List<StoreVisit>();

        private Store() { }
        
        private Store(int id, string name, Address address)
        {
            Id = id;
            Name = name;
            Address = address;
        }

        public static Result<Store> Create(string name, Address address)
        {
            return Result.Success<Store>(
                new Store(default, name, address)
            );
        }

        public Result RecordCustomerVisit(Customer customer, DateTime visitDateTime)
        {
            Result<StoreVisit> result = StoreVisit.Create(visitDateTime, customer, this);
            return result;
        }
        internal Result RecordCustomerVisit(StoreVisit visit)
        {
            if (visit.Id == 0 || (visit.Id > 0 && !_customerVisits.Any(v => v.Id == visit.Id)))
            {
                _customerVisits.Add(visit);
            }
            return Result.Success();
        }

        public Result RemoveStoreVisit(StoreVisit visit)
        {
            if (visit.Id > 0)
                _customerVisits.RemoveAll(v => v.Id == visit.Id);

            return Result.Success();
        }
    }
}
