using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTestForCLanguage.Authorization;
using OnlineTestForCLanguage.Controllers;
using OnlineTestForCLanguage.Tests.Dto;
using OnlineTestForCLanguage.Sessions;
using OnlineTestForCLanguage.Papers.Dto;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Web.Models.TestCounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTestForCLanguage.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Exams, PermissionNames.Pages_Papers, PermissionNames.Pages_TestCounts, PermissionNames.Pages_Tests)]
    public class TestCountsController : OnlineTestForCLanguageControllerBase
    {
        private readonly ITestCountsAppService _TestCountAppService;
        private readonly IPapersAppService _PaperAppService;
        public TestCountsController(ITestCountsAppService TestCountAppService, IPapersAppService PaperAppService)
        {
            _TestCountAppService = TestCountAppService;
            _PaperAppService = PaperAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new IndexTestCountViewModel();
            return View(model);
        }
        public async Task<ActionResult> InspectModal(long TestCountId)
        {
            var TestCount = await _TestCountAppService.GetInspectAsync(new EntityDto<long>(TestCountId));   
            var model = new InspectTestCountModalViewModel();
            model.TestCount = TestCount;
            return PartialView("_InspectModal", model);
        }
    }
}
