using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("GroupLocations")]
    public class GroupLocations
    {
        [Key]
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int LinkSubSeq { get; set; }
        [Required]
        public int LinkTagSeq { get; set; }
        [Required]
        public int MTagSeq { get; set; }
        [Required]
        public int TObjSeq { get; set; }
        [Required]
        public string LocationValue { get; set; }
        [Required]
        public bool Prompt { get; set; }
        [Required]
        public byte ModifyFlag { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public int CreateUser { get; set; }
        [Required]
        public int SystemUser { get; set; }
        [Required]
        public DateTime SystemTime { get; set; }
    }

}
