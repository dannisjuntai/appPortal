using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    public class MainToolViewModel
    {
        public int GroupId { get; set; }

        public string GroupTypeKey { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int AlarmCount { get; set; }
        public int StatusCount { get; set; }
        public int MaintainCount { get; set; }

        public string Color { get; set; }

        public TagStatus Status { get; set; }
    }

    public class TagStatus
    {
        public int LinkTagSeq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte? CurSubSta { get; set; }
        /// <summary>
        /// 維護
        /// </summary>
        public int Maintain { get; set; }
        /// <summary>
        /// 通訊狀態
        /// </summary>
        public byte CurLinkSta { get; set; }
    }
}
