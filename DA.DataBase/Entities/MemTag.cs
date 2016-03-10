using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("MemTag")]
    public class MemTag
    {
        [Key]
        [Required]
        public int MTagSeq { get; set; }

        public int DRSeq { get; set; }

        public int TObjSeq { get; set; }

        public string TagName { get; set; }

        public string MemAddr { get; set; }

        public byte DataType { get; set; }

        public byte BitAddr { get; set; }

        public decimal ValueUnit { get; set; }

        public byte ModifyFlag { get; set; }

        public string ShortName { get; set; }

        public string UnitName { get; set; }
    }
}
