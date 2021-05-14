using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("content-type", "application/json");
                await next();
            });

            app.Map("/person", a =>
            {
                a.MapWhen(context =>
                context.Request.Method.ToLower() == "get",
                config =>
                {
                    config.Run(async ctx =>
                    {
                        var person = new Person { FirstName = "Rajesh", LastName = "Nath" };
                        var personString = JsonConvert.SerializeObject(person);
                        await ctx.Response.WriteAsync(personString);
                    });
                });

                a.MapWhen(context =>
                context.Request.Method.ToLower() == "post"
                && context.Request.Headers.ContainsKey("content-type")
                && context.Request.Headers["content-type"] == "application/json",
                config =>
                {
                    config.Run(async ctx =>
                    {
                        var body = "";
                        var request = ctx.Request;


                        using (StreamReader reader
                                  = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                        {
                            body = await reader.ReadToEndAsync();
                        }

                        try
                        {
                            var person = JsonConvert.DeserializeObject<Person>(body);
                            await ctx.Response.WriteAsync($"Read success {person.FirstName} {person.LastName}");
                        }
                        catch (Exception)
                        {
                            ctx.Response.StatusCode = 400;
                        }
                    });
                });
            });

            app.Run(async context =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Could not process...");
            });
        }
    }
}
