using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using SharedTrip.Infrastructure.Data.Entities;
using SharedTrip.Infrastructure.Data.Seeding.Dtos;
using System.Drawing;

namespace SharedTrip.Infrastructure.Data.Seeding.Configuration
{
    public class ColourConfiguration : IEntityTypeConfiguration<Colour>
    {
        public void Configure(EntityTypeBuilder<Colour> builder)
        {
            builder.HasData(CreateColours());
        }

        private List<Colour> CreateColours()
        {
            var colours = JsonConvert.DeserializeObject<CommonDto[]>(File.ReadAllText(@"../SharedTrip.Infrastructure/Data/Seeding/SeedingData/Colours.json"));

            int counter = 0;
            var result = new List<Colour>();

            foreach (var item in colours)
            {
                counter++;

                result.Add(new Colour
                {
                    Id = counter,
                    Name = item.Name,
                });
            }

            return result;
        }
    }
}