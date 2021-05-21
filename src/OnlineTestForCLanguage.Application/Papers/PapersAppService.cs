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

            var singleSelects = exams.Where(e => e.ExamType == ExamType.SingleSelect).ToList();
            var singlesimpleSelects = singleSelects.Where(e => e.Difficulty == DifficultyType.simple).ToList();
            var singlegeneralSelects = singleSelects.Where(e => e.Difficulty == DifficultyType.general).ToList();
            var singledifficultSelects = singleSelects.Where(e => e.Difficulty == DifficultyType.difficult).ToList();

            var mulSelects = exams.Where(e => e.ExamType == ExamType.MulSelect).ToList();
            var singlesimplemulSelects = mulSelects.Where(e => e.Difficulty == DifficultyType.simple).ToList();
            var singlegeneralmulSelects = mulSelects.Where(e => e.Difficulty == DifficultyType.general).ToList();
            var singledifficultmulSelects = mulSelects.Where(e => e.Difficulty == DifficultyType.difficult).ToList();

            var judges = exams.Where(e => e.ExamType == ExamType.Judge).ToList();
            var singlesimplejudges = judges.Where(e => e.Difficulty == DifficultyType.simple).ToList();
            var singlegeneraljudges = judges.Where(e => e.Difficulty == DifficultyType.general).ToList();
            var singledifficultjudges = judges.Where(e => e.Difficulty == DifficultyType.difficult).ToList();


            var simpleanswers = exams.Where(e => e.ExamType == ExamType.ShortAnswer).ToList();
            var singlesimplesimpleanswers = simpleanswers.Where(e => e.Difficulty == DifficultyType.simple).ToList();
            var singlegeneralsimpleanswers = simpleanswers.Where(e => e.Difficulty == DifficultyType.general).ToList();
            var singledifficultsimpleanswers = simpleanswers.Where(e => e.Difficulty == DifficultyType.difficult).ToList();


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
                    //30
                    //取一个中等难度单选题 5分
                    var simpleGeneral_simple = singlegeneralSelects[random.Next(0, singlegeneralSelects.Count())];
                    single.Add(simpleGeneral_simple);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = simpleGeneral_simple.Id,
                        IsDeleted = false,

                    });
                    singlei++;

                    // 取5个简单难度单选题 25分
                    while (singlei < 6)
                    {
                        var index = random.Next(0, singlesimpleSelects.Count());
                        if (!single.Contains(index))
                        {
                            single.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singlesimpleSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            singlei++;
                        }
                    }
                    #endregion
                    //30
                    #region 多选
                    //取一个中等难度多选题 10分
                    var mulselectGeneral_simple = singlegeneralmulSelects[random.Next(0, singlegeneralmulSelects.Count())];
                    mulle.Add(mulselectGeneral_simple);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = mulselectGeneral_simple.Id,
                        IsDeleted = false,

                    });
                    muli++;

                    // 取2个简单难度多选题 20分
                    while (muli < 3)
                    {
                        var index = random.Next(0, singlesimplemulSelects.Count());
                        if (!mulle.Contains(index))
                        {
                            mulle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singlesimplemulSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            muli++;
                        }
                    }
                    #endregion
                    //10
                    #region 判断

                    //取一个中等难度判断题 5分
                    var judgeGeneral_simple = singlegeneraljudges[random.Next(0, singlegeneraljudges.Count())];
                    judgee.Add(judgeGeneral_simple);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = judgeGeneral_simple.Id,
                        IsDeleted = false,

                    });
                    judgei++;

                    // 取1个简单难度判断题 5分
                    while (judgei < 2)
                    {
                        var index = random.Next(0, singlesimplejudges.Count());
                        if (!judgee.Contains(index))
                        {
                            judgee.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singlesimplejudges.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            judgei++;
                        }
                    }
                    #endregion
                    //30
                    #region 简答
                    //取一个困难难度简答题 10分
                    var singleDifficult_simple = singledifficultsimpleanswers[random.Next(0, singledifficultsimpleanswers.Count())];
                    shortle.Add(singleDifficult_simple);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = singleDifficult_simple.Id,
                        IsDeleted = false,

                    });
                    shorti++;

                    // 取2个简单难度简答题 20分
                    while (shorti < 3)
                    {
                        var index = random.Next(0, singlesimplesimpleanswers.Count());
                        if (!shortle.Contains(index))
                        {
                            shortle.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singlesimplesimpleanswers.ToList()[index].Id,
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
                    //30
                    //取一个中等难度单选题 5分
                    var simpleGeneral_general = singlegeneralSelects[random.Next(0, singlegeneralSelects.Count())];
                    single.Add(simpleGeneral_general);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = simpleGeneral_general.Id,
                        IsDeleted = false,

                    });
                    singlei++;

                    // 取5个简单难度单选题 25分
                    while (singlei < 6)
                    {
                        var index = random.Next(0, singlesimpleSelects.Count());
                        if (!single.Contains(index))
                        {
                            single.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singlesimpleSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            singlei++;
                        }
                    }
                    #endregion
                    //30
                    #region 多选
                    //取一个简单难度多选题 10分
                    var mulselectSimple_general = singlesimplemulSelects[random.Next(0, singlesimplemulSelects.Count())];
                    mulle.Add(mulselectSimple_general);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = mulselectSimple_general.Id,
                        IsDeleted = false,

                    });

                    //取一个中等难度多选题 10分
                    var mulselectGeneral_general = singlegeneralmulSelects[random.Next(0, singlegeneralmulSelects.Count())];
                    mulle.Add(mulselectGeneral_general);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = mulselectGeneral_general.Id,
                        IsDeleted = false,

                    });


                    // 取1个困难难度多选题 10分
                    var mulselectDifficult_general = singledifficultmulSelects[random.Next(0, singledifficultmulSelects.Count())];
                    mulle.Add(mulselectDifficult_general);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = mulselectDifficult_general.Id,
                        IsDeleted = false,

                    });


                    #endregion
                    //10
                    #region 判断

                    //取一个简单难度判断题 5分
                    var judgeSimple_general = singlesimplejudges[random.Next(0, singlesimplejudges.Count())];
                    judgee.Add(judgeSimple_general);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = judgeSimple_general.Id,
                        IsDeleted = false,

                    });

                    // 取1个中等难度判断题 5分
                    var judgeGeneral_general = singlegeneraljudges[random.Next(0, singlegeneraljudges.Count())];
                    judgee.Add(judgeGeneral_general);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = judgeGeneral_general.Id,
                        IsDeleted = false,

                    });

                    #endregion
                    //30
                    #region 简答
                    //分别取各类难度简答题各一个 30分
                    var singleSimple_general = singlesimplesimpleanswers[random.Next(0, singlesimplesimpleanswers.Count())];
                    shortle.Add(singleSimple_general);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = singleSimple_general.Id,
                        IsDeleted = false,

                    });

                    var singleGeneral_general = singlegeneralsimpleanswers[random.Next(0, singlegeneralsimpleanswers.Count())];
                    shortle.Add(singleGeneral_general);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = singleGeneral_general.Id,
                        IsDeleted = false,

                    });


                    var singleDifficult_general = singledifficultsimpleanswers[random.Next(0, singledifficultsimpleanswers.Count())];
                    shortle.Add(singleDifficult_general);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = singleDifficult_general.Id,
                        IsDeleted = false,

                    });

                    #endregion

                    break;
                //3:5:2
                case Exams.DifficultyType.difficult:
                    #region 单选题
                    //30
                    //取1个简单 5个中等难度单选题
                    var simpleGeneral_difficult = singlesimpleSelects[random.Next(0, singlesimpleSelects.Count())];
                    single.Add(simpleGeneral_difficult);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = simpleGeneral_difficult.Id,
                        IsDeleted = false,

                    });
                    singlei++;

                    while (singlei < 6)
                    {
                        var index = random.Next(0, singlegeneralSelects.Count());
                        if (!single.Contains(index))
                        {
                            single.Add(index);
                            entity.PaperDetails.Add(new PaperDetail
                            {
                                ExamId = singlegeneralSelects.ToList()[index].Id,
                                IsDeleted = false,

                            });
                            singlei++;
                        }
                    }
                    #endregion
                    //30
                    #region 多选
                    //取一个简单难度多选题 10分
                    var mulselectSimple_difficult = singlesimplemulSelects[random.Next(0, singlesimplemulSelects.Count())];
                    mulle.Add(mulselectSimple_difficult);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = mulselectSimple_difficult.Id,
                        IsDeleted = false,

                    });

                    //取一个中等难度多选题 10分
                    var mulselectGeneral_difficult = singlegeneralmulSelects[random.Next(0, singlegeneralmulSelects.Count())];
                    mulle.Add(mulselectGeneral_difficult);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = mulselectGeneral_difficult.Id,
                        IsDeleted = false,

                    });


                    // 取1个困难难度多选题 10分
                    var mulselectDifficult_difficult = singledifficultmulSelects[random.Next(0, singledifficultmulSelects.Count())];
                    mulle.Add(mulselectDifficult_difficult);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = mulselectDifficult_difficult.Id,
                        IsDeleted = false,

                    });


                    #endregion
                    //10
                    #region 判断

                    //一个简单一个中等
                    var judgeSimple_difficult = singlesimplejudges[random.Next(0, singlesimplejudges.Count())];
                    judgee.Add(judgeSimple_difficult);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = judgeSimple_difficult.Id,
                        IsDeleted = false,

                    });

                    // 取1个中等难度判断题 5分
                    var judgeGeneral_difficult = singlegeneraljudges[random.Next(0, singlegeneraljudges.Count())];
                    judgee.Add(judgeGeneral_difficult);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = judgeGeneral_difficult.Id,
                        IsDeleted = false,

                    });

                    #endregion
                    //30
                    #region 简答
                    //分别取各类难度简答题各一个 30分
                    var singleSimple_difficult = singlesimplesimpleanswers[random.Next(0, singlesimplesimpleanswers.Count())];
                    shortle.Add(singleSimple_difficult);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = singleSimple_difficult.Id,
                        IsDeleted = false,

                    });

                    var singleGeneral_difficult = singlegeneralsimpleanswers[random.Next(0, singlegeneralsimpleanswers.Count())];
                    shortle.Add(singleGeneral_difficult);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = singleGeneral_difficult.Id,
                        IsDeleted = false,

                    });


                    var singleDifficult_difficult = singledifficultsimpleanswers[random.Next(0, singledifficultsimpleanswers.Count())];
                    shortle.Add(singleDifficult_difficult);
                    entity.PaperDetails.Add(new PaperDetail
                    {
                        ExamId = singleDifficult_difficult.Id,
                        IsDeleted = false,

                    });

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
