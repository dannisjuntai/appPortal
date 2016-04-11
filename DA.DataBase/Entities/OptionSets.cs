using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("OptionSets")]
    public class OptionSets
    {
        [Key]
        [Required]
        public int OptionId { get; set; }

        [Required]
        public string FieldName { get; set; }
        [Required]
        public byte OptionNo { get; set; }
        [Required]
        public string OptionName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public short SystemUser { get; set; }
        [Required]
        public DateTime SystemTime { get; set; }

    }
}
