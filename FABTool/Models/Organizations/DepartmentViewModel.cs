using DA.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABTool.Models.Organizations
{
    public class DepartmentViewModel
    {

        public Groups Organization { get; set; }

        public IEnumerable<Groups> Departments { get; set; }

    }
}
