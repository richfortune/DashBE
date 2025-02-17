using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }

        public string Exception { get; set; }

        public string LogLevel { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
