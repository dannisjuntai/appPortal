using DA.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DataBase
{
    public class CMSDBContext : DbContext
    {
        public CMSDBContext()
            : base("cmsDBEntities")
        {
            //var connectStr = Database.Connection.ConnectionString;
        }
        /// <summary>
        /// App選單
        /// </summary>
        public DbSet<AppStore> AppStore { get; set; }
        /// <summary>
        /// 群組階層
        /// </summary>
        public DbSet<Groups> Groups { get; set; }
        /// <summary>
        /// 群組類別
        /// </summary>
        public DbSet<GroupTypes> GroupTypes { get; set; }
        /// <summary>
        /// 紀錄圖控檔案
        /// </summary>
        public DbSet<GroupImages> GroupImages { get; set; }
        /// <summary>
        /// 紀錄圖檔資訊
        /// </summary>
        public DbSet<GroupLocations> GroupLocations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<LinkDevice> LinkDevice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<LinkDevSub> LinkDevSub { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<LinkTag> LinkTag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<MemTag> MemTag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<TagObj> TagObj { get; set; }
        /// <summary>
        /// 歷史資料
        /// </summary>
        public DbSet<TagHistory> TagHistory { get; set; }
        /// <summary>
        /// 事件資料
        /// </summary>
        public DbSet<EventSet> EventSet { get; set; }
        /// <summary>
        /// Attach 實作 IEFUnitOfWork
        /// 附加 Entity 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public virtual void Attach<T>(T entity) where T : class
        {
            this.GetSet<T>().Attach(entity);
        }

        /// <summary>
        /// SetModified 實作 IEFUnitOfWork
        /// 將資料標示為修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public virtual void SetModified<T>(T entity) where T : class
        {
            base.Entry<T>(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// GetSet 實作 IEFUnitOfWork
        /// 取得IDbSet實例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IDbSet<T> GetSet<T>() where T : class
        {
            return base.Set<T>();
        }
        /// <summary>
        /// 更新實體 需Find 實體物件其主鍵，不然會發生問題
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public int Update<T>(T instance, params object[] keyValues) where T : class
        {
            if (instance == null)
            {
                //throw new ArgumentNullException("instance");
                return -1;
            }

            var entry = base.Entry<T>(instance);

            if (entry.State == EntityState.Detached)
            {
                var set = base.Set<T>();

                T attachedEntity = set.Find(keyValues);

                if (attachedEntity != null)
                {
                    var attachedEntry = base.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(instance);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
            return this.SaveChanges();
        }
    }
}
