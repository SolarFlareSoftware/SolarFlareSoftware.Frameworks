using System.Collections.Generic;

namespace SolarFlareSoftware.Fw1.Core.Interfaces
{
    public interface ISupportsValidation
    {
        public bool IsValid { get; }
        public Dictionary<string, string> ValidationErrors { get; }
    }
}
