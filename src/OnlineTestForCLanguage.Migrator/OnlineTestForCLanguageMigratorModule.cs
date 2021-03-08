using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using OnlineTestForCLanguage.Configuration;
using OnlineTestForCLanguage.EntityFrameworkCore;
using OnlineTestForCLanguage.Migrator.DependencyInjection;

namespace OnlineTestForCLanguage.Migrator
{
    [DependsOn(typeof(OnlineTestForCLanguageEntityFrameworkModule))]
    public class OnlineTestForCLanguageMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public OnlineTestForCLanguageMigratorModule(OnlineTestForCLanguageEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(OnlineTestForCLanguageMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                OnlineTestForCLanguageConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(OnlineTestForCLanguageMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
