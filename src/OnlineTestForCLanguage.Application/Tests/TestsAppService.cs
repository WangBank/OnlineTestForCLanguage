using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using OnlineTestForCLanguage.Authorization;
using OnlineTestForCLanguage.Tests;
using OnlineTestForCLanguage.Tests.Dto;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Authorization.Users;

namespace OnlineTestForCLanguage.Sessions
{
    [AbpAuthorize(PermissionNames.Pages_Exams, PermissionNames.Pages_Papers, PermissionNames.Pages_TestCounts, PermissionNames.Pages_Tests)]
    public class TestsAppService : AsyncCrudAppService<Test, TestDto, long, PagedTestResultRequestDto,CreateTestDto,TestDto>, ITestsAppService
    {
        private readonly UserManager _userManager;
        private readonly IRepository<Test, long> _TestRepository;
        public TestsAppService(IRepository<Test, long> TestRepository, UserManager userManager) : base(TestRepository)
        {
            _userManager = userManager;
               _TestRepository = TestRepository;
        }
        public async Task<ListResultDto<TestDto>> GetTestsAsync(PagedTestResultRequestDto input)
        {
            var Tests = await GetAllAsync(input);
            Tests.Items = Tests.Items.OrderBy(e=>e.Id).ToList();
            return Tests;
        }
        public override async Task<TestDto> CreateAsync(CreateTestDto input)
        {
            CheckCreatePermission();

            var entity = MapToEntity(input);


            await _TestRepository.InsertOrUpdateAsync(entity);

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public override async Task<TestDto> UpdateAsync(TestDto input)
        {
            CheckUpdatePermission();

            var Test = await _TestRepository.FirstOrDefaultAsync(input.Id);

            MapToEntity(input, Test);

            return await GetAsync(input);
        }

        protected override void MapToEntity(TestDto input, Test entity)
        {
            entity.Title = input.Title;
            entity.BeginTime = input.BeginTime;
            entity.EndTime = input.EndTime;
            entity.PaperId = input.PaperId;
        }
        protected override TestDto MapToEntityDto(Test test)
        {
            var testDto = new TestDto
            {
                CreaterUserId = test.CreaterUserId,
                CreateUserName = _userManager.GetUserById(test.CreaterUserId).UserName,
                CreationTime = test.CreationTime,
                BeginTime = test.BeginTime,
                EndTime = test.EndTime,
                PaperId = test.PaperId,
                Id = test.Id,
                IsDeleted = test.IsDeleted,
                Title = test.Title,
            };
            return testDto;
        }

        protected override Test MapToEntity(CreateTestDto createInput)
        {

            var result = new Test
            {
                BeginTime = createInput.BeginTime,
                CreaterUserId = AbpSession.UserId.Value,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                Title = createInput.Title,
                EndTime = createInput.EndTime,
                PaperId = createInput.PaperId
            };

            return result;
        }
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var test = await _TestRepository.FirstOrDefaultAsync(input.Id);
            await _TestRepository.DeleteAsync(test);
        }
    }
}
