using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABTool.Models.Charts
{
    public class TagsViewModel
    {
        public string Label { get; set; }

        public int Yaxis { get; set; }

        public List<TagViewModel> Data { get; set; }

    }

    /// <summary>
    /// data
    /// </summary>
    public class TagViewModel
    {
        public double X { get; set; }
        public string Y { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ChartViewModel
    {
        //public Dataset Dataset { get; set; }
        //public Options Options { get; set; }

        public List<TagsViewModel> Datasets { get; set; }
        /// <summary>
        ///   "left" or "right"
        /// </summary>
        public List<Yaxis> Yaxes { get; set; }

        public Xaxis Xaxis { get; set; }

        /// <summary>
        /// 每頁數量
        /// </summary>
        public virtual decimal PagedItems
        {
            get;
            set;
        }
    }

    public class Dataset
    {

    }

    public class Options
    {
        public List<Yaxis> Yaxes { get; set; }

    }
    public class Yaxis
    {
        public string Position { get; set; }
    }

    public class Xaxis
    {
        /// <summary>
        /// mode: "time", tickSize: [10, "minute"],
        /// </summary>
        public string Mode { get; set; }

        public KeyValuePair<int, string> TickSize { get; set; }
    }
}
