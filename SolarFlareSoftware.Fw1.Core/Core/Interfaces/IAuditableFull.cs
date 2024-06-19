using System;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface IAuditableFull
    {
        DateTime AuditAddDate { get; set; }
        string AuditAddUserName { get; set; }
        DateTime? AuditChangeDate { get; set; }
        string? AuditChangeUserName { get; set; }
    }
}
