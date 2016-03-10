using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("TagObj")]
    public class TagObj
    {
        [Key]
        [Required]
        public int TObjSeq { get; set; }

        public string TObjName { get; set; }

    }
}
