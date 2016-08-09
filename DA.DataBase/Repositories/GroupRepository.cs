using DA.DataBase.Entities;
using DA.DataBase.Models;
using DA.DataBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.DataBase.Models.Departments;
using System.Web;
using System.IO;


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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
                        select a;
                if (q.Any() == true)
                {
                    foreach (var o in q.ToList())
                    {
                        //判斷是否還有下階
                        var p = from b in db.Groups
                                where b.ParentId == o.GroupId &&
                                      b.ModifyFlag < (int)ModifyFlagEnum.Delete
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
        //public List<KeyValuePair<int, string>> GetLinkDevice()
        //{
        //    List<KeyValuePair<int, string>> pairs = new List<KeyValuePair<int, string>>();

        //    using (var db = new CMSDBContext())
        //    {
        //        var q = from a in db.LinkDevice
        //                select a;
        //        if (q.Any())
        //        {
        //            foreach (var o in q.ToList())
        //            {
        //                KeyValuePair<int, string> item = new KeyValuePair<int, string>(o.LinkID, o.LinkDevName);
        //                pairs.Add(item);
        //            }
        //        }
        //        return pairs;
        //    }
        //}
        /// <summary>
        /// 取得連結子設備資料
        /// </summary>
        /// <param name="linkId"></param>
        /// <returns></returns>
        //public List<KeyValuePair<int, string>> GetLinkTagsBySubSeq(int linkSubSeq)
        //{
        //    List<KeyValuePair<int, string>> pairs = new List<KeyValuePair<int, string>>();

        //    using (var db = new CMSDBContext())
        //    {
        //        var q = from a in db.LinkTag
        //                join b in db.MemTag on a.MTagSeq equals b.MTagSeq
        //                join c in db.TagObj on b.TObjSeq equals c.TObjSeq
        //                where a.LinkSubSeq == linkSubSeq &&
        //                      a.ModifyFlag < (int)ModifyFlagEnum.Delete &&
        //                      b.ModifyFlag < (int)ModifyFlagEnum.Delete
        //                select new { a, b, c };

        //        if (q.Any())
        //        {
        //            foreach (var o in q.ToList())
        //            {
        //                KeyValuePair<int, string> item = new KeyValuePair<int, string>(o.a.LinkTagSeq, o.b.TagName);
        //                pairs.Add(item);

        //            }
        //        }
        //        return pairs;
        //    }
        //}
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
                        IsLowAlarm = o.a.AlarmFlag.GetLowAlarmFlag(),
                        IsUpAlarm = o.a.AlarmFlag.GetUpAlarmFlag(),
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
                    //增加更新AlarmFlag 值
                    linkTag.AlarmFlag = getAlarmFlag(vm.IsLowAlarm, vm.IsUpAlarm);
                    linkTag.UpAlarm = vm.UpAlarm;
                    linkTag.LowAlarm = vm.LowAlarm;
                    linkTag.ModifyFlag = (int)ModifyFlagEnum.Update;
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
        /// 取得 AlarmFlag 值
        /// </summary>
        /// <returns></returns>
        private byte getAlarmFlag(bool isLow, bool isUp)
        {
            byte lowValue = 0;
            byte upValue = 0;

            if (isLow == true)
            {
                lowValue = 2;
            }
            if (isUp == true)
            {
                upValue = 1;
            }
            return (byte)(lowValue + upValue);
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
                    memTag.ModifyFlag = (int)ModifyFlagEnum.Update;
                    errorCode = db.Update<MemTag>(memTag, memTag.MTagSeq);
                }
            }
            return errorCode;
        }
        /// <summary>
        /// 取得圖控介面 select 1
        /// </summary>
        /// <returns></returns>
        public List<LinkDeviceViewModel> GetLinkDevices(int groupId)
        {
            List<LinkDeviceViewModel> vms = new List<LinkDeviceViewModel>();
            using (var db = new CMSDBContext())
            {
                if (groupId == 0)
                {
                    return getLinkDevice(null, true);
                }
                //確認Group 階層
                var q = from a in db.Groups
                        where a.ParentId == groupId
                        select a;
                if (q.Any())
                {
                    var o = q.FirstOrDefault();
                    //mainTool
                    if (o.GroupTypeKey == "1")
                    {
                        return getLinkDevice1(groupId);
                    }
                    //Equipment
                    else if (o.GroupTypeKey == "2")
                    {
                        return getLinkDevice2(groupId);
                    }
                }
            }
            return vms;
        }
        /// <summary>
        /// MainTool 下拉選單
        /// </summary>
        /// <returns></returns>
        private List<LinkDeviceViewModel> getLinkDevice1(int groupId)
        {
            List<LinkDeviceViewModel> vms = new List<LinkDeviceViewModel>();
            using (var db = new CMSDBContext())
            {
                //確認Group 階層

                var q = from a in db.Groups //Main Tool
                        join b in db.Groups on a.GroupId equals b.ParentId //Equipment
                        join c in db.GroupLocations on b.GroupId equals c.GroupId //Link
                        where a.ParentId == groupId
                        select new { a, b, c };

                if (q.Any())
                {
                    var g = q.ToList().GroupBy(p => p.c.LinkSubSeq);
                    List<int> l = new List<int>();
                    foreach (var o in g.ToList())
                    {
                        l.Add(o.Key);
                    }
                    return getLinkDevice(l, false);
                }

            }
            return vms;
        }
        /// <summary>
        /// equment
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private List<LinkDeviceViewModel> getLinkDevice2(int groupId)
        {
            List<LinkDeviceViewModel> vms = new List<LinkDeviceViewModel>();
            using (var db = new CMSDBContext())
            {
                //確認Group 階層

                var q = from a in db.Groups //Main Tool
                        join b in db.GroupLocations on a.GroupId equals b.GroupId //Link
                        where a.ParentId == groupId
                        select new { a, b };

                if (q.Any())
                {
                    var g = q.ToList().GroupBy(p => p.b.LinkSubSeq);
                    List<int> l = new List<int>();
                    foreach (var o in g.ToList())
                    {
                        l.Add(o.Key);
                    }
                    return getLinkDevice(l, false);
                }
            }
            return vms;
        }

        private List<LinkDeviceViewModel> getLinkDevice(List<int> linkSubSeqs, bool isAll)
        {
            List<LinkDeviceViewModel> vms = new List<LinkDeviceViewModel>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkDevice
                        join b in db.LinkDevSub on a.LinkID equals b.LinkID
                        where a.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              b.ModifyFlag < (int)ModifyFlagEnum.Delete
                        select new { a.LinkID, a.LinkDevName, b.LinkSubSeq, b.LinkSubName };
                if (isAll == false)
                {
                    q = q.Where(p => linkSubSeqs.Contains(p.LinkSubSeq));
                }
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              b.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              a.UISelFlag == 1
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
                            TagName = o.a.TagName.Trim(),
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
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            List<TagAlramViewModel> vms = new List<TagAlramViewModel>();
            //group
            using (var db = new CMSDBContext())
            {
                var g = getGroupMapLinkSubSeq(groupId);

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
                                TagName = t.a.TagName,
                                TObjSeq = t.c.TObjSeq,
                                TObjName = t.c.TObjName,
                                CurLinkSta = t.a.CurLinkSta,
                                CurLinkStaName = t.a.CurLinkSta.GetCurLinkStaName(),
                                IsCurLinkSta = t.a.CurLinkSta.GetCurLinkSta(),
                                CurSubSta = t.a.CurSubSta,
                                CurSubStaName = t.a.CurSubSta.GetCurSubStaName(),
                                IsCurSubSta = true,
                                Maintain = t.a.Maintain,
                                //MaintainName = t.a.Maintain.GetMaintainName(),
                                //IsMaintain = t.a.Maintain.GetMaintain()
                            };
                            //斷線
                            if (tag.CurLinkSta != 1)
                            {
                                vms.Add(tag);
                                continue;
                            }
                            //2態告警
                            if (tag.TObjSeq >= 1 && tag.TObjSeq <= 3)
                            {
                                if (tag.CurfValue > 1)
                                {
                                    tag.CurSubStaName = "異常";
                                    vms.Add(tag);
                                    continue;
                                }
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
                                //vms.Add(tag);
                                continue;
                            }
                        }
                    }
                }
            }
            //加入維護
            var g1 = getGroups(groupId);
            ////取得本階
            var lastGroup = getGroup(groupId);
            if (lastGroup != null)
            {
                g1.Add(lastGroup);
            }
            foreach (var o in g1.ToList())
            {
                var e = getEventSetByData(o.GroupId);
                if (e != null)
                {
                    vms.Add(e);
                }

            }
            sw.Stop();
            System.Diagnostics.Debug.WriteLine(sw.ElapsedMilliseconds);
            return vms;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private TagAlramViewModel getEventSetByData(int groupId)
        {
            using (var db = new CMSDBContext())
            {
                try
                {
                    var q = from a in db.EventSet
                            from b in db.OptionSets
                            where a.Maintain == b.OptionNo &&
                                  b.FieldName == "Maintain" &&
                                  a.GroupId == groupId &&
                                  a.RestTime == null
                            select new { a, b };
                    if (q.Any())
                    {
                        var o = q.FirstOrDefault();

                        TagAlramViewModel tag = new TagAlramViewModel()
                        {
                            TagName = o.a.Name,
                            TObjName = o.b.OptionName,
                            CurfValue = Convert.ToDecimal(o.a.Value),
                            Maintain = o.a.Maintain,
                            MaintainName = o.a.Maintain.GetMaintainName(),
                            IsMaintain = o.a.Maintain.GetMaintain()
                        };
                        return tag;
                    }
                }
                catch
                {

                }

            }
            return null;
        }
        /// <summary>
        /// 取得Group對照 LinkSubSeq
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private List<int> getGroupMapLinkSubSeq(int groupId)
        {
            List<int> linkSubs = new List<int>();
            //取得 groupid list
            var g = getGroups(groupId);
            ////
            ////取得本階
            var lastGroup = getGroup(groupId);
            if (lastGroup != null)
            {
                g.Add(lastGroup);
            }
            List<int> groups = new List<int>();
            foreach (var group in g)
            {
                groups.Add(group.GroupId);
            }
            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupLocations
                        where groups.Contains(a.GroupId)
                        select a;
                if (q.Any())
                {
                    var list = q.ToList().GroupBy(p => p.LinkSubSeq);
                    foreach (var l in list)
                    {
                        linkSubs.Add(l.Key);
                    }
                }
            }
            //取得本階
            //var g = getGroupWithLinkSub(groupId).GroupBy(p => p.LinkSubSeq);
            //foreach (var o in g.ToList())
            //{
            //    linkSubs.Add(o.Key);
            //}
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
                        select a;
                if (q.Any())
                {
                    return q.FirstOrDefault();
                }
            }
            return null;
        }

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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
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
                //yellow
                return "#E3A21A";
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
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
                    obj.ModifyFlag = (int)ModifyFlagEnum.Delete;

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
                    ModifyFlag = (int)ModifyFlagEnum.Insert,
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
                //updateLinkTag(vm);
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
                    obj.ModifyFlag = (int)ModifyFlagEnum.Update;
                    obj.SystemTime = DateTime.Now;
                }
                errorCode = db.Update<GroupLocations>(obj, obj.LocationId);
                if (errorCode <= 0)
                {
                    return errorCode;
                }
                //更新 LinkTag GroupId
                //updateLinkTag(vm);
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
                    obj.ModifyFlag = (int)ModifyFlagEnum.Delete;
                    obj.SystemTime = DateTime.Now;
                }
                int errorCode = db.Update<GroupLocations>(obj, obj.LocationId);
                if (errorCode > 0)
                {
                    //刪除時，更新 LinkTag GroupId
                    vm.GroupId = 0;
                    //updateLinkTag(vm);
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
                    o.ModifyFlag = (int)ModifyFlagEnum.Delete;
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
                        select a;
                if (q.Any() == true)
                {
                    foreach (var o in q.ToList())
                    {
                        //判斷是否還有下階
                        var p = from b in db.Groups
                                where b.ParentId == o.GroupId &&
                                      b.ModifyFlag < (int)ModifyFlagEnum.Delete
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
                        select new { a, r };
                //下階有資料
                if (q.Any() == true)
                {
                    foreach (var o in q.ToList())
                    {
                        if (o.r != null)
                        {
                            if (o.r.ModifyFlag >= (int)ModifyFlagEnum.Delete)
                            {
                                continue;
                            }
                        }
                        //判斷是否還有下階
                        var p = from b in db.Groups
                                where b.ParentId == o.a.GroupId &&
                                      b.ModifyFlag < (int)ModifyFlagEnum.Delete
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
                                   a.ModifyFlag < (int)ModifyFlagEnum.Delete
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
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
                        obj.ModifyFlag = (int)ModifyFlagEnum.Update;

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
        public Chart GetTagHistory(TagParam param)
        {
            Chart chart = new Chart();

            List<TagValueViewModel> tags = new List<TagValueViewModel>();
            var sDt = getHistoryTime(param);
            var eDt = DateTime.Now;
            using (var db = new CMSDBContext())
            {
                var q = from a in db.GroupLocations
                        join b in db.TagHistory on a.LinkTagSeq equals b.LinkTagSeq
                        join c in db.LinkTag on b.LinkTagSeq equals c.LinkTagSeq
                        where a.GroupId == param.GroupId &&
                              a.LocationId == param.LocationId
                        //b.RecTime >= new DateTime(2016, 03, 01)
                        orderby b.RecTime
                        select new { a, b, c };
                if (param.Type > 0)
                {
                    q = q.Where(p => p.b.RecTime >= sDt & p.b.RecTime <= eDt);
                }

                if (q.Any())
                {
                    int count = 0;
                    string label = q.FirstOrDefault().c.TagName;

                    foreach (var obj in q.OrderByDescending(p => p.b.RecTime).Take(1000))
                    {
                        TagValueViewModel tag = new TagValueViewModel()
                        {
                            Labels = obj.b.RecTime.ToJavascriptTimestamp(),
                            Data = obj.b.fValue.ToString()

                        };

                        tags.Add(tag);
                        count++;
                        if (count > 30)
                        {
                            break;
                        }
                    }
                    TagValuesViewModel vms = new TagValuesViewModel()
                    {
                        List = tags,
                        Label = label,
                        Yaxis = 0
                    };
                    chart.Data.Add(vms);
                    double firstX = tags.FirstOrDefault().Labels;
                    double lastX = tags.LastOrDefault().Labels;
                    //加入上下限值
                    var uporlow = getUpAndLow(firstX, lastX, q.FirstOrDefault().a.LinkTagSeq);
                    if (uporlow != null)
                        chart.Data.AddRange(uporlow);
                    chart.Xaxis = new Xaxis()
                    {
                        Mode = "time",
                        TickSize = getTickSize(sDt, eDt)
                    };
                    return chart;
                }
            }

            return null;
        }
        /// <summary>
        /// 取得查詢時間
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private DateTime getHistoryTime(TagParam param)
        {
            var nowDt = DateTime.Now;
            //日
            if (param.Type == 1)
            {
                return nowDt.AddDays(param.TypeValue * -1);
            }
            //時
            if (param.Type == 2)
            {
                return nowDt.AddHours(param.TypeValue * -1);
            }
            if (param.Type == 3)
            {
                return nowDt.AddMinutes(param.TypeValue * -1);
            }
            //預設 30 分鐘前
            return nowDt.AddMinutes(-30);
        }
        private KeyValuePair<int, string> getTickSize(DateTime sDt, DateTime eDt)
        {

            TimeSpan s = new TimeSpan(sDt.Ticks);
            TimeSpan e = new TimeSpan(eDt.Ticks);
            var diff = e - s;
            double days = diff.TotalDays;
            double hours = diff.TotalHours;
            double minutes = diff.TotalMinutes;
            if (days > 2)
            {
                //
                return new KeyValuePair<int, string>(1, "day");
            }
            if (days >= 1 && days <= 2)
            {
                return new KeyValuePair<int, string>(6, "hour");
            }
            if (hours > 1)
            {
                //
                return new KeyValuePair<int, string>(1, "hour");
            }
            if (minutes > 10)
            {
                return new KeyValuePair<int, string>(5, "minute");
            }
            return new KeyValuePair<int, string>(1, "minute");
        }
        private List<TagValuesViewModel> getUpAndLow(double firstX, double lastX, int linkTagSeq)
        {
            List<TagValuesViewModel> vms = new List<TagValuesViewModel>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkTag
                        where a.LinkTagSeq == linkTagSeq
                        select a;
                if (q.Any())
                {
                    var o = q.FirstOrDefault();
                    if (o.AlarmFlag == 1)
                    {
                        TagValuesViewModel vm = new TagValuesViewModel()
                        {
                            List = getUp(firstX, lastX, o.UpAlarm),
                            Label = "超值",
                            Yaxis = 1
                        };
                        vms.Add(vm);

                    }
                    else if (o.AlarmFlag == 2)
                    {
                        TagValuesViewModel vm = new TagValuesViewModel()
                        {
                            List = getUp(firstX, lastX, o.LowAlarm),
                            Label = "低值",
                            Yaxis = 1
                        };
                        vms.Add(vm);
                    }
                    else if (o.AlarmFlag == 3)
                    {
                        TagValuesViewModel vm1 = new TagValuesViewModel()
                        {
                            List = getUp(firstX, lastX, o.UpAlarm),
                            Label = "超值",
                            Yaxis = 2
                        };
                        vms.Add(vm1);
                        TagValuesViewModel vm2 = new TagValuesViewModel()
                        {
                            List = getUp(firstX, lastX, o.LowAlarm),
                            Label = "低值",
                            Yaxis = 3
                        };
                        vms.Add(vm2);
                    }
                    return vms;
                }
            }
            return null;
        }
        private List<TagValueViewModel> getUp(double firstX, double lastX, decimal? y)
        {
            List<TagValueViewModel> vms = new List<TagValueViewModel>();
            TagValueViewModel tag1 = new TagValueViewModel()
            {
                Labels = firstX,
                Data = y.ToString()
            };
            vms.Add(tag1);
            TagValueViewModel tag2 = new TagValueViewModel()
            {
                Labels = lastX,
                Data = y.ToString()
            };
            vms.Add(tag2);
            return vms;
        }
        public List<TagValuesViewModel> GetHistoryTags(TagParamViewModel param)
        {
            List<TagValuesViewModel> tags = new List<TagValuesViewModel>();
            //開始時間
            DateTime sdt = new DateTime(param.StartDate.Year, param.StartDate.Month, param.StartDate.Day,
                param.StartTime.Hour, param.StartTime.Minute, 0);
            //結束時間
            var edt = new DateTime(param.EndDate.Year, param.EndDate.Month, param.EndDate.Day,
                param.EndTime.Hour, param.EndTime.Minute, 0);
            //多選
            List<int> selected = new List<int>();
            foreach (var linkTag in param.LinkTags)
            {
                if (linkTag.Selected == true)
                {
                    var t = getTagHistory(sdt, edt, linkTag.LinkTagSeq);
                    TagValuesViewModel tag = new TagValuesViewModel();
                    tag.List = t;
                    tags.Add(tag);
                }
            }
            return tags;
        }

        private List<TagValueViewModel> getTagHistory(DateTime sdt, DateTime edt, int linkTagSeq)
        {
            List<TagValueViewModel> tags = new List<TagValueViewModel>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.TagHistory
                        where a.RecTime >= sdt &&
                              a.RecTime <= edt &&
                              a.LinkTagSeq == linkTagSeq
                        orderby a.RecTime
                        select a;
                if (q.Any())
                {
                    foreach (var o in q.ToList().Take(1000))
                    {
                        TagValueViewModel tag = new TagValueViewModel()
                        {

                            Labels = o.RecTime.ToJavascriptTimestamp(),//(o.RecTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds,
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
        public int SetMaintain(MaintainParam param)
        {
            int errorCode = 0;
            byte maintain = 0;
            using (var db = new CMSDBContext())
            {
                //找到是否有維護
                var q = from a in db.EventSet
                        where a.GroupId == param.GroupId &&
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
                    errorCode = insertEvents(param);
                }
                if (errorCode > 0)
                {
                    //更新維護狀態  暫不執行
                    updateLinkTag(param.GroupId, maintain);
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
                            link.ModifyFlag = (int)ModifyFlagEnum.Update;
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
        private int insertEvents(MaintainParam param)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups
                        where a.GroupId == param.GroupId
                        select a;
                if (q.Any())
                {
                    var e = new EventSet()
                    {
                        EventLevel = 1,
                        GroupId = param.GroupId,
                        //不紀錄 LinkTag
                        LinkTagSeq = 0,
                        RecTime = DateTime.Now,
                        Name = q.FirstOrDefault().GroupName + "維護",
                        Value = "1",
                        Maintain = (byte)param.OptionNo
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
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
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            List<MainToolViewModel> mts = new List<MainToolViewModel>();
            using (var db = new CMSDBContext())
            {
                //找尋群組資料
                var q = from a in db.Groups
                        where a.ParentId == groupId &&
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
                        select a;

                if (q.Any())
                {
                    foreach (var obj in q.ToList())
                    {
                        var status = getLinkTagAlarm(obj.GroupId);
                        //取得維護次數
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
            sw.Stop();
            System.Diagnostics.Debug.WriteLine(sw.ElapsedMilliseconds);
            return mts;
        }
        private int getEventSetByMaintain(int groupId)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.EventSet
                        where a.GroupId == groupId &&
                              a.EventLevel == 1 &&
                              a.RestTime == null
                        select a;
                if (q.Any())
                {
                    return q.Count();
                }
            }
            return 0;
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

                foreach (var o in g.ToList())
                {
                    var q = from a in db.LinkTag
                            where a.LinkSubSeq == o &&
                                  a.ModifyFlag < (int)ModifyFlagEnum.Delete
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
            //var maintainCount = from a in ts
            //                    where a.Maintain == 1
            //                    group a by new { a.LinkTagSeq } into g
            //                    select new { Count = g.Count() };

            status.Alarm = alarmCount.ToList().Count();
            status.Status = linkStaCount.ToList().Count();
            status.Maintain = 0;
            //status.Maintain = maintainCount.ToList().Count();
            var g1 = getGroups(groupId);

            ////取得本階
            var lastGroup = getGroup(groupId);
            if (lastGroup != null)
            {
                g1.Add(lastGroup);
            }
            foreach (var o in g1.ToList())
            {
                status.Maintain = getEventSetByMaintain(o.GroupId) + status.Maintain;
            }

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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
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
                            join b in db.OptionSets on a.EventLevel equals b.OptionNo
                            where b.FieldName == "EventLevel"
                            where a.GroupId == o.GroupId &&
                                  a.ConfirmTime.Value == null
                            orderby a.RestTime descending
                            select new { a, b };
                    if (q.Any())
                    {
                        foreach (var p in q.ToList())
                        {
                            EventViewModel e = new EventViewModel()
                            {
                                LinkTagSeq = p.a.LinkTagSeq,
                                RecTime = p.a.RecTime,
                                RestTime = p.a.RestTime,
                                Name = p.a.Name,
                                EventName = p.b.OptionName
                            };
                            events.Add(e);

                        }

                    }
                }

            }
            return events;
        }
        #region 匯出 excel
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public EventExport ExportEvents(EventSetParam param)
        {
            List<EventViewModel> events = new List<EventViewModel>();

            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups                                         // Department
                        join b in db.Groups on a.GroupId equals b.ParentId          // MainTool
                        join c in db.Groups on b.GroupId equals c.ParentId          // Equipment
                        join d in db.GroupLocations on c.GroupId equals d.GroupId   // 位置
                        join e in db.LinkTag on d.LinkTagSeq equals e.LinkTagSeq
                        where d.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              a.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              b.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              c.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              e.ModifyFlag < (byte)ModifyFlagEnum.Delete
                        select new { a, b, c, d, e };
                if (param.GroupType == 1) //Department
                {
                    q = q.Where(p => p.a.GroupId == param.GroupId);
                }
                if (param.GroupType == 2) //MainTool
                {
                    q = q.Where(p => p.b.GroupId == param.GroupId);
                }
                if (param.GroupType == 3) //Equipment
                {
                    q = q.Where(p => p.c.GroupId == param.GroupId);
                }

                if (q.Any())
                {
                    List<int> sub = new List<int>();

                    foreach (var o in q.ToList().GroupBy(p => p.e.LinkSubSeq))
                    {
                        sub.Add(o.Key);
                    }
                    if (param.OptionNo != 1)
                    {
                        //取得 LinkSubSeq
                        events.AddRange(getEventSetByLinkSubSeq1(sub, param));
                    }
                    else
                    {
                        //取得 保養狀態的
                        events.AddRange(getEventSets1(param));
                    }
                }
            }
            string fileName =  WriteCsv(events);
            return new EventExport()
            {
                FileUrl = "/Export/",
                FileName = fileName
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private List<EventViewModel> getEventSetByLinkSubSeq1(List<int> list, EventSetParam param)
        {

            List<EventViewModel> events = new List<EventViewModel>();
            DateTime sDt = param.SDateTime.GetStartTime();
            DateTime eDt = param.EDateTime.GetEndTime();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkTag
                        join b in db.EventSet on a.LinkTagSeq equals b.LinkTagSeq
                        join c in db.OptionSets on b.EventLevel equals c.OptionNo
                        where list.Contains(a.LinkSubSeq) &&
                              c.FieldName == "EventLevel" &&
                              b.RecTime >= sDt &&
                              b.RecTime <= eDt
                        orderby b.RecTime
                        select new { a, b, c };
                if (param.OptionNo > 0)
                {
                    if (param.OptionNo == 4)
                    {
                        q = q.Where(p => p.b.EventLevel == param.OptionNo || p.b.EventLevel == 5);
                    }
                    else
                    {
                        q = q.Where(p => p.b.EventLevel == param.OptionNo);
                    }
                }
                if (q.Any())
                {
                    foreach (var o in q.ToList())
                    {
                        EventViewModel e = new EventViewModel()
                        {
                            LinkTagSeq = o.a.LinkTagSeq,
                            RecTime = o.b.RecTime,
                            RestTime = o.b.RestTime,
                            Name = o.b.Name,
                            EventName = o.c.OptionName
                        };
                        events.Add(e);
                    }
                }
            }
            return events;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private List<EventViewModel> getEventSets1(EventSetParam param)
        {
            List<EventViewModel> events = new List<EventViewModel>();
            DateTime sDt = param.SDateTime.GetStartTime();
            DateTime eDt = param.EDateTime.GetEndTime();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.EventSet
                        join b in db.OptionSets on a.EventLevel equals b.OptionNo
                        join c in db.OptionSets on a.Maintain equals c.OptionNo
                        where a.RecTime >= sDt &&
                              a.RecTime <= eDt &&
                              a.EventLevel == 1 &&
                              b.FieldName == "EventLevel" &&
                              c.FieldName == "Maintain"
                        orderby a.RecTime
                        select new { a, b, c };
                if (param.OptionNo > 0)
                {

                    if (param.OptionNo == 4)
                    {
                        q = q.Where(p => p.a.EventLevel == param.OptionNo || p.a.EventLevel == 5);
                    }
                    else
                    {
                        q = q.Where(p => p.a.EventLevel == param.OptionNo);
                    }
                }
                if (q.Any())
                {
                    foreach (var o in q.Skip(param.CurrentPage * param.ItemsPerPage).Take(param.ItemsPerPage))
                    {
                        EventViewModel e = new EventViewModel()
                        {
                            LinkTagSeq = o.a.LinkTagSeq,
                            RecTime = o.a.RecTime,
                            RestTime = o.a.RestTime,
                            Name = string.Format("{0}-{1}", o.a.Name, o.c.OptionName),
                            EventName = o.b.OptionName
                        };
                        events.Add(e);
                    }
                }

            }
            return events;
        }
        #endregion
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public EventsViewModel GetEvents(EventSetParam param)
        {
            List<EventViewModel> events = new List<EventViewModel>();

            using (var db = new CMSDBContext())
            {
                var q = from a in db.Groups                                         // Department
                        join b in db.Groups on a.GroupId equals b.ParentId          // MainTool
                        join c in db.Groups on b.GroupId equals c.ParentId          // Equipment
                        join d in db.GroupLocations on c.GroupId equals d.GroupId   // 位置
                        join e in db.LinkTag on d.LinkTagSeq equals e.LinkTagSeq
                        where d.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              a.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              b.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              c.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              e.ModifyFlag < (byte)ModifyFlagEnum.Delete
                        select new { a, b, c, d, e };
                if (param.GroupType == 1) //Department
                {
                    q = q.Where(p => p.a.GroupId == param.GroupId);
                }
                if (param.GroupType == 2) //MainTool
                {
                    q = q.Where(p => p.b.GroupId == param.GroupId);
                }
                if (param.GroupType == 3) //Equipment
                {
                    q = q.Where(p => p.c.GroupId == param.GroupId);
                }

                if (q.Any())
                {
                    List<int> sub = new List<int>();

                    foreach (var o in q.ToList().GroupBy(p => p.e.LinkSubSeq))
                    {
                        sub.Add(o.Key);
                    }
                    if (param.OptionNo != 1)
                    {
                        //取得 LinkSubSeq
                        return getEventSetByLinkSubSeq(sub, param);
                    }
                    else
                    {
                        //取得 保養狀態的
                        return getEventSets(param);
                    }
                }
            }

            //WriteTest(events);

            return null;
        }
        /// <summary>
        /// 寫入檔案
        /// </summary>
        /// <param name="events"></param>
        private string WriteCsv(List<EventViewModel> events)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string path = HttpContext.Current.Server.MapPath("~/Export/");

                string fileName = string.Format("{0}{1:00}{2:00}{3:00}{4:00}.csv", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);
                string fullPath = string.Format("{0}{1}", path, fileName);
                //修改檔案為非唯讀屬性(Normal)
                //System.IO.FileInfo FileAttribute = new FileInfo(path + fileName);
                //FileAttribute.Attributes = FileAttributes.Normal;
                //建立CSV檔案
                FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);

                foreach (var e in events)
                {
                    var data = string.Format("{0}, {1}, {2}, {3}", e.Name, e.RecTime, e.EventName, e.RestTime);
                    sw.WriteLine(data);
                }
                sw.Close();
                return fileName;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 取得 LinkSubSeq 的EventSets
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private EventsViewModel getEventSetByLinkSubSeq(List<int> list, EventSetParam param)
        {
            int count = 0;
            List<EventViewModel> events = new List<EventViewModel>();
            DateTime sDt = param.SDateTime.GetStartTime();
            DateTime eDt = param.EDateTime.GetEndTime();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkTag
                        join b in db.EventSet on a.LinkTagSeq equals b.LinkTagSeq
                        join c in db.OptionSets on b.EventLevel equals c.OptionNo
                        where list.Contains(a.LinkSubSeq) &&
                              c.FieldName == "EventLevel" &&
                              b.RecTime >= sDt &&
                              b.RecTime <= eDt
                        orderby b.RecTime
                        select new { a, b, c };
                if (param.OptionNo > 0)
                {
                    if (param.OptionNo == 4)
                    {
                        q = q.Where(p => p.b.EventLevel == param.OptionNo || p.b.EventLevel == 5);
                    }
                    else
                    {
                        q = q.Where(p => p.b.EventLevel == param.OptionNo);
                    }
                }
                if (q.Any())
                {
                    count = q.Count();
                    foreach (var o in q.Skip(param.CurrentPage * param.ItemsPerPage).Take(param.ItemsPerPage))
                    {
                        EventViewModel e = new EventViewModel()
                        {
                            LinkTagSeq = o.a.LinkTagSeq,
                            RecTime = o.b.RecTime,
                            RestTime = o.b.RestTime,
                            Name = o.b.Name,
                            EventName = o.c.OptionName
                        };
                        events.Add(e);
                    }
                }
            }

            EventsViewModel vm = new EventsViewModel()
            {
                EventSets = events,
                //分頁數量
                PagedItems = Math.Ceiling((decimal)count / (decimal)param.ItemsPerPage)
            };
            return vm;
        }

        private EventsViewModel getEventSets(EventSetParam param)
        {
            int count = 0;
            List<EventViewModel> events = new List<EventViewModel>();
            DateTime sDt = param.SDateTime.GetStartTime();
            DateTime eDt = param.EDateTime.GetEndTime();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.EventSet
                        join b in db.OptionSets on a.EventLevel equals b.OptionNo
                        join c in db.OptionSets on a.Maintain equals c.OptionNo
                        where a.RecTime >= sDt &&
                              a.RecTime <= eDt &&
                              a.EventLevel == 1 &&
                              b.FieldName == "EventLevel" &&
                              c.FieldName == "Maintain"
                        orderby a.RecTime
                        select new { a, b, c };
                if (param.OptionNo > 0)
                {

                    if (param.OptionNo == 4)
                    {
                        q = q.Where(p => p.a.EventLevel == param.OptionNo || p.a.EventLevel == 5);
                    }
                    else
                    {
                        q = q.Where(p => p.a.EventLevel == param.OptionNo);
                    }
                }
                if (q.Any())
                {
                    count = q.Count();
                    foreach (var o in q.Skip(param.CurrentPage * param.ItemsPerPage).Take(param.ItemsPerPage))
                    {
                        EventViewModel e = new EventViewModel()
                        {
                            LinkTagSeq = o.a.LinkTagSeq,
                            RecTime = o.a.RecTime,
                            RestTime = o.a.RestTime,
                            Name = string.Format("{0}-{1}", o.a.Name, o.c.OptionName),
                            EventName = o.b.OptionName
                        };
                        events.Add(e);
                    }
                }

            }
            EventsViewModel vm = new EventsViewModel()
            {
                EventSets = events,
                //分頁數量
                PagedItems = Math.Ceiling((decimal)count / (decimal)param.ItemsPerPage)
            };
            return vm;
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
                              a.ModifyFlag < (int)ModifyFlagEnum.Delete
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

        /// <summary>
        /// 取得資料集
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<OptionSets> GetOptionSets(string fieldName)
        {
            using (var db = new CMSDBContext())
            {
                DateTime sDt = DateTime.Now.GetStartTime();
                DateTime sEt = DateTime.Now.GetEndTime();

                var q = from a in db.OptionSets
                        where a.FieldName == fieldName &&
                              a.StartDate <= sDt &&
                              a.EndDate >= sEt
                        select a;

                if (q.Any() == true)
                {
                    return q.ToList();
                }
            }
            return null;
        }
        /// <summary>
        /// 參數設定List
        /// </summary>
        /// <returns></returns>
        public List<string> GetOptionFieldName()
        {
            using (var db = new CMSDBContext())
            {
                var q = (from a in db.OptionSets
                         select a.FieldName).Distinct();

                return q.ToList();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public List<OptionSets> SetOptionSets(OptionSets option)
        {
            int errorCode = 0;
            using (var db = new CMSDBContext())
            {
                var q = from a in db.OptionSets
                        where a.OptionId == option.OptionId
                        select a;
                //update
                if (q.Any())
                {
                    errorCode = updateOptionSets(option);
                    if (errorCode > 0)
                    {
                        return GetOptionSets(option.FieldName);
                    }
                }
                //insert
                else
                {
                    errorCode = insertOptionSets(option);
                    if (errorCode > 0)
                    {
                        return GetOptionSets(option.FieldName);
                    }
                }
                return null;
            }
        }
        private int insertOptionSets(OptionSets option)
        {
            using (var db = new CMSDBContext())
            {
                try
                {
                    option.OptionNo = getOptionNo(option.FieldName);
                    option.StartDate = DateTime.Now;
                    option.EndDate = new DateTime(2999, 12, 31, 23, 59, 00);
                    option.SystemTime = DateTime.Now;
                    option.SystemUser = 0;
                    db.OptionSets.Add(option);
                    return db.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }

            }
        }

        private int updateOptionSets(OptionSets option)
        {
            using (var db = new CMSDBContext())
            {
                try
                {
                    var q = from a in db.OptionSets
                            where a.OptionId == option.OptionId
                            select a;
                    if (q.Any())
                    {
                        var o = q.FirstOrDefault();
                        o.OptionName = option.OptionName;
                        o.EndDate = option.EndDate;
                        o.SystemTime = DateTime.Now;
                        //return db.Update<OptionSets>(o, o.FieldName, o.OptionNo, o.EndDate);
                        return db.SaveChanges();
                    }

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    return -1;
                }
                return 0;
            }
        }
        private byte getOptionNo(string fieldName)
        {
            using (var db = new CMSDBContext())
            {
                try
                {
                    var q = (from a in db.OptionSets
                             where a.FieldName == fieldName
                             select a.OptionNo).Max() + 1;

                    return (byte)q;
                }
                catch
                {
                    return 0;
                }

            }

        }
    }
}
