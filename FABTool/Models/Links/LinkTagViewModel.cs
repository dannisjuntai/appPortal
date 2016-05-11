using DA.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABTool.Models.Links
{
    public class LinkDeviceViewModel
    {
        /// <summary>
        /// 連線狀態統計次數
        /// </summary>
        public int CurLinkStaCount { get; set; }
        /// <summary>
        /// 告警狀態統計次數
        /// </summary>
        public int CurSubStaCount { get; set; }
        /// <summary>
        /// 維護狀態統計次數
        /// </summary>
        public int MaintainCount { get; set; }
        /// <summary>
        /// 偵測項目
        /// </summary>
        public List<LinkTagViewModel> LinkTags { get; set; }
    }

    public class LinkTagViewModel
    {
        public string LinkSubName { get; set; }
        public LinkTag LinkTag { get; set; }

        public string CurLinkStaName { get; set; }

        public string CurSubStaName { get; set; }

        public string MaintainName { get; set; }

        public string TObjName { get; set; }

        public string AlarmName { get; set; }
    }
}
