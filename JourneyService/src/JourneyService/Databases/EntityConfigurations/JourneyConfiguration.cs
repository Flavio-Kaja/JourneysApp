namespace JourneyService.Databases.EntityConfigurations;

using JourneyService.Domain.Journeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class JourneyConfiguration : BaseEntityConfiguration<Journey>
{
    /// <summary>
    /// The database configuration for Journeys. 
    /// </summary>
    public override void Configure(EntityTypeBuilder<Journey> builder)
    {
        base.Configure(builder);

        builder.Property(j => j.UserId)
            .IsRequired();

        builder.Property(j => j.StartingLocationId)
            .IsRequired();

        builder.HasOne(j => j.StartingLocation)
            .WithMany()
            .HasForeignKey(j => j.StartingLocationId);

        builder.Property(j => j.ArrivalLocationId)
            .IsRequired();

        builder.HasOne(j => j.ArrivalLocation)
            .WithMany()
            .HasForeignKey(j => j.ArrivalLocationId);

        builder.Property(j => j.StartingTime)
            .IsRequired();

        builder.Property(j => j.ArrivalTime)
            .IsRequired(false);

        builder.Property(j => j.TransportationTypeId)
            .IsRequired();

        builder.Property(j => j.RouteDistance)
            .IsRequired();

        builder.Property(j => j.IsGoalAchieved)
            .IsRequired().HasDefaultValue(false);

    }
}