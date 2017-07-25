using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MSTodoApi.Infrastructure;
using MSTodoApi.Infrastructure.Auth;
using MSTodoApi.Infrastructure.Http;
using MSTodoApi.Infrastructure.Utils;

namespace MSTodoApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            
            services.Configure<AppAuthOptions>(Configuration.GetSection("AuthCredentials"));
            services.Configure<AppRetryOptions>(Configuration.GetSection("PollyRetry"));

            services.AddScoped<IHttpClientFactory, HttpClientFactory>();

            services.AddSingleton<IDatetimeUtils, DatetimeUtils>();
            services.AddSingleton<ITokenStore, InMemoryTokenStore>();
            services.AddTransient<IEventsClient, EventsClient>();
            services.AddTransient<ITasksClient, TasksClient>();
            services.AddTransient<ITodoService, TodoService>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
