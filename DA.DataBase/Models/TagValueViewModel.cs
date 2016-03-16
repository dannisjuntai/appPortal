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
    }
}
