using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using OnlineTestForCLanguage.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Papers.Dto
{
    [AutoMapTo(typeof(Paper))]
    public class CreatePaperDto : IShouldNormalize
    {

        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Score { get; set; }
        public long CreateUserId { get; set; }
        public List<long> ExamList { get; set; }
        public void Normalize()
        {
            IsDeleted = false;
        }
    }

    public class AutoCreatePaperDto
    {
        public string Title { get; set; }
        public DifficultyType Difficulty { get; set; }
    }


}
