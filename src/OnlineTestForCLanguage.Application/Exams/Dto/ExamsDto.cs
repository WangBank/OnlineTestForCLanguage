using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using OnlineTestForCLanguage.Exams;
using OnlineTestForCLanguage.Exams.Dto;
using OnlineTestForCLanguage.Helpers;
using System;
using System.Collections.Generic;

namespace OnlineTestForCLanguage.Sessions.Dto
{
    [AutoMapFrom(typeof(Exam))]
    public class ExamDto : EntityDto<long>, IHasCreationTime
    {
   
        public ExamType ExamType { get; set; }

        public DifficultyType Difficulty { get; set; }


        public string Content { get; set; }
       
        public string Explain { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Score { get; set; }
        /// <summary>
        /// 正确的考题明细id,以,号分割
        /// </summary>
        public string CorrectDetailIds { get; set; }
        public string CorrectDetailIdsWithABCD {
            get
            {
                if (!string.IsNullOrEmpty(CorrectDetailIds))
                {
                    return CorrectDetailIds.Replace("answerid0", "A").Replace("answerid1", "B").Replace("answerid2", "C").Replace("answerid3", "D");
                }
                else
                {
                    return "";
                }
               
            }
        }
        public List<ExamDetailDto> answers { get; set; }
        public string ExamType_Info
        {
            get
            {
                return EnumHelper.GetEnumDescription(ExamType);
            }
        }
        public string Difficulty_Info
        {
            get
            {
                return EnumHelper.GetEnumDescription(Difficulty);
            }
        }
    }
}
