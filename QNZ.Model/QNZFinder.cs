using QNZ.Infrastructure.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QNZ.Model
{
    public class DirectoryVM
    {
        public string Name { get; set; }
        public string DirPath { get; set; }
        public bool HasChildren { get; set; }
        public bool IsOpen { get; set; }
        public IEnumerable<DirectoryVM> Children { get; set; }
    }

    public class FileVM
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string CreatedDate { get; set; }
        public string FilePath { get; set; }
        public double FileSize { get; set; }
        public string ImgUrl { get; set; }
        //{
        //    get
        //    {
        //        return ".jpg.png.gif".Contains(this.Extension.ToLower()) ? this.FilePath + "?width=80&height=80&mode=Crop" : string.Format("{0}/{1}.png", SettingsManager.File.ExtensionDir, this.Extension); 
        //    }
        //}
    }

    
}
