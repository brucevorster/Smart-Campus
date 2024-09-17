using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UmojaCampus.Business.Persistence.Configuration
{
    public static class SoftDeleteFilter
    {
        private const string IsDeleted = "IsDeleted";
        public static void ApplyFilter(ModelBuilder modelBuilder)
        {
            // Apply soft delete filter for all entities with IsDeleted property
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var isDeletedProperty = entityType.FindProperty(IsDeleted);

                if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "entity");
                    var body = Expression.Equal(
                        Expression.Property(parameter, isDeletedProperty.PropertyInfo),
                        Expression.Constant(false)
                    );

                    var lambda = Expression.Lambda(body, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}
