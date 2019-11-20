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
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderComment> OrderComments { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<PcategoryProduct> PcategoryProducts { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostCategory> PostCategories { get; set; }
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

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Data Source=DESKTOP-5NKIOPC;Initial Catalog=TZGCMS_NetCore;Integrated Security=True", x => x.UseNetTopologySuite());
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

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

                entity.Property(e => e.OpenId).ValueGeneratedNever();
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

            modelBuilder.Entity<MailingAddress>(entity =>
            {
                entity.Property(e => e.Mobile).IsUnicode(false);

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

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OpenId).HasDefaultValueSql("((0))");

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
                entity.Property(e => e.Importance).HasDefaultValueSql("((0))");
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
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

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
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipes_User");
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
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
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