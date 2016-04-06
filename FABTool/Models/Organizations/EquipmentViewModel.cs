using DA.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABTool.Models.Organizations
{
    public class EquipmentViewModel
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

        public Groups Organization { get; set; }

        public Groups Department { get; set; }

        public Groups MainTool { get; set; }

        public IEnumerable<Groups> Equipments { get; set; }
    }
}
