using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using OnlineTestForCLanguage.Controllers;

namespace OnlineTestForCLanguage.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : OnlineTestForCLanguageControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
