using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("LinkDevSub")]
    public class LinkDevSub
    {
        [Key]
        [Required]
        public int LinkSubSeq { get; set; }
        
        public int LinkID { get; set; }

        public int DId { get; set; }

        public string LinkSubName { get; set; }

        public byte ModifyFlag { get; set; }
    }
}
