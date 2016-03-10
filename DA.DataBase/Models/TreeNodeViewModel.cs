using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Models
{
    public class TreeNode
    {
        private int id;

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        private string label;

        public string Label
        {
            get { return this.label; }
            set { this.label = value; }
        }

        private List<TreeNode> children;

        public List<TreeNode> Children
        {
            get { return this.children; }
            set { this.children = value; }
        }

        private string data;

        public string Data
        {
            get { return data; }
            set { data = value; }
        }

    }
}
