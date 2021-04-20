using OnlineTestForCLanguage.Sessions.Dto;

namespace OnlineTestForCLanguage.Web.Views.Shared.Components.SideBarUserArea
{
    public class SideBarUserAreaViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }
        public string GetShownLoginName()
        {
            var userName = LoginInformations.User.UserName;
            return  userName;
        }
    }
}
