using System.Collections.Generic;
using OnlineTestForCLanguage.Roles.Dto;

namespace OnlineTestForCLanguage.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
