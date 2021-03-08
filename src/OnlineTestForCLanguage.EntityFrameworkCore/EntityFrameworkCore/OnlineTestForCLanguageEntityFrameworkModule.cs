using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using OnlineTestForCLanguage.EntityFrameworkCore.Seed;

namespace OnlineTestForCLanguage.EntityFrameworkCore
{
    [DependsOn(
        typeof(OnlineTestForCLanguageCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class OnlineTestForCLanguageEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<OnlineTestForCLanguageDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        OnlineTestForCLanguageDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        OnlineTestForCLanguageDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(OnlineTestForCLanguageEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
