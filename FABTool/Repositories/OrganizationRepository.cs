using DA.DataBase;
using DA.DataBase.Utilities;
using FABTool.Common;
using FABTool.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABTool.Repositories
{
    public class OrganizationRepository : IDisposable
    {
        #region 取得組織 GetOrganizations
        /// <summary>
        /// 取得組織
        /// </summary>
        /// <returns></returns>
        public OrganizationViewModel GetOrganizations()
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.ParentId == 1 &&
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
                        select a;
                if (q.Any())
                {
                    OrganizationViewModel vms = new OrganizationViewModel()
                    {
                        Organizations = q.ToList()
                    };
                    return vms;
                }
                return null;
            }
        }
        #endregion
        #region 取得部門 GetDepartments
        /// <summary>
        /// 取得部門 GetDepartment
        /// </summary>
        /// <returns></returns>
        public DepartmentViewModel GetDepartments(int groupId)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        join b in db.Groups on a.GroupId equals b.ParentId
                        where a.ParentId == 1 &&
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              b.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              a.GroupId == groupId
                        select new { a, b };
                if (q.Any())
                {
                    var o = q.ToList();

                    var orgs = o.GroupBy(p => new { p.a }).Select(s => s.Key.a);
                    var deps = o.Select(p => p.b);

                    DepartmentViewModel vms = new DepartmentViewModel()
                    {
                        Organization = orgs.FirstOrDefault(),
                        Departments = deps
                    };
                    return vms;
                }
                return null;
            }
        }
        #endregion
        #region 取得主機台 GetMainTools
        /// <summary>
        /// 取得主機台
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public MainToolViewModel GetMainTools(int groupId)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        join b in db.Groups on a.GroupId equals b.ParentId
                        join c in db.Groups on b.GroupId equals c.ParentId
                        where a.ParentId == 1 &&
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              b.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              c.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              b.GroupId == groupId
                        select new { a, b, c };
                if (q.Any())
                {
                    var o = q.ToList();

                    var orgs = o.GroupBy(p => new { p.a }).Select(s => s.Key.a);
                    var deps = o.GroupBy(p => new { p.b }).Select(s => s.Key.b);
                    var mainTools = o.Select(p => p.c).ToList();
                    List<int> mList = new List<int>();
                    foreach (var m in mainTools)
                    {
                        mList.Add(m.GroupId);
                    }
                    //計算總數

                    MainToolViewModel vms = new MainToolViewModel()
                    {
                        Organization = orgs.FirstOrDefault(),
                        Department = deps.FirstOrDefault(),
                        MainTools = getGroupStatus(GroupTypeEnum.MainTool, mList)
                    };
                    return vms;
                }
                return null;
            }
        }
        #endregion
        #region 取得設備 GetEquipments
        /// <summary>
        /// 取得設備
        /// </summary>
        /// <param name="groupId">MainTool 序號</param>
        /// <returns></returns>
        public EquipmentViewModel GetEquipments(int groupId)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups                                //組織
                        join b in db.Groups on a.GroupId equals b.ParentId //部門
                        join c in db.Groups on b.GroupId equals c.ParentId //主機台
                        join d in db.Groups on c.GroupId equals d.ParentId //設備
                        where a.ParentId == 1 &&
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              b.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              c.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              d.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              c.GroupId == groupId
                        select new { a, b, c, d };
                if (q.Any())
                {
                    var o = q.ToList();

                    var orgs = o.GroupBy(p => new { p.a }).Select(s => s.Key.a);
                    var deps = o.GroupBy(p => new { p.b }).Select(s => s.Key.b);
                    var mainTools = o.GroupBy(p => new { p.c }).Select(s => s.Key.c);
                    var equipments = o.Select(p => p.d);

                    EquipmentViewModel vms = new EquipmentViewModel()
                    {
                        Organization = orgs.FirstOrDefault(),
                        Department = deps.FirstOrDefault(),
                        MainTool = mainTools.FirstOrDefault(),
                        Equipments = equipments
                    };
                    return vms;
                }
                return null;
            }
        }
        #endregion
        /// <summary>
        /// 取得群組狀態
        /// </summary>
        /// <returns></returns>
        private List<GroupViewModel> getGroupStatus(GroupTypeEnum type, List<int> groups)
        {
            /*
             
               select  distinct(e.LinkSubSeq), f.*
    from  Groups as b								   -- 課別
	 join Groups as c on b.GroupId = c.ParentId        --MainTool(主機台)
	 join Groups as d on c.GroupId = d.ParentId        -- Equipment(設備)
	 join GroupLocations as e on d.GroupId = e.GroupId -- 圖控點位設定
	 join LinkTag as f on e.LinkSubSeq = f.LinkSubSeq  -- LinkTag
	 where c.ModifyFlag < 3
	   and d.ModifyFlag < 3
	   and e.ModifyFlag < 3
	   and f.ModifyFlag <3
	   and b.GroupId = 1038
             
             */
            List<GroupViewModel> vms = new List<GroupViewModel>();
            //query
            using(var db = new CMSDBContext())
            {
                var q = from b in db.Groups                                     //課別
                        join c in db.Groups on b.GroupId equals c.ParentId      //MainTool
                        join d in db.Groups on c.GroupId equals d.ParentId      //Equipment
                        join e in db.GroupLocations on d.GroupId equals e.GroupId
                        join f in db.LinkTag on e.LinkSubSeq equals f.LinkTagSeq
                        where b.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              c.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              d.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              e.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              f.ModifyFlag < (int)ModifyFlagEnum.Delete
                        select new { b, c, d, e, f };
                //if main tool
                if (type == GroupTypeEnum.MainTool)
                {
                    q = q.Where(p => groups.Contains(p.c.GroupId));
                    //distinct 
                    var g = q.GroupBy(p => new { p.c.GroupId, p.e.LinkSubSeq });
                    if (g.Any())
                    {
                        var obj = g.ToList();
                        //Group
                        //LinkSub
                        foreach (var o in obj)
                        {
                            //
                            GroupViewModel vm = new GroupViewModel()
                            {
                                CurLinkStaCount = 0,
                                CurSubStaCount = 0,
                                MaintainCount = 0,
                               
                            };
                            vms.Add(vm);
                        }
                    }
                }
                //if equipment
                if (type == GroupTypeEnum.Equipment)
                {
                    q = q.Where(p => groups.Contains(p.d.GroupId));
                    //distinct 
                    var g = q.GroupBy(p => new { p.d.GroupId, p.e.LinkSubSeq });
                    if (g.Any())
                    {
                        var obj = g.ToList();
                        foreach (var o in obj)
                        {
                            //
                            GroupViewModel vm = new GroupViewModel()
                            {
                                CurLinkStaCount = 0,
                                CurSubStaCount = 0,
                                MaintainCount = 0,
                            };
                            vms.Add(vm);
                        }
                    }
                }
            }
            return vms;
        }
        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {

        }
    }
}
