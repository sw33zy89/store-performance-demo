using CSharpFunctionalExtensions;

namespace PerformanceTesting.Core
{
    public record GeoCoordinates
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        private GeoCoordinates() { }
        private GeoCoordinates(double latitude, double longitude) {
            Latitude = latitude;
            Longitude = longitude;
        }
        public static Result<GeoCoordinates> Create(double latitude, double longitude)
        {
            if (longitude < -180 || longitude > 180)
                return Result.Failure<GeoCoordinates>("Longitude must be between -180 and 180");

            if (latitude < -90 || latitude > 90)
                return Result.Failure<GeoCoordinates>("Latitdue must be between -90 and 90");

            return new GeoCoordinates(latitude, longitude);
        }
    }
}
