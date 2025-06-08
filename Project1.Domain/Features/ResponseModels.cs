using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Domain.Features
{
    public class ResponseModels
    {
        public ResponseModels(bool isSuccess,string message,object data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public object? Data { get; set; }

    }
}
