using System.Collections.Generic;
using System.Linq;
using OnlineTestForCLanguage.Roles.Dto;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Users.Dto;

namespace OnlineTestForCLanguage.Web.Models.Papers
{
    public class EditPaperModalViewModel
    {
        public PaperDto Paper { get; set; }
        public IReadOnlyList<ExamDto> Exams { get; set; }
        public bool ExamIsInDetail(ExamDto exam)
        {
            return Paper.PaperDetails != null && Paper.PaperDetails.Any(r => r.ExamId == exam.Id);
        }
        
    }
}
