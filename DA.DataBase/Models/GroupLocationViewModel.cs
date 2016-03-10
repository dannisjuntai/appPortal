using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    public class GroupLocationViewModel
    {
        public int LocationId { get; set; }
  
        public int GroupId { get; set; }

        public int LinkSubSeq { get; set; }

        public string LinkDevName { get; set; }

        public int LinkTagSeq { get; set; }

        public string TagName { get; set; }

        public int MTagSeq { get; set; }

        public int TObjSeq { get; set; }

        public string LocationValue { get; set; }

        public decimal? CurfValue { get; set; }

        public bool Prompt { get; set; }

        public string Color { get; set; }

        public byte ModifyFlag { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUser { get; set; }

        public int SystemUser { get; set; }

        public DateTime SystemTime { get; set; }

        public string ShortName { get; set; }

        public string UnitName { get; set; }
    }
}
