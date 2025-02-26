using CadFermaHub.Data.EFModels;
using Microsoft.EntityFrameworkCore;
 
namespace CadFermaHub.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Microservice> Microservices { get; set; } = null!;
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=FermaHub.db");
        }
    }
}