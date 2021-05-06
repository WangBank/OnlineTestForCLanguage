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
using OnlineTestForCLanguage.Papers;
using OnlineTestForCLanguage.Papers.Dto;
using OnlineTestForCLanguage.Sessions.Dto;
using OnlineTestForCLanguage.Authorization.Users;
using OnlineTestForCLanguage.Exams;
using System.Collections;

namespace OnlineTestForCLanguage.Sessions
{
    [AbpAuthorize(PermissionNames.Pages_Exams, PermissionNames.Pages_Papers, PermissionNames.Pages_TestCounts, PermissionNames.Pages_Tests)]
    public class PapersAppService : AsyncCrudAppService<Paper, PaperDto, long,PagedPaperResultRequestDto,CreatePaperDto,PaperDto>, IPapersAppService
    {
        private readonly IRepository<Paper,long> _PaperRepository;
        private readonly IRepository<Exam, long> _ExamRepository;
        private readonly UserManager _userManager;
        public PapersAppService(IRepository<Paper, long> PaperRepository, UserManager userManager, IRepository<Exam, long> ExamRepository) : base(PaperRepository)
        {
            _PaperRepository = PaperRepository;
            _userManager = userManager;
            _ExamRepository = ExamRepository;
        }
        public override async Task<PaperDto> GetAsync(EntityDto<long> input)
        {
            var paper = await _PaperRepository.GetAllIncluding(p => p.PaperDetails).FirstOrDefaultAsync(p => p.Id == input.Id && !p.IsDeleted);
            var result = MapToEntityDto(paper);
            return result;
        }

        public async Task<ListResultDto<PaperDto>> GetPapersAsync(PagedPaperResultRequestDto input)
        {
            var Papers = await GetAllAsync(input);
            Papers.Items = Papers.Items.Where(p=>!p.IsDeleted).OrderBy(e=>e.Id).ToList();
            return Papers;
        }

        public override async Task<PaperDto> CreateAsync(CreatePaperDto input)
        {
            CheckCreatePermission();

            var entity = MapToEntity(input);

          
            await _PaperRepository.InsertOrUpdateAsync(entity);

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public async Task<PaperDto> AutoCreateAsync(AutoCreatePaperDto input)
        {
            CheckCreatePermission();

            var entity = new Paper { 
                Score = 100,
                CreateUserId =AbpSession.UserId.GetValueOrDefault() ,
                CreationTime = DateTime.Now,
                IsDeleted =false ,
                Title = input.Title
            };
            entity.PaperDetails = new List<PaperDetail>
            ();
            // 单选6 多选3 判断2 简答3
            var exams = await _ExamRepository.GetAllListAsync(p=>!p.IsDeleted);
            var singleSelects = exams.Where(e => e.ExamType == ExamType.SingleSelect);
            var mulSelects = exams.Where(e => e.ExamType == ExamType.MulSelect);
            var judges = exams.Where(e => e.ExamType == ExamType.Judge);
            var simpleanswers = exams.Where(e => e.ExamType == ExamType.ShortAnswer);

            Random random = new Random();
            ArrayList single = new ArrayList();
            ArrayList mulle = new ArrayList();
            ArrayList judgee = new ArrayList();
            ArrayList shortle = new ArrayList();
            int singlei = 0;
            int muli = 0;
            int judgei = 0;
            int shorti = 0;
            switch (input.Difficulty)
            {
                //7:2:1
                case Exams.DifficultyType.simple:
                    #region 单选题
                    
                    while (singlei < 6)
                    {
                        var index = random.Next(0, singleSelects.Count());
                        if (!single.Contains(index))
                        {
                            single.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singleSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            singlei++;
                        }
                    }
                    #endregion

                    #region 多选
                  
                    while (muli < 3)
                    {
                        var index = random.Next(0, mulSelects.Count());
                        if (!mulle.Contains(index))
                        {
                            mulle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = mulSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            muli++;
                        }
                    }
                    #endregion

                    #region 判断
                    
                    while (judgei < 2)
                    {
                        var index = random.Next(0, judges.Count());
                        if (!judgee.Contains(index))
                        {
                            judgee.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = judges.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            judgei++;
                        }
                    }
                    #endregion

                    #region 简答
                   
                    while (shorti < 3)
                    {
                        var index = random.Next(0, simpleanswers.Count());
                        if (!shortle.Contains(index))
                        {
                            shortle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = simpleanswers.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            shorti++;
                        }
                    }
                    #endregion
                    break;
                //5:3:2
                case Exams.DifficultyType.general:
                    #region 单选题

                    while (singlei < 6)
                    {
                        var index = random.Next(0, singleSelects.Count());
                        if (!single.Contains(index))
                        {
                            single.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singleSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            singlei++;
                        }
                    }
                    #endregion

                    #region 多选

                    while (muli < 3)
                    {
                        var index = random.Next(0, mulSelects.Count());
                        if (!mulle.Contains(index))
                        {
                            mulle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = mulSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            muli++;
                        }
                    }
                    #endregion

                    #region 判断

                    while (judgei < 2)
                    {
                        var index = random.Next(0, judges.Count());
                        if (!judgee.Contains(index))
                        {
                            judgee.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = judges.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            judgei++;
                        }
                    }
                    #endregion

                    #region 简答

                    while (shorti < 3)
                    {
                        var index = random.Next(0, simpleanswers.Count());
                        if (!shortle.Contains(index))
                        {
                            shortle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = simpleanswers.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            shorti++;
                        }
                    }
                    #endregion

                    break;
                //3:5:2
                case Exams.DifficultyType.difficult:
                    #region 单选题

                    while (singlei < 6)
                    {
                        var index = random.Next(0, singleSelects.Count());
                        if (!single.Contains(index))
                        {
                            single.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singleSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            singlei++;
                        }
                    }
                    #endregion

                    #region 多选

                    while (muli < 3)
                    {
                        var index = random.Next(0, mulSelects.Count());
                        if (!mulle.Contains(index))
                        {
                            mulle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = mulSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            muli++;
                        }
                    }
                    #endregion

                    #region 判断

                    while (judgei < 2)
                    {
                        var index = random.Next(0, judges.Count());
                        if (!judgee.Contains(index))
                        {
                            judgee.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = judges.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            judgei++;
                        }
                    }
                    #endregion

                    #region 简答

                    while (shorti < 3)
                    {
                        var index = random.Next(0, simpleanswers.Count());
                        if (!shortle.Contains(index))
                        {
                            shortle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = simpleanswers.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            shorti++;
                        }
                    }
                    #endregion
                    break;
                default:
                    #region 单选题

                    while (singlei < 6)
                    {
                        var index = random.Next(0, singleSelects.Count());
                        if (!single.Contains(index))
                        {
                            single.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singleSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            singlei++;
                        }
                    }
                    #endregion

                    #region 多选

                    while (muli < 3)
                    {
                        var index = random.Next(0, mulSelects.Count());
                        if (!mulle.Contains(index))
                        {
                            mulle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = mulSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            muli++;
                        }
                    }
                    #endregion

                    #region 判断

                    while (judgei < 2)
                    {
                        var index = random.Next(0, judges.Count());
                        if (!judgee.Contains(index))
                        {
                            judgee.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = judges.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            judgei++;
                        }
                    }
                    #endregion

                    #region 简答

                    while (shorti < 3)
                    {
                        var index = random.Next(0, simpleanswers.Count());
                        if (!shortle.Contains(index))
                        {
                            shortle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = simpleanswers.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            shorti++;
                        }
                    }
                    #endregion
                    break;
            }

            await _PaperRepository.InsertOrUpdateAsync(entity);

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }
        public override async Task<PaperDto> UpdateAsync(PaperDto input)
        {
            CheckUpdatePermission();

            var paper = await _PaperRepository.GetAllIncluding(p => p.PaperDetails).FirstOrDefaultAsync(p => p.Id == input.Id);

            MapToEntity(input, paper);

            return MapToEntityDto(paper);
        }


        protected override void MapToEntity(PaperDto input, Paper paper)
        {
            paper.Score = input.Score;
            paper.Title = input.Title;
            paper.PaperDetails = input.ExamList.Select(p => new PaperDetail
            {
                ExamId = p,
                IsDeleted = false,
            }).ToList();
        }

        protected override Paper MapToEntity(CreatePaperDto createInput)
        {

            var result = new Paper {
                Score = createInput.Score,
                CreateUserId = AbpSession.UserId.Value,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                Title = createInput.Title,
                PaperDetails = createInput.ExamList.Select(p=>new PaperDetail { 
                    ExamId = p,
                    IsDeleted = false,
                }).ToList()
            };

            return result;
        }


        protected override PaperDto MapToEntityDto(Paper paper)
        {
            var paperDto = new PaperDto { 
                Score = paper.Score,
                CreateUserId = paper.CreateUserId,
                CreateUserName =_userManager.GetUserById(paper.CreateUserId).UserName,
                CreationTime = paper.CreationTime,
                Id = paper.Id,
                IsDeleted = paper.IsDeleted,
                PaperDetails =new List<PaperDetailDto>() ,
                Title =paper.Title 
            };
            if (paper.PaperDetails  != null&& paper.PaperDetails.Count != 0)
            {
                paperDto.PaperDetails = paper.PaperDetails.Select(p => new PaperDetailDto
                {
                    ExamId = p.ExamId,
                    Id = p.Id,
                    IsDeleted = p.IsDeleted,
                    PaperId = p.PaperId
                }).ToList();
            }
            
            return paperDto;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var Paper = await _PaperRepository.FirstOrDefaultAsync(input.Id);
            await _PaperRepository.DeleteAsync(Paper);
        }
    }
}
