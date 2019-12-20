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
using Microsoft.Extensions.FileProviders;
using SIG.Infrastructure.Cache;

namespace QNZCMS
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        #region elFinder config
        public static string WebRootPath { get; private set; }

        public static string MapPath(string path, string basePath = null)
        {
            if (string.IsNullOrEmpty(basePath))
            {
                basePath = WebRootPath;
            }

            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(basePath, path);
        }
        #endregion

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<YicaiyunContext>(options =>
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
               options.LoginPath = new PathString("/Account/LogIn");
               options.LogoutPath = new PathString("/Account/LogOff");
               options.AccessDeniedPath = new PathString("/Errors/AccessDenied");
               options.ExpireTimeSpan = TimeSpan.FromDays(1);
               options.SlidingExpiration = false;
           });
         

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policy => policy.Requirements.Add(new PermissionRequirement("/errors/accessdenied")));
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllersWithViews();
            services.AddRazorPages();


            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IRoleServices, RoleServices>();
            services.AddTransient<IMenuServices, MenuServices>();
            services.AddTransient<IMenuCategoryServices, MenuCategoryServices>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<ICacheService, CacheService>();
            // To list physical files from a path provided by configuration:
            var physicalProvider = new PhysicalFileProvider(Configuration.GetValue<string>("StoredFilesPath"));

            // To list physical files in the temporary files folder, use:
            //var physicalProvider = new PhysicalFileProvider(Path.GetTempPath());
            services.AddSingleton<IFileProvider>(physicalProvider);

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
          

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                 name: "areaAdminRoute", "Admin",
                 pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

            WebRootPath = env.WebRootPath;
        }
    }
}
