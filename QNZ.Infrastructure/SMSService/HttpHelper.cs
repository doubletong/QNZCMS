using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SIG.Infrastructure.SMSService
{
    public class HttpHelper
    {
        /// <summary>
        /// post方法
        /// </summary>
        /// <param name="postUrl">地址</param>
        /// <param name="paramData">参数</param>
        /// <param name="Header">请求头</param>
        /// <param name="dataEncode">编码</param>
        /// <returns></returns>
        public static byte[] postdata(string postUrl, Byte[] paramData, Hashtable Header, Encoding dataEncode, string secretKey)
        {
            string reuslt = "";
            byte[] backstr = null;
            try
            {
                byte[] byteArray = paramData;
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;
                if (Header != null)
                {
                    foreach (DictionaryEntry de in Header)
                    {
                        webReq.Headers.Add(de.Key.ToString(), de.Value.ToString());
                    }
                }
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                if (response != null)
                {
                    Stream stream = response.GetResponseStream();
                    string code = response.GetResponseHeader("result").ToString();
                    if (code == "SUCCESS")
                    {
                        if (stream.CanRead)
                        {
                            //将基础流写入内存流
                            MemoryStream memoryStream = new MemoryStream();
                            const int bufferLength = 1024;
                            int actual;
                            byte[] buffer = new byte[bufferLength];
                            while ((actual = stream.Read(buffer, 0, bufferLength)) > 0)
                            {
                                memoryStream.Write(buffer, 0, actual);
                            }
                            memoryStream.Position = 0;
                            backstr = StreamToBytes(memoryStream);
                            memoryStream.Close();
                        }
                        stream.Close();
                    }
                    else
                    {
                        backstr = AESHelper.AESEncrypt(code, secretKey);
                    }
                }
                response.Close();
                newStream.Close();
                if (backstr != null)
                    return backstr;
            }
            catch (Exception e)
            {
                reuslt = e.ToString();
                //错误日志
            }
            return null;
        }

        /// <summary>
        /// post方法
        /// </summary>
        /// <param name="postUrl">地址</param>
        /// <param name="paramData">参数</param>
        /// <param name="dataEncode">编码</param>
        /// <returns></returns>
        public static string postdata(string postUrl, Dictionary<string, string> paramData, Encoding dataEncode)
        {
            string reuslt = "";
            try
            {
                StringBuilder builder = new StringBuilder();
                int i = 0;
                foreach (var item in paramData)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
                byte[] byteArray = Encoding.UTF8.GetBytes(builder.ToString());
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                reuslt = sr.ReadToEnd();
                sr.Close();
                response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                reuslt = "-1";
                //错误日志
            }
            return reuslt;
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
    }
}
