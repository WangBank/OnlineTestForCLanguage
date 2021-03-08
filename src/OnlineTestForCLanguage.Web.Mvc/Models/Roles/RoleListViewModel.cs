using System.Collections.Generic;
using OnlineTestForCLanguage.Roles.Dto;

namespace OnlineTestForCLanguage.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
