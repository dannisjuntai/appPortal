using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    /// <summary>
    /// 事件
    /// </summary>
    public class EventViewModel
    {
        public int LinkTagSeq { get; set; }

        public DateTime RecTime { get; set; }

        public DateTime? RestTime { get; set; }

        public string Name { get; set; }
    }
}
