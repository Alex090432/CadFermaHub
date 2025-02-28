using CadFermaHub.Data;
using CadFermaHub.Service;
using Microsoft.EntityFrameworkCore;

namespace CadFermaHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var startup=new Startup();
            startup.ConfigureServices(builder.Services);
            builder.Services.AddDbContextFactory<ApplicationContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationContext>>();
                using (var dbContext = dbContextFactory.CreateDbContext())
                {
                    dbContext.Database.Migrate();
                }
            }

            startup.Configure(app);
            app.MapGrpcService<ApiServiceWork>();
            app.Run();
        }
    }
}