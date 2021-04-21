using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Tests.Dto
{
    [AutoMapTo(typeof(TestDetail))]
    public class CreateTestCountDto : IShouldNormalize
    {
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }

        public long StudentId { get; set; }
        public long TestId { get; set; }
        /// <summary>
        /// 是否允许阅卷
        /// </summary>
        public bool IsInspected { get; set; }
        /// <summary>
        /// 学生总分
        /// </summary>
        public decimal StudentScoreSum { get; set; }

        public void Normalize()
        {
            CreationTime = DateTime.Now;
            IsDeleted = false;
        }
    }
}
