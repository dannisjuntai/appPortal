using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("EventSets")]
    public class EventSet
    {
        [Key]
        [Required]
        public int EventNo { get; set; }
        [Required]
        public int EventLevel { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int LinkTagSeq { get; set; }
        [Required]
        public DateTime RecTime { get; set; }

        public DateTime? ConfirmTime { get; set; }

        public DateTime? RestTime { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
    }

}
