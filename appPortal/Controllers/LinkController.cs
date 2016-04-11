using FABTool.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace appPortal.Controllers
{
    public class LinkController : ApiController
    {
        static OrganizationRepository orgRepo = new OrganizationRepository();
        static LinkRepository linkRepo = new LinkRepository();

        #region 取得組織 GetOrganization
        /// <summary>
        /// 取得組織 GetOrganization
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetOrganization()
        {
            var vms = orgRepo.GetOrganizations();
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "取得組織錯誤!!");
        }
        #endregion
        #region 取得部門 GetDepartment
        /// <summary>
        /// 取得部門 GetDepartment
        /// </summary>
        /// <param name="id">組織序號</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDepartments(int id)
        {
            var vms = orgRepo.GetDepartments(id);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "取得部門錯誤!!");
        }
        #endregion
        #region 取得主機 GetMainTools
        /// <summary>
        /// 取得主機台 GetMainTools
        /// </summary>
        /// <param name="id">部門序號</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMainTools(int id)
        {
            var vms = orgRepo.GetMainTools(id);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "取得主機錯誤!!");
        }
        #endregion
        #region 取得設備 GetEquipments
        /// <summary>
        /// 取得設備
        /// </summary>
        /// <param name="id">主機台序號</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetEquipments(int id)
        {
            var vms = orgRepo.GetEquipments(id);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "取得設備錯誤!!");
        }
        #endregion
        #region 取得機台 LinkTag
        /// <summary>
        /// 取得機台 LinkTag
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMainToolLinkTags(int id)
        {
            var vms = linkRepo.GetMainToolLinkTags(id);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "取得機台LinkTag錯誤!!");
        }
        #endregion
        #region 取得設備  LinkTag
        /// <summary>
        /// 取得設備 LinkTag
        /// </summary>
        /// <param name="id">MainTool Id</param>
        /// <returns></returns>
        public HttpResponseMessage GetEquipmentLinkTags(int id)
        {
            var vms = linkRepo.GetEquipmentLinkTags(id);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "取得設備LinkTag錯誤!!");
        }
        #endregion
        #region 取得設備LinkTag  GetDeviceLinkTags
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDeviceLinkTags(int id)
        {
            var vms = linkRepo.GetDeviceLinkTags(id);
            if (vms != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, vms);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "取得設備LinkTag錯誤!!");
        }
        #endregion
        #region 取得趨勢圖 GetHistoryTags
        /// <summary>
        /// 取得趨勢圖
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetHistoryTags([FromBody]DA.DataBase.Models.TagParamViewModel param)
        {

            var result = linkRepo.GetHistoryTags(param);

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "取得趨勢圖錯誤");
        }
        #endregion
        /// <summary>
        /// 取得趨勢圖
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetHistoryTag(DA.DataBase.Models.TagParam param)
        {
            var result = linkRepo.GetHistoryTag(param);

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, result);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "取得趨勢圖錯誤");
        }
    }
}
