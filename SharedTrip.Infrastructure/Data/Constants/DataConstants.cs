namespace SharedTrip.Infrastructure.Data.Constants
{
    public class DataConstants
    {
        public class User
        {
            public const int FirstNameMaxLength = 30;
            public const int LastNameMaxLength = 30;
        }

        public class Car
        {
            public const int ModelMaxLength = 30;
            public const int BrandNameMaxLength = 40;
            public const int ColourMaxLength = 20;
        }

        public class Comment
        {
            public const int ContentMaxLength = 500;
        }

        public class PopulatedPlace
        {
            public const int PopulatedPlaceNameMaxLength = 40;
        }

        public class Trip
        {
            public const int AdditionalInformationMaxLength = 500;
        }
    }
}
