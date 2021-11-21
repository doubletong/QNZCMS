using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QNZ.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using QNZ.Services.Identity;
using QNZ.Services.Menus;
using QNZ.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.IO;
using System.Linq;
using System.Security.Claims;
using QNZ.Infrastructure.Cache;
using QNZ.Infrastructure.Email;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using QNZ.Model.Administrator;
using QNZ.Model.Settings;
using QNZ.Model.Site;
using QNZCMS.Services;
using Serilog.Context;

namespace QNZCMS
{
    public class Startup
    {
     
        public static string WebRootPath { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


       

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<QNZContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                     b => b.MigrationsAssembly("QNZCMS")));

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
           

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie( options =>
           {
               options.Cookie.Name = "QNZAUTH";
               options.LoginPath = new PathString("/account/login");
               options.LogoutPath = new PathString("/account/logout");
               options.AccessDeniedPath = new PathString("/errors/accessdenied");
               options.ExpireTimeSpan = TimeSpan.FromDays(1);
               options.SlidingExpiration = false;
           });
         

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policy => policy.Requirements.Add(new PermissionRequirement("/errors/accessdenied")));
            });


            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
                mc.AddProfile(new AdminMappingProfile());
                mc.AddProfile(new SiteMappingProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddBrowserDetection();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IRoleServices, RoleServices>();
            services.AddTransient<IMenuServices, MenuServices>();
            services.AddTransient<IMenuCategoryServices, MenuCategoryServices>();
            //services.AddScoped<IViewRenderService, ViewRendererService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IEmailService, MimeKitService>();
            
            services.ConfigureWritable<AdminProductSet>(Configuration.GetSection("Modules:Product:Administrator"));
            services.ConfigureWritable<AdminPageSet>(Configuration.GetSection("Modules:Page:Administrator"));
            services.ConfigureWritable<AdminLogSet>(Configuration.GetSection("Modules:Log:Administrator"));
            services.ConfigureWritable<SiteLogSet>(Configuration.GetSection("Modules:Log:Site"));
            
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            //     app.UseMigrationsEndPoint();
            // }
            // else
            // {
            //
            //     //app.UseHsts();
            //     app.UseStatusCodePagesWithRedirects("/errors/{0}");
            //
            // }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
        
            // below code is needed to get User name for Log             
            app.Use(async (httpContext, next) =>  
                {
                    var ip = httpContext.Connection.RemoteIpAddress;
                    var userName = httpContext.User.Identity is { IsAuthenticated: true } ? httpContext.User.Identity.Name : "游客"; //Gets user Name from user Identity  
                    LogContext.PushProperty("UserName", userName); //Push user in LogContext;  
                    LogContext.PushProperty("IP", ip);
                    await next.Invoke();  
                }  
            );  

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                   name: "areaAdminRoute",
                   areaName: "Admin",
                   pattern: "qnz-admin/{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapAreaControllerRoute(
                    name: "areaElFinderRoute",
                    areaName: "ElFinder",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapRazorPages();
            });

            WebRootPath = env.WebRootPath;
        }
    }
}
