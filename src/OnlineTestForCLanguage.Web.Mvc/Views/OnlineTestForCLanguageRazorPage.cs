using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace OnlineTestForCLanguage.Web.Views
{
    public abstract class OnlineTestForCLanguageRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected OnlineTestForCLanguageRazorPage()
        {
            LocalizationSourceName = OnlineTestForCLanguageConsts.LocalizationSourceName;
        }
    }
}
