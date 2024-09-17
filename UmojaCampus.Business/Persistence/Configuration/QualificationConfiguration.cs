using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UmojaCampus.Business.Entities;

namespace UmojaCampus.Business.Persistence.Configuration
{
    public class QualificationConfiguration : IEntityTypeConfiguration<Qualification>
    {
        public void Configure(EntityTypeBuilder<Qualification> builder)
        {
            builder.Property(p => p.Fees)
              .HasColumnType("decimal(18,2)"); 
        }
    }
}
