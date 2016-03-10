using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("GroupTypes")]
    public class GroupTypes
    {
        public GroupTypes()
        {
        }
        [Key]
        [Required]
        public string GroupTypeKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string GroupTypeValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public bool Visible { get; set; }
        [Required]
        public byte GroupLayer { get; set; }
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
