using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("tblInterchange")]
    public class Interchange : BaseModel
    {
        
        public int ID { get; set; }
        public string idTag { get; set; }
        [StringLength(3, MinimumLength = 3)]
        public int NewNum { get; set; }
        public int Gate { get; set; }
        [MaxLength(50)]
        public string GateLetter { get; set; }
        [MaxLength(1)]
        public string dCode { get; set; }
        [MaxLength(50)]
        public string simpleName { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        public Single Milepost { get; set; }
        public Single ActualMP { get; set; }
        public Single mpDetail { get; set; }
        [Column(TypeName = "decimal(9,3)")]
        public decimal MileCalc { get; set; }
        public Single s_nearMP { get; set; }
        public Single e_nearMP { get; set; }
        public bool TollCalc { get; set; }
        public bool RefundCalc { get; set; }
        public bool NE { get; set; }
        public int icType { get; set; }
        [MaxLength(1)]
        public string AWC { get; set; }
        public bool active { get; set; }
        public bool icRamp { get; set; }
        [Column(TypeName = "decimal(9,3)")]
        public decimal sNum { get; set; }
        public byte priority { get; set; }
        public byte jsIndex { get; set; }
        public byte sIndex { get; set; }
        public int sectionID { get; set; }
        public int districtID { get; set; }
        public int fareDistrictID { get; set; }
        public int PSPzone { get; set; }
        public int Shed { get; set; }
        public bool MLinterSect { get; set; }
        public byte Tsection { get; set; }
        [Column(TypeName = "decimal(9,3)")]
        public decimal snumStart { get; set; }
        [Column(TypeName = "decimal(9,3)")]
        public decimal snumEnd { get; set; }
        public byte speedSect { get; set; }
        [MaxLength(2)]
        public string speedLayer { get; set; }
        public int speedLayBit { get; set; }
        public int vcsBit { get; set; }
        public int FCDistrict { get; set; }
        public int numLanes { get; set; }
        public int pmtType { get; set; }
        public bool isENS { get; set; }
    }
}