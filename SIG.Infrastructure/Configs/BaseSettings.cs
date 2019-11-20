using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SIG.Infrastructure.Configs
{
  
        public abstract class BaseSettings
        {
            [XmlElement(DataType = "boolean", ElementName = "EnableCaching")]
            public bool EnableCaching { get; set; }

            [XmlElement(DataType = "int", ElementName = "CacheDuration")]
            public int CacheDuration { get; set; }
            [XmlElement(DataType = "int", ElementName = "PageSize")]      
            public int PageSize { get; set; }
        }
    
}
