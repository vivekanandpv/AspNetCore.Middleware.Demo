using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Middleware.Demo.Middleware
{
    public class SampleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SampleMiddleware> _logger;

        //  Class middleware can get dependency injected
        //  RequestDelegate will be provided, in case we need it (non-terminal?)

        public SampleMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            this._logger = loggerFactory.CreateLogger<SampleMiddleware>();
        }

        //  InvokeAsync will be called with HttpContext
        //  We use InvokeAsync here hence the middleware is asynchronous
        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Pre-next");

            await _next(context);   //  makes non-terminal

            _logger.LogInformation("Post-next");
        }
    }
}
