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
using OnlineTestForCLanguage.Exams;
using OnlineTestForCLanguage.Exams.Dto;

namespace OnlineTestForCLanguage.Sessions
{
    [AbpAuthorize(PermissionNames.Pages_Exams, PermissionNames.Pages_Papers, PermissionNames.Pages_TestCounts, PermissionNames.Pages_Tests)]
    public class TestsAppService : AsyncCrudAppService<Test, TestDto, long, PagedTestResultRequestDto,CreateTestDto,TestDto>, ITestsAppService
    {
        private readonly UserManager _userManager;
        private readonly IRepository<Test, long> _TestRepository;
        private readonly IRepository<TestDetail, long> _TestDetailRepository;
        public TestsAppService(IRepository<Test, long> TestRepository, IRepository<TestDetail, long> TestDetailRepository, UserManager userManager) : base(TestRepository)
        {
            _userManager = userManager;
               _TestRepository = TestRepository;
            _TestDetailRepository = TestDetailRepository;
        }
        public async Task<ListResultDto<TestDto>> GetTestsAsync(PagedTestResultRequestDto input)
        {
            var Tests = await GetAllAsync(input);
            if (await _userManager.IsGrantedAsync(AbpSession.UserId.GetValueOrDefault(),PermissionNames.CanBeginTest))
            {
                var items = Tests.Items.ToList();
                foreach (var item in items)
                {
                    var details =await _TestDetailRepository.GetAll().Where(t => t.TestId == item.Id && !t.IsDeleted && t.StudentId == AbpSession.UserId.GetValueOrDefault()).FirstOrDefaultAsync();
                    if (details == null)
                    {
                        if (item.BeginTime> DateTime.Now && item.EndTime < DateTime.Now)
                        {
                            item.CanBeginTest = true;
                        }
                       
                    }
                    
                }
                Tests.Items = items;
            }
            
            Tests.Items = Tests.Items.OrderBy(e=>e.Id).ToList();
            return Tests;
        }
        public override async Task<TestDto> GetAsync(EntityDto<long> input)
        {
            var test = await _TestRepository.Query(
                p=>
                    p.Include(p=>p.Paper)
                    .ThenInclude(p=>p.PaperDetails)
                    .ThenInclude(d=>d.Exam)
                    .ThenInclude(e => e.ExamDetails)
                    .AsNoTracking()
                    .Where(p=>p.Id == input.Id && !p.IsDeleted).FirstOrDefaultAsync()
            );
            var result = MapToEntityDto(test);
            return result;
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
            if (test.Paper != null)
            {
                testDto.Paper = new PaperDto
                {
                    Score = test.Paper.Score,
                    CreateUserId = test.Paper.CreateUserId,
                    CreateUserName = _userManager.GetUserById(test.Paper.CreateUserId).UserName,
                    CreationTime = test.Paper.CreationTime,
                    ExamList = test.Paper.PaperDetails.Select(p => p.ExamId).ToList(),
                    Id = test.Paper.Id,
                    IsDeleted = test.Paper.IsDeleted,
                    PaperDetails = test.Paper.PaperDetails.Select(p => new PaperDetailDto
                    {
                        ExamId = p.ExamId,
                        Id = p.Id,
                        IsDeleted = p.IsDeleted,
                        PaperId = p.PaperId,
                        Exam = MapToEntityDto(p.Exam),
                    }).ToList(),
                    Title = test.Paper.Title
                };
            }
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
        protected ExamDto MapToEntityDto(Exam exam)
        {
            var examDto = new ExamDto
            {
                Score = exam.Score,
                Content = exam.Content,
                CorrectDetailIds = exam.CorrectDetailIds,
                CreationTime = exam.CreationTime,
                Difficulty = exam.Difficulty,
                ExamType = exam.ExamType,
                Explain = exam.Explain,
                Id = exam.Id,
                IsDeleted = exam.IsDeleted,
                Title = exam.Title
            };
            if (exam.ExamDetails != null && exam.ExamDetails.Count != 0)
            {
                switch (exam.ExamType)
                {
                    case ExamType.SingleSelect:
                        examDto.answers = exam.ExamDetails.Select(e => new ExamDetailDto
                        {
                            IsSelected = e.AnswerId == exam.CorrectDetailIds ? true : false,
                            AnswerId = e.AnswerId,
                            Content = e.Content,
                            Id = e.Id
                        }).ToList();
                        break;
                    case ExamType.MulSelect:
                        examDto.answers = exam.ExamDetails.Select(e => new ExamDetailDto
                        {
                            IsSelected = exam.CorrectDetailIds.Split(",").Contains(e.AnswerId) ? true : false,
                            AnswerId = e.AnswerId,
                            Content = e.Content,
                            Id = e.Id
                        }).ToList();
                        break;
                    case ExamType.Judge:
                        examDto.answers = exam.ExamDetails.Select(e => new ExamDetailDto
                        {
                            IsSelected = e.AnswerId == exam.CorrectDetailIds ? true : false,
                            AnswerId = e.AnswerId,
                            Content = e.Content,
                            Id = e.Id
                        }).ToList();
                        break;
                    case ExamType.ShortAnswer:
                        examDto.answers = exam.ExamDetails.Select(e => new ExamDetailDto
                        {
                            IsSelected = true,
                            AnswerId = e.AnswerId,
                            Content = e.Content,
                            Id = e.Id
                        }).ToList();
                        break;
                    default:
                        break;
                }
            }

            return examDto;
        }
    }
}
