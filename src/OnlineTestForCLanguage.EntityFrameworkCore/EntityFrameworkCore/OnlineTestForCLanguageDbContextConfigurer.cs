using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace OnlineTestForCLanguage.EntityFrameworkCore
{
    public static class OnlineTestForCLanguageDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<OnlineTestForCLanguageDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<OnlineTestForCLanguageDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
