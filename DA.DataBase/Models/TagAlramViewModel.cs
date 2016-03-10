using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TagAlramViewModel
    {
        public int LinkTagSeq { get; set; }
        public int GroupId { get; set; }
        public string TagName { get; set; }
        public int CurValue { get; set; }
        public decimal? CurfValue { get; set; }

        public string TObjName { get; set; }

        public byte CurLinkSta { get; set; }

        public string CurLinkStaName { get; set; }
        public byte? CurSubSta { get; set; }
        public string CurSubStaName { get; set; }
        public bool IsCurSubSta { get; set; }
        /// <summary>
        /// 維護狀態
        /// </summary>
        public byte Maintain { get; set; }
        /// <summary>
        /// 維護狀態名稱
        /// </summary>
        public string MaintainName { get; set; }
        /// <summary>
        /// 顯示保養狀態
        /// </summary>
        public bool  IsMaintain{ get; set; }
    }
}
