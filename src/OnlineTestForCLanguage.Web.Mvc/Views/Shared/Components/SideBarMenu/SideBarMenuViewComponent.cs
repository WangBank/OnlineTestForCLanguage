using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using OnlineTestForCLanguage.Authorization.Users;

namespace OnlineTestForCLanguage.Web.Views.Shared.Components.SideBarMenu
{
    public class SideBarMenuViewComponent : OnlineTestForCLanguageViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        public SideBarMenuViewComponent(
            IUserNavigationManager userNavigationManager,
            UserManager userManager,
            IAbpSession abpSession)
        {
            _userNavigationManager = userNavigationManager;
            _abpSession = abpSession;
            _userManager = userManager;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var roles = await _userManager.GetRolesAsync(user);
            

            var model = new SideBarMenuViewModel
            {
                MainMenu = await _userNavigationManager.GetMenuAsync("MainMenu", _abpSession.ToUserIdentifier())
            };
            if (roles.Contains("学生"))
            {
                foreach (var item in model.MainMenu.Items)
                {
                    if (item.DisplayName == "考试管理")
                    {
                        item.DisplayName = "进行考试";
                    }

                    if (item.DisplayName == "考试统计")
                    {
                        item.DisplayName = "成绩查询";
                    }
                }
            }
            return View(model);
        }
    }
}
