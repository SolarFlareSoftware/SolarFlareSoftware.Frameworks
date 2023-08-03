using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("BroadcastMessage")]
    public class BroadcastMessage : BaseModel, IAuditableFull 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BroadcastMessageID { get; set; }
        [Required(ErrorMessage = "You must indicate the Broadcast Message type")]
        public int BroadcastMessageTypeID { get; set; }
        [Required(ErrorMessage ="You must select the mode in which the Broadcast Message will be displayed in the application")]
        public int BroadcastMessageModeID { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Broadcast Message Text may not exceed 1000 characters. This includes any hidden formatting characters.")]
        public string MessageText { get; set; }
        [MaxLength(75, ErrorMessage = "Broadcast Message Title may not exceed 75 characters")]
        public string MessageTitle { get; set; } = null;
        [Required]
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

        public virtual BroadcastMessageType BroadcastMessageType { get; set; }
        public virtual BroadcastMessageMode BroadcastMessageMode { get; set; }
    }
}
