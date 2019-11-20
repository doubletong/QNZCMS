using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using SIG.Model.Admin.ViewModel;
using SIG.Model.ViewModel;
using SIG.Resources.Admin;
using YCY.Data;

namespace SIG.SIGCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Permission")]
    public class OrdersController : BaseController
    {
        private readonly YicaiyunContext _context;

        public OrdersController(YicaiyunContext context)
        {
            _context = context;
        }

        //GET: Admin/Orders
        public async Task<IActionResult> Index(byte? status,string mobile, int page = 1)
        {
            var vm = new OrderPagedVM
            {
                PageIndex = page,    
                Status = status,
                Mobile = mobile           

            };

            var query = _context.Orders.Include(d => d.OrderDetails).Include(d=>d.Open).AsQueryable();
            if (status != null)
            {
                query = query.Where(d => d.Status == status);
            }

            if (!string.IsNullOrEmpty(mobile))
            {
                query = query.Where(d => d.Open.Mobile == mobile);
            }
            //if (!string.IsNullOrEmpty(cashier))
            //{
            //    query = query.Where(d => d.Cashier == cashier);
            //}
            var orders = await query.OrderByDescending(d => d.Id)
                 .Select(d => new OrderVM
                 {
                     Id = d.Id,
                     Amount = d.Amount,
                     Status = d.Status,
                     Nickname = d.Open.WechatNickName,
                     CustomerMobile = d.Open.Mobile,
                     CreatedDate = d.CreatedDate                  

                 }).Skip((vm.PageIndex - 1) * 10).Take(10).ToListAsync();

            var ids = orders.Select(d => d.Id).ToList();

            var orderDetails = await _context.OrderDetails.Include(o => o.Product).Include(o=>o.Store)
                .Where(o => ids.Contains(o.OrderId)).Select(o => new OrderDetailVM
            {
                Id = o.Id,
                Price = o.Price,
                Quantity = o.Quantity,
                ProductId = o.ProductId,
                ProductName = o.Product.Name,
                Summary = o.Product.Summary,
                Thumbnail = o.Product.Thumbnail,
                OrderId = o.OrderId,
                StoreId = o.StoreId,
                StoreName = o.Store.Name

            }).ToListAsync();

            foreach (var item in orders)
            {
                item.OrderDetails = orderDetails.Where(d => d.OrderId == item.Id).ToList();
            }
            vm.TotalCount = await query.CountAsync();
            vm.Orders = new StaticPagedList<OrderVM>(orders, vm.PageIndex, 10, vm.TotalCount);

            //ViewData["Stores"] = new SelectList(_context.Stores, "Id", "Name");

            return View(vm);
        }


        //public async Task<IActionResult> MySaleList(int? storeId, string mobile, int page = 1)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(d => d.UserName == User.Identity.Name);

        //    var vm = new OrderPagedVM
        //    {
        //        PageIndex = page,
        //        StoreId = storeId,
        //        Mobile = mobile,
        //        Cashier = user.UserName

        //    };

        //    var query = _context.Orders.Include(d => d.OrderDetails).Where(d => d.Cashier == vm.Cashier).AsQueryable();
        //    if (storeId > 0)
        //    {
        //        query = query.Where(d => d.StoreId == storeId);
        //    }

        //    if (!string.IsNullOrEmpty(mobile))
        //    {
        //        query = query.Where(d => d.CustomerMobile == mobile);
        //    }

        //    var orders = await query.OrderByDescending(d => d.Id)
        //         .Select(d => new OrderVM
        //         {
        //             Id = d.Id,
        //             Amount = d.Amount,
        //             Cashier = d.Cashier,
        //             CreatedDate = d.CreatedDate,
        //             Concessional = d.Concessional,
        //             StoreName = d.Store.Name,
        //             AppType = d.AppType,
        //             Cancelled = d.Cancelled,
        //             CustomerMobile = d.CustomerMobile,
        //             CustomerAge = d.CustomerAge,
        //             CustomerName = d.CustomerName,
        //             CustomerGender = d.CustomerGender,
        //             CustomerWorkplace = d.CustomerWorkplace


        //         }).Skip((vm.PageIndex - 1) * 10).Take(10).ToListAsync();

        //    var ids = orders.Select(d => d.Id).ToList();

        //    var orderDetails = await _context.OrderDetails.Include(o => o.Product).Where(o => ids.Contains(o.OrderId)).Select(o => new OrderDetailVM
        //    {
        //        Id = o.Id,
        //        Price = o.Price,
        //        Quantity = o.Quantity,
        //        ProductId = o.ProductId,
        //        ProductName = o.Product.Name,
        //        Summary = o.Product.Summary,
        //        Thumbnail = o.Product.Thumbnail,
        //        OrderId = o.OrderId

        //    }).ToListAsync();

        //    foreach (var item in orders)
        //    {
        //        item.OrderDetails = orderDetails.Where(d => d.OrderId == item.Id).ToList();
        //    }
        //    vm.TotalCount = await query.CountAsync();
        //    vm.Orders = new StaticPagedList<OrderVM>(orders, vm.PageIndex, 10, vm.TotalCount);

        //    ViewData["Stores"] = new SelectList(_context.Stores, "Id", "Name");

        //    return View(vm);
        //}

        //public async Task<IActionResult> StoreSaleList(DateTime? startDate,DateTime? endDate, int storeId)
        //{
        //    var vm = new StoreSalesVM
        //    {
        //        StartDate = startDate,
        //        EndDate = endDate,
        //        StoreId = storeId,
        //       // Products =

        //    };
        //    var query = _context.OrderDetails.Include(o => o.Product).Where(o => o.Order.StoreId == storeId).AsQueryable();
        //    if(startDate!= null)
        //    {
        //        query = query.Where(d => d.Order.CreatedDate >= startDate);
        //    }
        //    if (endDate != null)
        //    {
        //        query = query.Where(d => d.Order.CreatedDate < endDate);
        //    }

        //    vm.Products = await query
        //        .GroupBy(d =>new { d.ProductId, d.Product.Name })
        //        .Select(p => new SalesProductVM
        //        {
        //            Id= p.Key.ProductId,
        //            Name = p.Key.Name,
        //            Qty = p.Sum(d => d.Quantity),
        //            Amount = p.Sum(d => d.Quantity * d.Price)
        //        }).ToListAsync();


        //    return View(vm);
        //}
        //public async Task<IActionResult> BuyList(string mobile)
        //{
        //    var orders = await _context.Orders.Where(o=>o.CustomerMobile == mobile).OrderByDescending(d=>d.Id)
        //        .Select(d=>new OrderVM
        //        {
        //            Id = d.Id,
        //            Amount = d.Amount,
        //            Cashier = d.Cashier,
        //            CreatedDate = d.CreatedDate,
        //            Concessional = d.Concessional,
        //            StoreName = d.Store.Name,
        //            AppType = d.AppType,                  
        //            Cancelled = d.Cancelled
        //        }).ToListAsync();

        //    var orderDetails = await _context.OrderDetails.Include(o=>o.Product).Where(o => o.Order.CustomerMobile == mobile).Select(o => new OrderDetailVM
        //    {
        //        Id = o.Id,
        //        Price = o.Price,
        //        Quantity = o.Quantity,
        //        ProductId = o.ProductId,
        //        ProductName = o.Product.Name,
        //        Summary = o.Product.Summary,
        //        Thumbnail = o.Product.Thumbnail,
        //        OrderId = o.OrderId

        //    }).ToListAsync();

        //    foreach (var item in orders)
        //    {
        //        item.OrderDetails = orderDetails.Where(d=>d.OrderId==item.Id).ToList();
        //    }

        //    return View(orders);
        //}

        // GET: Admin/Orders/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders             
        //        .Include(o => o.Store)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        // GET: Admin/Orders/Create
        //public IActionResult Create()
        //{
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Mobile");
        //    ViewData["StoreId"] = new SelectList(_context.Stores, "Id", "Address");
        //    return View();
        //}

        //// POST: Admin/Orders/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,CreatedDate,CustomerId,AppType,StoreId,Concessional,Amount,Cashier")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Mobile", order.CustomerId);
        //    ViewData["StoreId"] = new SelectList(_context.Stores, "Id", "Address", order.StoreId);
        //    return View(order);
        //}

        //// GET: Admin/Orders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Mobile", order.CustomerId);
        //    ViewData["StoreId"] = new SelectList(_context.Stores, "Id", "Address", order.StoreId);
        //    return View(order);
        //}

        //// POST: Admin/Orders/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedDate,CustomerId,AppType,StoreId,Concessional,Amount,Cashier")] Order order)
        //{
        //    if (id != order.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Mobile", order.CustomerId);
        //    ViewData["StoreId"] = new SelectList(_context.Stores, "Id", "Address", order.StoreId);
        //    return View(order);
        //}

            /// <summary>
            /// 发货
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delivery(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            order.Status = 2; //订单状态（0：待付款；1：待发货；2：已发货；3：待评价；4：已完成；10：已取消）
            _context.Update(order);
            await _context.SaveChangesAsync();

            AR.SetSuccess(Messages.AlertActionSuccess);
            return Json(AR);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelled(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            order.Status = 10; //订单状态（0：待付款；1：待发货；2：已发货；3：待评价；4：已完成；10：已取消）
            _context.Update(order);
            await _context.SaveChangesAsync();

            AR.SetSuccess(Messages.AlertActionSuccess);
            return Json(AR);
        }

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.Id == id);
        //}
    }
}
