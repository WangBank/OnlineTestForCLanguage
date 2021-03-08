using System.Collections.Generic;
using OnlineTestForCLanguage.Roles.Dto;

namespace OnlineTestForCLanguage.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}