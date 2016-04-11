using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    public class TagValueViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public double Labels { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Data { get; set; }

    }

    public class TagValuesViewModel
    {
        public List<TagValueViewModel> List { get; set; }

        public string Label { get; set; }

        public int Yaxis { get; set; }
    }

    public class Chart
    {
        public Chart()
        {
            this.Data = new List<TagValuesViewModel>();
        }
       public List<TagValuesViewModel> Data { get; set; }
    }
}
