using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using OnlineTestForCLanguage.Papers;
using OnlineTestForCLanguage.Papers.Dto;
using OnlineTestForCLanguage.Sessions.Dto;

namespace OnlineTestForCLanguage.Sessions
{
    public interface IPapersAppService : IAsyncCrudAppService<PaperDto, long, PagedPaperResultRequestDto, CreatePaperDto, PaperDto>
    {
    }
}
