using Abp.AutoMapper;
using OnlineTestForCLanguage.Roles.Dto;
using OnlineTestForCLanguage.Web.Models.Common;

namespace OnlineTestForCLanguage.Web.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class EditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool HasPermission(FlatPermissionDto permission)
        {
            return GrantedPermissionNames.Contains(permission.Name);
        }
    }
}
