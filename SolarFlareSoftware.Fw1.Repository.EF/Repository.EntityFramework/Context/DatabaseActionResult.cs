using SolarFlareSoftware.Fw1.Core.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFlareSoftware.Fw1.Repository.EF.Context
{
    public class DatabaseActionResult : IDatabaseActionResult
    {
        public bool Succeeded { get; set; }
        public int RecordsAffected { get; set; }
        public string? Message { get; set; }
        public Exception? Exception { get; set; } = null;
    }
}
