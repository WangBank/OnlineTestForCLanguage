using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using OnlineTestForCLanguage.Authorization;

namespace OnlineTestForCLanguage
{
    [DependsOn(
        typeof(OnlineTestForCLanguageCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class OnlineTestForCLanguageApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<OnlineTestForCLanguageAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(OnlineTestForCLanguageApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
