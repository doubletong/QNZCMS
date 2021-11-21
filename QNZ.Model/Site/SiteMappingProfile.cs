using AutoMapper;
using QNZ.Data;
using QNZ.Model.Site.ViewModel;

namespace QNZ.Model.Site
{
    public class SiteMappingProfile: Profile
    {
        /// <summary>
        /// 创建site所有映射
        /// </summary>
        public SiteMappingProfile()
        {
            CreateMap<Log, LogVM>();
        }
    }
}