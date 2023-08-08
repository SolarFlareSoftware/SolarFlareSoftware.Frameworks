using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarFlareSoftware.Fw1.Core.Core.Interfaces
{
    public interface IDatabaseActionResult
    {
        bool Succeeded { get; set; }
        int RecordsAffected { get; set; }
        string Message { get; set; }
        Exception Exception { get; set; }
    }
}
