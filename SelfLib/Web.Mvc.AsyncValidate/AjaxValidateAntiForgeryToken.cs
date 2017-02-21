using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
namespace Web.Mvc.AsyncValidate
{
    /// <summary>
    /// 异步表单防止CSRF攻击
    /// </summary>
    public class AjaxValidateAntiForgeryToken : AuthorizeAttribute
    {
        private string cookiesName = "";
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                if (request.IsAjaxRequest())
                {
                    var antiForgeryCookie = request.Cookies[cookiesName];
                    var cookieValue = antiForgeryCookie != null
                     ? antiForgeryCookie.Value
                     : null;
                    //从cookies 和 Headers 中 验证防伪标记
                    //这里可以加try-catch
                    try
                    {
                        AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                    }
                    catch (Exception ex)
                    {
                        filterContext.Result = new JsonResult()
                        {
                            Data = new JsonModel() { Success = false, Msg = ex.Message }
                        };
                    }
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute()
                     .OnAuthorization(filterContext);
                }
            }
        }
    }
}