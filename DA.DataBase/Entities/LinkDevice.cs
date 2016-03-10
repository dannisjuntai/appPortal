using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("LinkDevice")]
    public class LinkDevice
    {
        [Key]
        [Required]
        public int LinkID { get; set; }
        [Required]
        public int DevModeID { get; set; }
        [Required]
        public string LinkDevName { get; set; }
        [Required]
        public byte LinkMode { get; set; }
        [Required]
        public string ParAddr { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public int PingIntervalms { get; set; }
        [Required]
        public int PingTimeoutms { get; set; }
        [Required]
        public int PingOutCnt { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public byte ModifyFlag { get; set; }
        [Required]
        public byte CurLinkSta { get; set; }
    }
}
