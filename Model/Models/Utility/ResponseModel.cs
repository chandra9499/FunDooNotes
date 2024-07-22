using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Models.Utility
{
    public class ResponseModel<T>
    {
       

        public int StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        // public string? Token { get; set; }
        public T? Data { get; set; }

        public ResponseModel(int statusCode, bool success, string message, T? data)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Data = data;
        }

        public ResponseModel()
        {
        }
    }
}
