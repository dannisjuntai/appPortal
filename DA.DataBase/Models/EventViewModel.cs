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

        public string EventName { get; set; }
    }

    public class EventsViewModel
    {
        public IEnumerable<EventViewModel> EventSets { get; set; }

        /// <summary>
        /// 每頁數量
        /// </summary>
        public virtual decimal PagedItems
        {
            get;
            set;
        }

    }

    public class EventExport
    {
        public string FileUrl { get; set; }

        public string  FileName { get; set; }

        public string FullPath { get { return this.FileUrl + this.FileName; } }
    }
}
