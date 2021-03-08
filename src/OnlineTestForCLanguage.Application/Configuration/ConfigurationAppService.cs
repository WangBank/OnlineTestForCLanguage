using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using OnlineTestForCLanguage.Configuration.Dto;

namespace OnlineTestForCLanguage.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : OnlineTestForCLanguageAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
