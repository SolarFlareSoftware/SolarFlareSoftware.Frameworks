using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("BridgeDefinitionFiles")]
    public class BridgeDefinitionFile : BaseModel
    {
        [Key]
        public Guid BridgeDefinitionFilesId { get; set; }
        [Required(ErrorMessage = "You must provide the Bridge Definition ID")]
        public Guid BridgeDefinitionId { get; set; }
        [Required(ErrorMessage = "You must provide the Analysis Structure Type code")]
        [MaxLength(50)]
        public string AnalysisStructureTypeCode { get; set; }
        [Required]
        public bool IgnoreAnalysisFlag { get; set; } = false;
        [MaxLength(50)]
        public string InputFileName { get; set; }
        public string InputFileContent { get; set; }
        [Required(ErrorMessage = "You must provide the Rating Threshold")]
        [Column(TypeName = "decimal(10,3)")]
        public decimal RatingThreshold { get; set; }
        public string RatingThresholdMultiSpan { get; set; }
        [Required]
        public bool IsActiveInd { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime VersionStartDate { get; set; }
        public DateTime VersionEndDate { get; set; }
        [Required(ErrorMessage = "You must provide the Bridge Defintion File Version Number")]
        public byte VersionNumber { get; set; }
        [ForeignKey(nameof(CurrentBridgeDefinitionFile))]
        public Guid? VersionCurrentId { get; set; }
        [Required(ErrorMessage = "You must provide the Add Date")]
        public DateTime AuditAddDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who added the Bridge Definition File")]
        [MaxLength(24, ErrorMessage = "The User who added the Bridge Definition File may not exceed 24 characters")]
        public string AuditAddUserName { get; set; }
        [Required(ErrorMessage = "You must provide a Change Date")]
        public DateTime AuditChangeDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who changed the Bridge Definition File")]
        [MaxLength(24, ErrorMessage = "The User who changed the Bridge Definition File may not exceed 24 characters")]
        public string AuditChangeUserName { get; set; } = null;

        public virtual BridgeDefinition BridgeDefinition { get; set; }
        public virtual BridgeDefinitionFile CurrentBridgeDefinitionFile { get; set; }
    }
}
