using Microsoft.EntityFrameworkCore;
using CadFermaHub.Service;
using Microsoft.AspNetCore.Components;

namespace CadFermaHub.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<EFModels.Microservice> Microservices { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=FermaHub.db");
        }
    }
}