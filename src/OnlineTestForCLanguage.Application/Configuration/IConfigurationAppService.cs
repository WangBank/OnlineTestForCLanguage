using System.Threading.Tasks;
using OnlineTestForCLanguage.Configuration.Dto;

namespace OnlineTestForCLanguage.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
