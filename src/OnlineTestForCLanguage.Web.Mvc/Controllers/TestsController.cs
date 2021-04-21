using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTestForCLanguage.Authorization;
using OnlineTestForCLanguage.Controllers;
using OnlineTestForCLanguage.Tests.Dto;
using OnlineTestForCLanguage.Sessions;
using OnlineTestForCLanguage.Papers.Dto;
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
        private readonly IPapersAppService _PaperAppService;
        public TestsController(ITestsAppService TestAppService, IPapersAppService PaperAppService)
        {
            _TestAppService = TestAppService;
            _PaperAppService = PaperAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _PaperAppService.GetAllAsync(new PagedPaperResultRequestDto());
            var model = new IndexViewModel(output.Items);
            return View(model);
        }
        public async Task<ActionResult> EditModal(long TestId)
        {
            var Test = await _TestAppService.GetAsync(new EntityDto<long>(TestId));
            var output = await _PaperAppService.GetAllAsync(new PagedPaperResultRequestDto());
            var model = new EditTestModalViewModel();
            model.Test = Test;
            model.Papers = output.Items;
            return PartialView("_EditModal", model);
        }


        public async Task<ActionResult> StartModal(long TestId)
        {
            var Test = await _TestAppService.GetAsync(new EntityDto<long>(TestId));
            var output = await _PaperAppService.GetAllAsync(new PagedPaperResultRequestDto());
            var model = new EditTestModalViewModel();
            model.Test = Test;
            model.Papers = output.Items;
            return PartialView("_StartModal", model);
        }
    }
}
