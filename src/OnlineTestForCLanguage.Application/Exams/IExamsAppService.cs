using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using OnlineTestForCLanguage.Exams;
using OnlineTestForCLanguage.Exams.Dto;
using OnlineTestForCLanguage.Sessions.Dto;

namespace OnlineTestForCLanguage.Sessions
{
    public interface IExamsAppService : IAsyncCrudAppService<ExamDto, long, PagedExamResultRequestDto, CreateExamDto, ExamDto>
    {
         Task<List<ExamDto>> GetAllNoPageAsync();
    }
}
