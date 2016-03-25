using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    public class EventSetPara
    {
        public int GroupId { get; set; }
        public DateTime SDateTime { get; set; }
        public DateTime EDateTime { get; set; }
        public int EventLevel { get; set; }
    }
}
