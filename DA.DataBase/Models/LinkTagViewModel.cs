using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    /// <summary>
    /// 圖控介面 select2 
    /// </summary>
    public class LinkTagViewModel
    {
        public int LinkSubSeq { get; set; }

        public int LinkTagSeq { get; set; }

        public string TagName { get; set; }

        public int MTagSeq { get; set; }

        public int TObjSeq { get; set; }

        public string TObjName { get; set; }

        public int CurValue { get; set; }

        public decimal CurfValue { get; set; }

        public byte AlarmFlag { get; set; }
        public bool IsUpAlarm { get; set; }
        public decimal? UpAlarm { get; set; }
        public bool IsLowAlarm { get; set; }
        public decimal? LowAlarm { get; set; }

        public string UnitName { get; set; }

        public string ShortName { get; set; }

        public bool Selected { get; set; }

        public byte UISelFlag { get; set; }

    }
}
