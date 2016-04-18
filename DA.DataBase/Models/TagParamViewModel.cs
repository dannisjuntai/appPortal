using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    public class TagParamViewModel
    {
        public int LinkTagSeq { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime EndTime { get; set; }

        public List<LinkTagViewModel> LinkTags { get; set; }

        /// <summary>
        /// 分頁數量
        /// </summary>
        public int ItemsPerPage { get; set; }
        /// <summary>
        /// 目前分頁位置
        /// </summary>
        public int CurrentPage { get; set; }


    }
    public class TagParam
    {
        public int GroupId { get; set; }

        public int LocationId { get; set; }
    }
}
