using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using React.AspNet;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using CadFermaHub.Data.DAL;
using CadFermaHub.Service;

namespace CadFermaHub
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddGrpc();

            services.AddTransient<IClassConverter, ClassConverter>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDB, DB>();
            services.AddSingleton<EndpointsService>();
            services.AddReact();
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();
        }
 
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseReact(config => { });
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}