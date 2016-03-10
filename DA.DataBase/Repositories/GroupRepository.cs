using DA.DataBase.Entities;
using DA.DataBase.Models;
using DA.DataBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataBase.Utilities;
using DA.DataBase.Models.Departments;

namespace DA.DataBase.Repositories
{
    /// <summary>
    /// 群組
    /// </summary>
    public class GroupRepository
    {
        #region Constructor 建構子
        /// <summary>
        /// 建構子
        /// </summary>
        public GroupRepository()
        {

        }
        #endregion

        #region Method 方法
        /// <summary>
        /// 取得群組資料
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetGroups()
        {
            List<TreeNode> app = new List<TreeNode>();
            using (var db = new CMSDBContext())
            {

                var q = from a in db.Groups
                        where a.ParentId == 0 &&
                              a.ModifyFlag < 3
                        select a;

                if (q.Any() == true)
                {
                    var o = q.FirstOrDefault();

                    //取得下階群組
                    var children = GetGroups(o.GroupId);
                    //節點 = 0
                    var root = new TreeNode()
                    {
                        Id = o.GroupId,
                        Label = o.GroupName,
                        Children = children,
                        Data = o.GroupTypeKey
                    };

                    //加入集合
                    app.Add(new TreeNode()
                    {
                        Id = root.Id,
                        Label = root.Label,
                        Children = root.Children,
                        Data = root.Data
                    });
                }
            }

            return app;
        }
        /// <summary>
        /// 取得下階群組
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<TreeNode> GetGroups(int parentId)
        {
            List<TreeNode> node = new List<TreeNode>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.ParentId == parentId &&
                              a.ModifyFlag < 3
                        select a;
                if (q.Any() == true)
                {
                    foreach (var o in q.ToList())
                    {
                        //判斷是否還有下階
                        var p = from b in db.Groups
                                where b.ParentId == o.GroupId &&
                                      b.ModifyFlag < 3
                                select b;
                        if (p.Any() == true)
                        {
                            node.Add(new TreeNode()
                            {

                                Id = o.GroupId,
                                Label = o.GroupName,
                                Children = GetGroups(o.GroupId),
                                Data = o.GroupTypeKey
                            });
                        }
                        else
                        {
                            node.Add(new TreeNode()
                            {
                                Id = o.GroupId,
                                Label = o.GroupName,
                                Data = o.GroupTypeKey
                            });
                        }
                    }

                }
            }

            return node;
        }

        /// <summary>
        /// 取得群組明細
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public GroupViewModel GetGroupViewModel(int groupId)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.GroupId == groupId
                        select a;
                if (q.Any())
                {
                    //取得父階層資料
                    var parent = getParentGroup(q.FirstOrDefault().ParentId);

                    return new GroupViewModel()
                    {
                        ParentGroup = parent,
                        CurrentGroup = q.FirstOrDefault()
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// 取得父階層資料
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private Groups getParentGroup(int parentId)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.GroupId == parentId
                        select a;
                if (q.Any())
                {
                    return q.FirstOrDefault();
                }
                return new Groups()
                {
                    ParentId = 0,
                    GroupId = 0,
                    GroupName = ""
                };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupTypeVlaue"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetGroupTypes(string groupTypeVlaue)
        {
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();

            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupTypes
                        where (string.Compare(a.GroupTypeValue, groupTypeVlaue) >= 0)
                        orderby a.GroupTypeValue
                        select a;

                if (q.Any() == true)
                {
                    foreach (var o in q.ToList())
                    {
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>(o.GroupTypeKey, o.GroupTypeValue);
                        pairs.Add(item);
                    }
                }
            }
            return pairs;
        }
        /// <summary>
        /// 取得連結設備資料
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetLinkDevice()
        {
            List<KeyValuePair<int, string>> pairs = new List<KeyValuePair<int, string>>();

            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkDevice
                        select a;
                if (q.Any())
                {
                    foreach (var o in q.ToList())
                    {
                        KeyValuePair<int, string> item = new KeyValuePair<int, string>(o.LinkID, o.LinkDevName);
                        pairs.Add(item);
                    }
                }
                return pairs;
            }
        }
        /// <summary>
        /// 取得連結子設備資料
        /// </summary>
        /// <param name="linkId"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetLinkTagsBySubSeq(int linkSubSeq)
        {
            List<KeyValuePair<int, string>> pairs = new List<KeyValuePair<int, string>>();

            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkTag
                        join b in db.MemTag on a.MTagSeq equals b.MTagSeq
                        join c in db.TagObj on b.TObjSeq equals c.TObjSeq
                        where a.LinkSubSeq == linkSubSeq &&
                              a.ModifyFlag < 3 &&
                              b.ModifyFlag < 3
                        select new { a, b, c };

                if (q.Any())
                {
                    foreach (var o in q.ToList())
                    {
                        KeyValuePair<int, string> item = new KeyValuePair<int, string>(o.a.LinkTagSeq, o.b.TagName);
                        pairs.Add(item);

                    }
                }
                return pairs;
            }
        }
        /// <summary>
        /// 取得 LinkTag
        /// </summary>
        /// <param name="linkTagSeq"></param>
        /// <returns></returns>
        public LinkTagViewModel GetLinkTag(int linkTagSeq)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkTag
                        join b in db.MemTag on a.MTagSeq equals b.MTagSeq
                        where a.LinkTagSeq == linkTagSeq
                        select new { a, b };
                if (q.Any())
                {
                    var o = q.FirstOrDefault();
                    var link = new LinkTagViewModel()
                    {
                        LinkTagSeq = o.a.LinkTagSeq,
                        LinkSubSeq = o.a.LinkSubSeq,
                        MTagSeq = o.a.MTagSeq,
                        UpAlarm = o.a.UpAlarm,
                        LowAlarm = o.a.LowAlarm,
                        ShortName = o.b.ShortName,
                        UnitName = o.b.UnitName
                    };
                    return link;
                }
            }
            return null;
        }
        /// <summary>
        /// 設定 LinkTag 高低值
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public int SetLinkTag(LinkTagViewModel vm)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                var linkTag = db.LinkTag.Find(vm.LinkTagSeq);
                if (linkTag != null)
                {
                    linkTag.UpAlarm = vm.UpAlarm;
                    linkTag.LowAlarm = vm.LowAlarm;
                    errorCode = db.Update<LinkTag>(linkTag, linkTag.LinkTagSeq);
                }
                if (vm.MTagSeq > 0)
                {
                    errorCode = updateMemTag(vm.MTagSeq, vm.ShortName, vm.UnitName);
                }

            }
            return errorCode;
        }
        /// <summary>
        /// 更新簡稱及單位
        /// </summary>
        /// <param name="MTagSeq"></param>
        /// <param name="shortName"></param>
        /// <param name="unitName"></param>
        /// <returns></returns>
        private int updateMemTag(int MTagSeq, string shortName, string unitName)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                var memTag = db.MemTag.Find(MTagSeq);
                if (memTag != null)
                {
                    memTag.ShortName = shortName;
                    memTag.UnitName = unitName;
                    errorCode = db.Update<MemTag>(memTag, memTag.MTagSeq);
                }
            }
            return errorCode;
        }
        /// <summary>
        /// 取得圖控介面 select 1
        /// </summary>
        /// <returns></returns>
        public List<LinkDeviceViewModel> GetLinkDevices()
        {
            List<LinkDeviceViewModel> vms = new List<LinkDeviceViewModel>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkDevice
                        join b in db.LinkDevSub on a.LinkID equals b.LinkID
                        where a.ModifyFlag < 3 &&
                              b.ModifyFlag < 3
                        select new { a.LinkID, a.LinkDevName, b.LinkSubSeq, b.LinkSubName };
                if (q.Any())
                {
                    foreach (var o in q.ToList())
                    {
                        var ld = new LinkDeviceViewModel()
                        {
                            LinkID = o.LinkID,
                            LinkDevName = string.Format("{0} - {1}", o.LinkDevName, o.LinkSubName),
                            LinkSubSeq = o.LinkSubSeq
                        };
                        vms.Add(ld);
                    }
                }
            }
            return vms;
        }
        /// <summary>
        /// 取得圖控介面 select 2
        /// </summary>
        /// <param name="linkSubSeq"></param>
        /// <returns></returns>
        public List<LinkTagViewModel> GetLinkTags(int linkSubSeq)
        {
            List<LinkTagViewModel> vms = new List<LinkTagViewModel>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkTag
                        join b in db.MemTag on a.MTagSeq equals b.MTagSeq
                        join c in db.TagObj on b.TObjSeq equals c.TObjSeq
                        where a.LinkSubSeq == linkSubSeq &&
                              a.ModifyFlag < 3 &&
                              b.ModifyFlag < 3
                        select new { a, b, c };
                if (q.Any())
                {
                    foreach (var o in q.ToList())
                    {
                        var lt = new LinkTagViewModel()
                        {
                            LinkSubSeq = o.a.LinkSubSeq,
                            LinkTagSeq = o.a.LinkTagSeq,
                            MTagSeq = o.b.MTagSeq,
                            TagName = o.b.TagName,
                            TObjSeq = o.c.TObjSeq,
                            TObjName = o.c.TObjName
                        };
                        vms.Add(lt);

                    }
                }
            }
            return vms;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<TagAlramViewModel> GetTagAlarm(int groupId)
        {
            List<TagAlramViewModel> vms = new List<TagAlramViewModel>();
            //group
            using (var db = new CMSDBContext())
            {
                var g = getGroupMapLinkSubSeq(groupId);
                //var g = getGroups(groupId);
                ////取得本階
                //var lastGroup = getGroup(groupId);
                //if (lastGroup != null)
                //{
                //    g.Add(lastGroup);
                //}
                foreach (var obj in g.ToList())
                {
                    var q = from a in db.LinkTag
                            join b in db.MemTag on a.MTagSeq equals b.MTagSeq
                            join c in db.TagObj on b.TObjSeq equals c.TObjSeq
                            where a.LinkSubSeq == obj
                            select new { a, b, c };
                    if (q.Any())
                    {
                        foreach (var t in q.ToList())
                        {
                            TagAlramViewModel tag = new TagAlramViewModel()
                            {
                                LinkTagSeq = t.a.LinkTagSeq,
                                GroupId = t.a.GroupId,
                                CurValue = t.a.CurValue,
                                CurfValue = t.a.CurfValue,
                                TagName = t.b.TagName,
                                TObjName = t.c.TObjName,
                                CurLinkSta = t.a.CurLinkSta,
                                CurLinkStaName = t.a.CurLinkSta.GetCurLinkStaName(),
                                CurSubSta = t.a.CurSubSta,
                                CurSubStaName = t.a.CurSubSta.GetCurSubStaName(),
                                IsCurSubSta = false,
                                Maintain = t.a.Maintain,
                                MaintainName = t.a.Maintain.GetMaintainName(),
                                IsMaintain = t.a.Maintain.GetMaintain()
                            };
                            //斷線
                            if (tag.CurLinkSta != 1)
                            {
                                vms.Add(tag);
                                continue;
                            }
                            //Alarm
                            if (tag.CurSubSta >= 2)
                            {
                                vms.Add(tag);
                                continue;
                            }
                            if (tag.Maintain > 0)
                            {
                                //不顯示保養
                                vms.Add(tag);
                                continue;
                            }
                        }
                    }
                }
            }
            return vms;
        }

        /// <summary>
        /// 取得Group對照 LinkSubSeq
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private List<int> getGroupMapLinkSubSeq(int groupId)
        {
            List<int> linkSubs = new List<int>();
            //取得本階
            var g = getGroupWithLinkSub(groupId).GroupBy(p => p.LinkSubSeq);
            
            foreach(var o in g.ToList())
            {
                linkSubs.Add(o.Key);
            }
            return linkSubs;
        }
        /// <summary>
        /// 取得本階
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private Groups getGroup(int groupId)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.GroupId == groupId &&
                              a.ModifyFlag < 3
                        select a;
                if (q.Any())
                {
                    return q.FirstOrDefault();
                }
            }
            return null;
        }
        #endregion

        #region GroupLocations
        /// <summary>
        /// 取得圖控訊息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<GroupLocationViewModel> GetGroupLocations(int groupId)
        {
            List<GroupLocationViewModel> vms = new List<GroupLocationViewModel>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupLocations
                        join b in db.LinkDevSub on a.LinkSubSeq equals b.LinkSubSeq
                        join c in db.LinkDevice on b.LinkID equals c.LinkID
                        join d in db.LinkTag on a.LinkTagSeq equals d.LinkTagSeq
                        join e in db.MemTag on a.MTagSeq equals e.MTagSeq
                        where a.GroupId == groupId &&
                              a.ModifyFlag < 3
                        select new { a, b, c, d, e };
                if (q.Any())
                {
                    foreach (var o in q.ToList())
                    {
                        var vm = new GroupLocationViewModel()
                        {
                            LocationId = o.a.LocationId,
                            GroupId = o.a.GroupId,
                            LinkSubSeq = o.a.LinkSubSeq,
                            LinkDevName = string.Format("{0}-{1}", o.c.LinkDevName, o.b.LinkSubName),
                            LinkTagSeq = o.a.LinkTagSeq,
                            TagName = o.e.TagName,
                            MTagSeq = o.a.MTagSeq,
                            TObjSeq = o.a.TObjSeq,
                            LocationValue = o.a.LocationValue,
                            CurfValue = o.d.CurfValue,
                            Prompt = o.a.Prompt,
                            Color = getTagColor(o.d),
                            ModifyFlag = o.a.ModifyFlag,
                            CreateTime = o.a.CreateTime,
                            CreateUser = o.a.CreateUser,
                            SystemUser = o.a.SystemUser,
                            SystemTime = o.a.SystemTime,
                            ShortName = o.e.ShortName,
                            UnitName = o.e.UnitName.GetNullString()
                        };
                        vms.Add(vm);
                    }
                }

                return vms;
            }
        }
        /// <summary>
        /// 取得Tag 顏色
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string getTagColor(LinkTag tag)
        {
            LinkTagStatus status = new LinkTagStatus();
            //alram
            if (tag.CurSubSta > 1)
            {
                //red
                return "#E25A59";
            }
            if (tag.CurLinkSta != 1)
            {
                //grey
                return "#7F8C8D";
            }
            if (tag.Maintain == 1)
            {
                //blue
                return "#578EBE";
            }
            //green
            return "#43B5AD";
        }
        /// <summary>
        /// 設定圖控資料
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public List<GroupLocationViewModel> SetGroupLocations(List<GroupLocationViewModel> locations)
        {
            int errorCode = 0;
            int groupId = locations.FirstOrDefault().GroupId;

            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupLocations
                        where a.GroupId == groupId &&
                              a.ModifyFlag < 3
                        select a;
                if (q.Any())
                {
                    errorCode = updateGroupLocations(locations);
                }
                //第一次設定圖控
                else
                {
                    //insert
                    foreach (var o in locations)
                    {
                        //空陣列 沒有群組編號
                        if (o.GroupId <= 0)
                        {
                            continue;
                        }
                        errorCode = insertGroupLocation(o);
                        if (errorCode <= 0)
                        {
                            return null;
                        }
                    }
                }
                if (errorCode > 0)
                {

                    return GetGroupLocations(groupId);
                }
            }
            return null;
        }
        /// <summary>
        /// 更新群組
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private int updateLinkTag(GroupLocationViewModel vm)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {

                var obj = db.LinkTag.Find(vm.LinkTagSeq);
                if (obj != null)
                {
                    obj.GroupId = vm.GroupId;
                    obj.ModifyFlag = 2;

                }
                errorCode = db.Update<LinkTag>(obj, obj.LinkTagSeq);
            }
            return errorCode;
        }
        /// <summary>
        /// 新增圖控資料
        /// </summary>
        /// <returns></returns>
        private int insertGroupLocation(GroupLocationViewModel vm)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {

                var location = new GroupLocations()
                {
                    GroupId = vm.GroupId,
                    LinkSubSeq = vm.LinkSubSeq,
                    LinkTagSeq = vm.LinkTagSeq,
                    MTagSeq = vm.MTagSeq,
                    TObjSeq = vm.TObjSeq,
                    LocationValue = vm.LocationValue,
                    Prompt = vm.Prompt,
                    ModifyFlag = 0,
                    CreateTime = DateTime.Now,
                    CreateUser = 0,
                    SystemUser = 0,
                    SystemTime = DateTime.Now
                };
                db.GroupLocations.Add(location);
                errorCode = db.SaveChanges();
                if (errorCode <= 0)
                {
                    return errorCode;
                }
                //更新 LinkTag GroupId
                updateLinkTag(vm);
            }
            return errorCode;
        }
        /// <summary>
        /// 更新圖控資料
        /// </summary>
        /// <param name="vms"></param>
        /// <returns></returns>
        private int updateGroupLocations(List<GroupLocationViewModel> vms)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                foreach (var o in vms)
                {
                    //新增
                    if (o.LocationId == 0)
                    {
                        errorCode = insertGroupLocation(o);
                        if (errorCode <= 0)
                        {
                            return 0;
                        }
                    }
                    //更新
                    else
                    {
                        errorCode = updateGroupLocation(o);
                        if (errorCode <= 0)
                        {
                            return 0;
                        }
                    }

                }
            }
            return errorCode;
        }
        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private int updateGroupLocation(GroupLocationViewModel vm)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                var obj = db.GroupLocations.Find(vm.LocationId);
                if (obj != null)
                {
                    obj.LocationValue = vm.LocationValue;
                    obj.ModifyFlag = 1;
                    obj.SystemTime = DateTime.Now;
                }
                errorCode = db.Update<GroupLocations>(obj, obj.LocationId);
                if (errorCode <= 0)
                {
                    return errorCode;
                }
                //更新 LinkTag GroupId
                updateLinkTag(vm);
            }
            return errorCode;
        }
        /// <summary>
        /// 刪除圖控資訊
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public List<GroupLocationViewModel> DeleteGroupLocation(GroupLocationViewModel vm)
        {
            using (var db = new CMSDBContext())
            {
                var obj = db.GroupLocations.Find(vm.LocationId);
                if (obj != null)
                {
                    //obj.LocationValue = vm.LocationValue;
                    obj.ModifyFlag = 3;
                    obj.SystemTime = DateTime.Now;
                }
                int errorCode = db.Update<GroupLocations>(obj, obj.LocationId);
                if (errorCode > 0)
                {
                    //刪除時，更新 LinkTag GroupId
                    vm.GroupId = 0;
                    updateLinkTag(vm);
                    return GetGroupLocations(obj.GroupId);
                }
            }
            return null;
        }
        #endregion
        /// <summary>
        /// 更新群組
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public int UpdateGroup(GroupViewModel group)
        {
            using (var db = new CMSDBContext())
            {
                try
                {
                    group.CurrentGroup.SystemTime = DateTime.Now;
                    return db.Update<Groups>(group.CurrentGroup, group.CurrentGroup.GroupId);
                }
                catch (Exception e)
                {
                }
                return 0;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public int InsertGroup(GroupViewModel group)
        {
            using (var db = new CMSDBContext())
            {
                try
                {
                    group.CurrentGroup.CreateTime = DateTime.Now;
                    group.CurrentGroup.SystemTime = DateTime.Now;
                    db.Groups.Add(group.CurrentGroup);
                    return db.SaveChanges();
                }
                catch (Exception e)
                {
                }
                return 0;
            }
        }

        public GroupImages SaveBackground(GroupImages image)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupImages
                        where a.ImageId == image.ImageId
                        select a;
                if (q.Any())
                {
                    //Update
                    image.ImageId = q.FirstOrDefault().ImageId;
                    image.CreateTime = q.FirstOrDefault().CreateTime;

                    return UpdateGroupImages(image);
                }
                else
                {
                    //Insert
                    //InsertGroupImages(image);
                    return InsertGroupImages(image);
                }
            }
        }
        /// <summary>
        /// 新增圖檔
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private GroupImages InsertGroupImages(GroupImages image)
        {
            using (var db = new CMSDBContext())
            {
                try
                {
                    var img = db.GroupImages.Find(image.GroupId);

                    if (img == null)
                    {
                        image.CreateTime = DateTime.Now;
                        image.SystemTime = DateTime.Now;

                        image.Data = image.Base64.GetBytes();
                        db.GroupImages.Add(image);
                        var errorCode = db.SaveChanges();
                        if (errorCode > 0)
                        {
                            return GetGroupImages(image);
                        }
                    }

                    return null;
                }
                catch (Exception e)
                {
                }
                return null;
            }
        }
        /// <summary>
        /// 更新圖檔
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private GroupImages UpdateGroupImages(GroupImages image)
        {
            using (var db = new CMSDBContext())
            {
                try
                {

                    image.SystemTime = DateTime.Now;
                    image.Data = image.Base64.GetBytes();
                    var errorCode = db.Update<GroupImages>(image, image.ImageId);
                    if (errorCode > 0)
                    {
                        return GetGroupImages(image);
                    }
                    return null;
                }
                catch (Exception e)
                {
                }
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private GroupImages GetGroupImages(GroupImages image)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupImages
                        where a.GroupId == image.GroupId
                        select a;
                if (q.Any())
                {
                    q.FirstOrDefault().Base64 = q.FirstOrDefault().Data.GetString();
                    return q.FirstOrDefault();
                }
                return null;
            }
        }
        /// <summary>
        /// 取得圖檔圖檔資料
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public GroupImages GetGroupImages(int groupId)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupImages
                        where a.GroupId == groupId
                        select a;
                if (q.Any())
                {
                    var o = q.FirstOrDefault();
                    //Byte 轉成 字串
                    o.Base64 = o.Data.GetString();
                    return o;
                }
                return null;
            }

        }
        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public int DeleteGroup(int groupId)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.GroupId == groupId
                        select a;
                if (q.Any())
                {
                    var o = q.FirstOrDefault();
                    o.ModifyFlag = 3;
                    o.SystemTime = DateTime.Now;
                    return db.SaveChanges();
                }
                return 0;
            }
        }

        #region 狀態顯示
        /// <summary>
        /// 取得圖控資訊
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<EquipmentViewModel> GetLinkGroups(int groupId)
        {
            List<EquipmentViewModel> vms = new List<EquipmentViewModel>();
            //判斷點選是什麼類型
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.GroupId == groupId
                        select a;
                if (q.Any())
                {
                    var o = q.FirstOrDefault();
                    if (o.GroupTypeKey == "2")
                    {
                        return getEquipment(o);
                    }
                    else
                    {
                        return getDepartments(o);
                    }
                }
            }
            return vms;
        }
        /// <summary>
        /// 依類型回傳顯示內容
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private List<EquipmentViewModel> getDepartments(Groups groups)
        {
            List<EquipmentViewModel> vms = new List<EquipmentViewModel>();

            var g = getGroups(groups.GroupId);

            foreach (var o in g.ToList())
            {
                //GroupType 是 Equipment 類別
                if (o.GroupTypeKey == "2")
                {
                    vms.Add(new EquipmentViewModel()
                    {
                        Identity = o.GroupId,
                        Parent = getParentGroup(o.ParentId).GroupName,
                        Name = o.GroupName,
                        Tags = getTags(o.GroupId, true)
                    });
                }
            }


            return vms;
        }
        /// <summary>
        /// 取得樹狀階層
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private List<Groups> getGroups(int groupId)
        {
            List<Groups> groups = new List<Groups>();

            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.ParentId == groupId &&
                              a.ModifyFlag < 3
                        select a;
                if (q.Any() == true)
                {
                    foreach (var o in q.ToList())
                    {
                        //判斷是否還有下階
                        var p = from b in db.Groups
                                where b.ParentId == o.GroupId &&
                                      b.ModifyFlag < 3
                                select b;

                        if (p.Any() == true)
                        {
                            var g = getGroups(o.GroupId);
                            groups.AddRange(g);
                        }
                        groups.Add(o);
                    }
                }
            }
            return groups;
        }
        /// <summary>
        /// 取得Group 對照到的 LinkSubSeq
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private List<GroupModel> getGroupWithLinkSub(int groupId)
        {
            List<GroupModel> groups = new List<GroupModel>();

            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        join b in db.GroupLocations on a.GroupId equals b.GroupId into p
                        from r in p.DefaultIfEmpty()
                        where a.ParentId == groupId &&
                              a.ModifyFlag < 3
                        select new { a, r };
                //下階有資料
                if (q.Any() == true)
                {
                    foreach (var o in q.ToList())
                    {
                        if (o.r != null)
                        {
                            if (o.r.ModifyFlag >= 3)
                            {
                                continue;
                            }
                        }
                        //判斷是否還有下階
                        var p = from b in db.Groups
                                where b.ParentId == o.a.GroupId &&
                                      b.ModifyFlag < 3
                                select b;
                        var g = new GroupModel()
                        {
                            GroupId = o.a.GroupId,
                            GroupName = o.a.GroupName,
                            LinkSubSeq = o.r == null ? 0 : o.r.LinkSubSeq
                        };
                        if (p.Any() == true)
                        {
                            var subGroups = getGroupWithLinkSub(o.a.GroupId);
                            groups.AddRange(subGroups);
                        }
                        groups.Add(g);
                    }
                }
                else
                {
                    var q1 = from a in db.Groups
                             join b in db.GroupLocations on a.GroupId equals b.GroupId
                             where a.GroupId == groupId &&
                                   a.ModifyFlag < 3
                             select new { a, b };

                    if (q1.Any() == true)
                    {
                        foreach (var o in q1.ToList())
                        {
                            var g = new GroupModel()
                            {
                                GroupId = o.a.GroupId,
                                GroupName = o.a.GroupName,
                                LinkSubSeq = o.b.LinkSubSeq
                            };
                            groups.Add(g);
                        }
   
                    }

                }
            }

            return groups;
        }
        /// <summary>
        /// 取得 Equipment Tags 狀態
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private List<TagsViewModel> getTags(int groupId, bool prompt)
        {
            List<TagsViewModel> tags = new List<TagsViewModel>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupLocations
                        join b in db.LinkDevSub on a.LinkSubSeq equals b.LinkSubSeq
                        join c in db.LinkTag on a.LinkTagSeq equals c.LinkTagSeq
                        join d in db.TagObj on a.TObjSeq equals d.TObjSeq
                        join e in db.MemTag on a.MTagSeq equals e.MTagSeq
                        where a.GroupId == groupId &&
                              a.ModifyFlag < 3
                        select new { a, b, c, d, e };
                if (prompt == true)
                {
                    q = q.Where(p => p.a.Prompt == prompt);
                }

                //
                if (q.Any())
                {
                    foreach (var o in q.ToList())
                    {
                        tags.Add(new TagsViewModel()
                        {
                            Identity = o.a.LocationId,
                            Name = o.e.TagName,
                            Value = o.c.CurValue,
                            DValue = o.c.CurfValue,
                            Color = getTObjColor(o.d.TObjSeq, o.c.CurValue)
                        });
                    }
                }
            }
            return tags;
        }

        private string getTObjColor(int tObjSeq, int value)
        {
            string color;
            switch (tObjSeq)
            {
                case 1:
                case 3:
                    if (value == 0)
                    {
                        color = "badge badge-success";
                    }
                    else
                    {
                        color = "badge badge-danger";
                    }
                    break;
                case 2:
                case 4:
                    if (value == 0)
                    {
                        color = "badge badge-info";
                    }
                    else if (value == 1)
                    {
                        color = "badge badge-warning";
                    }
                    else
                    {
                        color = "badge badge-danger";
                    }
                    break;
                case 9:
                    if (value > 25000 && value <= 26000)
                    {
                        color = "badge badge-danger";
                    }
                    else if (value > 26000 && value < 27000)
                    {
                        color = "badge badge-warning";
                    }
                    else
                    {
                        color = "badge badge-info";
                    }
                    break;
                default:
                    color = "badge badge-info";
                    break;

            }

            return color;
        }

        /// <summary>
        /// 取得設備狀態
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private List<EquipmentViewModel> getEquipment(Groups group)
        {
            List<EquipmentViewModel> vms = new List<EquipmentViewModel>();
            vms.Add(new EquipmentViewModel()
            {
                Identity = group.GroupId,
                Name = group.GroupName,
                Tags = getTags(group.GroupId, false)
            });
            return vms;
        }
        #endregion
        /// <summary>
        /// 模擬資料
        /// </summary>
        /// <param name="linkTagSeq"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int SetLinkTag(int linkTagSeq, int value)
        {
            int result = 0;

            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkTag
                        where a.LinkTagSeq == linkTagSeq
                        select a;

                if (q.Any())
                {
                    var obj = db.LinkTag.Find(linkTagSeq);
                    if (obj != null)
                    {
                        obj.CurValue = value;
                        obj.ModifyFlag = 2;

                    }
                    result = db.Update<LinkTag>(obj, obj.LinkTagSeq);
                }
            }
            return result;
        }
        /// <summary>
        /// 取得歷史紀錄
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="locationId"></param>
        /// <param name="limitDt"></param>
        /// <returns></returns>
        public List<TagValueViewModel> GetTagHistories(int groupId, int locationId, int limitDt)
        {
            List<TagValueViewModel> tags = new List<TagValueViewModel>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupLocations
                        join b in db.TagHistory on a.LinkTagSeq equals b.LinkTagSeq
                        where a.GroupId == groupId &
                              a.LocationId == locationId &
                              b.RecTime >= new DateTime(2015, 12, 20)
                        orderby b.RecTime
                        select new { a, b };
                if (q.Any())
                {
                    int count = 0;
                    
                    foreach (var obj in q.OrderByDescending(p => p.b.RecTime).Take(100))
                    {
                        //minute = obj.b.RecTime.ToString("mm:ss")
                        TagValueViewModel tag = new TagValueViewModel()
                        {
                            Labels = obj.b.RecTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds,
                            Data = obj.b.fValue.ToString()
                        };

                        tags.Add(tag);
                        count++;
                        if (count > 30)
                        {
                            break;
                        }
                    }
                }
            }

            return tags;
        }
        /// <summary>
        /// 取得歷史資料
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<TagValueViewModel> GetTagHistories(TagParamViewModel param)
        {
            List<TagValueViewModel> tags = new List<TagValueViewModel>();
            string[] time = param.StartTime.Split(':');
            //開始時間
            DateTime sdt = new DateTime(param.StartDate.Year, param.StartDate.Month, param.StartDate.Day,
                int.Parse(time[0]), int.Parse(time[1]), 0);
            //結束時間
            time = param.EndTime.Split(':');
            var edt = new DateTime(param.EndDate.Year, param.EndDate.Month, param.EndDate.Day,
                int.Parse(time[0]), int.Parse(time[1]), 0);
            using (var db = new CMSDBContext())
            {
                var q = from a in db.TagHistory
                        where a.RecTime >= sdt &&
                             a.RecTime <= edt &&
                             a.LinkTagSeq == param.LinkTagSeq
                        select a;
                if (q.Any())
                {
                    foreach (var o in q.ToList().Take(1000))
                    {
                        TagValueViewModel tag = new TagValueViewModel()
                        {
                           
                            Labels =  o.RecTime.ToJavascriptTimestamp(),
                            Data = o.fValue.ToString()
                        };
                        tags.Add(tag);
                    }
                }
            }
            return tags;
        }
        /// <summary>
        /// 設定機台維護狀態
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int SetMaintain(int groupId)
        {
            int errorCode = 0;
            byte maintain = 0;
            using (var db = new CMSDBContext())
            {
                //找到是否有維護
                var q = from a in db.EventSet
                        where a.GroupId == groupId &&
                        a.RestTime == null
                        select a;
                if (q.Any())
                {
                    //更新 回復時間
                    errorCode = updateEventSet(q.FirstOrDefault().EventNo);
                }
                else
                {
                    maintain = 1;
                    //新增維護事件
                    errorCode = insertEvents(groupId);
                }
                if (errorCode > 0)
                {
                    //更新維護狀態
                    updateLinkTag(groupId, maintain);
                }
            }
            return errorCode;
        }
        /// <summary>
        /// 更新維護狀態
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="maintain"></param>
        private int updateLinkTag(int groupId, byte maintain)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkTag
                        where a.GroupId == groupId
                        select a;
                if (q.Any())
                {
                    foreach (var o in q.ToList())
                    {
                        var link = db.LinkTag.Find(o.LinkTagSeq);
                        if (link != null)
                        {
                            link.Maintain = maintain;
                            errorCode = db.Update<LinkTag>(link, link.LinkTagSeq);
                            if (errorCode <= 0)
                            {
                                return errorCode;
                            }
                        }
                    }
                }
            }
            return errorCode;
        }
        /// <summary>
        /// 新增維護事件
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private int insertEvents(int groupId)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.GroupId == groupId
                        select a;
                if (q.Any())
                {
                    var e = new EventSet()
                    {
                        EventLevel = 1,
                        GroupId = groupId,
                        LinkTagSeq = 0,
                        RecTime = DateTime.Now,
                        Name = q.FirstOrDefault().GroupName + "維護",
                        Value = "1"
                    };
                    db.EventSet.Add(e);
                    errorCode = db.SaveChanges();
                }
            }
            return errorCode;
        }
        /// <summary>
        /// 更新 EventSet 回復時間
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private int updateEventSet(int eventNo)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                var eventSet = db.EventSet.Find(eventNo);
                if (eventSet != null)
                {
                    eventSet.RestTime = DateTime.Now;
                    errorCode = db.Update<EventSet>(eventSet, eventSet.EventNo);
                }
            }
            return errorCode;
        }
        /// <summary>
        /// 顯示tag value
        /// </summary>
        public class TagValue
        {
            public DateTime CurrentDt { get; set; }
            public float Value { get; set; }
        }
        /// <summary>
        /// 判斷是否不同值
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="nextValue"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private bool getThenSameData(TagValue currentValue, TagValue nextValue, int range)
        {
            long diff = currentValue.CurrentDt.Second - nextValue.CurrentDt.Second;
            return false;
        }
        /// <summary>
        /// 取得部門資料
        /// </summary>
        /// <returns></returns>
        public List<DepartmentViewModel> GetDepartments()
        {
            List<DepartmentViewModel> departments = new List<DepartmentViewModel>();

            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.ParentId == 1 &&
                              a.ModifyFlag < 3
                        select a;
                if (q.Any())
                {
                    foreach (var obj in q.ToList())
                    {
                        int count = getEventSets(obj.GroupId);
                        string color = "panel panel-primary";
                        if (count > 0)
                        {
                            color = "panel panel-red";
                        }

                        DepartmentViewModel department = new DepartmentViewModel()
                        {
                            GroupId = obj.GroupId,
                            Code = obj.GroupCode,
                            Name = obj.GroupName,
                            Counts = count,
                            Color = color,
                            MainTools = getMainTools(obj.GroupId)
                        };
                        departments.Add(department);
                    }
                }
            }
            return departments;
        }
        /// <summary>
        /// 取得部門資料
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<DepartmentViewModel> GetDepartments(int groupId)
        {
            List<DepartmentViewModel> dts = new List<DepartmentViewModel>();
            using (var db = new CMSDBContext())
            {
                //找尋群組資料
                var q = from a in db.Groups
                        where a.ParentId == groupId &&
                              a.ModifyFlag < 3
                        select a;
                if (q.Any())
                {
                    foreach (var obj in q.ToList())
                    {
                        DepartmentViewModel dt = new DepartmentViewModel()
                        {
                            GroupId = obj.GroupId,
                            GroupTypeKey = obj.GroupTypeKey,
                            Code = obj.GroupCode,
                            Name = obj.GroupName,
                            Counts = 0,
                            Color = "quick-button metro " + obj.Color,
                            Icon = obj.Icon
                            //MainTools = getMainTools(obj.GroupId)
                        };
                        dts.Add(dt);
                    }
                }
            }
            return dts;
        }

        /// <summary>
        /// 取得 Main Tool
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<MainToolViewModel> GetMainTools(int groupId)
        {
            List<MainToolViewModel> mts = new List<MainToolViewModel>();
            using (var db = new CMSDBContext())
            {
                //找尋群組資料
                var q = from a in db.Groups
                        where a.ParentId == groupId &&
                              a.ModifyFlag < 3
                        select a;

                if (q.Any())
                {
                    foreach (var obj in q.ToList())
                    {
                        var status = getLinkTagAlarm(obj.GroupId);

                        MainToolViewModel mt = new MainToolViewModel()
                        {
                            GroupId = obj.GroupId,
                            GroupTypeKey = obj.GroupTypeKey,
                            Code = obj.GroupCode,
                            Name = obj.GroupName,
                            //取得下階訊息總數
                            AlarmCount = status.Alarm,
                            StatusCount = status.Status,
                            MaintainCount = status.Maintain,
                            Color = status.Color
                            //取得main tool 事件
                            //MainTools = getMainTools(obj.GroupId)
                        };
                        mts.Add(mt);
                    }
                }
            }
            return mts;
        }
        /// <summary>
        /// 取得告警
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private LinkTagStatus getLinkTagAlarm(int groupId)
        {
            LinkTagStatus status = new LinkTagStatus();
            List<TagStatus> ts = new List<TagStatus>();
            using (var db = new CMSDBContext())
            {
                var g = getGroupMapLinkSubSeq(groupId);
                //var g = getGroups(groupId);
                ////
                ////取得本階
                //var lastGroup = getGroup(groupId);
                //if (lastGroup != null)
                //{
                //    g.Add(lastGroup);
                //}
                foreach (var o in g.ToList())
                {
                    var q = from a in db.LinkTag
                            where a.LinkSubSeq == o &&
                                  a.ModifyFlag < 3
                            select a;
                    if (q.Any())
                    {
                        foreach (var t in q.ToList())
                        {
                            TagStatus tag = new TagStatus()
                            {
                                LinkTagSeq = t.LinkTagSeq,
                                CurSubSta = t.CurSubSta,
                                Maintain = t.Maintain,
                                CurLinkSta = t.CurLinkSta
                            };
                            ts.Add(tag);
                        }

                    }
                }
            }

            //取得 alarm 總數
            var alarmCount = from a in ts
                             where a.CurSubSta > 1
                             group a by new { a.LinkTagSeq } into g
                             select new { Count = g.Count() };

            //取得 LinkSta總數
            var linkStaCount = from a in ts
                               where a.CurLinkSta != 1
                               group a by new { a.LinkTagSeq } into g
                               select new { Count = g.Count() };

            //取得 Maintain 數  > 1
            var maintainCount = from a in ts
                                where a.Maintain == 1
                                group a by new { a.LinkTagSeq } into g
                                select new { Count = g.Count() };

            status.Alarm = alarmCount.ToList().Count();
            status.Status = linkStaCount.ToList().Count();
            status.Maintain = maintainCount.ToList().Count();

            return status;
        }
        /// <summary>
        /// 取得 equipment 集合
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<EquipmentViewModel> GetEquipments(int groupId)
        {
            List<EquipmentViewModel> ets = new List<EquipmentViewModel>();
            using (var db = new CMSDBContext())
            {
                //找尋群組資料
                var q = from a in db.Groups
                        where a.ParentId == groupId &&
                              a.ModifyFlag < 3
                        select a;

                if (q.Any())
                {
                    var o = q.ToList();
                    foreach (var obj in q.ToList())
                    {
                        var status = getLinkTagAlarm(obj.GroupId);

                        EquipmentViewModel et = new EquipmentViewModel()
                        {
                            GroupId = obj.GroupId,
                            GroupTypeKey = obj.GroupTypeKey,
                            Code = obj.GroupCode,
                            Name = obj.GroupName,
                            AlarmCount = status.Alarm,
                            StatusCount = status.Status,
                            MaintainCount = status.Maintain,
                            Color = status.Color
                            //取得main tool 事件
                            //MainTools = getMainTools(obj.GroupId)
                        };
                        ets.Add(et);
                    }
                }
            }

            return ets;
        }
        /// <summary>
        /// 取得告警次數
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private int getEventSets(int groupId)
        {
            var g = getGroups(groupId);
            int count = 0;
            foreach (var o in g.ToList())
            {
                using (var db = new CMSDBContext())
                {
                    var q = from a in db.EventSet
                            where a.GroupId == o.GroupId &&
                                  a.ConfirmTime.Value == null
                            select a;
                    count = count + q.Count();
                }
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<EventViewModel> GetEvents(int groupId)
        {
            List<EventViewModel> events = new List<EventViewModel>();
            using (var db = new CMSDBContext())
            {
                var g = getGroups(groupId);
                //取得本階
                var lastGroup = getGroup(groupId);
                if (lastGroup != null)
                {
                    g.Add(lastGroup);
                }
                foreach (var o in g.ToList())
                {
                    var q = from a in db.EventSet
                            where a.GroupId == o.GroupId &&
                                  a.ConfirmTime.Value == null
                            orderby a.RestTime descending
                            select a;
                    if (q.Any())
                    {
                        foreach (var p in q.ToList())
                        {
                            EventViewModel e = new EventViewModel()
                            {
                                LinkTagSeq = p.LinkTagSeq,
                                RecTime = p.RecTime,
                                RestTime = p.RestTime,
                                Name = p.Name
                            };
                            events.Add(e);

                        }

                    }
                }

            }
            return events;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int SetEvents(int groupId)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                var g = getGroups(groupId);
                foreach (var o in g.ToList())
                {
                    var q = from a in db.EventSet
                            where a.GroupId == o.GroupId &&
                                  a.ConfirmTime.Value == null
                            select a;
                    if (q.Any())
                    {
                        foreach (var e in q.ToList())
                        {
                            var eventSet = db.EventSet.Find(e.EventNo);
                            if (eventSet != null)
                            {
                                eventSet.ConfirmTime = DateTime.Now;
                                errorCode = db.Update<EventSet>(eventSet, eventSet.EventNo);
                                if (errorCode <= 0)
                                {
                                    return errorCode;
                                }
                            }
                        }
                    }
                }
            }
            return errorCode;
        }
        /// <summary>
        /// 取得MainTool 資料
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private List<MainToolViewModel> getMainTools(int groupId)
        {
            List<MainToolViewModel> mainTools = new List<MainToolViewModel>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.ParentId == groupId &&
                              a.ModifyFlag < 3
                        select a;
                if (q.Any())
                {
                    foreach (var obj in q.ToList())
                    {
                        MainToolViewModel mailTool = new MainToolViewModel()
                        {
                            GroupId = obj.GroupId,
                            Name = obj.GroupName,
                            Code = obj.GroupCode
                            //Count = getEventSets(obj.GroupId),
                            //Color = "badge badge-success"
                        };
                        mainTools.Add(mailTool);
                    }
                }
            }
            return mainTools;
        }
    }
}
