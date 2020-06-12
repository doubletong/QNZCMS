using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QNZ.Data;
using QNZ.Data.Enums;
using QNZ.Infrastructure.Cache;
using QNZ.Model.Front.ViewModel;
using QNZ.Model.ViewModel;
using X.PagedList;

namespace QNZCMS.Controllers
{
    public class AboutController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly YicaiyunContext _context;
        private readonly ICacheService _cacheService;
        public AboutController(YicaiyunContext context, IMapper mapper, ICacheService cacheService)
        {
            _mapper = mapper;
            _context = context;
            _cacheService = cacheService;
        }
        #region 公司概况
        public async Task<IActionResult> Index()
        {
            var vm = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "about" && d.Active);
            if (vm == null)
            {
                return NotFound();
            }
            vm.ViewCount++;
            _context.Update(vm);

            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == vm.SeoName && d.ModuleType == (short)ModuleType.PAGE);

            await _context.SaveChangesAsync();

            return View(vm);
        }
        public async Task<IActionResult> History()
        {
            var memoList = await _context.Memorabilia.Where(d => d.Active == true).OrderByDescending(d => d.DateAt)
                .ProjectTo<MemorabiliaVM>(_mapper.ConfigurationProvider).ToListAsync();
            var years = memoList.Select(d => d.DateAt.Year).Distinct();
            var vm = new HistoryVM
            {
                Memorabilias = memoList,
                Years = years
            };

            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }
        public async Task<IActionResult> Framework()
        {
            var vm = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "framework" && d.Active);
            if (vm == null)
            {
                return NotFound();
            }
            vm.ViewCount++;
            _context.Update(vm);

            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == vm.SeoName && d.ModuleType == (short)ModuleType.PAGE);

            await _context.SaveChangesAsync();
            return View(vm);
        }
        public async Task<IActionResult> Quality()
        {
            string alias = "quality";
            string keyNav = $"ALBUM_{alias}_TRUE";

            if (!_cacheService.IsSet(keyNav))
            {
                var album = await _context.Albums.Include(d => d.Photos)
                  .FirstOrDefaultAsync(d => d.Alias == alias && d.Active == true);

                if (album != null)
                {
                    _cacheService.Set(keyNav, album, 30);
                }
   
            }


            var vm = (Album)_cacheService.Get(keyNav);
            if (vm == null)
            {
                return NotFound();
            }

            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }
        public async Task<IActionResult> Honors()
        {
            string alias = "honors";
            string keyNav = $"ALBUM_{alias}_TRUE";

            if (!_cacheService.IsSet(keyNav))
            {
                var album = await _context.Albums.Include(d => d.Photos)
                 .FirstOrDefaultAsync(d => d.Alias == alias && d.Active == true);

                if (album != null)
                {
                    _cacheService.Set(keyNav, album, 30);
                }

            }

            var vm = (Album)_cacheService.Get(keyNav);
            if (vm == null)
            {
                return NotFound();
            }

            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }
        #endregion

        #region 新闻资讯
        public async Task<IActionResult> CompanyNews(int? page)
        {
            string alias = "news";

            var vm = new ArticlePageVM
            {
                PageIndex = page ?? 1,
                PageSize = 12,
                RecommendArticles = await _context.Articles.Where(d => d.Recommend && d.Active==true && d.Category.Alias == alias)
                .OrderByDescending(d=>d.Pubdate).ThenByDescending(d=>d.Id)
                .ProjectTo<ArticleVM>(_mapper.ConfigurationProvider).ToListAsync()
            };

            var query = _context.Articles.Where(d => d.Active == true && d.Category.Alias == alias).AsNoTracking();


            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id).ProjectTo<ArticleVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize)
                .ToListAsync();

            vm.Articles = new StaticPagedList<ArticleVM>(articles, vm.PageIndex, vm.PageSize, vm.TotalCount);


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }
        public async Task<IActionResult> CompanyNewsDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            article.ViewCount++;
            _context.Update(article);
            await _context.SaveChangesAsync();

            var vm = new ArticleDetailVM
            {
                ArticleDetail = article,
                ArticlePrev = await _context.Articles.OrderByDescending(d => d.Id).FirstOrDefaultAsync(d => d.Id < article.Id && d.Active == true && d.CategoryId == article.CategoryId),
                ArticleNext = await _context.Articles.OrderBy(d => d.Id).FirstOrDefaultAsync(d => d.Id > article.Id && d.Active == true && d.CategoryId == article.CategoryId)
            };


            var objectId = id.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == objectId && d.ModuleType == (short)ModuleType.ARTICLE);

            return View(vm);
        }
        public async Task<IActionResult> MediaAsync(int? page)
        {
            int categoryId = 20;

            var vm = new ArticlePageVM
            {
                PageIndex = page ?? 1,
                PageSize = 12,
                RecommendArticles = await _context.Articles.Where(d => d.Recommend && d.Active == true && d.CategoryId == categoryId)
                .OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id)
                .ProjectTo<ArticleVM>(_mapper.ConfigurationProvider).ToListAsync()
            };

            var query = _context.Articles.Where(d => d.Active == true && d.CategoryId == categoryId).AsNoTracking();


            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id).ProjectTo<ArticleVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize)
                .ToListAsync();

            vm.Articles = new StaticPagedList<ArticleVM>(articles, vm.PageIndex, vm.PageSize, vm.TotalCount);


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }
        public async Task<IActionResult> MediaDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            article.ViewCount++;
            _context.Update(article);
            await _context.SaveChangesAsync();

            var vm = new ArticleDetailVM
            {
                ArticleDetail = article,
                ArticlePrev = await _context.Articles.OrderByDescending(d => d.Id).FirstOrDefaultAsync(d => d.Id < article.Id && d.Active == true && d.CategoryId == article.CategoryId),
                ArticleNext = await _context.Articles.OrderBy(d => d.Id).FirstOrDefaultAsync(d => d.Id > article.Id && d.Active == true && d.CategoryId == article.CategoryId)
            };


            var objectId = id.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == objectId && d.ModuleType == (short)ModuleType.ARTICLE);

            return View(vm);
        }
        public async Task<IActionResult> ExhibitionAsync(int? page, int? type)
        {          

            var vm = new ExhibitionPageVM
            {
                Type = type,
                PageIndex = page ?? 1,
                PageSize = 12             
            };

            var query = _context.Exhibitions.Where(d => d.Active == true).AsNoTracking();

            if(type == 1)
            {
                query = query.Where(d => d.DateStart > DateTime.Now);
            }
            if (type == 2)
            {
                query = query.Where(d => d.DateEnd < DateTime.Now);
            }

            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.DateStart).ThenByDescending(d => d.Id).ProjectTo<ExhibitionVM> (_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize)
                .ToListAsync();

            vm.Exhibitions = new StaticPagedList<ExhibitionVM>(articles, vm.PageIndex, vm.PageSize, vm.TotalCount);

            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
           
        }

        public async Task<IActionResult> ExhibitionDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Exhibitions             
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            article.ViewCount++;
            _context.Update(article);
            await _context.SaveChangesAsync();

            var vm = new ExhibitionDetailVM
            {
                ExhibitionDetail = article,
                ExhibitionPrev = await _context.Exhibitions.OrderByDescending(d => d.Id).FirstOrDefaultAsync(d => d.Id < article.Id && d.Active == true),
                ExhibitionNext = await _context.Exhibitions.OrderBy(d => d.Id).FirstOrDefaultAsync(d => d.Id > article.Id && d.Active == true)
            };


            var objectId = id.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == objectId && d.ModuleType == (short)ModuleType.EXHIBITION);

            return View(vm);
        }
        public async Task<IActionResult> NewsletterAsync(int? page)
        {
            int categoryId = 21;

            var vm = new ArticlePageVM
            {
                PageIndex = page ?? 1,
                PageSize = 12,
                RecommendArticles = await _context.Articles.Where(d => d.Recommend && d.Active == true && d.CategoryId == categoryId)
                .OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id)
                .ProjectTo<ArticleVM>(_mapper.ConfigurationProvider).ToListAsync()
            };

            var query = _context.Articles.Where(d => d.Active == true && d.CategoryId == categoryId).AsNoTracking();


            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id).ProjectTo<ArticleVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize)
                .ToListAsync();

            vm.Articles = new StaticPagedList<ArticleVM>(articles, vm.PageIndex, vm.PageSize, vm.TotalCount);


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }
        public async Task<IActionResult> NewsletterDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            article.ViewCount++;
            _context.Update(article);
            await _context.SaveChangesAsync();

            var vm = new ArticleDetailVM
            {
                ArticleDetail = article,
                ArticlePrev = await _context.Articles.OrderByDescending(d => d.Id).FirstOrDefaultAsync(d => d.Id < article.Id && d.Active == true && d.CategoryId == article.CategoryId),
                ArticleNext = await _context.Articles.OrderBy(d => d.Id).FirstOrDefaultAsync(d => d.Id > article.Id && d.Active == true && d.CategoryId == article.CategoryId)
            };


            var objectId = id.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == objectId && d.ModuleType == (short)ModuleType.ARTICLE);

            return View(vm);
        }


        #endregion

        #region 投资者关系
        public async Task<IActionResult> DisclosureAsync(int? page)
        {
          
            string alias = "disclosure";

            var vm = new DocumentPageVM
            {
                PageIndex = page ?? 1,
                PageSize = 15
            
            };

            var query = _context.Documents.Where(d => d.Active == true && d.Category.Alias == alias).AsNoTracking();


            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id).ProjectTo<DocumentVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize)
                .ToListAsync();

            vm.Documents = new StaticPagedList<DocumentVM>(articles, vm.PageIndex, vm.PageSize, vm.TotalCount);


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        
        }
        public async Task<IActionResult> FinancialAsync(int? page)
        {
            string alias = "financial";

            var vm = new DocumentPageVM
            {
                PageIndex = page ?? 1,
                PageSize = 15

            };

            var query = _context.Documents.Where(d => d.Active == true && d.Category.Alias == alias).AsNoTracking();


            vm.TotalCount = await query.CountAsync();
            var articles = await query.OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id).ProjectTo<DocumentVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * vm.PageSize).Take(vm.PageSize)
                .ToListAsync();

            vm.Documents = new StaticPagedList<DocumentVM>(articles, vm.PageIndex, vm.PageSize, vm.TotalCount);


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }

        public async Task<IActionResult> EGMAsync()
        {
            var vm = await _context.Staffs.Where(d => d.Active==true && d.Organization.Alias == "egm")
                .OrderByDescending(d=>d.Importance).ThenByDescending(d=>d.Id).ProjectTo<StaffVM>(_mapper.ConfigurationProvider).ToListAsync();



            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);


            return View(vm);
        }
        public async Task<IActionResult> DirectorateAsync()
        {
            var vm = await _context.Staffs.Where(d => d.Active == true && d.Organization.Alias == "directorate")
               .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id).ProjectTo<StaffVM>(_mapper.ConfigurationProvider).ToListAsync();


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);


            return View(vm);
        }
        public async Task<IActionResult> AufsichtsratAsync()
        {
            var vm = await _context.Staffs.Where(d => d.Active == true && d.Organization.Alias == "aufsichtsrat")
                .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id).ProjectTo<StaffVM>(_mapper.ConfigurationProvider).ToListAsync();


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);


            return View(vm);
        }
        public async Task<IActionResult> ExecutivesAsync()
        {
            var vm = await _context.Staffs.Where(d => d.Active == true && d.Organization.Alias == "executives")
                  .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id).ProjectTo<StaffVM>(_mapper.ConfigurationProvider).ToListAsync();


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);


            return View(vm);
        }
        public async Task<IActionResult> ProfileAsync(int id)
        {
            var item = await _context.Staffs.Include(d=>d.Organization).FirstOrDefaultAsync(d=>d.Id == id);
            if (item == null)
                return NotFound();

            var staffs = await _context.Staffs.Where(d => d.Active == true && d.OrganizationId == item.OrganizationId)
                .OrderByDescending(d => d.Importance).ThenBy(d => d.Id).ProjectTo<StaffVM>(_mapper.ConfigurationProvider).Take(3).ToListAsync();


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.STAFF);
            var vm = new StaffDetailVM
            {
                StaffDetail = item,
                Staffs = staffs
            };


            return View(vm);
        }
        #endregion

        #region 人力资源
        public async Task<IActionResult> SocialAsync()
        {

            var vm = await _context.Jobs.Where(d => d.Active == true && d.Category.Alias == "social")
               .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id).ToListAsync();


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);


            return View(vm);
        }
        public async Task<IActionResult> CampusAsync()
        {

            var vm = await _context.Jobs.Where(d => d.Active == true && d.Category.Alias == "campus")
               .OrderByDescending(d => d.Importance).ThenByDescending(d => d.Id).ToListAsync();


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);


            return View(vm);
        }
        #endregion

        #region 河南重点实验室
        public async Task<IActionResult> LabAsync()
        {
            string alias = "lab_honors";
            string keyNav = $"PHOTOS_{alias}_TRUE";

            if (!_cacheService.IsSet(keyNav))
            {
                var photos = await _context.Photos
                 .Where(d => d.Album.Alias == alias && d.Active == true)
                 .OrderByDescending(d=>d.Importance).ThenByDescending(d=>d.Id).Take(3).ProjectTo<PhotoVM>(_mapper.ConfigurationProvider).ToListAsync();

                if (photos != null)
                {
                    _cacheService.Set(keyNav, photos, 30);
                }

            }

            var honors = (IEnumerable<PhotoVM>)_cacheService.Get(keyNav);

            var articles = await _context.Articles.Where(d => d.Recommend && d.Active == true && d.Category.Alias == "researches")
                .OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id).Take(3)
                .ProjectTo<ArticleVM>(_mapper.ConfigurationProvider).ToListAsync();

            var page1 = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "lab_intro" && d.Active);
            if (page1 != null)
            {
                page1.ViewCount++;
                _context.Update(page1);
            }
           
            var page2 = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "lab_intro2" && d.Active);
            if (page2 != null)
            {
                page1.ViewCount++;
                _context.Update(page2);
            }
            if (page2 != null || page1 != null)
                await _context.SaveChangesAsync();

            var vm = new LabPageVM
            {
                Photos = honors,
                Articles = articles,
                LabIntro = page1,
                LabIntro2 = page2
            };
         

            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }

        public async Task<IActionResult> LabIntroAsync()
        {
            var vm = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "lab_intro_detail" && d.Active);
            if (vm == null)
            {
                return NotFound();
            }
            vm.ViewCount++;
            _context.Update(vm);
            await _context.SaveChangesAsync();


            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == vm.SeoName && d.ModuleType == (short)ModuleType.PAGE);

        
            return View(vm);
        }
        public async Task<IActionResult> ResearchesAsync()
        {
            string alias = "researches";

            var vm = await _context.Articles.Where(d =>d.Active == true && d.Category.Alias == alias)
                .OrderByDescending(d => d.Pubdate).ThenByDescending(d => d.Id)
                .ProjectTo<ArticleVM>(_mapper.ConfigurationProvider).ToListAsync();


            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }
        public async Task<IActionResult> ResearchesDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            article.ViewCount++;
            _context.Update(article);
            await _context.SaveChangesAsync();

            //var vm = new ArticleDetailVM
            //{
            //    ArticleDetail = article,
            //    ArticlePrev = await _context.Articles.OrderByDescending(d => d.Id).FirstOrDefaultAsync(d => d.Id < article.Id && d.Active == true && d.CategoryId == article.CategoryId),
            //    ArticleNext = await _context.Articles.OrderBy(d => d.Id).FirstOrDefaultAsync(d => d.Id > article.Id && d.Active == true && d.CategoryId == article.CategoryId)
            //};


            var objectId = id.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == objectId && d.ModuleType == (short)ModuleType.ARTICLE);

            return View(article);
        }


        public async Task<IActionResult> TeamAsync()
        {
            var vm = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "team" && d.Active);
            if (vm == null)
            {
                return NotFound();
            }
            vm.ViewCount++;
            _context.Update(vm);

            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == vm.SeoName && d.ModuleType == (short)ModuleType.PAGE);

            await _context.SaveChangesAsync();
            return View(vm);
        }
        public async Task<IActionResult> ExchangeAsync()
        {
            var vm = await _context.Pages.FirstOrDefaultAsync(d => d.SeoName == "exchange" && d.Active);
            if (vm == null)
            {
                return NotFound();
            }
            vm.ViewCount++;
            _context.Update(vm);

            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == vm.SeoName && d.ModuleType == (short)ModuleType.PAGE);

            await _context.SaveChangesAsync();
            return View(vm);
        }
        public async Task<IActionResult> HonorsLabAsync()
        {
            string alias = "lab_honors";
            string keyNav = $"ALBUM_{alias}_TRUE";

            if (!_cacheService.IsSet(keyNav))
            {
                var album = await _context.Albums.Include(d => d.Photos)
                 .FirstOrDefaultAsync(d => d.Alias == alias && d.Active == true);

                if (album != null)
                {
                    _cacheService.Set(keyNav, album, 30);
                }

            }

            var vm = (Album)_cacheService.Get(keyNav);
            if (vm == null)
            {
                return NotFound();
            }

            var url = Request.Path.ToString();
            ViewData["PageMeta"] = await _context.PageMetas.FirstOrDefaultAsync(d => d.ObjectId == url && d.ModuleType == (short)ModuleType.MENU);

            return View(vm);
        }
        #endregion
    }
}