using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using OnlineTestForCLanguage.Tests;
using OnlineTestForCLanguage.Helpers;
using System;
using System.Collections.Generic;

namespace OnlineTestForCLanguage.Sessions.Dto
{
    [AutoMapFrom(typeof(Test))]
    public class TestDto : EntityDto<long>, IHasCreationTime
    {

        public string Title { get; set; }

        public decimal TotalPoints { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
        public long CreaterUserId { get; set; }
        public long PaperId { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
