using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using SIG.Infrastructure.Helper;

namespace SIG.Infrastructure.Configs
{
    internal sealed class SettingLoader
    {
        /// <summary>
        /// Private constructor
        /// </summary>
        private SettingLoader() { }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadConfig<T>() where T : class
        {
            return LoadConfig<T>(null);
        }

        /// <summary>
        /// Return Settings object from cache or from the xml file  
        /// </summary>
        /// <typeparam name="T">The type we will passing</typeparam>
        /// <param name="fileName"> </param>
        /// <returns></returns>
        ///
        public static T LoadConfig<T>(string fileName) where T : class
        {
            T configObj = null;
          
            if (string.IsNullOrEmpty(fileName))
            {
                

                fileName = PlatformServices.Default.MapPath(string.Concat("/Config/", typeof(T).Name, ".config"));
                fileName = fileName.ToLower().Replace(@"\bin\debug\netcoreapp2.1", "");

                OperatingSystem os = Environment.OSVersion;
                if(os.Platform == PlatformID.Unix){
                    fileName = PlatformServices.Default.MapPath("/Config");
                    fileName = fileName.Replace(@"/bin/Debug/netcoreapp2.1", "");
                    fileName = Path.Combine(fileName, $"{typeof(T).Name}.config");
                }

                //fileName = string.Concat("/Config/", typeof(T).Name, ".config").Replace("/", "\\");
                //if (fileName.StartsWith("\\"))
                //{
                //    fileName = fileName.TrimStart('\\');
                //}

                //fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            }

            string cacheKey = fileName;
            IOptions<MemoryCacheOptions> option = new MemoryCacheOptions();
            var myCache = new MemoryCache(option);
            configObj = myCache.Get<T>(cacheKey);
            if (configObj == null)
            {
                configObj = LoadFromXml<T>(fileName);
                myCache.Set(cacheKey, configObj);
            }

            return configObj;
        }
        /// <summary>
        /// Load the settings xml file and retun the Settings Type with Deserialize with xml content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">File Name of the custom xml Settings file</param>
        /// <returns>The T type which we have have paased with LoadFromXml<T> </returns>
        private static T LoadFromXml<T>(string fileName) where T : class
        {
            FileStream fs = null;
            try
            {
                //Serialize of the Type
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return (T)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                //write error log
                //ILoggingService _logger = new LoggingService();
                //_logger.Error(LogUtility.BuildExceptionMessage(ex));
                //LoggingFactory.GetLogger().Fatal(LogUtility.BuildExceptionMessage(ex));
                throw ex;
            }
            finally
            {
                fs?.Close();
            }
        }
    }
}
