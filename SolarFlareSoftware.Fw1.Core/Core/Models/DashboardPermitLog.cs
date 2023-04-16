using SolarFlareSoftware.Fw1.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("DashboardPermitLogs")]
    public class DashboardPermitLog : BaseModel, IAuditableAddDate
    {
        [Key]
        public Guid DashboardPermitLogId { get; set; }
        [Required(ErrorMessage = "You must provide the Correlation ID")]
        public Guid CorrelationId { get; set; }
        [Required(ErrorMessage = "You must provide the Request Permit Token")]
        [MaxLength(50)]
        public string RequestPermitToken { get; set; }
        [Required(ErrorMessage = "You must provide the Request Permit Number")]
        [MaxLength(50)]
        public string RequestPermitNumber { get; set; }
        public DateTime? RequestPermitStartDate { get; set; }
        public DateTime? RequestPerRequestPermitEndDatemitStartDate { get; set; }
        [Required(ErrorMessage = "You must indicate the Request Status")]
        [MaxLength(50)]
        public string RequestStatus { get; set; }
        public DateTime? RequestProcessStartDate { get; set; }
        public DateTime? RequestProcessEndDate { get; set; }
        public DateTime? PublishDate { get; set; }
        [Required(ErrorMessage = "You must indicate the Total Records Processed")]
        public int TotalRecordsProcess { get; set; }
        [Required(ErrorMessage = "You must indicate the Remaining Records to Process")]
        public int RemainingRecordsProcess { get; set; }
        [Required(ErrorMessage = "You must provide the Request")]
        public string RequestBlob { get; set; }
        public string ProcessBlob { get; set; }
        public string ResponseBlob { get; set; }
        [Required(ErrorMessage = "You must provide the Add Date")]
        public DateTime AuditAddDate { get; set; }

    }
}
