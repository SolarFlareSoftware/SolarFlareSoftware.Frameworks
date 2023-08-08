using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Serializable]
    [NotMapped]
    public class BaseModel : IBaseModel
    {
        [NotMapped]
        public bool IsValid { get; set; } = true;

        [NotMapped]
        public Dictionary<string, string> ValidationErrors { get; set; } = new();
    }
}
