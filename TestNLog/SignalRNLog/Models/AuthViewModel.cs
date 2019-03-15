using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalRNLog.Models
{
    public class AuthViewModel
    {
        public object Times { get; set; }
        public string Name { get; set; } 
        public string AuthType { get; set; } 
        public bool IsAuth { get; set; }
    }
}
