using System;
using System.Collections.Generic;
using System.Text;

namespace WeavyMobile.Models
{
    public class JwtTokenResult
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
