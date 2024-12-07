using CSharpFunctionalExtensions;

namespace PerformanceTesting.Core
{
    public enum DistanceUnit
    {
        Feet,
        Mile,
        Meter,
        Kilometer
    }
    public class Distance
    {
        public double Value { get; private set; }
        public DistanceUnit Unit { get; private set; }
        private Distance(double value, DistanceUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public static Result<Distance> Create(double value, DistanceUnit unit)
        {
            Distance distance = new Distance(Math.Abs(value), unit);

            return Result.Success(distance);
        }

        public Distance Convert(DistanceUnit unit)
        {
            double value = Value;
            if (unit == DistanceUnit.Feet && this.Unit == DistanceUnit.Mile) { 
                value = FeetToMiles(value);
            }
            else if (unit == DistanceUnit.Feet && Unit == DistanceUnit.Meter)
            {
                value = FeetToMeter(value);
            }
            else if (unit == DistanceUnit.Feet && Unit == DistanceUnit.Kilometer)
            {
                value = MetersToKilometers(FeetToMeter(value));
            }
            else if (unit == DistanceUnit.Meter && Unit == DistanceUnit.Feet)
            {
                value = MeterToFeet(value);
            }
            else if (unit == DistanceUnit.Meter && Unit == DistanceUnit.Mile)
            {
                value = MetersToMiles(value);
            }
            else if (unit == DistanceUnit.Meter && Unit == DistanceUnit.Kilometer)
            {
                value = MetersToKilometers(value);
            }
            else if (unit == DistanceUnit.Kilometer && Unit == DistanceUnit.Meter)
            {
                value = KilometersToMeters(value);
            }
            else if (unit == DistanceUnit.Kilometer && Unit == DistanceUnit.Feet)
            {
                value = MilesToFeet(KilometersToMiles(value));
            }
            else if (unit == DistanceUnit.Kilometer && Unit == DistanceUnit.Mile)
            {
                value = KilometersToMiles(value);
            }
            else if (unit == DistanceUnit.Mile && Unit == DistanceUnit.Feet)
            {
                value = MilesToFeet(value);
            }
            else if (unit == DistanceUnit.Mile && Unit == DistanceUnit.Kilometer)
            {
                value = MetersToKilometers(FeetToMeter(MilesToFeet(value)));
            }
            else if (unit == DistanceUnit.Mile && Unit == DistanceUnit.Meter)
            {
                value = FeetToMeter(MilesToFeet(value));
            }

            return new Distance(value,  unit);
        }

        private static double FeetToMiles(double ft)
        {
            return ft / 5280;
        }

        private static double FeetToMeter(double ft)
        {
            return ft * 0.3048;
        }

        private static double MilesToFeet(double miles)
        {
            return miles * 5280;
        }

        private static double MetersToMiles(double meters)
        {
            return meters / 1609.344;
        }

        private static double MeterToFeet(double meters)
        {
            return meters * 3.28084;
        }

        private static double MetersToKilometers(double meters)
        {
            return meters / 1000;
        }

        private static double KilometersToMiles(double km)
        {
            return km * 0.62137;
        }

        private static double KilometersToMeters(double km)
        {
            return km * 1000;
        }
    }
}
