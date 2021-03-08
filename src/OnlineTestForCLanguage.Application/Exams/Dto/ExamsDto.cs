using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using OnlineTestForCLanguage.Exams;
using OnlineTestForCLanguage.Helpers;
using System;
using System.Collections.Generic;

namespace OnlineTestForCLanguage.Sessions.Dto
{
    [AutoMapFrom(typeof(Exam))]
    public class ExamDto : EntityDto, IHasCreationTime
    {
        public ExamType ExamType { get; set; }

        public DifficultyType Difficulty { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }

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
