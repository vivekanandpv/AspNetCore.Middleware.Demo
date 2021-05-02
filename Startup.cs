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

            //  Middleware 1: Non-terminal
            app.Use(async (context, next) => {
                //  pre-next phase
                logger.LogInformation("MW 1 : Non-terminal : Use : Pre-next");

                //  call to next middleware
                await next();

                //  post-next phase
                logger.LogInformation("MW 1 : Non-terminal : Use : Post-next");
            });

            //  Middleware 2: Non-terminal
            app.Use(async (context, next) =>
            {
                //  pre-next phase
                logger.LogInformation("MW 2 : Non-terminal : Use : Pre-next");

                //  call to next middleware
                await next();

                //  post-next phase
                logger.LogInformation("MW 2 : Non-terminal : Use : Post-next");
            });

            //  Middleware 3: Terminal
            app.Run(async context =>
            {
                //  Task of the terminal middleware is to prepare the response and thus terminate the chain
                logger.LogInformation("MW 3 : Terminal : Run");
                await context.Response.WriteAsync("MW3 writes response for the incoming HTTP request");
            });

            //  Any middleware from down here, do not run as the chain is terminated
        }
    }
}
