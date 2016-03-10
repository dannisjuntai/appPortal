using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("Groups")]
    public class Groups
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string GroupCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string GroupName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string GroupTypeKey { get; set; }
        [Required]
        public int ParentId { get; set; }
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
        /// <summary>
        /// 顏色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 圖示
        /// </summary>
        public string Icon { get; set; }
    }

}
