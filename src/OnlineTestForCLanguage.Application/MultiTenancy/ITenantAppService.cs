using Abp.Application.Services;
using OnlineTestForCLanguage.MultiTenancy.Dto;

namespace OnlineTestForCLanguage.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

