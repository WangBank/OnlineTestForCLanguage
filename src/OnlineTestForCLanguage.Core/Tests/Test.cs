using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Tests
{
    [Table("Tests")]
    public class Test : Entity<long>, IHasCreationTime, ISoftDelete
    {
        public string Title { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public decimal TotalPoints { get; set; }

        /// <summary>
        /// 考试开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
        public long CreaterUserId { get; set; }
        public long PaperId { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public Test()
        {
            IsDeleted = false;
            CreationTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 这个表是存储 学生考试记录
    /// </summary>
    [Table("TestDetails")]
    public class TestDetail : Entity<long>, IHasCreationTime, ISoftDelete
    {
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }

        public TestDetail()
        {
            IsDeleted = false;
            CreationTime = DateTime.Now;
        }

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

    }

    /// <summary>
    /// 这个表存储学生考试填写的答案
    /// </summary>
    [Table("TestDetail_Exams")]
    public class TestDetail_Exam : Entity<long>, IHasCreationTime, ISoftDelete
    {
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }

        public TestDetail_Exam()
        {
            IsDeleted = false;
            CreationTime = DateTime.Now;
        }
        /// <summary>
        /// 哪场考试 哪个学生
        /// </summary>
        public long TestDetailId { get; set; }

        /// <summary>
        /// 哪个题目
        /// </summary>
        public long ExamId { get; set; }

        /// <summary>
        /// 得了多少分
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 简答题的话是 具体的内容
        /// 单选 多选或者判断的话 是examdetailid 以,号分割
        /// </summary>
        public string Answer { get; set; }
    }

}
