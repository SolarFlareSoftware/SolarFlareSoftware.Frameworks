using SolarFlareSoftware.Fw1.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("InterchangeGores")]
    public class InterchangeGore : BaseModel, IAuditableFull
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid InterchangeGoresId { get; set; }
        [Required(ErrorMessage = "You must provide a Direction Type code")]
        [MaxLength(50)]
        public string DirectionTypeCode { get; set; }
        [Required(ErrorMessage = "You must provide the Interchange ID")]
        [ForeignKey(nameof(Interchange))]
        public int InterchangeId { get; set; }
        [Required(ErrorMessage = "You must provide the Mile Marker")]
        [Column(TypeName = "decimal(10,3)")]
        public decimal MileMarker { get; set; }
        [Required(ErrorMessage = "You must provide a Structure")]
        [MaxLength(50)]
        public string Structure { get; set; }
        [Required(ErrorMessage = "You must provide a Point Type code")]
        [MaxLength(50)]
        public string PointTypeCode { get; set; }
        [Required(ErrorMessage = "You must provide the Name")]
        [MaxLength(250)]
        public string Name { get; set; }
        [Required(ErrorMessage = "You must provide the SNum")]
        [MaxLength(50)]
        public string snum { get; set; }
        [Required]
        public bool IsActiveInd { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime VersionStartDate { get; set; }
        public DateTime VersionEndDate { get; set; }
        [Required(ErrorMessage = "You must provide the Interchange Gore Version Number")]
        public short VersionNumber { get; set; }
        public Guid VersionCurrentId { get; set; }
        [Required(ErrorMessage = "You must provide the Add Date")]
        public DateTime AuditAddDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who added the Interchange Gore")]
        [MaxLength(24, ErrorMessage = "The User who added the Interchange Gore may not exceed 24 characters")]
        public string AuditAddUserName { get; set; }
        [Required(ErrorMessage = "You must provide a Change Date")]
        public DateTime AuditChangeDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who changed the Interchange Gore")]
        [MaxLength(24, ErrorMessage = "The User who changed the Interchange Gore may not exceed 24 characters")]
        public string AuditChangeUserName { get; set; } = null;

        public virtual Interchange Interchange { get; set; }
    }
}
