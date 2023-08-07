using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JourneyService.Domain;

namespace JourneyService.Databases.EntityConfigurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Common configurations for all entities go here
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedOn)
                .IsRequired();

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(e => e.LastModifiedOn)
                .IsRequired(false);

            builder.Property(e => e.LastModifiedBy)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}