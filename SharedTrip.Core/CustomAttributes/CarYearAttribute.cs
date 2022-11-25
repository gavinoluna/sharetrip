using SharedTrip.Core.Models.Car;
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Core.CustomAttributes
{
    public class CarYearAttribute : ValidationAttribute
    {
        private int releaseYear = 1980;
        private int currentYear = DateTime.UtcNow.Year;

        public string GetErrorMessage() =>
        $"The year of the car must be between {releaseYear} and {currentYear}";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var car = (ICarModel)validationContext.ObjectInstance;

            if (car.Year < this.releaseYear || car.Year > this.currentYear)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}