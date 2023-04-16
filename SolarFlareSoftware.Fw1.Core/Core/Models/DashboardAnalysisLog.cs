using SolarFlareSoftware.Fw1.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("DashboardAnalysisLogs")]
    public class DashboardAnalysisLog : BaseModel, IAuditableAddDate
    {
        [Key]
        public Guid DashboardAnalysisLogId { get; set; }
        [Required(ErrorMessage = "You must provide the Dashboard Permit Log ID")]
        public Guid DashboardPermitLogId { get; set; }
        public DateTime AnalysisStartDate { get; set; }
        public DateTime AnalysisEndDate { get; set; }
        [Required(ErrorMessage = "You must provide the Mile Marker")]
        [Column(TypeName = "decimal(10,3)")]
        public decimal MileMarker { get; set; }
        [Required(ErrorMessage = "You must provide the Analysis Identifier")]
        [MaxLength(100)]
        public string AnalysisIdentifier { get; set; }
        [Required(ErrorMessage = "You must provide the Analysis Structure Type code")]
        [MaxLength(50)]
        public string AnalysisStructureTypeCode { get; set; }
        [Required(ErrorMessage = "You must provide the Bridge Description")]
        public string BridgeDescription { get; set; }
        [Required(ErrorMessage = "You must provide the Input File Name")]
        [MaxLength(50)]
        public string InputFileName { get; set; }
        [Required(ErrorMessage = "The Input File content may not be empty")]
        public string InputFileContent { get; set; }
        [Required(ErrorMessage = "You must provide the Analysis Input")]
        public string AnalysisInputBlob { get; set; }
        [Required(ErrorMessage = "You must provide the Analysis Status")]
        [MaxLength(50)]
        public string AnalysisStatus { get; set; }
        [Required(ErrorMessage = "You must provide the Business Analysis Status")]
        [MaxLength(50)]
        public string BusinessAnalysisStatus { get; set; }
        public string AnalysisOutputBlob { get; set; }
        [Required(ErrorMessage = "You must provide the Add Date")]
        public DateTime AuditAddDate { get; set; }

        public virtual DashboardPermitLog DashboardPermitLog { get; set; }
    }
}
