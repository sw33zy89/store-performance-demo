using CSharpFunctionalExtensions;

namespace PerformanceTesting.Core
{
    public class StoreVisit
    {
        public int Id { get; set; }
        public DateTime VisitDateTime { get; private set; }
        public int CustomerId { get; private set; }
        public Customer Customer { get; private set; }
        public int StoreId { get; private set; }
        public Store Store { get; private set; }

        private StoreVisit() { }

        private StoreVisit(int id, DateTime visitDateTime, Customer customer, Store store)
        {
            Id = id;
            VisitDateTime = visitDateTime;
            Customer = customer;
            CustomerId = customer.Id;
            Store = store;
            StoreId = store.Id;
        }

        public static Result<StoreVisit> Create(DateTime visitDateTime, Customer customer, Store store)
        {
            StoreVisit visit = new StoreVisit(default, visitDateTime, customer, store);
            
            Result result = customer.VisitStore(visit);
            if (result.IsFailure)
                return Result.Failure<StoreVisit>(result.Error);

            result = store.RecordCustomerVisit(visit);
            if (result.IsFailure)
                return Result.Failure<StoreVisit>(result.Error);

            return Result.Success(visit);
        }
    }
}
