using Microsoft.EntityFrameworkCore;
using CadFermaHub.Service;
using Microsoft.AspNetCore.Components;

namespace CadFermaHub.Data
{
    public class ApplicationContext : DbContext
    {
        [Inject]
        private EndpointsService Endpoints { get; set; }
        public DbSet<EFModels.Microservice> Microservices { get; set; } = null!;
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=FermaHub.db");
        }

        public override int SaveChanges()
        {

            foreach (var entry in ChangeTracker.Entries<EFModels.Microservice>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var item = entry.Entity;
                    Endpoints.EndpointsDict.AddOrUpdate(item.Id, item.Endpoints, (key, oldValue) => item.Endpoints);
                }
                if (entry.State == EntityState.Deleted)
                    Endpoints.EndpointsDict.TryRemove(entry.Entity.Id, out _);
            }


            return base.SaveChanges(); ;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entry in ChangeTracker.Entries<EFModels.Microservice>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var item = entry.Entity;
                    Endpoints.EndpointsDict.AddOrUpdate(item.Id, item.Endpoints, (key, oldValue) => item.Endpoints);
                }
                if (entry.State == EntityState.Deleted)
                    Endpoints.EndpointsDict.TryRemove(entry.Entity.Id, out _);
            }

            return await base.SaveChangesAsync(cancellationToken); ;
        }
    }
}