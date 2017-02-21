using System;
using System.Web.Mvc;

namespace Web.Mvc.AsyncValidate
{
    /// <summary>
    /// 只能门店能够操作的权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ShopAuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null)
            {
                var user = (UserInfo)filterContext.HttpContext.Session["UserInfo"];
                if (!user.IsShop)
                {
                    filterContext.Result = new ContentResult {Content = @"抱歉,只有门店有当前操作的权限！"};
                }
            }
        }
    }

    /// <summary>
    /// 只能店长能够操作的权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AdminAuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null)
            {
                var user = (UserInfo)filterContext.HttpContext.Session["UserInfo"];
                if (!user.IsAdmin)
                {
                    bool isAjax = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();
                    if (isAjax)
                    {
                        filterContext.Result = new JsonResult()
                        {
                            Data = new JsonModel() {Success = false, Msg = "抱歉,只有店长有当前操作的权限！"}
                        };
                    }
                    else
                    {
                        filterContext.Result = new ContentResult {Content = @"抱歉,只有店长有当前操作的权限！"};
                    }
                }
            }
        }
    }
}