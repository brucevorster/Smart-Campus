using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UmojaCampus.Business.Entities;

namespace UmojaCampus.Business.Persistence.Configuration
{
    public class SemesterConfiguration : IEntityTypeConfiguration<Semester>
    {
        public void Configure(EntityTypeBuilder<Semester> builder)
        {
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
