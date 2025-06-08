using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Domain.Features
{
   public static class DevCode
    {
        public static bool isNull(this string? str)
        {
            return str == null || string.IsNullOrEmpty(str.Trim()) || string.IsNullOrWhiteSpace(str.Trim());
        }
    }
}
