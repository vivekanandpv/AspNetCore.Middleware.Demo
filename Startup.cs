using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Middleware.Demo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            ILogger<Startup> logger = loggerFactory.CreateLogger<Startup>();

            //  Non-terminal used as terminal
            app.Use(async (context, next) => {
                logger.LogInformation("MW1 : As terminal : Use");
                await context.Response.WriteAsync("MW1 writes response for the incoming HTTP request");
            });

            //  Below chain will be ignored as next() is absent in MW1

            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW 2 : Non-terminal : Use : Pre-next");

                await next();

                logger.LogInformation("MW 2 : Non-terminal : Use : Post-next");
            });

            app.Run(async context =>
            {
                logger.LogInformation("MW3 : Terminal : Run");
                await context.Response.WriteAsync("MW3 writes response for the incoming HTTP request");
            });
        }
    }
}
