using SharedTrip.Core.Models.Car;

namespace SharedTrip.Core.Models.ServiceModels.Car
{
    public class AllCarsQueryServiceModel
    {
        public int TotalCarsCount { get; set; }

        public IEnumerable<AllCarsViewModel> Cars { get; set; } = new List<AllCarsViewModel>();
    }
}