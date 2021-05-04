using System.Collections.Generic;
using System.Linq;
using OnlineTestForCLanguage.Exams.Dto;
using OnlineTestForCLanguage.Roles.Dto;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Users.Dto;

namespace OnlineTestForCLanguage.Web.Models.TestCounts
{
    public class CheckTestCountModalViewModel
    {
        public TestCountDto TestCount { get; set; }

        public List<ExamDetailDto> examDetails { get; set; }

    }
}
