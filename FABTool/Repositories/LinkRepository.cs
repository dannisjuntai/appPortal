﻿using DA.DataBase;
using DA.DataBase.Entities;
using DA.DataBase.Utilities;
using FABTool.Common;
using FABTool.Models.Charts;
using FABTool.Models.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABTool.Repositories
{
    public class LinkRepository : IDisposable
    {
        #region 取得機台 即時LinkTag 狀態
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public LinkDeviceViewModel GetMainToolLinkTags(int groupId)
        {
            return GetLinkTagsStatus(GroupTypeEnum.MainTool, groupId);
        }
        #endregion
        #region 取得機台 即時LinkTag 狀態
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public LinkDeviceViewModel GetEquipmentLinkTags(int groupId)
        {
            return GetLinkTagsStatus(GroupTypeEnum.Equipment, groupId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public LinkDeviceViewModel GetDeviceLinkTags(int groupId)
        {
            return GetLinkTagsStatus(GroupTypeEnum.Device, groupId);
        }
        #endregion

        #region 取得 LinkTag Status
        /// <summary>
        /// 取得 LinkTag 狀態
        /// </summary>
        /// <param name="type"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public LinkDeviceViewModel GetLinkTagsStatus(GroupTypeEnum type, int groupId)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();

            using (var db = new CMSDBContext())
            {
                var q = from b in db.Groups                                         // Department
                        join c in db.Groups on b.GroupId equals c.ParentId          // MainTool
                        join d in db.Groups on c.GroupId equals d.ParentId          // Equipment
                        join e in db.GroupLocations on d.GroupId equals e.GroupId
                        join f in db.LinkTag on e.LinkSubSeq equals f.LinkSubSeq
                        join h in db.MemTag on f.MTagSeq equals h.MTagSeq
                        join i in db.TagObj on h.TObjSeq equals i.TObjSeq
                        where b.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              c.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              d.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              e.ModifyFlag < (int)ModifyFlagEnum.Delete &&
                              f.ModifyFlag < (int)ModifyFlagEnum.Delete
                        select new { b, c, d, e, f, i };
                if (type == GroupTypeEnum.MainTool)
                {
                    q = q.Where(p => p.c.ParentId == groupId);
                }
                if (type == GroupTypeEnum.Equipment)
                {
                    q = q.Where(p => p.d.ParentId == groupId);
                }
                if (type == GroupTypeEnum.Device)
                {
                    q = q.Where(p => p.e.GroupId == groupId);
                }
                //群組
                var g = q.GroupBy(p => new { p.e.LinkSubSeq, p.d, p.f, p.i }).Select(r => new { r.Key.d, r.Key.f, r.Key.i });
                //維護
                var maintains = q.GroupBy(p => new { p.d.GroupId }).Select(r => new { r.Key.GroupId });
                List<int> mlist = new List<int>();
                foreach (var maintain in maintains.ToList())
                {
                    mlist.Add(maintain.GroupId);
                }

                if (g.Any())
                {
                    var o = g.ToList();
                    var m = getMaintains(mlist);

                    int curLinkStaCount = o.Where(p => p.f.CurLinkSta != 1).Count();
                    int curSubStaCount = o.Where(p => p.f.CurSubSta > 1).Count();
                    //多態告警的?
                    int MaintainCount = m.Count();
                    //LinkTags
                    List<LinkTagViewModel> tags = new List<LinkTagViewModel>();
                    foreach (var l in g.ToList())
                    {
                        LinkTagViewModel tag = new LinkTagViewModel();

                        if (l.f.CurLinkSta != 1)
                        {
                            tag.CurLinkStaName = l.f.CurLinkSta.GetCurLinkStaName();//"連線狀態";
                        }
                        else if (l.f.CurSubSta > 1)
                        {
                            tag.CurSubStaName = l.f.CurSubSta.GetCurSubStaName();//"告警狀態";
                        }
                        else if (l.i.TObjSeq >= 1 && l.i.TObjSeq <= 3)
                        {
                            if (l.f.CurfValue > 0)
                            {
                                tag.AlarmName = "多態告警狀態";
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        tag.LinkSubName = l.d.GroupName;
                        tag.LinkTag = l.f;
                        tag.TObjName = l.i.TObjName;
                        tags.Add(tag);
                    }
                    //加入保養?

                    //連線狀態
                    LinkDeviceViewModel vm = new LinkDeviceViewModel()
                    {
                        CurLinkStaCount = curLinkStaCount,
                        CurSubStaCount = curSubStaCount,
                        MaintainCount = MaintainCount,
                        LinkTags = tags
                    };
                    //sw.Stop();
                    //System.Diagnostics.Debug.WriteLine(sw.ElapsedMilliseconds);
                    return vm;
                }
            }
            return null;
        }

        /// <summary>
        /// 取得機台維修數量
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        private List<LinkTag> getMaintains(List<int> groups)
        {
            List<LinkTag> tags = new List<LinkTag>();
            using (var db = new CMSDBContext())
            {
                var q = from a in db.EventSet
                        where groups.Contains(a.GroupId) &&
                              a.EventLevel == 1 &&
                              a.RestTime == null
                        select a;

                if (q.Any())
                {
                    foreach (var o in q.ToList())
                    {
                        LinkTag tag = new LinkTag()
                        {

                        };
                        tags.Add(tag);
                    }

                }
            }
            return tags;
        }
        #endregion

        #region  取得群組 告警狀態
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstX"></param>
        /// <param name="lastX"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private List<TagViewModel> getUp(double firstX, double lastX, decimal? y)
        {
            List<TagViewModel> vms = new List<TagViewModel>();
            TagViewModel tag1 = new TagViewModel()
            {
                X = firstX,
                Y = y.ToString()
            };
            vms.Add(tag1);
            TagViewModel tag2 = new TagViewModel()
            {
                X = lastX,
                Y = y.ToString()
            };
            vms.Add(tag2);
            return vms;
        }
        private List<TagViewModel> getLow(double firstX, double lastX, decimal? y)
        {
            List<TagViewModel> vms = new List<TagViewModel>();
            TagViewModel tag1 = new TagViewModel()
            {
                X = firstX,
                Y = y.ToString()
            };
            vms.Add(tag1);
            TagViewModel tag2 = new TagViewModel()
            {
                X = lastX,
                Y = y.ToString()
            };
            vms.Add(tag2);
            return vms;
        }
        /// <summary>
        /// 取得歷史紀錄
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ChartViewModel GetHistoryTags(DA.DataBase.Models.TagParamViewModel param)
        {
            List<TagsViewModel> tags = new List<TagsViewModel>();
            //多選
            List<Yaxis> yaxes = new List<Yaxis>();
            int y = 0;
            foreach (var LinkTagSeq in param.Selection)
            {
                y++;
                var t = getTagHistory(param, LinkTagSeq);
                string link = getLinkTag(LinkTagSeq);
                TagsViewModel tag = new TagsViewModel()
                {
                    Label = link,
                    Yaxis = y,
                    Data = t,
                };
                tags.Add(tag);
                //
                var yaxis = new Yaxis();
                if (y > 1)
                {
                    yaxis.Position = "right";
                }
                else
                {
                    yaxis.Position = "left";
                }
                yaxes.Add(yaxis);

            }
            //加入上下限值
            if (param.Selection.Length == 1)
            {
                double firstX = tags.FirstOrDefault().Data.FirstOrDefault().X;
                double lastX = tags.LastOrDefault().Data.LastOrDefault().X;
                //加入上下限值
                var uporlow = getUpAndLow(firstX, lastX, param.Selection[0]);
                if (uporlow != null)
                {
                    tags.AddRange(uporlow);
                }
            }
            //
            ChartViewModel vm = new ChartViewModel()
            {
                Datasets = tags,
                Yaxes = yaxes,
                Xaxis = new Xaxis()
                {
                    Mode = "time",
                    TickSize = getTickSize(param)
                },
                PagedItems = 0
            };
            return vm;
        }
        private List<TagsViewModel> getUpAndLow(double firstX, double lastX, int linkTagSeq)
        {
            List<TagsViewModel> vms = new List<TagsViewModel>();
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
                        TagsViewModel vm = new TagsViewModel()
                        {
                            Data = getUp(firstX, lastX, o.UpAlarm),
                            Label = "超值",
                            Yaxis = 1
                        };
                        vms.Add(vm);

                    }
                    else if (o.AlarmFlag == 2)
                    {
                        TagsViewModel vm = new TagsViewModel()
                        {
                            Data = getUp(firstX, lastX, o.LowAlarm),
                            Label = "低值",
                            Yaxis = 1
                        };
                        vms.Add(vm);
                    }
                    else if (o.AlarmFlag == 3)
                    {
                        TagsViewModel vm1 = new TagsViewModel()
                        {
                            Data = getUp(firstX, lastX, o.UpAlarm),
                            Label = "超值",
                            Yaxis = 1
                        };
                        vms.Add(vm1);
                        TagsViewModel vm2 = new TagsViewModel()
                        {
                            Data = getUp(firstX, lastX, o.LowAlarm),
                            Label = "低值",
                            Yaxis = 1
                        };
                        vms.Add(vm2);
                    }
                    return vms;
                }
            }
            return null;
        }
        private List<TagViewModel> getUp(double firstX, double lastX, double y)
        {
            List<TagViewModel> vms = new List<TagViewModel>();

            TagViewModel tag1 = new TagViewModel()
            {
                X = firstX,
                Y = y.ToString()
            };
            vms.Add(tag1);
            TagViewModel tag2 = new TagViewModel()
            {
                X = lastX,
                Y = y.ToString()
            };
            vms.Add(tag2);
            return vms;
        }
        /// <summary>
        /// 取得動態時間
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private KeyValuePair<int, string> getTickSize(DA.DataBase.Models.TagParamViewModel param)
        {

            TimeSpan s = new TimeSpan(param.StartDate.Ticks);
            TimeSpan e = new TimeSpan(param.EndDate.Ticks);
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
                return new KeyValuePair<int, string>(3, "hour");
            }
            if (hours > 6)
            {
                //
                return new KeyValuePair<int, string>(1, "hour");
            }
            if (hours > 3)
            {
                //
                return new KeyValuePair<int, string>(30, "minute");
            }
            if (hours > 1)
            {
                //
                return new KeyValuePair<int, string>(10, "minute");
            }
            if (minutes > 10)
            {
                return new KeyValuePair<int, string>(5, "minute");
            }
            return new KeyValuePair<int, string>(1, "minute");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdt"></param>
        /// <param name="edt"></param>
        /// <param name="linkTagSeq"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        private List<TagViewModel> getTagHistory(DA.DataBase.Models.TagParamViewModel param, int linkTagSeq)
        {
            List<TagViewModel> tags = new List<TagViewModel>();

            using (var db = new CMSDBContext())
            {
                var q = from a in db.TagHistory
                        where a.RecTime >= param.StartDate &&
                              a.RecTime <= param.EndDate &&
                              a.LinkTagSeq == linkTagSeq
                        orderby a.RecTime
                        select a;
                if (q.Any())
                {
                    List<TagHistory> t = new List<TagHistory>();

                    t = q.ToList().Skip(param.CurrentPage * param.ItemsPerPage).Take(param.ItemsPerPage).ToList();

                    foreach (var o in t)
                    {
                        TagViewModel tag = new TagViewModel()
                        {
                            X = o.RecTime.ToJavascriptTimestamp(),
                            Y = o.fValue.ToString()
                        };
                        tags.Add(tag);
                    }
                }

            }
            return tags;
        }
        /// <summary>
        /// 取得 LinkTag
        /// </summary>
        /// <param name="linkTagSeq"></param>
        /// <returns></returns>
        private string getLinkTag(int linkTagSeq)
        {
            using (var db = new CMSDBContext())
            {
                var q = from a in db.LinkTag
                        join b in db.MemTag on a.MTagSeq equals b.MTagSeq
                        where a.LinkTagSeq == linkTagSeq
                        select new { a, b };
                if (q.Any())
                {
                    var tagName = string.Format("{0}-{1}", q.FirstOrDefault().a.TagName,
                                                 q.FirstOrDefault().b.UnitName);
                    return tagName;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {

        }


    }
}
