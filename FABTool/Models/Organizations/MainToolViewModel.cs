using DA.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABTool.Models.Organizations
{
    public class MainToolViewModel
    {


        public Groups Organization { get; set; }

        public Groups Department { get; set; }

        public List<GroupViewModel> MainTools { get; set; }
    }
}
