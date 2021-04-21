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
    [AbpAuthorize(PermissionNames.Pages_TestCounts)]
    public class TestCountsAppService : AsyncCrudAppService<TestDetail, TestCountDto, long, PagedTestCountResultRequestDto,CreateTestCountDto,TestCountDto>, ITestCountsAppService
    {
        private readonly UserManager _userManager;
        private readonly IRepository<TestDetail, long> _TestCountRepository;
        public TestCountsAppService(IRepository<TestDetail, long> TestCountRepository, UserManager userManager) : base(TestCountRepository)
        {
            _userManager = userManager;
               _TestCountRepository = TestCountRepository;
        }
        public async Task<ListResultDto<TestCountDto>> GetTestCountsAsync(PagedTestCountResultRequestDto input)
        {
            var TestCounts = await GetAllAsync(input);
            TestCounts.Items = TestCounts.Items.OrderBy(e=>e.Id).ToList();
            return TestCounts;
        }
        public override async Task<TestCountDto> CreateAsync(CreateTestCountDto input)
        {
            CheckCreatePermission();

            var entity = MapToEntity(input);


            await _TestCountRepository.InsertOrUpdateAsync(entity);

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public override async Task<TestCountDto> UpdateAsync(TestCountDto input)
        {
            CheckUpdatePermission();

            var TestCount = await _TestCountRepository.FirstOrDefaultAsync(input.Id);

            MapToEntity(input, TestCount);

            return await GetAsync(input);
        }

        protected override void MapToEntity(TestCountDto input, TestDetail entity)
        {
            entity.StudentId = input.StudentId;
            entity.StudentScoreSum = input.StudentScoreSum;
            entity.IsInspected = input.IsInspected;
            entity.TestId = input.TestId;
        }
        protected override TestCountDto MapToEntityDto(TestDetail TestCount)
        {
            var TestCountDto = new TestCountDto
            {
                StudentId = TestCount.StudentId,
                StudentScoreSum = TestCount.StudentScoreSum,
                CreationTime = TestCount.CreationTime,
                IsInspected = TestCount.IsInspected,
                TestId = TestCount.TestId,
                Id = TestCount.Id,
                IsDeleted = TestCount.IsDeleted,

            };
            return TestCountDto;
        }

        protected override TestDetail MapToEntity(CreateTestCountDto createInput)
        {

            var result = new TestDetail
            {
                StudentId = createInput.StudentId,
                StudentScoreSum = createInput.StudentScoreSum,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                IsInspected = createInput.IsInspected,
                TestId = createInput.TestId
            };

            return result;
        }
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var TestCount = await _TestCountRepository.FirstOrDefaultAsync(input.Id);
            await _TestCountRepository.DeleteAsync(TestCount);
        }
    }
}
