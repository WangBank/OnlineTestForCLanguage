using System.Threading.Tasks;
using Abp.Application.Services;
using OnlineTestForCLanguage.Authorization.Accounts.Dto;

namespace OnlineTestForCLanguage.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
