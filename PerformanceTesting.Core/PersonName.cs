using CSharpFunctionalExtensions;

namespace PerformanceTesting.Core
{
    public record PersonName
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        private PersonName() { }
        private PersonName(string firstName, string lastName)
        {
            FirstName = firstName; 
            LastName = lastName;
        }

        public static Result<PersonName> Create(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Result.Failure<PersonName>("First name is required");

            if (string.IsNullOrWhiteSpace(lastName))
                return Result.Failure<PersonName>("Last name is required");

            return Result.Success<PersonName>(
                new PersonName(firstName, lastName)
            );
        }
    }
}
