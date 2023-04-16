using SolarFlareSoftware.Fw1.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("BridgeDefinitions")]
    public class BridgeDefinition : BaseModel, IAuditableFull
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid BridgeDefinitionsId { get; set; }
        [Required(ErrorMessage = "You must provide the Exchanges Code")]
        [MaxLength(50)]
        public string ExchangesCode { get; set; }
        [Required(ErrorMessage = "You must provide the Mile Marker")]
        [Column(TypeName = "decimal(10,3)")]
        public decimal MileMarker { get; set; }
        [ForeignKey(nameof(Interchange))]
        public int? InterchangeId { get; set; }
        [MaxLength(50)]
        public string snum { get; set; }
        [Required(ErrorMessage = "You must provide the Bridge Number")]
        [MaxLength(50)]
        public string BridgeNumber { get; set; }
        [Required(ErrorMessage = "You must provide the Bridge Key")]
        [MaxLength(50)]
        public string BridgeKey { get; set; }
        [Required(ErrorMessage = "You must provide the BMS String ID")]
        [MaxLength(50)]
        public string BMSStringId { get; set; }
        [Required(ErrorMessage = "You must provide the Bridge Location Type code")]
        [MaxLength(50)]
        public string BridgeLocationTypeCode { get; set; }
        [Required(ErrorMessage = "You must provide the Bridge Direction Type code")]
        [MaxLength(50)]
        public string BridgeDirectionTypeCode { get; set; }
        [Required]
        public bool IsActiveInd { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime VersionStartDate { get; set; }
        public DateTime VersionEndDate { get; set; }
        [Required(ErrorMessage = "You must provide the Bridge Definition Version Number")]
        public short VersionNumber { get; set; }
        [ForeignKey(nameof(CurrentBridgeDefinition))]
        public int? VersionCurrentId { get; set; }
        [Required(ErrorMessage = "You must provide the Add Date")]
        public DateTime AuditAddDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who added the Bridge Definition")]
        [MaxLength(24, ErrorMessage = "The User who added the Bridge Definition may not exceed 24 characters")]
        public string AuditAddUserName { get; set; }
        [Required(ErrorMessage = "You must provide a Change Date")]
        public DateTime AuditChangeDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who changed the Bridge Definition")]
        [MaxLength(24, ErrorMessage = "The User who changed the Bridge Definition may not exceed 24 characters")]
        public string AuditChangeUserName { get; set; } = null;

        public virtual Interchange Interchange { get; set; }
        public virtual BridgeDefinition CurrentBridgeDefinition { get; set; }
    }
}
