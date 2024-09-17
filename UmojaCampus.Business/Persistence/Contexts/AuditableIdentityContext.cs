using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UmojaCampus.Business.Entities;
using UmojaCampus.Business.Entities.Base;

namespace UmojaCampus.Business.Persistence.Contexts
{
    public abstract class AuditableIdentityContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
        public virtual int SaveChanges(CurrentUser user = null)
        {
            if(user != null)
            {
                OnBeforeSaveChanges(user);
            }

            ValidateEntities();
            var result = base.SaveChanges();
            return result;
        }
        public virtual async Task<int> SaveChangesAsync(CurrentUser user = null)
        {
            if (user != null)
            {
                OnBeforeSaveChanges(user);
            }

            ValidateEntities();
            var result = await base.SaveChangesAsync();
            return result;
        }

        private void OnBeforeSaveChanges(CurrentUser user = null)
        {
            ChangeTracker.DetectChanges();

            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added ||
                    x.State == EntityState.Modified ||
                    x.State == EntityState.Deleted));

            foreach (var entry in modifiedEntries)
            {
                var entity = (BaseEntity)entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedById = user.Id;
                        entity.EditedById = user.Id;
                        entity.CreatedDateTime = DateTime.Now;
                        entity.EditedDateTime = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entity.EditedById = user.Id;
                        entity.EditedDateTime = DateTime.Now;
                        break;
                }
            }
        }

        private void ValidateEntities()
        {
            var entities = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            var validationResults = new List<ValidationResult>();

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.TryValidateObject(entity, validationContext, validationResults, validateAllProperties: true);

                if (validationResults.Any())
                {
                    throw new ValidationException(string.Join(", ", validationResults.Select(vr => vr.ErrorMessage)));
                }
            }
        }
    }
}
