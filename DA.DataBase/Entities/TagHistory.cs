using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("TagHistory")]
    public class TagHistory
    {
        [Key, Column(Order = 0)]
        [Required]
        public int LinkTagSeq { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        public DateTime RecTime { get; set; }

        public int Value { get; set; }

        public decimal fValue { get; set; }

        public byte LinkSta { get; set; }

    }

}
