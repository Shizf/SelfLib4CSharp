using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Mvc.AsyncValidate
{
    /// <summary>
    /// 身份验证信息
    /// </summary>
    public class AuthValidate : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session != null)
            {
                var session = httpContext.Session["UserInfo"];
                if (session != null)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //重新登录
            filterContext.HttpContext.Response.Redirect("~/Member/UserInfo/Login");
        }
    }
}