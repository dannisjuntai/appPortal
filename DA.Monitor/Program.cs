using DA.DataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DA.Monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            int value = 0;
            int value2 = 65535;
            while(true)
            {
                var rep = new GroupRepository();
                if (value == 0)
                {
                    value = 1;
                    rep.SetLinkTag(1, value);
                    
                }
                else
                {
                    value =0;
                    rep.SetLinkTag(1, value);
                }
                value2 = new Random().Next(25000, 28000);

                int result = rep.SetLinkTag(38, value2);
                if (result > 0)
                {
                    Console.WriteLine(value2);
                }
                Console.WriteLine(value);
                Thread.Sleep(3000);
            }
        }
    }
}
