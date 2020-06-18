namespace QNZ.Infrastructure.Configs
{
    public static class SettingsManager
    {
        public static CompanyConfig Company => SettingLoader.LoadJsonConfig<CompanyConfig>();
        public static GlobalConfig SiteInfo => SettingLoader.LoadJsonConfig<GlobalConfig>();
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

        public static ArticleSettings Article => SettingLoader.LoadConfig<ArticleSettings>();
        public static PageSettings Page => SettingLoader.LoadConfig<PageSettings>();
        public static PageMetaSettings PageMeta => SettingLoader.LoadConfig<PageMetaSettings>();

        public static AdsSettings Ads => SettingLoader.LoadConfig<AdsSettings>();

        public static TeamSettings Team => SettingLoader.LoadConfig<TeamSettings>();

        public static JobSettings Job => SettingLoader.LoadConfig<JobSettings>();

        public static LogSettings Log => SettingLoader.LoadConfig<LogSettings>();

        public static ContactSettings Contact => SettingLoader.LoadConfig<ContactSettings>();

        public static SocialSettings Social => SettingLoader.LoadConfig<SocialSettings>();


        public static RoleSettings Role => SettingLoader.LoadConfig<RoleSettings>();

        public static UserSettings User => SettingLoader.LoadConfig<UserSettings>();

        public static MenuSettings Menu => SettingLoader.LoadConfig<MenuSettings>();

        public static SMTPSettings SMTP => SettingLoader.LoadConfig<SMTPSettings>();
        public static EmailSettings Email
        {
            get
            {
                return SettingLoader.LoadConfig<EmailSettings>();
            }
        }
        public static EmailAccountSettings EmailAccount => SettingLoader.LoadConfig<EmailAccountSettings>();
        public static EmailTemplateSettings EmailTemplate => SettingLoader.LoadConfig<EmailTemplateSettings>();

        public static WeiXinSettings WeiXin => SettingLoader.LoadConfig<WeiXinSettings>();


        public static ChronicleSettings Chronicle => SettingLoader.LoadConfig<ChronicleSettings>();

        public static AlbumSettings Album => SettingLoader.LoadConfig<AlbumSettings>();

        public static PhotoSettings Photo => SettingLoader.LoadConfig<PhotoSettings>();

    }
}
