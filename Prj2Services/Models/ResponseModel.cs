using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj2Services.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } 
        public ResponseModel(bool success,string message) {
            Success = success;
            Message = message;
        }
    }
}
