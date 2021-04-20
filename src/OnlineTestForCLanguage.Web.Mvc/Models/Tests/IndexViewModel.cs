using OnlineTestForCLanguage.Sessions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Web.Models.Tests
{
    public class IndexViewModel
    {
        public IReadOnlyList<TestDto> Tests { get; }

        public IndexViewModel(IReadOnlyList<TestDto> _Tests)
        {
            Tests = _Tests;
        }
    }
}
