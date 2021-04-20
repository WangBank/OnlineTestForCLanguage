using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Tests.Dto
{
    [AutoMapTo(typeof(Test))]
    public class CreateTestDto : IShouldNormalize
    {
        public string Title { get; set; }
      
        public decimal TotalPoints { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
        public long CreaterUserId { get; set; }
        public long PaperId { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }

        public void Normalize()
        {
            IsDeleted = false;
        }
    }
}
