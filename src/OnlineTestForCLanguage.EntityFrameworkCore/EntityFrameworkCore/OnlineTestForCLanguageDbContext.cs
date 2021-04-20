using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using OnlineTestForCLanguage.Authorization.Roles;
using OnlineTestForCLanguage.Authorization.Users;
using OnlineTestForCLanguage.MultiTenancy;
using OnlineTestForCLanguage.Exams;
using OnlineTestForCLanguage.Tests;
using OnlineTestForCLanguage.Papers;

namespace OnlineTestForCLanguage.EntityFrameworkCore
{
    public class OnlineTestForCLanguageDbContext : AbpZeroDbContext<Tenant, Role, User, OnlineTestForCLanguageDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamDetail> ExamDetails { get; set; }
       
        
        public DbSet<Paper> Papers { get; set; }
        public DbSet<PaperDetail> PaperDetails { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<TestDetail> TestDetails { get; set; }
        public DbSet<TestDetail_Exam> TestDetail_Exams { get; set; }
        
        public OnlineTestForCLanguageDbContext(DbContextOptions<OnlineTestForCLanguageDbContext> options)
            : base(options)
        {
        }
    }
}
