using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("BroadcastMessageHistory")]
    public class BroadcastMessageHistory : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BroadcastMessageHistoryID { get; set; }
        public int BroadcastMessageID { get; set; }
        public int BroadcastMessageTypeID { get; set; }
        public int BroadcastMessageModeID { get; set; }
        [MaxLength(1000, ErrorMessage = "Broadcast Message Text may not exceed 1000 characters. This includes any hidden formatting characters.")]
        public string MessageText { get; set; }
        [MaxLength(75, ErrorMessage = "Broadcast Message Title may not exceed 75 characters")]
        public string MessageTitle { get; set; } = null;
        public DateTime BeginBroadcast { get; set; }
        public DateTime? EndBroadcast { get; set; } = null;
        public bool IsActive { get; set; } = true;
        [Required(ErrorMessage = "You must provide the Add Date")]
        public DateTime AuditAddDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who added the Broadcast Message")]
        [MaxLength(24, ErrorMessage = "The User who added the Broadcast Message may not exceed 24 characters")]
        public string AuditAddUserName { get; set; }
        [Required(ErrorMessage = "You must provide a Change Date")]
        public DateTime AuditChangeDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who changed the Broadcast Message")]
        [MaxLength(24, ErrorMessage = "The User who changed the Broadcast Message may not exceed 24 characters")]
        public string AuditChangeUserName { get; set; } = null;
        [Required(ErrorMessage = "Action Date must be provided")]
        public DateTime ActionDate { get; set; }
        [Required(ErrorMessage = "Action Code must be provided")]
        public int ActionCode { get; set; }
        [Required(ErrorMessage = "The User who performed the Action must be provided")]
        [MaxLength(24, ErrorMessage = "Broadcast Message Action By may not exceed 50 characters")]
        public string ActionBy { get; set; }
    }
}
