using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    public class TagParamViewModel
    {
        public int LinkTagSeq { get; set; }

        public DateTime StartDate { get; set; }

        public string StartTime { get; set; }

        public DateTime EndDate { get; set; }

        public string EndTime { get; set; }
    }
}
