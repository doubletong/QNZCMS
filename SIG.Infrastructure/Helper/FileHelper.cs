﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace SIG.Infrastructure.Helper
{
    public class FileHelper
    {
        public static byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenRead(fullFilePath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

        }

        private static readonly string[] _extensions = new[]
                                                          {
                                                              "zip", "rar", "pdf",
                                                              "jpg", "png", "gif", "psd",
                                                              "tiff", "xls", "xlsx", "doc", "docx"
                                                          };


        //public static List<SelectListItem> GetExtensions()
        //{
        //    var extensions = new List<SelectListItem>();
        //    foreach (string item in _extensions)
        //    {
        //        extensions.Add(new SelectListItem { Value = item, Text = item });
        //    }
        //    return extensions;
        //}

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <param name="filaName"></param>
        /// <param name="localPath"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetFileName(string filaName, string localPath, string ex)
        {
            filaName = filaName.Replace("(", "");
            filaName = filaName.Replace(")", "");

            var filePathName = filaName + ex;

            var savePath = Path.Combine(localPath, filePathName);
            if (System.IO.File.Exists(savePath))
            {
                if (filaName.Contains("_"))
                {
                    var lastNum = filaName.Substring(filaName.LastIndexOf("_") + 1);
                    int num;
                    if (int.TryParse(lastNum, out num))
                    {
                        num++;
                        filaName = filaName.Substring(0, filaName.LastIndexOf("_")) + "_" + num.ToString();
                    }
                    else
                    {
                        filaName = filaName + "_1";
                    }
                }
                else
                {
                    filaName = filaName + "_1";

                }
                return GetFileName(filaName, localPath, ex);
            }
            else
            {
                return filaName;
            }
        }


       

     }
}
