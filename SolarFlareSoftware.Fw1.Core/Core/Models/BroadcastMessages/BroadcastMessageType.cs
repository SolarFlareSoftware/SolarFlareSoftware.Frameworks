using SolarFlareSoftware.Fw1.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarFlareSoftware.Fw1.Core.Models
{
    [Table("BroadcastMessageType")]
    public class BroadcastMessageType : BaseModel, IAuditableFull
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BroadcastMessageTypeID { get; set; }
        [Required(ErrorMessage ="You must provide a Type Name")]
        [MaxLength(8, ErrorMessage = "Type Name may not exceed 8 characters")]
        public string TypeName { get; set; }
        [Required(ErrorMessage = "You must provide a Sort Order")]
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        [Required(ErrorMessage = "You must provide the Add date")]
        public DateTime AuditAddDate { get; set; }
        [Required(ErrorMessage = "You must provide the User who added the Broadcast Message Type")]
        [MaxLength(24, ErrorMessage = "The User who added the Broadcast Message Type may not exceed 24 characters")]
        public string AuditAddUserName { get; set; }
        [Required(ErrorMessage = "You must provide a Change date")]
        public DateTime AuditChangeDate { get; set; }
        [MaxLength(24, ErrorMessage = "The User who modified the Broadcast Message Type may not exceed 24 characters")]
        [Required(ErrorMessage = "You must provide the User who Modified the Broadcast Message Type")]
        public string AuditChangeUserName { get; set; } = null;
    }
}
