using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using SharedTrip.Infrastructure.Data.Entities;
using SharedTrip.Infrastructure.Data.Seeding.Dtos;

namespace SharedTrip.Infrastructure.Data.Seeding.Configuration
{
    public class CarBrandsConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasData(CreateCarBrands());
        }

        private List<Brand> CreateCarBrands()
        {
            var carBrands = JsonConvert.DeserializeObject<CommonDto[]>(File.ReadAllText(@"../SharedTrip.Infrastructure/Data/Seeding/SeedingData/CarBrands.json"));

            int counter = 0;
            var result = new List<Brand>();

            foreach (var item in carBrands)
            {
                counter++;

                result.Add(new Brand
                {
                    Id = counter,
                    Name = item.Name,
                });
            }

            return result;
        }
    }
}