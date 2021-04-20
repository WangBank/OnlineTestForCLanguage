using OnlineTestForCLanguage.Sessions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Web.Models.Papers
{
    public class IndexPaperModalViewModel
    {
        public IReadOnlyList<ExamDto> Exams { get; set; }

    }
}
