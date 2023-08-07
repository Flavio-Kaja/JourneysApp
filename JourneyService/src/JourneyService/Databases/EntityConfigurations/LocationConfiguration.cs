using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JourneyService.Domain.Locations;

namespace JourneyService.Databases.EntityConfigurations
{
    public class LocationConfiguration : BaseEntityConfiguration<Location>
    {
        public override void Configure(EntityTypeBuilder<Location> builder)
        {
            base.Configure(builder);

            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(l => l.Latitude)
                .HasPrecision(9, 6)
                .IsRequired();

            builder.Property(l => l.Longitude)
                .HasPrecision(9, 6)
                .IsRequired();

        }
    }
}