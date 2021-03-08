using Abp.AspNetCore.Mvc.ViewComponents;

namespace OnlineTestForCLanguage.Web.Views
{
    public abstract class OnlineTestForCLanguageViewComponent : AbpViewComponent
    {
        protected OnlineTestForCLanguageViewComponent()
        {
            LocalizationSourceName = OnlineTestForCLanguageConsts.LocalizationSourceName;
        }
    }
}
