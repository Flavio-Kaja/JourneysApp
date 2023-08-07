namespace JourneyService.Databases.EntityConfigurations;

using JourneyService.Domain.TransportationTypes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TransportationTypeConfiguration : BaseEntityConfiguration<TransportationType>
{
    /// <summary>
    /// The database configuration for TransportationTypes. 
    /// </summary>
    public override void Configure(EntityTypeBuilder<TransportationType> builder)
    {
        base.Configure(builder);

        builder.Property(l => l.Type)
                      .IsRequired()
                      .HasMaxLength(250);
    }
}