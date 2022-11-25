using SharedTrip.Core.Models.Car;

namespace SharedTrip.Core.Models.ServiceModels.Car
{
    public class CarQueryServiceModel
    {
        public int TotalCarsCount { get; set; }

        public IEnumerable<ProfileCarViewModel> Cars { get; set; } = new List<ProfileCarViewModel>();
    }
}