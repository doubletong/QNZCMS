using AutoMapper;
using QNZ.Data;
using QNZ.Model.Administrator.InputModel;
using QNZ.Model.Administrator.ViewModel;

namespace QNZ.Model.Administrator
{
    public class AdminMappingProfile: Profile
    {
        /// <summary>
        /// 创建admin所有映射
        /// </summary>
        public AdminMappingProfile()
        {
            CreateMap<Webpart, WebpartVM>();
            CreateMap<Log, LogVM>();
            
            CreateMap<PostCategory, PostCategoryBVM>();
            CreateMap<PostCategory, PostCategoryIM>();
            CreateMap<PostCategoryIM, PostCategory>();
            
            CreateMap<Post, PostBVM>()
                .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title));
            CreateMap<Post, PostIM>();
            CreateMap<PostIM, Post>();
        }
    }
}