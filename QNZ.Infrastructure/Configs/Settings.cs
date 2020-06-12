using System.Xml.Serialization;

namespace QNZ.Infrastructure.Configs
{
    public class GlobalConfig
    {
        /// <summary>
        /// 网站名称
        /// </summary>       
        public string SiteName { get; set; }

        /// <summary>
        /// 域名
        /// </summary>      
        public string SiteDomainName { get; set; }

      
        /// <summary>
        /// 备案号
        /// </summary>
        public string WebNumber { get; set; }


        /// <summary>
        /// 百度统计ID
        /// </summary>
        public string BaiduSiteID { get; set; }

 
    }

    public class CompanyConfig
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string  Email { get; set; }
    }

    /// <summary>
    /// 全局设置
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class GlobalSettings
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        [XmlElement("SiteName")]
        public string SiteName { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        [XmlElement("SiteDomainName")]
        public string SiteDomainName { get; set; }

        /// <summary>
        /// 开发者域名
        /// </summary>
        [XmlElement("Developer")]
        public string Developer{ get; set; }
        /// <summary>
        /// 开发者域名
        /// </summary>
        [XmlElement("DeveloperDomainName")]       
        public string DeveloperDomainName { get; set; }

        /// <summary>
        /// 备案号
        /// </summary>
        [XmlElement("WebNumber")]
        public string WebNumber { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [XmlElement("Version")]
        public string Version { get; set; }

        /// <summary>
        /// 百度统计ID
        /// </summary>
        [XmlElement("BaiduSiteID")]
        public string BaiduSiteID { get; set; }

        /// <summary>
        /// 招聘接收简历邮箱
        /// </summary>
        [XmlElement("EmailHr")]
        public string EmailHr { get; set; }

        [XmlElement("DatabaseBackupDir")]
        public string DatabaseBackupDir { get; set; }               

        /// <summary>
        /// 缓存时间
        /// </summary>
        [XmlElement(DataType = "int", ElementName = "CacheDuration")]
        public int CacheDuration { get; set; }

        [XmlElement(DataType = "boolean", ElementName = "EnableCaching")]
        public bool EnableCaching { get; set; }


        [XmlElement(DataType = "boolean", ElementName = "IsClose")]
        public bool IsClose { get; set; }
        [XmlElement("CloseInfo")]
        public string CloseInfo { get; set; }
 

        /// <summary>
        /// 控制台logo设置
        /// </summary>
        [XmlElement("DashboardLogo")]
        public string DashboardLogo { get; set; }

        [XmlElement("MailTo")]
        public string MailTo { get; set; }

        
    }

    /// <summary>
    /// Videos configs
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class VideoSettings : BaseSettings
    {
        [XmlElement(DataType = "int", ElementName = "FrontPageSize")]
        public int FrontPageSize { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbHeight")]
        public int ThumbHeight { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbWidth")]
        public int ThumbWidth { get; set; }
        [XmlElement(DataType = "int", ElementName = "Timer")]
        public int Timer { get; set; }
       
    }

    /// <summary>
    /// Specialists configs
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class AnnouncementSettings : BaseSettings
    {

    }

    [XmlRoot("Settings")]//serializable attribute
    public class LuceneSettings : BaseSettings
    {
        [XmlElement(DataType = "int", ElementName = "FrontPageSize")]
        public int FrontPageSize { get; set; }
        
    }
    /// <summary>
    /// Category configs
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class StoreSettings : BaseSettings
    {
       

        [XmlElement("PhotoWidth")]
        public int PhotoWidth { get; set; }

        [XmlElement("PhotoHeight")]
        public int PhotoHeight { get; set; }
    }


    /// <summary>
    /// Questions configs
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class QuestionSettings : BaseSettings
    {
      
    }

    /// <summary>
    /// Links configs
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class LinkSettings : BaseSettings
    {
      
    }
    [XmlRoot("Settings")]//serializable attribute
    public class BlogSettings : BaseSettings
    {
        [XmlElement(DataType = "int", ElementName = "FrontPageSize")]
        public int FrontPageSize { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbHeight")]
        public int ThumbHeight { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbWidth")]
        public int ThumbWidth { get; set; }
    }
    /// <summary>
    /// Category configs
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class CategorySettings : BaseSettings
    {
       
    }

    [XmlRoot("Settings")]
    public class TeamSettings : BaseSettings
    {

    }
    [XmlRoot("Settings")]
    public class JobSettings : BaseSettings
    {

    }

    /// <summary>
    /// 日志设置
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class LogSettings : BaseSettings
    {
      
    }


    /// <summary>
    /// 微信设置
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class WeiXinSettings
    {

        [XmlElement("Token")]
        public string Token { get; set; }

        [XmlElement("AppId")]
        public string AppId { get; set; }

        [XmlElement("AppSecret")]
        public string AppSecret { get; set; }

        [XmlElement("EncodingAESKey")]
        public string EncodingAESKey { get; set; }
       

    }


    /// <summary>
    /// 联系表单设置
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class ContactSettings
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [XmlElement("CompanyName")]
        public string CompanyName { get; set; }

        [XmlElement("CompanyShortName")]
        public string CompanyShortName { get; set; }

        [XmlElement("Address")]
        public string Address { get; set; }

        [XmlElement("Coordinate")]
        public string Coordinate { get; set; }

        [XmlElement("ContactMan")]
        public string ContactMan { get; set; }

        [XmlElement("Fax")]
        public string Fax { get; set; }

        [XmlElement("Phone")]
        public string Phone { get; set; }

        [XmlElement("ZipCode")]
        public string ZipCode { get; set; }
        [XmlElement("Mobile")]
        public string Mobile { get; set; }

        [XmlElement("MailTo")]
        public string MailTo { get; set; }

        [XmlElement("MailCC")]
        public string MailCC { get; set; }      
    }

    /// <summary>
    /// 社交设置
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class SocialSettings
    {
        
        [XmlElement("Oicq")]
        public string Oicq { get; set; }

        [XmlElement("OicqTwo")]
        public string OicqTwo { get; set; }

        [XmlElement("SinaWeibo")]
        public string SinaWeibo { get; set; }

        [XmlElement("WeiXing")]
        public string WeiXing { get; set; }

        [XmlElement("WeiXingCode")]
        public string WeiXingCode { get; set; }

        
    }
   
    /// <summary>
    /// 角色设置
    /// </summary>
    [XmlRoot("Settings")]
    public class RoleSettings :BaseSettings
    {
        //[XmlElement("modelName")]
        //public string ModelName { get; set; }

        //[XmlElement("IconClass")]
        //public string IconClass { get; set; }

        [XmlElement(DataType = "int", ElementName = "Founder")]
        public int Founder { get; set; }
        
    }

    /// <summary>
    /// 用户设置
    /// </summary>
    [XmlRoot("Settings")]
    public class UserSettings : BaseSettings
    {
               

        [XmlElement("DefaultAvatar")]
        public string DefaultAvatar { get; set; }
        [XmlElement("AvatarDir")]
        public string AvatarDir { get; set; }
        
        /// <summary>
        /// 创始人
        /// </summary>
        [XmlElement("Founder")]
        public string Founder { get; set; }
    }

    /// <summary>
    /// Carousel config
    /// </summary>
    [XmlRoot("Settings")]
    public class AdsSettings : BaseSettings
    {    

        [XmlElement("IconClass")]
        public string IconClass { get; set; }

    }

    /// <summary>
    /// Page config
    /// </summary>
    [XmlRoot("Settings")]
    public class ProductSettings : BaseSettings
    {
        [XmlElement(DataType = "int", ElementName = "CategoryImageHeight")]
        public int CategoryImageHeight { get; set; }
        [XmlElement(DataType = "int", ElementName = "CategoryImageWidth")]
        public int CategoryImageWidth { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbHeight")]
        public int ThumbHeight { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbWidth")]
        public int ThumbWidth { get; set; }

        [XmlElement(DataType = "int", ElementName = "ImageHeight")]
        public int ImageHeight { get; set; }
        [XmlElement(DataType = "int", ElementName = "ImageWidth")]
        public int ImageWidth { get; set; }

        [XmlElement(DataType = "int", ElementName = "FrontPageSize")]
        public int FrontPageSize { get; set; }
        

    }

    /// <summary>
    /// Page config
    /// </summary>
    [XmlRoot("Settings")]
    public class ArticleSettings : BaseSettings
    {
        [XmlElement(DataType = "int", ElementName = "FrontPageSize")]
        public int FrontPageSize { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbHeight")]
        public int ThumbHeight { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbWidth")]
        public int ThumbWidth { get; set; }
        [XmlElement(DataType = "int", ElementName = "ImageWidth")]
        public int ImageWidth { get; set; }
        [XmlElement(DataType = "int", ElementName = "ImageHeight")]
        public int ImageHeight { get; set; }

    }

    /// <summary>
    /// FilterTemplate configs
    /// </summary>
    [XmlRoot("Settings")]//serializable attribute
    public class FilterTemplateSettings : BaseSettings
    {

    }

    /// <summary>
    /// Page config
    /// </summary>
    [XmlRoot("Settings")]
    public class PageSettings : BaseSettings
    {
            

    }

    /// <summary>
    /// PageMeta config
    /// </summary>
    [XmlRoot("Settings")]
    public class PageMetaSettings : BaseSettings
    {

    }


    /// <summary>
    /// 菜单设置
    /// </summary>
    [XmlRoot("Settings")]
    public class MenuSettings : BaseSettings
    {


        /// <summary>
        /// 后台菜单固定ID
        /// </summary>
        [XmlElement(DataType = "int", ElementName = "BackMenuCId")]
        public int BackMenuCId { get; set; }
        /// <summary>
        /// 前台菜单固定ID
        /// </summary>
        [XmlElement(DataType = "int", ElementName = "FrontMenuCId")]
        public int FrontMenuCId { get; set; }
        

    }



    /// <summary>
    /// Email config
    /// </summary>
    [XmlRoot("Settings")]
    public class EmailSettings : BaseSettings
    {

    }
    [XmlRoot("Settings")]
    public class EmailAccountSettings : BaseSettings
    {

    }
    [XmlRoot("Settings")]
    public class EmailTemplateSettings : BaseSettings
    {

    }

    [XmlRoot("Settings")]
    public class ChronicleSettings : BaseSettings
    {

    }

    [XmlRoot("Settings")]
    public class ClientSettings : BaseSettings
    {

    }

    [XmlRoot("Settings")]
    public class CaseSettings : BaseSettings
    {
        [XmlElement(DataType = "int", ElementName = "FrontPageSize")]
        public int FrontPageSize { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbHeight")]
        public int ThumbHeight { get; set; }
        [XmlElement(DataType = "int", ElementName = "ThumbWidth")]
        public int ThumbWidth { get; set; }
    }

    [XmlRoot("Settings")]
    public class AlbumSettings : BaseSettings
    {
        [XmlElement("DefaultCover")]
        public string DefaultCover { get; set; }
    }

    [XmlRoot("Settings")]
    public class PhotoSettings : BaseSettings
    {
        [XmlElement("ThumbDir")]
        public string ThumbDir { get; set; }
        [XmlElement("ThumbWidth")]
        public int ThumbWidth { get; set; }

        [XmlElement("ThumbHeight")]
        public int ThumbHeight { get; set; }
    }
    ///// <summary>
    ///// 邮件服务设置
    ///// </summary>
    //[XmlRoot("Settings")]
    //public class SMTPSettings
    //{
    //    [XmlElement("From")]
    //    public string From { get; set; }

    //    [XmlElement(DataType = "string", ElementName = "SmtpServer")]
    //    public string SmtpServer { get; set; }

    //    [XmlElement(DataType = "int", ElementName = "Port")]
    //    public int Port { get; set; }

    //    [XmlElement(DataType = "string", ElementName = "UserName")]
    //    public string UserName { get; set; }

    //    [XmlElement(DataType = "string", ElementName = "Password")]
    //    public string Password { get; set; }

    //    [XmlElement(DataType = "boolean", ElementName = "EnableSsl")]
    //    public bool EnableSsl { get; set; }

    //    [XmlElement("iconClass")]
    //    public string IconClass { get; set; }



    //}
}
