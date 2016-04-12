using DA.DataBase;
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
                var g = q.GroupBy(p => new { p.e.LinkSubSeq, p.f, p.i }).Select(r => new { r.Key.f, r.Key.i });
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
                                tag.CurSubStaName = "多態告警狀態";
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
        /// <param name="param"></param>
        /// <returns></returns>
        public ChartViewModel GetHistoryTag(DA.DataBase.Models.TagParam param)
        {
            List<TagsViewModel> tags = new List<TagsViewModel>();
            var recTime = DateTime.Now.AddDays(-60);
            List<Yaxis> yaxes = new List<Yaxis>();

            using (var db = new CMSDBContext())
            {
                //取得單點
                var q = from a in db.GroupLocations
                        join b in db.LinkTag on a.LinkTagSeq equals b.LinkTagSeq
                        where a.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              b.ModifyFlag < (byte)ModifyFlagEnum.Delete &&
                              a.GroupId == param.GroupId &&
                              a.LocationId == param.LocationId
                        select new { a, b };

                if (q.Any())
                {
                    int y = 0;
                    var o = q.FirstOrDefault();
                    var t = getTagHistory(recTime, DateTime.Now, o.a.LinkTagSeq);

                    TagsViewModel tag = new TagsViewModel()
                    {
                        Label = o.b.TagName,
                        Yaxis = y,
                        Data = t,
                    };
                    tags.Add(tag);

                    var yaxis = new Yaxis();
                    yaxis.Position = "left";
                    yaxes.Add(yaxis);

                    double first = t.FirstOrDefault().X;
                    double last = t.LastOrDefault().X;

                    //設定 上下限值
                    if (o.b.AlarmFlag >= 1)
                    {
                        //超值
                        if (o.b.AlarmFlag == 1)
                        {
                            tags.Add(new TagsViewModel()
                            {
                                Label = "超值",
                                Yaxis = 1,
                                Data = getUp(first, last, o.b.UpAlarm)
                            });
                            var yaxis1 = new Yaxis();
                            yaxis1.Position = "right";
                            yaxes.Add(yaxis1);
                        }
                        //低值
                        if (o.b.AlarmFlag == 2)
                        {
                            tags.Add(new TagsViewModel()
                            {
                                Label = "低值",
                                Yaxis = 1,
                                Data = getLow(first, last, o.b.LowAlarm)
                            });
                            var yaxis1 = new Yaxis();
                            yaxis1.Position = "right";
                            yaxes.Add(yaxis1);
                        }
                        //2者都有
                        if (o.b.AlarmFlag == 3)
                        {
                            tags.Add(new TagsViewModel()
                            {
                                Label = "超值",
                                Yaxis = 1,
                                Data = getUp(first, last, o.b.UpAlarm)
                            });
                            var yaxis2 = new Yaxis();
                            yaxis2.Position = "right";
                            yaxes.Add(yaxis2);
                            {
                                tags.Add(new TagsViewModel()
                                {
                                    Label = "低值",
                                    Yaxis = 2,
                                    Data = getLow(first, last, o.b.LowAlarm)
                                });
                            }
                            var yaxis3 = new Yaxis();
                            yaxis3.Position = "right";
                            yaxes.Add(yaxis3);
                        }
                    }
                }
            }
            ChartViewModel vm = new ChartViewModel()
            {
                Datasets = tags,
                Yaxes = yaxes,
                Xaxis = new Xaxis()
                {
                    Mode = "time",
                    TickSize = new KeyValuePair<int, string>(5, "minute") 
                }
            };
            return vm;
        }
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
            //開始時間
            DateTime sdt = new DateTime(param.StartDate.Year, param.StartDate.Month, param.StartDate.Day,
                param.StartTime.Hour, param.StartTime.Minute, 0);
            //結束時間
            var edt = new DateTime(param.EndDate.Year, param.EndDate.Month, param.EndDate.Day,
                param.EndTime.Hour, param.EndTime.Minute, 0);
            //多選
            List<int> selected = new List<int>();
            List<Yaxis> yaxes = new List<Yaxis>();
            int y = 0;
            foreach (var linkTag in param.LinkTags)
            {
                if (linkTag.Selected == true)
                {
                    y++;
                    var t = getTagHistory(sdt, edt, linkTag.LinkTagSeq);
                    TagsViewModel tag = new TagsViewModel()
                    {
                        Label = linkTag.TagName,
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
            }
            
            ChartViewModel vm = new ChartViewModel()
            {
                Datasets = tags,
                Yaxes = yaxes,
                Xaxis = new Xaxis()
                {
                    Mode = "time",
                    TickSize = new KeyValuePair<int, string>(5, "minute")
                }

            };
            return vm;
        }

        private List<TagViewModel> getTagHistory(DateTime sdt, DateTime edt, int linkTagSeq)
        {
            List<TagViewModel> tags = new List<TagViewModel>();
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
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {

        }


    }
}
