using AutoMapper;
using QNZ.Data;
using QNZ.Model.ViewModel;
using System.Linq;

namespace QNZCMS
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Navigation, NavIM>();
            CreateMap<NavIM, Navigation>();

            CreateMap<Menu, MenuIM>();
            CreateMap<MenuIM, Menu>();

            CreateMap<MenuCategory, MenuCategoryIM>();
            CreateMap<MenuCategoryIM, MenuCategory>();

            CreateMap<NavigationCategory, MenuCategoryVM>();
            CreateMap<MenuCategoryVM, NavigationCategory>();

            CreateMap<NavigationCategory, NavigationCategoryIM>();
            CreateMap<NavigationCategoryIM, NavigationCategory>();
            
            CreateMap<MenuCategory, MenuCategoryVM>();
            CreateMap<MenuCategoryVM, MenuCategory>();

            CreateMap<UserDetailVM, User>();
            CreateMap<User, UserDetailVM>();
            CreateMap<ProfileIM, User>();
            CreateMap<User, ProfileIM>();
            CreateMap<Role, RoleIM>();
            CreateMap<RoleIM, Role>();

          
        
            //// CreateMap<StoreVM, Store>();            
            //CreateMap<Store, StoreVM>()
            //    .ForMember(d => d.Coordinate, opt => opt.MapFrom(source => $"{source.Longitude},{source.Latitude}"));

            //CreateMap<StoreIM, Store>();
            //CreateMap<Store, StoreIM>();

         

            CreateMap<Album, AlbumVM>();
            CreateMap<Album, AlbumBVM>();
            CreateMap<Album, AlbumIM>();
            CreateMap<AlbumIM, Album>();

            CreateMap<Photo, PhotoVM>();
            CreateMap<Photo, PhotoBVM>()
                 .ForMember(d => d.AlbumTitle, opt => opt.MapFrom(source => source.Album.Title));

            CreateMap<Photo, PhotoIM>();
            CreateMap<PhotoIM, Photo>();

         
    

            //CreateMap<Recipe, RecipeVM>()
            //        .ForMember(d => d.Username, opt => opt.MapFrom(source => source.User.UserName));

            //CreateMap<RecipesItem, RecipesItemVM>()
            //      .ForMember(d => d.ProductName, opt => opt.MapFrom(source => source.Product.Name))
            //      .ForMember(d => d.Price, opt => opt.MapFrom(source => source.Product.Price));



            CreateMap<ProductCategory, ProductCategoryBVM>();

            CreateMap<ProductCategory, ProductCategoryIM>();
            CreateMap<ProductCategoryIM, ProductCategory>();



            CreateMap<Product, ProductVM>();
            CreateMap<Product, ProductDetailVM>();

            CreateMap<ProductIM, Product>();
            CreateMap<Product, ProductIM>();

            CreateMap<Page, PageVM>();
            CreateMap<Page, PageDetailVM>();
            CreateMap<Page, PageBVM>();
        
            CreateMap<Page, PageIM>();
            CreateMap<PageIM, Page>();

            CreateMap<Memorabilium, MemorabiliaVM>();
            CreateMap<Memorabilium, MemorabiliaIM>();
            CreateMap<MemorabiliaIM, Memorabilium>();
  

            CreateMap<AdvertisingSpace, AdvertisingSpaceVM>();
            CreateMap<AdvertisingSpace, AdvertisingSpaceIM>();
            CreateMap<AdvertisingSpaceIM, AdvertisingSpace>();

            CreateMap<Advertisement, AdvertisementVM>().ForMember(d => d.SpaceTitle, opt => opt.MapFrom(source => source.Space.Title));
            CreateMap<Advertisement, AdvertisementIM>();
            CreateMap<AdvertisementIM, Advertisement>();

            CreateMap<PostCategory, PostCategoryBVM>();
            CreateMap<PostCategory, PostCategoryIM>();
            CreateMap<PostCategoryIM, PostCategory>();

            CreateMap<Post, PostBVM>()
                .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title));
            CreateMap<Post, PostIM>();
            CreateMap<PostIM, Post>();

            CreateMap<Solution, SolutionVM>();
            CreateMap<Solution, SolutionBVM>();
            CreateMap<Solution, SolutionIM>();
            CreateMap<SolutionIM, Solution>();

            CreateMap<Work, WorkBVM>()
            .ForMember(d => d.SolutionTitle, opt => opt.MapFrom(source => source.Solution.Title))
            .ForMember(d => d.ClientName, opt => opt.MapFrom(source => source.Client.ClientName));

            CreateMap<Work, WorkFVM>()
           .ForMember(d => d.SolutionTitle, opt => opt.MapFrom(source => source.Solution.Title))
           .ForMember(d => d.ClientName, opt => opt.MapFrom(source => source.Client.ClientName));

            CreateMap<Work, WorkIM>();
            CreateMap<WorkIM, Work>();

            CreateMap<Client, ClientBVM>();
            CreateMap<Client, ClientIM>();
            CreateMap<ClientIM, Client>();

            CreateMap<ArticleCategory, ArticleCategoryVM>();
            CreateMap<ArticleCategory, ArticleCategoryBVM>();
            CreateMap<ArticleCategory, ArticleCategoryIM>();
            CreateMap<ArticleCategoryIM, ArticleCategory>();

            CreateMap<Article, ArticleVM>()
                .ForMember(d => d.CategoryAlias, opt => opt.MapFrom(source => source.Category.Alias));
            CreateMap<Article, ArticleBVM>()
                 .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title));

            CreateMap<Article, ArticleIM>();
            CreateMap<ArticleIM, Article>();

            CreateMap<Exhibition, ExhibitionVM>();
            CreateMap<Exhibition, ExhibitionBVM>();           
            CreateMap<Exhibition, ExhibitionIM>();
            CreateMap<ExhibitionIM, Exhibition>();

            CreateMap<Shope, ShopeVM>();
            CreateMap<Shope, ShopeBVM>();
            CreateMap<Shope, ShopeIM>();
            CreateMap<ShopeIM, Shope>();

            CreateMap<SocialApp, SocialAppVM>();
            CreateMap<SocialApp, SocialAppBVM>();
            CreateMap<SocialApp, SocialAppIM>();
            CreateMap<SocialAppIM, SocialApp>();

            CreateMap<Video, VideoVM>();
            CreateMap<Video, VideoBVM>();
            CreateMap<VideoBVM, Video>();
            CreateMap<Video, VideoIM>();
            CreateMap<VideoIM, Video>();

            CreateMap<Branch, BranchVM>();
            CreateMap<Branch, BranchBVM>();
            CreateMap<Branch, BranchIM>();
            CreateMap<BranchIM, Branch>();


            CreateMap<DocCategory, DocCategoryVM>();
            CreateMap<DocCategory, DocCategoryBVM>();
            CreateMap<DocCategory, DocCategoryIM>();
            CreateMap<DocCategoryIM, DocCategory>();

            CreateMap<Document, DocumentVM>();
            CreateMap<Document, DocumentBVM>()
                 .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title));

            CreateMap<Document, DocumentIM>();
            CreateMap<DocumentIM, Document>();


            CreateMap<Organization, OrganizationVM>();
            CreateMap<Organization, OrganizationBVM>();
            CreateMap<Organization, OrganizationIM>();
            CreateMap<OrganizationIM, Organization>();

            CreateMap<Staff, StaffVM>();
            CreateMap<Staff, StaffBVM>()
                 .ForMember(d => d.OrganizationName, opt => opt.MapFrom(source => source.Organization.Title));

            CreateMap<Staff, StaffIM>();
            CreateMap<StaffIM, Staff>();

            CreateMap<JobCategory, JobCategoryVM>();
            CreateMap<JobCategory, JobCategoryBVM>();
            CreateMap<JobCategory, JobCategoryIM>();
            CreateMap<JobCategoryIM, JobCategory>();

            CreateMap<Job, JobVM>();
            CreateMap<Job, JobBVM>()
                 .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title))
                 .ForMember(d => d.CompanyName, opt => opt.MapFrom(source => source.Branch.Name));

            CreateMap<Job, JobIM>();
            CreateMap<JobIM, Job>();


            CreateMap<ProductCategory, ProductCategoryVM>();
            CreateMap<ProductCategory, ProductCategoryBVM>();
            CreateMap<ProductCategory, ProductCategoryIM>();
            CreateMap<ProductCategoryIM, ProductCategory>();

            CreateMap<Product, ProductVM>();
            CreateMap<Product, ProductBVM>()
                 .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title));

            CreateMap<Product, ProductIM>();
            CreateMap<ProductIM, Product>();
        }
    }
}
