using Abp.MultiTenancy;
using OnlineTestForCLanguage.Authorization.Users;

namespace OnlineTestForCLanguage.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
