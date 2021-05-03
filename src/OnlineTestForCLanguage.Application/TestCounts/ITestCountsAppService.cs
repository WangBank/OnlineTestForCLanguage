using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using OnlineTestForCLanguage.Tests;
using OnlineTestForCLanguage.Tests.Dto;
using OnlineTestForCLanguage.Sessions.Dto;

namespace OnlineTestForCLanguage.Sessions
{
    public interface ITestCountsAppService : IAsyncCrudAppService<TestCountDto, long, PagedTestCountResultRequestDto, CreateTestCountDto, TestCountDto>
    {
        Task<TestCountDto> GetInspectAsync(EntityDto<long> input);
    }
}
