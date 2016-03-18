using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("LinkTag")]
    public class LinkTag
    {
        [Key]
        [Required]
        public int LinkTagSeq { get; set; }
        
        public int LinkSubSeq { get; set; }

        public int MTagSeq { get; set; }

        public string TagName { get; set; }

        public byte ModifyFlag { get; set; }

        public int CurValue { get; set; }

        public decimal? CurfValue { get; set; }

        public byte CurLinkSta { get; set; }

        public int GroupId { get; set; }

        public byte? AlarmFlag { get; set; }

        public byte? Enable { get; set; }

        public byte? CurSubSta { get; set; }

        public decimal? UpAlarm { get; set; }

        public decimal? LowAlarm { get; set; }

        public byte Maintain { get; set; }

        public byte UISelFlag { get; set; }
    }
}
