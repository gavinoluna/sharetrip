namespace SharedTrip.Core.Models.Car
{
    public class CarQueryServiceModel
    {
        public int TotalCarsCount { get; set; }

        public IEnumerable<ProfileCarViewModel> Cars { get; set; } = new List<ProfileCarViewModel>();
    }
}