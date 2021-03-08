using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OnlineTestForCLanguage.Configuration;
using OnlineTestForCLanguage.Web;

namespace OnlineTestForCLanguage.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class OnlineTestForCLanguageDbContextFactory : IDesignTimeDbContextFactory<OnlineTestForCLanguageDbContext>
    {
        public OnlineTestForCLanguageDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<OnlineTestForCLanguageDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            OnlineTestForCLanguageDbContextConfigurer.Configure(builder, configuration.GetConnectionString(OnlineTestForCLanguageConsts.ConnectionStringName));

            return new OnlineTestForCLanguageDbContext(builder.Options);
        }
    }
}
