using System.Collections.Generic;
using System.Linq;
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

namespace OnlineTestForCLanguage.Sessions
{
    [AbpAuthorize(PermissionNames.Pages_Tests)]
    public class TestsAppService : AsyncCrudAppService<Test, TestDto, long, PagedTestResultRequestDto,CreateTestDto,TestDto>, ITestsAppService
    {
        private readonly IRepository<Test, long> _TestRepository;
        public TestsAppService(IRepository<Test, long> TestRepository) : base(TestRepository)
        {
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

            var Test = ObjectMapper.Map<Test>(input);

          
            await _TestRepository.InsertOrUpdateAsync(Test);

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(Test);
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
            entity.TotalPoints = input.TotalPoints;
        }


        public async Task DeleteAsync(long id)
        {
            var Test = await _TestRepository.FirstOrDefaultAsync(id);
            await _TestRepository.DeleteAsync(Test);
        }
    }
}
