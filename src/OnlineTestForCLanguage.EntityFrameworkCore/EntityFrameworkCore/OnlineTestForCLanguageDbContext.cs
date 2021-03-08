using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using OnlineTestForCLanguage.Authorization.Roles;
using OnlineTestForCLanguage.Authorization.Users;
using OnlineTestForCLanguage.MultiTenancy;
using OnlineTestForCLanguage.Exams;

namespace OnlineTestForCLanguage.EntityFrameworkCore
{
    public class OnlineTestForCLanguageDbContext : AbpZeroDbContext<Tenant, Role, User, OnlineTestForCLanguageDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Exam> Exams { get; set; }
        public OnlineTestForCLanguageDbContext(DbContextOptions<OnlineTestForCLanguageDbContext> options)
            : base(options)
        {
        }
    }
}
