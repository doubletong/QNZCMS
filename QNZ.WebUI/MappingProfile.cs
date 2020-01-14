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

            CreateMap<ArticleCategory, ArticleCategoryVM>();
            CreateMap<ArticleCategory, ArticleCategoryBVM>();
            CreateMap<ArticleCategory, ArticleCategoryIM>();
            CreateMap<ArticleCategoryIM, ArticleCategory>();

            CreateMap<Customer, CustomerVM>();
            CreateMap<Team, TeamVM>();

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
            CreateMap<Page, PageIM>();
            CreateMap<PageIM, Page>();

            CreateMap<Team, TeamIM>();
            CreateMap<TeamIM, Team>();

            CreateMap<AdvertisingSpace, AdvertisingSpaceVM>();
            CreateMap<AdvertisingSpace, AdvertisingSpaceIM>();
            CreateMap<AdvertisingSpaceIM, AdvertisingSpace>();

            CreateMap<Advertisement, AdvertisementVM>().ForMember(d => d.SpaceTitle, opt => opt.MapFrom(source => source.Space.Title));
            CreateMap<Advertisement, AdvertisementIM>();
            CreateMap<AdvertisementIM, Advertisement>();

            CreateMap<PostCategory, PostCategoryBVM>();
            CreateMap<PostCategory, PostCategoryIM>();
            CreateMap<PostCategoryIM, PostCategory>();

            //CreateMap<Post, PostBVM>()
            //    .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Pos.Title));
            //CreateMap<Post, PostIM>();
            //CreateMap<PostIM, Post>();

            CreateMap<Solution, SolutionBVM>()
              .ForMember(d => d.WorkCount, opt => opt.MapFrom(source => source.Works.Count()));
            CreateMap<Solution, SolutionIM>();
            CreateMap<SolutionIM, Solution>();

            CreateMap<Work, WorkBVM>()
            .ForMember(d => d.SolutionTitle, opt => opt.MapFrom(source => source.Solution.Title))
            .ForMember(d => d.ClientName, opt => opt.MapFrom(source => source.Client.ClientName));
            CreateMap<Work, WorkIM>();
            CreateMap<WorkIM, Work>();

            CreateMap<Client, ClientBVM>();
            CreateMap<Client, ClientIM>();
            CreateMap<ClientIM, Client>();

            CreateMap<Article, ArticleVM>();
            CreateMap<Article, ArticleBVM>()
                 .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title));

            CreateMap<Article, ArticleIM>();
            CreateMap<ArticleIM, Article>();
        }
    }
}
