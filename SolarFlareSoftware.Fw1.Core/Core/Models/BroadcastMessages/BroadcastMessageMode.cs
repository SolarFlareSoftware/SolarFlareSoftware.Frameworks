using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("BroadcastMessageMode")]
    public class BroadcastMessageMode : BaseModel, IAuditableFull 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BroadcastMessageModeID { get; set; }
        [Required(ErrorMessage ="You must provide a Mode Name")]
        [MaxLength(6, ErrorMessage ="Mode Name may not exceed 6 characters")]
        public string ModeName { get; set; }
        [Required(ErrorMessage = "You must provide a Sort Order")]
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        [Required(ErrorMessage = "You must provide the Add Date")]
        public DateTime AuditAddDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who added the Broadcast Message Mode")]
        [MaxLength(24, ErrorMessage = "The User who added the Broadcast Message Mode may not exceed 24 characters")]
        public string AuditAddUserName { get; set; }
        [Required(ErrorMessage = "You must provide a Change Date")]
        public DateTime AuditChangeDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who changed the Broadcast Message Mode")]
        [MaxLength(24, ErrorMessage = "The User who changed the Broadcast Message Mode may not exceed 24 characters")]
        public string AuditChangeUserName { get; set; } = null;
    }
}
