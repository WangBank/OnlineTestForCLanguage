using OnlineTestForCLanguage.Sessions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Web.Models.Exams
{
    public class IndexViewModel
    {
        public IReadOnlyList<ExamDto> Exams { get; }

        public IndexViewModel(IReadOnlyList<ExamDto> exams)
        {
            Exams = exams;
        }
    }
}
