﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QNZ.Data
{
    public partial class YicaiyunContext : DbContext
    {
        public YicaiyunContext()
        {
        }

        public YicaiyunContext(DbContextOptions<YicaiyunContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<AdvertisingSpace> AdvertisingSpaces { get; set; }
        public virtual DbSet<AgentSet> AgentSets { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerRecipe> CustomerRecipes { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<FeedbackSet> FeedbackSets { get; set; }
        public virtual DbSet<FeedbackTypeSet> FeedbackTypeSets { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<MailingAddress> MailingAddresses { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuCategory> MenuCategories { get; set; }
        public virtual DbSet<MobileCodeSet> MobileCodeSets { get; set; }
        public virtual DbSet<Navigation> Navigations { get; set; }
        public virtual DbSet<NavigationCategory> NavigationCategories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderComment> OrderComments { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<PageMeta> PageMetas { get; set; }
        public virtual DbSet<PcategoryProduct> PcategoryProducts { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostCategory> PostCategories { get; set; }
        public virtual DbSet<PostTag> PostTags { get; set; }
        public virtual DbSet<PostTagesLinkPost> PostTagesLinkPosts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipesItem> RecipesItems { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleMenu> RoleMenus { get; set; }
        public virtual DbSet<Solution> Solutions { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Work> Works { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advertisement>(entity =>
            {
                entity.HasOne(d => d.Space)
                    .WithMany(p => p.Advertisements)
                    .HasForeignKey(d => d.SpaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Advertisements_AdvertisingSpaces");
            });

            modelBuilder.Entity<AgentSet>(entity =>
            {
                entity.Property(e => e.City).HasComment("市");

                entity.Property(e => e.District).HasComment("区/县");

                entity.Property(e => e.Mobile).HasComment("手机");

                entity.Property(e => e.Name).HasComment("名称");

                entity.Property(e => e.Principal).HasComment("负责人");

                entity.Property(e => e.Province).HasComment("省");

                entity.Property(e => e.WechatId).HasComment("微信号");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Articles_ArticleCategories");
            });

            modelBuilder.Entity<ArticleCategory>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_CartItemSet_CartSet");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartItemSet_ProductSet");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartItem_Store");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_T_City_T_Province");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.OpenId)
                    .HasName("PK_CustomerSet");

                entity.Property(e => e.Avatar).HasComment("用户头像");

                entity.Property(e => e.CreatedDate).HasComment("创建时间");

                entity.Property(e => e.LastLogin).HasComment("最后一次登录时间");

                entity.Property(e => e.Mobile).HasComment("手机");

                entity.Property(e => e.WechatNickName).HasComment("微信号");
            });

            modelBuilder.Entity<CustomerRecipe>(entity =>
            {
                entity.HasOne(d => d.Recipes)
                    .WithMany(p => p.CustomerRecipes)
                    .HasForeignKey(d => d.RecipesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerRecipes_Recipes");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasOne(d => d.City)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_District_T_City");
            });

            modelBuilder.Entity<FeedbackSet>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasComment("反馈时间");

                entity.Property(e => e.FeedbackTypeId).HasComment("反馈类型ID");

                entity.Property(e => e.Message).HasComment("内容");

                entity.Property(e => e.Mobile).HasComment("手机号");

                entity.Property(e => e.OpenId).HasComment("顾客ID");

                entity.HasOne(d => d.FeedbackType)
                    .WithMany(p => p.FeedbackSets)
                    .HasForeignKey(d => d.FeedbackTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FeedbackSet_FeedbackTypeSet");

                entity.HasOne(d => d.Open)
                    .WithMany(p => p.FeedbackSets)
                    .HasForeignKey(d => d.OpenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FeedbackSet_CustomerSet");
            });

            modelBuilder.Entity<FeedbackTypeSet>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasComment("创建人");

                entity.Property(e => e.CreatedDate).HasComment("创建时间");

                entity.Property(e => e.Title).HasComment("反馈类型");
            });

            modelBuilder.Entity<MailingAddress>(entity =>
            {
                entity.Property(e => e.Active).HasComment("是否默认");

                entity.Property(e => e.Mobile)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Open)
                    .WithMany(p => p.MailingAddresses)
                    .HasForeignKey(d => d.OpenId)
                    .HasConstraintName("FK_MailingAddress_CustomerSet");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_MenuSet_MenuCategorySet_CategoryId");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_MenuSet_MenuSet_ParentId");
            });

            modelBuilder.Entity<MobileCodeSet>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasComment("创建时间");

                entity.Property(e => e.IsUsed).HasComment("是否使用");

                entity.Property(e => e.Mobile).HasComment("手机号");

                entity.Property(e => e.ValidateCode).HasComment("验证码");
            });

            modelBuilder.Entity<Navigation>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Navigations)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Navigations_NavigationCategories");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Navigations_Navigations");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Amount).HasComment("优惠金额");

                entity.Property(e => e.Consignee).HasComment("收件人");

                entity.Property(e => e.CreatedDate).HasComment("购买时间");

                entity.Property(e => e.OpenId)
                    .HasDefaultValueSql("((0))")
                    .HasComment("客户ID");

                entity.Property(e => e.Remark).HasComment("备注");

                entity.Property(e => e.Status).HasComment("订单状态（0：待付款；1：待发货；2：已发货；3：待评价；4：已完成；10：已取消）");

                entity.HasOne(d => d.Open)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OpenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customers_OrderSet");
            });

            modelBuilder.Entity<OrderComment>(entity =>
            {
                entity.Property(e => e.OrderId).ValueGeneratedNever();

                entity.HasOne(d => d.Order)
                    .WithOne(p => p.OrderComment)
                    .HasForeignKey<OrderComment>(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderComment_Order");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Products");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Store");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.Property(e => e.Importance)
                    .HasDefaultValueSql("((0))")
                    .HasComment("权重，值越高越排前");
            });

            modelBuilder.Entity<PageMeta>(entity =>
            {
                entity.HasKey(e => new { e.ModuleType, e.ObjectId });
            });

            modelBuilder.Entity<PcategoryProduct>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.ProductId });

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.PcategoryProducts)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PCategoryProducts_ProductCategory");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PcategoryProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PCategoryProducts_Products");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_PostCategory");
            });

            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.HasIndex(e => e.Alias)
                    .HasName("IX_PostCategory")
                    .IsUnique();

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.HasIndex(e => e.TagName)
                    .HasName("IX_PostTags")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<PostTagesLinkPost>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.TagId });

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostTagesLinkPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostTagesLinkPosts_PostTagesLinkPosts");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PostTagesLinkPosts)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_PostTagesLinkPosts_PostTags");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Active).HasComment("上下架");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasComment("详情描述");

                entity.Property(e => e.Name).HasComment("产品名称");

                entity.Property(e => e.Price).HasComment("价格");

                entity.Property(e => e.Specification).HasComment("规格");

                entity.Property(e => e.Stock).HasComment("库存");

                entity.Property(e => e.StoreId).HasComment("店铺基地ID");

                entity.Property(e => e.Summary).HasComment("简介");

                entity.Property(e => e.Thumbnail).HasComment("图片");

                entity.Property(e => e.Unit).HasComment("单位");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_Store");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Description).HasComment("吃法说明");

                entity.Property(e => e.Title).HasComment("营养食谱名称");
            });

            modelBuilder.Entity<RecipesItem>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.RecipesItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipesItem_Products");

                entity.HasOne(d => d.Recipes)
                    .WithMany(p => p.RecipesItems)
                    .HasForeignKey(d => d.RecipesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipesItem_Recipes");
            });

            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.MenuId })
                    .HasName("PK_RoleMenuSet");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.RoleMenus)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_RoleMenuSet_MenuSet_MenuId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleMenus)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleMenuSet_RoleSet_RoleId");
            });

            modelBuilder.Entity<Solution>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.Address).HasComment("地址");

                entity.Property(e => e.City).HasComment("城市 ");

                entity.Property(e => e.Contact).HasComment("联系人");

                entity.Property(e => e.CreatedBy).HasComment("创建人");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("创建时间");

                entity.Property(e => e.District).HasComment("区/县");

                entity.Property(e => e.Longitude).HasComment("经度");

                entity.Property(e => e.Name).HasComment("店铺名称");

                entity.Property(e => e.Phone).HasComment("电话");

                entity.Property(e => e.Province).HasComment("省");

                entity.Property(e => e.UpdatedBy).HasComment("最后更新人");

                entity.Property(e => e.UpdatedDate).HasComment("最后更新时间");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.Email).IsFixedLength();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_UserRoleSet");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserRoleSet_RoleSet_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRoleSet_UserSet_UserId");
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.Property(e => e.SolutionId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Works)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Work_Client");

                entity.HasOne(d => d.Solution)
                    .WithMany(p => p.Works)
                    .HasForeignKey(d => d.SolutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Work_Solution");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}