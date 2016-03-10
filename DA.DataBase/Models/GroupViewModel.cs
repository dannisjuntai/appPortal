using DA.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    public class GroupViewModel
    {

        /// <summary>
        /// 父階群組
        /// </summary>
        public Groups ParentGroup {get;set;}

        /// <summary>
        /// 本階群組
        /// </summary>
        public Groups CurrentGroup { get; set; }

        //private List<GroupViewModel> children;

        //public List<GroupViewModel> Children
        //{
        //    get { return this.children; }
        //    set { this.children = value; }
        //}
    }
    /// <summary>
    /// 群組模組
    /// </summary>
    public class GroupModel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int LinkSubSeq { get; set; }
    }
}
