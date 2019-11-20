using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using SIG.Model;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using SIG.Resources.Admin;
using SIG.SIGCMS.Extensions;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RecipesController : BaseController
    {
        private readonly YicaiyunContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public RecipesController(YicaiyunContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        // GET: Admin/Recipes
        public async Task<IActionResult> Index(string keyword, int? page)
        {
            RecipePageVM vm = new RecipePageVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword

            };
            var pageSize = 10;

            var query = _context.Recipes.Include(d=>d.User).AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));



            vm.TotalCount = await query.CountAsync();
            var list = await query.Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ProjectTo<RecipeVM>(_mapper.ConfigurationProvider).ToListAsync();
            //   var list = _mapper.Map<IEnumerable<CustomerVM>>(agents);

            vm.Recipes = new StaticPagedList<RecipeVM>(list, vm.PageIndex, pageSize, vm.TotalCount);
            return View(vm);
        }

        public async Task<IActionResult> MyRecipes(string keyword, int? page)
        {
            RecipePageVM vm = new RecipePageVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword

            };
            var pageSize = 10;

            var userId = new Guid(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value);

            var query = _context.Recipes.Include(d => d.User).AsNoTracking().Where(d=>d.UserId == userId).AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));

         

            vm.TotalCount = await query.CountAsync();
            var list = await query.Skip((vm.PageIndex - 1) * pageSize).Take(pageSize).ProjectTo<RecipeVM>(_mapper.ConfigurationProvider).ToListAsync();
            //   var list = _mapper.Map<IEnumerable<CustomerVM>>(agents);

            vm.Recipes = new StaticPagedList<RecipeVM>(list, vm.PageIndex, pageSize, vm.TotalCount);
            return View(vm);
        }

        // GET: Admin/Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            var vm = new RecipesDetailVM
            {
                Id = recipe.Id,
                UserId = recipe.UserId,
                Username = recipe.User.UserName,
                CreatedDate = recipe.CreatedDate,
                Title = recipe.Title,
                Description = recipe.Description,
                RecipesItems = await _context.RecipesItems.Include(d => d.Product).Where(d => d.RecipesId == id).ProjectTo<RecipesItemVM>(_mapper.ConfigurationProvider).ToListAsync(),
            };
            vm.Amount = vm.RecipesItems.Sum(d => d.Price * d.Quantity);

            return View(vm);
        }


       /// <summary>
       /// 设备清单第一步
       /// </summary>
       /// <returns></returns>
        public async Task<IActionResult> AddToList(string keyword, int? page)
        {

            ProductPageVM vm = new ProductPageVM()
            {
                PageIndex = page == null || page <= 0 ? 1 : page.Value,
                Keyword = keyword
            };
            var pageSize = 10;

            var query = _context.Products.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Name.Contains(keyword) || d.SubName.Contains(keyword));


            vm.TotalCount = await query.CountAsync();
            var products = await query.OrderByDescending(d => d.Id).ProjectTo<ProductVM>(_mapper.ConfigurationProvider)
                .Skip((vm.PageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync();


            foreach (var item in products)
            {
                var categories = _context.ProductCategories.Where(d => d.PcategoryProducts.Any(c => c.ProductId == item.Id)).ToList();
                item.CategoryTitle = string.Join("、", categories.Select(d => d.Title).ToArray());
            }

            vm.Products = new StaticPagedList<ProductVM>(products, vm.PageIndex, pageSize, vm.TotalCount);


            ViewData["CartItems"] = _httpContextAccessor.HttpContext.Session.GetObject<List<CartItemVM>>("CartItems");

            return View(vm);
        }

        [HttpPost]
    
        public async Task<IActionResult> AddToList(int productId)
        {
            var p = _context.Products.Find(productId);
            if (p == null)
            {
                AR.Setfailure(Messages.HttpNotFound);
                return Json(AR);
            }

            var products = _httpContextAccessor.HttpContext.Session.GetObject<List<CartItemVM>>("CartItems");
            

            if (products!=null)
            {
                foreach(var item in products)
                {
                    if(item.ProductId == productId)
                    {
                        item.Quantity++;
                    }
                }
                var itemp = products.FirstOrDefault(d => d.ProductId == productId);
                if (itemp==null)
                {
                    products.Add(new CartItemVM
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Quantity = 1,
                        Thumbnail = p.Thumbnail,
                        Unit = p.Unit
                    });
                }
              //  products.Add(productId);
            }
            else
            {
                products = new List<CartItemVM> { new CartItemVM
                {
                    ProductId = p.Id,
                        ProductName = p.Name,
                        Quantity = 1,
                        Thumbnail = p.Thumbnail,
                        Unit = p.Unit
                    }
                };
            }

         
            _httpContextAccessor.HttpContext.Session.SetObject("CartItems", products);
           
            return PartialView("_CartView", products);
        }
        // GET: Admin/Recipes/Create
        public IActionResult Create()
        {
            var products = _httpContextAccessor.HttpContext.Session.GetObject<List<CartItemVM>>("CartItems");
            if (products == null)
            {
                TempData["Error"] = "还未设置营养清单。";
                return RedirectToAction(nameof(AddToList));
            }
            var im = new RecipeIM
            {
                RecipeItems = new List<RecipeItemIM>()
            };

            foreach(var item in products)
            {
                im.RecipeItems.Add(new RecipeItemIM { Name = item.ProductName, Quantity = item.Quantity, ProductId = item.ProductId,Unit = item.Unit });
            }

            return View(im);
        }

        // POST: Admin/Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeIM im)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(d => d.UserName == User.Identity.Name);
                var model = new Recipe
                {
                    Title = im.Title,
                    CustomerMobile = im.CustomerMobile,
                    CreatedDate = DateTime.Now,
                    Description = im.Description,
                    UserId = user.Id
                };
                foreach(var item in im.RecipeItems)
                {
                    model.RecipesItems.Add(new RecipesItem { ProductId = item.ProductId, Quantity = item.Quantity });
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyRecipes));
            }

            return View(im);
        }

        // GET: Admin/Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", recipe.UserId);
            return View(recipe);
        }

        // POST: Admin/Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,Description,CreatedDate")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", recipe.UserId);
            return View(recipe);
        }

        // GET: Admin/Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            var vm = new RecipesDetailVM
            {
                Id = recipe.Id,
                UserId = recipe.UserId,
                Username = recipe.User.UserName,
                CreatedDate = recipe.CreatedDate,
                Title = recipe.Title,
                Description = recipe.Description,
                RecipesItems = await _context.RecipesItems.Include(d => d.Product).Where(d => d.RecipesId == id).ProjectTo<RecipesItemVM>(_mapper.ConfigurationProvider).ToListAsync(),
            };
            vm.Amount = vm.RecipesItems.Sum(d => d.Price * d.Quantity);
            return View(vm);
        }

        // POST: Admin/Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
