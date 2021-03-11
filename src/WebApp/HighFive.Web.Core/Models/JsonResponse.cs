using System;
using System.Collections.Generic;

namespace HighFive.Web.Core.Models
{
    public class JsonResponse
    {
        public bool IsSuccess { get; set; } = false;
        public List<string> Messages { get; set; } = new List<string>();
        public object Result { get; set; }
    }
}
