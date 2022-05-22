using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CG.API.Middlewares
{
    public class ExceptionMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        #endregion

        #region Ctor

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ExceptionMiddleware)} Exception: {ex.Message}", ex);

                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = JsonSerializer.Serialize(new { message = "An Exception Occured!" });
                await response.WriteAsync(result);
            }
        }

        #endregion
    }
}
