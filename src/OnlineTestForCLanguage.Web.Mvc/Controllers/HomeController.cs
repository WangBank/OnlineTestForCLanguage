using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using OnlineTestForCLanguage.Controllers;
using OnlineTestForCLanguage.Users;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : OnlineTestForCLanguageControllerBase
    {
        private readonly IUserAppService _userAppService;
        public HomeController(IUserAppService userAppService)
        {
             _userAppService = userAppService;
        }
        public async Task<ActionResult> Index()
        {
            //判断当前人的角色是老师还是学生还是管理员
            var nowRole =await _userAppService.GetNowRoles();
            if (nowRole.Contains("学生"))
            {
                return View("stuIndex");
            }

            if (nowRole.Contains("教师"))
            {
                return View("teaIndex");
            }
            return View();
        }
    }
}
