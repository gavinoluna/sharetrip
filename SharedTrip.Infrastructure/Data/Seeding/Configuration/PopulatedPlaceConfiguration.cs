using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using SharedTrip.Infrastructure.Data.Entities;
using SharedTrip.Infrastructure.Data.Seeding.Dtos;
using System.Drawing;

namespace SharedTrip.Infrastructure.Data.Seeding.Configuration
{
    public class PopulatedPlaceConfiguration : IEntityTypeConfiguration<PopulatedPlace>
    {
        public void Configure(EntityTypeBuilder<PopulatedPlace> builder)
        {
            builder.HasData(CreatePopulatedPlaces());
        }

        private List<PopulatedPlace> CreatePopulatedPlaces()
        {
            var populatedPlaces = JsonConvert.DeserializeObject<CommonDto[]>(File.ReadAllText(@"../SharedTrip.Infrastructure/Data/Seeding/SeedingData/PopulatedPlaces.json"));

            int counter = 0;
            var result = new List<PopulatedPlace>();

            foreach (var item in populatedPlaces)
            {
                counter++;

                result.Add(new PopulatedPlace
                {
                    Id = counter,
                    Name = item.Name,
                });
            }

            return result;
        }
    }
}