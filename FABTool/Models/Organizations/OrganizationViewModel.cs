using DA.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABTool.Models.Organizations
{
    public class OrganizationViewModel
    {
        public IEnumerable<Groups> Organizations { get; set; }
    }
}
