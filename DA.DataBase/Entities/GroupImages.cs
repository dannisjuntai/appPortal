using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Entities
{
    [Table("GroupImages")]
    public class GroupImages
    {
        [Key]
        [Required]
        public int ImageId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        [MaxLength]
        public byte[] Data { get; set; }
        [NotMapped]
        public string Base64 { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileType { get; set; }
        [Required]
        public int FileSize { get; set; }
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
