using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using OnlineTestForCLanguage.Tests;
using OnlineTestForCLanguage.Tests.Dto;
using OnlineTestForCLanguage.Sessions.Dto;

namespace OnlineTestForCLanguage.Sessions
{
    public interface ITestsAppService : IAsyncCrudAppService<TestDto, long, PagedTestResultRequestDto, CreateTestDto, TestDto>
    {
    }
}
