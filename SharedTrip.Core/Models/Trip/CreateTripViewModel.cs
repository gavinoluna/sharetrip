﻿using SharedTrip.Core.CustomAttributes;
using SharedTrip.Core.Models.Car;
using System.ComponentModel.DataAnnotations;

using static SharedTrip.Infrastructure.Data.Constants.DataConstants.Trip;

namespace SharedTrip.Core.Models.Trip
{
    public class CreateTripViewModel
    {
        public int CarId { get; set; }

        public IEnumerable<CreateTripCarViewModel> Cars { get; set; } = new List<CreateTripCarViewModel>();

        [Display(Name = "Price per Person")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [DataType(DataType.Currency)]
        public decimal PricePerPerson { get; set; }

        public int StartDestinationId { get; set; }

        public IEnumerable<PopulatedPlaceViewModel> StartDestinations { get; set; } = new List<PopulatedPlaceViewModel>();

        public int EndDestinationId { get; set; }

        public IEnumerable<PopulatedPlaceViewModel> EndDestinations { get; set; } = new List<PopulatedPlaceViewModel>();

        [TripDate]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Space for Luggage")]
        public bool SpaceForLuggage { get; set; }

        [Display(Name = "Allowed Baverages")]
        public bool AllowedBaverages { get; set; }

        [Display(Name = "Allowed Food")]
        public bool AllowedFood { get; set; }

        [Display(Name = "Allowed Smoking")]
        public bool AllowedSmoking { get; set; }

        [StringLength(AdditionalInformationMaxLength)]
        public string? AdditionalInformation { get; set; }
    }
}