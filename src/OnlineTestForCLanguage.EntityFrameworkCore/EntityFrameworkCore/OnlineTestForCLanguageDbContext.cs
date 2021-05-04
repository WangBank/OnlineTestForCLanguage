using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using OnlineTestForCLanguage.Authorization.Roles;
using OnlineTestForCLanguage.Authorization.Users;
using OnlineTestForCLanguage.MultiTenancy;
using OnlineTestForCLanguage.Exams;
using OnlineTestForCLanguage.Tests;
using OnlineTestForCLanguage.Papers;
using Microsoft.Extensions.Logging;
using System;

namespace OnlineTestForCLanguage.EntityFrameworkCore
{
    public class EFLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new EFLogger(categoryName);
        public void Dispose() { }
    }

    public class EFLogger : ILogger
    {
        private readonly string categoryName;

        public EFLogger(string categoryName) => this.categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command"
                    && logLevel == LogLevel.Information)
            {
                var logContent = formatter(state, exception);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(logContent);
                Console.ResetColor();
            }
        }

        IDisposable BeginScope<TState>(TState state) => null;

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var loggerFactory = new LoggerFactory();
            //loggerFactory.AddProvider(new EFLoggerProvider());
            //optionsBuilder.UseLoggerFactory(loggerFactory);
            //optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }

    }
}
