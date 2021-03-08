using Abp.Authorization;
using OnlineTestForCLanguage.Authorization.Roles;
using OnlineTestForCLanguage.Authorization.Users;

namespace OnlineTestForCLanguage.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
