using Abp.AutoMapper;
using OnlineTestForCLanguage.Sessions.Dto;

namespace OnlineTestForCLanguage.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}
