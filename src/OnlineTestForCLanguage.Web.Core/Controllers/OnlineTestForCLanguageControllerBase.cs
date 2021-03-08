using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace OnlineTestForCLanguage.Controllers
{
    public abstract class OnlineTestForCLanguageControllerBase: AbpController
    {
        protected OnlineTestForCLanguageControllerBase()
        {
            LocalizationSourceName = OnlineTestForCLanguageConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
