

using Microsoft.EntityFrameworkCore;
using UmojaCampus.Business.Persistence.Contexts;

namespace UmojaCampus.Business.Data
{
    public class DatabaseSeeder(ApplicationDbContext context): IDatabaseSeeder
    {
        private readonly ApplicationDbContext _context = context;

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync()
                .ConfigureAwait(false);
        }
    }
}
