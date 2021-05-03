using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using OnlineTestForCLanguage.Tests;
using OnlineTestForCLanguage.Helpers;
using System;
using System.Collections.Generic;
using OnlineTestForCLanguage.Tests.Dto;

namespace OnlineTestForCLanguage.Sessions.Dto
{
    [AutoMapFrom(typeof(TestDetail))]
    public class TestCountDto : EntityDto<long>, IHasCreationTime
    {
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }

        public TestCountDto()
        {
            IsDeleted = false;
            CreationTime = DateTime.Now;
        }

        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public long TestId { get; set; }
        /// <summary>
        /// 是否允许阅卷
        /// </summary>
        public bool IsInspected { get; set; }

        public  bool CanInspect { get; set; }
        /// <summary>
        /// 学生总分
        /// </summary>
        public decimal StudentScoreSum { get; set; }
        public decimal SumScore { get; set; } = 100;
        public long TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TestTitle { get; set; }
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
        public List<TestDetail_ExamDto> detail_Exams { get; set; }
    }
}
