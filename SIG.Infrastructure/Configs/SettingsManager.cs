namespace SIG.Infrastructure.Configs
{
    public class SettingsManager
    {
        public static GlobalSettings Site => SettingLoader.LoadConfig<GlobalSettings>();

        public static QuestionSettings Question => SettingLoader.LoadConfig<QuestionSettings>();

        public static LuceneSettings Lucene => SettingLoader.LoadConfig<LuceneSettings>();


        public static FilterTemplateSettings FilterTemplate => SettingLoader.LoadConfig<FilterTemplateSettings>();

        public static VideoSettings Video => SettingLoader.LoadConfig<VideoSettings>();

        public static BlogSettings Blog => SettingLoader.LoadConfig<BlogSettings>();

        public static AnnouncementSettings Announcement => SettingLoader.LoadConfig<AnnouncementSettings>();

        public static LinkSettings Link => SettingLoader.LoadConfig<LinkSettings>();


        public static ClientSettings Client => SettingLoader.LoadConfig<ClientSettings>();

        public static CaseSettings Case => SettingLoader.LoadConfig<CaseSettings>();

        public static StoreSettings Store => SettingLoader.LoadConfig<StoreSettings>();

        public static CategorySettings Category => SettingLoader.LoadConfig<CategorySettings>();

        public static ProductSettings Product => SettingLoader.LoadConfig<ProductSettings>();

        public static ArticleSettings Article
        {
            get
            {
                return SettingLoader.LoadConfig<ArticleSettings>();
            }
        }
        public static PageSettings Page
        {
            get
            {
                return SettingLoader.LoadConfig<PageSettings>();
            }
        }
        public static PageMetaSettings PageMeta
        {
            get
            {
                return SettingLoader.LoadConfig<PageMetaSettings>();
            }
        }

        public static AdsSettings Ads
        {
            get
            {
                return SettingLoader.LoadConfig<AdsSettings>();
            }
        }

        public static TeamSettings Team => SettingLoader.LoadConfig<TeamSettings>();

        public static JobSettings Job => SettingLoader.LoadConfig<JobSettings>();

        public static LogSettings Log => SettingLoader.LoadConfig<LogSettings>();

        public static ContactSettings Contact
        {
            get
            {
                return SettingLoader.LoadConfig<ContactSettings>();
            }
        }

        public static SocialSettings Social
        {
            get
            {
                return SettingLoader.LoadConfig<SocialSettings>();
            }
        }
        

        public static RoleSettings Role
        {
            get
            {
                return SettingLoader.LoadConfig<RoleSettings>();
            }
        }

        public static UserSettings User
        {
            get
            {
                return SettingLoader.LoadConfig<UserSettings>();
            }
        }

        public static MenuSettings Menu
        {
            get
            {
                return SettingLoader.LoadConfig<MenuSettings>();
            }
        }

        //public static SMTPSettings SMTP
        //{
        //    get
        //    {
        //        return SettingLoader.LoadConfig<SMTPSettings>();
        //    }
        //}
        public static EmailSettings Email
        {
            get
            {
                return SettingLoader.LoadConfig<EmailSettings>();
            }
        }
        public static EmailAccountSettings EmailAccount
        {
            get
            {
                return SettingLoader.LoadConfig<EmailAccountSettings>();
            }
        }
        public static EmailTemplateSettings EmailTemplate
        {
            get
            {
                return SettingLoader.LoadConfig<EmailTemplateSettings>();
            }
        }
        public static FileSettings File
        {
            get
            {
                return SettingLoader.LoadConfig<FileSettings>();
            }
        }
        public static WeiXinSettings WeiXin
        {
            get
            {
                return SettingLoader.LoadConfig<WeiXinSettings>();
            }
        }


        public static ChronicleSettings Chronicle
        {
            get
            {
                return SettingLoader.LoadConfig<ChronicleSettings>();
            }
        }

        public static AlbumSettings Album
        {
            get
            {
                return SettingLoader.LoadConfig<AlbumSettings>();
            }
        }

        public static PhotoSettings Photo
        {
            get
            {
                return SettingLoader.LoadConfig<PhotoSettings>();
            }
        }

    }
}
