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

        /// <summary>
        /// Calculate the distance in meters between two points using Vincenty formula ported from the Android SDK Maps Vincenty formula
        /// Based on http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf Using the "Inverse Formula" (section 4)
        /// https://github.com/aosp-mirror/platform_frameworks_base/blob/master/location/java/android/location/Location.java#L335
        /// 
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public Distance CalculateDistance(GeoCoordinates coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate));

            // Based on http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf
            // using the "Inverse Formula" (section 4)

            int MAXITERS = 20;
            // Convert lat/long to radians
            double lat1Rads = Latitude * Math.PI / 180.0;
            double lat2Rads = coordinate.Latitude * Math.PI / 180.0;
            double lon1Rads = Longitude * Math.PI / 180.0;
            double lon2Rads = coordinate.Longitude * Math.PI / 180.0;

            double a = 6378137.0; // WGS84 major axis
            double b = 6356752.3142; // WGS84 semi-major axis
            double f = (a - b) / a;
            double aSqMinusBSqOverBSq = (a * a - b * b) / (b * b);

            double L = lon2Rads - lon1Rads;
            double A = 0.0;
            double U1 = Math.Atan((1.0 - f) * Math.Tan(lat1Rads));
            double U2 = Math.Atan((1.0 - f) * Math.Tan(lat2Rads));

            double cosU1 = Math.Cos(U1);
            double cosU2 = Math.Cos(U2);
            double sinU1 = Math.Sin(U1);
            double sinU2 = Math.Sin(U2);
            double cosU1cosU2 = cosU1 * cosU2;
            double sinU1sinU2 = sinU1 * sinU2;

            double sigma = 0.0;
            double deltaSigma = 0.0;
            double cosSqAlpha;
            double cos2SM;
            double cosSigma;
            double sinSigma;
            double cosLambda = 0.0;
            double sinLambda = 0.0;

            double lambda = L; // initial guess
            for (int iter = 0; iter < MAXITERS; iter++)
            {
                double lambdaOrig = lambda;
                cosLambda = Math.Cos(lambda);
                sinLambda = Math.Sin(lambda);
                double t1 = cosU2 * sinLambda;
                double t2 = cosU1 * sinU2 - sinU1 * cosU2 * cosLambda;
                double sinSqSigma = t1 * t1 + t2 * t2; // (14)
                sinSigma = Math.Sqrt(sinSqSigma);
                cosSigma = sinU1sinU2 + cosU1cosU2 * cosLambda; // (15)
                sigma = Math.Atan2(sinSigma, cosSigma); // (16)
                double sinAlpha = (sinSigma == 0) ? 0.0 :
                    cosU1cosU2 * sinLambda / sinSigma; // (17)
                cosSqAlpha = 1.0 - sinAlpha * sinAlpha;
                cos2SM = (cosSqAlpha == 0) ? 0.0 :
                    cosSigma - 2.0 * sinU1sinU2 / cosSqAlpha; // (18)

                double uSquared = cosSqAlpha * aSqMinusBSqOverBSq; // defn
                A = 1 + (uSquared / 16384.0) * // (3)
                    (4096.0 + uSquared *
                     (-768 + uSquared * (320.0 - 175.0 * uSquared)));
                double B = (uSquared / 1024.0) * // (4)
                    (256.0 + uSquared *
                     (-128.0 + uSquared * (74.0 - 47.0 * uSquared)));
                double C = (f / 16.0) *
                    cosSqAlpha *
                    (4.0 + f * (4.0 - 3.0 * cosSqAlpha)); // (10)
                double cos2SMSq = cos2SM * cos2SM;
                deltaSigma = B * sinSigma * // (6)
                    (cos2SM + (B / 4.0) *
                     (cosSigma * (-1.0 + 2.0 * cos2SMSq) -
                      (B / 6.0) * cos2SM *
                      (-3.0 + 4.0 * sinSigma * sinSigma) *
                      (-3.0 + 4.0 * cos2SMSq)));

                lambda = L +
                    (1.0 - C) * f * sinAlpha *
                    (sigma + C * sinSigma *
                     (cos2SM + C * cosSigma *
                      (-1.0 + 2.0 * cos2SM * cos2SM))); // (11)

                double delta = (lambda - lambdaOrig) / lambda;
                if (Math.Abs(delta) < 1.0e-12)
                {
                    break;
                }
            }

            var mDistance = (double)(b * A * (sigma - deltaSigma));
            return Distance.Create(mDistance, DistanceUnit.Meter).Value;
        }
    }
}
