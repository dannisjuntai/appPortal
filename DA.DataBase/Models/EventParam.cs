using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    public class EventSetParam
    {
        public int GroupType { get; set; }
        public int GroupId { get; set; }
        public DateTime SDateTime { get; set; }
        public DateTime EDateTime { get; set; }
        public int OptionNo { get; set; }

        /// <summary>
        /// 分頁數量
        /// </summary>
        public int ItemsPerPage { get; set; }
        /// <summary>
        /// 目前分頁位置
        /// </summary>
        public int CurrentPage { get; set; }
    }
}
