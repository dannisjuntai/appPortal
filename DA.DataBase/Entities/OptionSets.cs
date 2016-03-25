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
        [Column(Order = 1)]
        [Required]
        public string FieldName { get; set; }
        [Key]
        [Column(Order = 2)]
        [Required]
        public byte OptionNo { get; set; }
        [Required]
        public string OptionName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Key]
        [Column(Order = 3)]
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public short SystemUser { get; set; }
        [Required]
        public DateTime SystemTime { get; set; }

    }
}
