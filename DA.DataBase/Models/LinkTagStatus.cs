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
    public class LinkTagStatus
    {
        public LinkTagStatus()
        {
            Alarm = 0;
            Status = 0;
            Maintain = 0;
        }
        public int Alarm { get; set; }
        public int Status { get; set; }
        public int Maintain { get; set; }

        public int Count
        {
            get 
            { 
                return Alarm + Status + Maintain; 
            }
        }

        public string Color
        {
            get
            {
                if (Alarm > 0)
                {
                    return "status-button red";
                }
                if (Status > 0)
                {
                    return "status-button grey";
                }
                if (Maintain > 0)
                {
                    return "status-button blue";
                }
                return "status-button green";
            }
        }
    }
}
