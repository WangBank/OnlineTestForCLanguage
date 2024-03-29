﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Exams
{
    [Table("Exams")]
    public class Exam : Entity<long>, IHasCreationTime, ISoftDelete
    {
        [Required]
        public ExamType ExamType { get; set; }

        public DifficultyType Difficulty { get; set; }

        [StringLength(4000)]
        public string Content { get; set; }
        [StringLength(4000)]
        public string Explain { get; set; }

        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Score { get; set; }
        public virtual ICollection<ExamDetail> ExamDetails { get; set; }
        /// <summary>
        /// 正确的考题明细id,以,号分割
        /// </summary>
        public string CorrectDetailIds { get; set; }

        public Exam()
        {
            IsDeleted = false;
            CreationTime = DateTime.Now;
        }
    }

    public enum DifficultyType
    {
        [Description("简单")]
        simple = 0,

        [Description("一般")]
        general = 1,

        [Description("困难")]
        difficult = 2,
        [Description("全部")]
        [JsonIgnore]
        all = 3
    }

    public enum ExamType
    {
        [Description("单选题")]
        SingleSelect = 0,

        [Description("多选题")]
        MulSelect = 1,

        [Description("判断题")]
        Judge = 2,

        [Description("简答题")]
        ShortAnswer = 3,

        [Description("全部")]
        [JsonIgnore]
        all =4

    }

    [Table("ExamDetails")]
    public class ExamDetail : Entity<long>, IHasCreationTime, ISoftDelete
    {
        public virtual long ExamId { get; set; }
        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public string AnswerId { get; set; }
        public ExamDetail()
        {
            IsDeleted = false;
            CreationTime = DateTime.Now;
        } 
    }

}
