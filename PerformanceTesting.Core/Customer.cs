using CSharpFunctionalExtensions;

namespace PerformanceTesting.Core
{
    public class Customer
    {
        public int Id { get; private set; }
        public PersonName Name { get; private set; }
        public IReadOnlyList<Address> Addresses => _addresses;
        private List<Address> _addresses = new List<Address>();
        public IReadOnlyList<StoreVisit> StoreVisits => _storeVisits;
        private List<StoreVisit> _storeVisits = new List<StoreVisit>();

        private Customer() { }

        private Customer(int id, PersonName name, List<Address> addresses) {
            Id = id;
            Name = name;
            _addresses = addresses;
        }

        public static Result<Customer> Create(PersonName name) {
            return Result.Success<Customer>(
                new Customer(default, name, new List<Address>())
            );
        }

        public Result AddAddress(Address address)
        {
            if (!_addresses.Any(a => a.Id == address.Id))
            {
                _addresses.Add(address);
            } 

            return Result.Success();
        }

        public Result RemoveAddress(Address address) { 
            _addresses.Remove(address);

            return Result.Success();
        }

        public Result<StoreVisit> VisitStore(Store store, DateTime visitDateTime)
        {
            Result<StoreVisit> result = StoreVisit.Create(visitDateTime, this, store);
            return result;
        }
        internal Result VisitStore(StoreVisit visit)
        {
            if (visit.Id == 0 || (visit.Id > 0 && !_storeVisits.Any(v => v.Id == visit.Id)))
            {
                _storeVisits.Add(visit);
            }
            return Result.Success();
        }

        public Result RemoveStoreVisit(StoreVisit visit)
        {
            if (visit.Id > 0)
                _storeVisits.RemoveAll(v => v.Id == visit.Id);

            return Result.Success();
        }
    }
}
