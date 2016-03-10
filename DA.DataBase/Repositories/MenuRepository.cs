using DA.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase.Repositories
{
    public class MenuRepository
    {
        public class Menus 
        {
            private AppStore parentApp;

            public AppStore ParentApp
            {
                get { return parentApp; }
                set { parentApp = value; }
            }
            
            private List<AppStore> items;

            public List<AppStore> Items
            {
                get { return items; }
                set { items = value; }
            }
            
            public Menus() 
            {

            }
        }

        CMSDBContext context;

        /// <summary>
        /// 建構子
        /// </summary>
        public MenuRepository()
        {
            context = new CMSDBContext();
        }

        public List<Menus> GetMenus() 
        {
            List<Menus> app = new List<Menus>();

            try
            {
                var q = from a in context.AppStore
                        where a.ParentAppNo == 0
                        select a;

                var o = q.AsQueryable();

                foreach (var p in o.ToList())
                {
                    var r = from b in context.AppStore
                            where b.ParentAppNo == p.AppNo
                            select b;

                    app.Add(new Menus()
                    {
                        ParentApp = p,
                        Items = r.ToList()
                    });
                   
                }
                return app;
            }
            catch (Exception e)
            {
                var ee = e.ToString();
            }

            return null;
        }
    }
}
