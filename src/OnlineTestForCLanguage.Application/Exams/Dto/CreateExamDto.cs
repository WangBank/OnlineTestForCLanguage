using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Exams.Dto
{
    [AutoMapTo(typeof(Exam))]
    public class CreateExamDto : IShouldNormalize
    {
        public ExamType ExamType { get; set; }
        public decimal Score { get; set; }
        public DifficultyType Difficulty { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public List<CreateExamDetailDto> answers { get; set; }
        public string CorrectAnswerId { get; set; }
        public string Explain { get; set; }
        public void Normalize()
        {
            CreationTime = DateTime.Now;
            IsDeleted = false;
        }
    }

    [AutoMapTo(typeof(ExamDetail))]
    public class CreateExamDetailDto
    {
        public string Content { get; set; }
        public string AnswerId { get; set; }
    }
}
