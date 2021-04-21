using System.Collections.Generic;
using System.Linq;
using OnlineTestForCLanguage.Roles.Dto;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Users.Dto;

namespace OnlineTestForCLanguage.Web.Models.Tests
{
    public class EditTestModalViewModel
    {
        public TestDto Test { get; set; }

        public IReadOnlyList<PaperDto> Papers { get; set; }

        public bool PaperIsInDetail(PaperDto paper)
        {
            return Test.PaperId == paper.Id;
        }
    }
}
