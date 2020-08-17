using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApiBase.Domain.Models
{
    public class Message
    {
        public string Clientuniqueid { get; set; }
        public string Type { get; set; }
        public string MessageDesc { get; set; }
        public DateTime Date { get; set; }
    }
}
