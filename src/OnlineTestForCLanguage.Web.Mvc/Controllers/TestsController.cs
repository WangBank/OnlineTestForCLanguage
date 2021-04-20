using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTestForCLanguage.Authorization;
using OnlineTestForCLanguage.Controllers;
using OnlineTestForCLanguage.Tests.Dto;
using OnlineTestForCLanguage.Sessions;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Web.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tests)]
    public class TestsController : OnlineTestForCLanguageControllerBase
    {
        private readonly ITestsAppService _TestAppService;

        public TestsController(ITestsAppService TestAppService)
        {
            _TestAppService = TestAppService;
        }

        public async Task<ActionResult> Index(PagedTestResultRequestDto input)
        {
            var output = await _TestAppService.GetAllAsync(input);
            var model = new IndexViewModel(output.Items);
            return View(model);
        }


        public async Task<ActionResult> EditModal(long TestId)
        {
            var Test = await _TestAppService.GetAsync(new EntityDto<long>(TestId));
            var model = new EditTestModalViewModel();
            model.Test = Test;
            return PartialView("_EditModal", model);
        }
    }
}
