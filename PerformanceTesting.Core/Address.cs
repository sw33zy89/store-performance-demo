using CSharpFunctionalExtensions;

namespace PerformanceTesting.Core
{
    public class Address
    {
        public int Id { get; private set; }
        public string Address1 { get; private set; }
        public string? Address2 { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public GeoCoordinates Coordinates { get; private set; }

        private Address() { }
        private Address(int id, string address1, string? address2, string city, string state, string country, GeoCoordinates coordinates)
        {
            Id = id;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Country = country;
            Coordinates = coordinates;
        }

        public static Result<Address> Create(string address1, string? address2, string city, string state, string country, GeoCoordinates coordinates)
        {
            if (string.IsNullOrWhiteSpace(address1))
                return Result.Failure<Address>("Address1 required");

            if (string.IsNullOrWhiteSpace(city))
                return Result.Failure<Address>("City required");

            if (string.IsNullOrWhiteSpace(state))
                return Result.Failure<Address>("State required");

            if (string.IsNullOrWhiteSpace(country))
                return Result.Failure<Address>("Country required");

            if (coordinates == null)
                return Result.Failure<Address>("Coordinates required");

            return Result.Success<Address>(
                new Address(default, address1, address2, city, state, country, coordinates)
            );
        }
    }
}
