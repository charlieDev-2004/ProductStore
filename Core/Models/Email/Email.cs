using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models.Email
{
    public class Email
    {
        public string EmailReceiver { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;

    }
}