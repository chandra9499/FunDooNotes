using Microsoft.AspNetCore.Http;
using Model.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataBaseLayer.Helper
{
    public class GlobalExceptionHandler : IMiddleware
    {
        

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) 
            {
                HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ResponseModel<object>()
            {
                StatusCode = context.Response.StatusCode,
                Success = false,
                Message = "An unexpected error occurred.",
                Data = null
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
