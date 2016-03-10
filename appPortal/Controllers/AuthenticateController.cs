using DA.DataBase.Entities;
using DA.DataBase.Models;
using DA.DataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace appPortal.Controllers
{
    public class AuthenticateController : ApiController
    {
        //DA.DataBase.CMSDBContext()
        MenuRepository _Repository;
        public AuthenticateController()
        {
            _Repository = new MenuRepository();


        }

        [HttpGet]
        public HttpResponseMessage CustomerById(int id)
        {
            var menus = _Repository.GetMenus();

            return Request.CreateResponse(HttpStatusCode.OK, menus);
        }

        public HttpResponseMessage Login([FromBody] UserModel user)
        {
            if (user.Name == "admin")
            {
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            return Request.CreateResponse(HttpStatusCode.OK, false);
        }
    }
}
