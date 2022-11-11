using SharedTrip.Core.Models.Trip;
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Core.CustomAttributes
{
    public class TripDateAttribute : ValidationAttribute
    {
        private DateTime currentDate = DateTime.UtcNow;
        private int maxYear = DateTime.UtcNow.Year + 1;

        public string GetErrorMessage() =>
        $"Invalid date";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var trip = (CreateTripViewModel)validationContext.ObjectInstance;

            if (trip.Date < this.currentDate || trip.Date.Year > maxYear)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}