using Abp.AutoMapper;
using Abp.Runtime.Validation;
using OnlineTestForCLanguage.Sessions.Dto;
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

        public long StudentId { get; set; } = -1;
        public long TestId { get; set; }
        /// <summary>
        /// 是否允许阅卷
        /// </summary>
        public bool IsInspected { get; set; }
        /// <summary>
        /// 学生总分
        /// </summary>
        public decimal StudentScoreSum { get; set; }
        
        public List<TestDetail_ExamDto> detail_Exams { get; set; }

        public void Normalize()
        {
            CreationTime = DateTime.Now;
            IsDeleted = false;
        }
    }
    [AutoMapTo(typeof(TestDetail_Exam))]
    public class TestDetail_ExamDto : IShouldNormalize
    {
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 哪个题目
        /// </summary>
        public long ExamId { get; set; }

        public ExamDto Exam { get; set; }
        /// <summary>
        /// 得了多少分
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 简答题的话是 具体的内容
        /// 单选 多选或者判断的话 是examdetailid 以,号分割
        /// </summary>
        public string Answers { get; set; }
        public string AnswersWithAbc { get
            {
                if (!string.IsNullOrEmpty(Answers))
                {
                    return Answers.Replace("answerid0", "A").Replace("answerid1", "B").Replace("answerid2", "C").Replace("answerid3", "D");
                }
                else
                {
                    return "";
                }
            } }
        public void Normalize()
        {
            CreationTime = DateTime.Now;
            IsDeleted = false;
        }
    }

}
