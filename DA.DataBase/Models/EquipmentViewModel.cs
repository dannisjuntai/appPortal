using DA.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class EquipmentViewModel
    {
        public int GroupId { get; set; }
        public string  GroupTypeKey { get; set; }

        public string Code { get; set; }
        public int Identity { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public int AlarmCount { get; set; }
        public int StatusCount { get; set; }
        public int MaintainCount { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public List<TagsViewModel> Tags { get; set; }
    }

    public class TagsViewModel
    {
        public int Identity { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public int Value { get; set; }

        public decimal? DValue  { get; set; }

    }
}
