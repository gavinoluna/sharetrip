using SharedTrip.Core.Models.Trip;
using System;
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Core.CustomAttributes
{
    public class TripDateAttribute : ValidationAttribute
    {
        private DateTime currentDate = DateTime.Now;
        private int maxYear = DateTime.UtcNow.Year + 1;

        public string GetErrorMessage() =>
        $"Invalid date";

        public string GetTimeErrorMessage() =>
         "The hour of departure must be at least 1 hour after the creation of the trip";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var trip = (ITripModel)validationContext.ObjectInstance;

            if (DateTime.Compare(trip.Date, currentDate) <= 0
                || trip.Date.Year > maxYear)
            {
                return new ValidationResult(GetErrorMessage());
            }

            if (DateTime.Compare(trip.Date, currentDate.AddHours(1)) < 0)
            {
                return new ValidationResult(GetTimeErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}