using DA.DataBase.Entities;
using DA.DataBase.Models;
using DA.DataBase.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace appPortal.Controllers
{

    public class GroupController : ApiController
    {
        static GroupRepository repository = new GroupRepository();
        public GroupController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetGroups()
        {
            var root = repository.GetGroups();
            if (root != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, root);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤!!");
        }

        [HttpGet]
        public HttpResponseMessage GetGroup(int id)
        {
            var vm = new GroupViewModel();
            //Id = 0 回傳空值 新增群組用
            if (id == 0)
            {
                vm = new GroupViewModel()
                {
                    ParentGroup = new Groups(),
                    CurrentGroup = new Groups()
                };
            }
            else
            {
                vm = repository.GetGroupViewModel(id);
            }

            if (vm != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vm);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤!!");
        }
        /// <summary>
        /// 取得群組類型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupTypeValue"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetGroupTypes(int id, string groupTypeValue)
        {
            var pairs = repository.GetGroupTypes(groupTypeValue);
            if (pairs != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, pairs);
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤!!");
        }
        /// <summary>
        /// 新增群組
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertGroup([FromBody]GroupViewModel group)
        {
            var g = group;
            var status = repository.InsertGroup(group);

            if (status > 0)
            {
                return Request.CreateResponse<GroupViewModel>(HttpStatusCode.Accepted, group);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "新增 Groups 錯誤");
        }
        public HttpResponseMessage PutGroup(int id, [FromBody]GroupViewModel group)
        {
            //更新群組資料
            var status = repository.UpdateGroup(group);
            if (status > 0)
            {
                return Request.CreateResponse<GroupViewModel>(HttpStatusCode.Accepted, group);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "新增 Groups 錯誤");
        }
        public HttpResponseMessage DeleteGroup(int id)
        {
            var status = repository.DeleteGroup(id);
            if (status > 0)
            {
                return Request.CreateResponse<int>(HttpStatusCode.Accepted, id);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "刪除 Groups 錯誤");
        }

        /// <summary>
        /// 取得歷史資料
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetHistoryTags([FromBody]TagParamViewModel param)
        {
            //var result = repository.GetTagHistories(param);
            var result = repository.GetHistoryTags(param);

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "新增 Groups 錯誤");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage SetMaintain(int id)
        {
            var result = repository.SetMaintain(id);

            if (result >=0 )
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, " SetMaintain 錯誤");
        }
        [HttpPost]
        public HttpResponseMessage SaveBackground([FromBody]GroupImages image)
        {
            //var f = image;
            if (image != null)
            {
                var result = repository.SaveBackground(image);
                return Request.CreateResponse<GroupImages>(HttpStatusCode.Accepted, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得圖控圖檔
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetGroupImages(int id)
        {
            var result = repository.GetGroupImages(id);
            if (result != null)
            {

                return Request.CreateResponse<GroupImages>(HttpStatusCode.Accepted, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得圖控資訊
        /// </summary>
        /// <param name="id">GroupId</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetGroupLocations(int id)
        {
            var result = repository.GetGroupLocations(id);
            if (result != null)
            {

                return Request.CreateResponse(HttpStatusCode.Accepted, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        /// <summary>
        /// 更新圖控資訊
        /// </summary>
        /// <param name="id"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetGroupLocations(List<GroupLocationViewModel> locations)
        {
            var result = repository.SetGroupLocations(locations);
            if (result != null)
            {

                return Request.CreateResponse(HttpStatusCode.Accepted, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        /// <summary>
        /// 刪除圖控資訊
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteGroupLocation(GroupLocationViewModel location)
        {
            var result = repository.DeleteGroupLocation(location);
            if (result != null)
            {

                return Request.CreateResponse(HttpStatusCode.Accepted, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得圖控介面 select 1 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetLinkDevices()
        {
            var vms = repository.GetLinkDevices();
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得圖控介面 select 2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetLinkTags(int id)
        {
            var vms = repository.GetLinkTags(id);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得LinkTag
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetLinkTag(int id)
        {
            var vms = repository.GetLinkTag(id);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        /// <summary>
        /// 設定 LinkTag
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage SetLinkTag([FromBody] LinkTagViewModel vm)
        {
            var vms = repository.SetLinkTag(vm);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        
        /// <summary>
        /// 取得告警資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetTagAlarm(int id)
        {
            var vms = repository.GetTagAlarm(id);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
        }
        [HttpGet]
        public HttpResponseMessage GetDateTime()
        {
            var dt = DateTime.Now.ToString();
            //return Request.CreateResponse(HttpStatusCode.OK, dt);
            //return Request.CreateErrorResponse(HttpStatusCode.NotModified, "資料讀取錯誤");
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "資料讀取錯誤");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">GroupId</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetLinkGroups(int id)
        {
            var vms = repository.GetLinkGroups(id);

            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得歷史紀錄
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetTagHistories(int id, int locationId)
        {
            var vms = repository.GetTagHistories(id, locationId, 0);

            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得部門資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDepartments()
        {
            var vms = repository.GetDepartments();

            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得部門
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDepartment(int id)
        {
            var vms = repository.GetDepartments(id);

            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得Main Tool
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMainTools(int id)
        {
            var vms = repository.GetMainTools(id);

            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得 equipment 資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetEquipments(int id)
        {
            var vms = repository.GetEquipments(id);

            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "資料讀取錯誤");
        }
        /// <summary>
        /// 取得事件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetEvents(int id)
        {
            var vms = repository.GetEvents(id);

            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "資料讀取錯誤");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public HttpResponseMessage SetEvents(int id)
        {
            var vms = repository.SetEvents(id);

            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "資料讀取錯誤");
        }
    }
}
