using OnlineTestForCLanguage.Sessions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Web.Models.Tests
{
    public class IndexViewModel
    {
        public IReadOnlyList<PaperDto> Papers { get; }
        public bool CanAddTest { get; }

        public IndexViewModel(IReadOnlyList<PaperDto> _Papers,bool canAddTest)
        {
            Papers = _Papers;
            CanAddTest = canAddTest;
        }
    }
}
