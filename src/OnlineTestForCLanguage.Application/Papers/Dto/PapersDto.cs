using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using OnlineTestForCLanguage.Papers;
using OnlineTestForCLanguage.Helpers;
using System;
using System.Collections.Generic;

namespace OnlineTestForCLanguage.Sessions.Dto
{
    [AutoMapFrom(typeof(Paper))]
    public class PaperDto : EntityDto<long>, IHasCreationTime
    {
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Score { get; set; }
        public long CreateUserId { get; set; }
        public virtual ICollection<PaperDetailDto> PaperDetails { get; set; }
    }

    public class PaperDetailDto : EntityDto<long>
    {
        public long PaperId { get; set; }
        public Paper Paper { get; set; }
        public long ExamId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
