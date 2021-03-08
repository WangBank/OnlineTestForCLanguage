using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using OnlineTestForCLanguage.Configuration;

namespace OnlineTestForCLanguage.Web.Startup
{
    [DependsOn(typeof(OnlineTestForCLanguageWebCoreModule))]
    public class OnlineTestForCLanguageWebMvcModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public OnlineTestForCLanguageWebMvcModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<OnlineTestForCLanguageNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(OnlineTestForCLanguageWebMvcModule).GetAssembly());
        }
    }
}
