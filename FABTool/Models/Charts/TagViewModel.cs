using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABTool.Models.Charts
{
    public class TagsViewModel
    {
        public string  Label { get; set; }

        public int Yaxis { get; set; }

        public List<TagViewModel> Data { get; set; }
    }

    public class TagViewModel
    {
        public double X { get; set; }

        public string Y { get; set; }
    }
}
