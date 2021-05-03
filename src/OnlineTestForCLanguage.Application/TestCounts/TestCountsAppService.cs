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
    public class TestCountsAppService : AsyncCrudAppService<TestDetail, TestCountDto, long, PagedTestCountResultRequestDto,CreateTestCountDto,TestCountDto>, ITestCountsAppService
    {
        private readonly UserManager _userManager;
        private readonly IRepository<TestDetail, long> _TestCountRepository;
        private readonly IRepository<Test, long> _TestRepository;
        public TestCountsAppService(IRepository<TestDetail, long> TestCountRepository, IRepository<Test, long> TestRepository, UserManager userManager) : base(TestCountRepository)
        {
            _userManager = userManager;
               _TestCountRepository = TestCountRepository;
            _TestRepository = TestRepository;
        }
        public override async Task<TestCountDto> CreateAsync(CreateTestCountDto input)
        {
            CheckCreatePermission();

            var entity = MapToEntity(input);


            await _TestCountRepository.InsertOrUpdateAsync(entity);

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }
        public override async Task<PagedResultDto<TestCountDto>> GetAllAsync(PagedTestCountResultRequestDto input)
        {
            
            var temp =await _TestCountRepository.GetAll().Include(t => t.Test).Take(input.MaxResultCount).Skip(input.SkipCount).OrderBy(t => t.Id).ToListAsync();
            var result =  new PagedResultDto<TestCountDto> { 
                Items = temp.Select(t=> MapToEntityDto(t)).ToList(),
                TotalCount = _TestCountRepository.GetAll().Count(),
            };
            if (await _userManager.IsGrantedAsync(AbpSession.UserId.GetValueOrDefault(), PermissionNames.CanInspected))
            {
                var items = result.Items.ToList();
                foreach (var item in items)
                {
                    if (!item.IsInspected)
                    {
                        item.CanInspect = true;
                    }
                    
                }
                result.Items = items;
            }

            return result;

        }

        public override async Task<TestCountDto> GetAsync(EntityDto<long> input)
        {

            var testCount = await _TestCountRepository.GetAll().Include(t=>t.Test).Include(t=>t.TestDetail_Exams).FirstOrDefaultAsync(t=>t.Id ==input.Id);
            var result = MapToEntityDto(testCount);
            return result;
        }

        public async Task<TestCountDto> GetInspectAsync(EntityDto<long> input)
        {

            var testCount = await _TestCountRepository.GetAll().Include(t => t.Test).Include(t => t.TestDetail_Exams.Where(z => z.Exam.ExamType == Exams.ExamType.ShortAnswer)).FirstOrDefaultAsync(t => t.Id == input.Id);
            var result = MapToEntityDto(testCount);
            return result;
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
                StudentName = _userManager.GetUserById(TestCount.StudentId).UserName,
                BeginTime = TestCount.Test.BeginTime,
                EndTime = TestCount.Test.EndTime,
                TeacherId = TestCount.TeacherId,
                TeacherName = TestCount.TeacherId != -1 ?_userManager.GetUserById(TestCount.TeacherId).UserName: null,
                TestTitle = TestCount.Test.Title

            };
            if (TestCount.TestDetail_Exams != null && TestCount.TestDetail_Exams.Count != 0)
            {
                TestCountDto.detail_Exams = TestCount.TestDetail_Exams.Select(
                    t =>
                    new TestDetail_ExamDto
                    {
                        Score = t.Score,
                        Answers = t.Answer,
                        CreationTime = t.CreationTime,
                        ExamId = t.ExamId,
                        IsDeleted = t.IsDeleted
                    }).ToList();
            }
            return TestCountDto;
        }

        protected override TestDetail MapToEntity(CreateTestCountDto createInput)
        {

            var result = new TestDetail
            {
                StudentId = createInput.StudentId == -1 ? AbpSession.UserId.Value: createInput.StudentId,
                StudentScoreSum = createInput.StudentScoreSum,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                IsInspected = false,
                TestId = createInput.TestId,
                Test =_TestRepository.FirstOrDefault(createInput.TestId),
                TeacherId = -1
            };
            if (createInput.detail_Exams != null && createInput.detail_Exams.Count != 0)
            {
                result.TestDetail_Exams = createInput.detail_Exams.Select(c => new TestDetail_Exam { 
                    Score = c.Score,
                    Answer =c.Answers ,
                    CreationTime = DateTime.Now,
                    ExamId = c.ExamId,
                    IsDeleted =false ,
                }).ToList();
            }
            return result;
        }
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var TestCount = await _TestCountRepository.FirstOrDefaultAsync(input.Id);
            await _TestCountRepository.DeleteAsync(TestCount);
        }
    }
}
