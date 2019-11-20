using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SIG.Infrastructure.Helper
{
    public class WeiXinHelper
    {
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public static string getAccessToken()
        {
            var appId = "XXXXX";
            var appSecret = "XXXXX";

            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appSecret;

            HttpWebRequest webrequest = (HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();//请求连接,并反回数据
            Stream stream = webresponse.GetResponseStream();//把返回数据转换成流文件

            byte[] rsByte = new Byte[webresponse.ContentLength];  //把流文件转换为字节数组

            try
            {
                stream.Read(rsByte, 0, (int)webresponse.ContentLength);
                string responseStr = System.Text.Encoding.Default.GetString(rsByte, 0, rsByte.Length).ToString().Replace("{", "").Replace("}", "");
                string[] jsons = responseStr.Split(',');
                if (jsons.Length == 2)
                {
                    string[] param = jsons[0].Split(':');
                    if (param.Length == 2 && param[0] == "\"access_token\"")
                    {
                        string tempAccessToken = param[1].Replace("\"", "");

                        return tempAccessToken;
                    }
                    else
                    {
                        return "error";
                    }
                }
                return "error";
            }
            catch
            {
                return "error";
            }
        }

        private static string GetJsapiTicket(string accessToken)
        {
            string interfaceUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accessToken + "&type=jsapi";

            HttpWebRequest webrequest = (HttpWebRequest)System.Net.HttpWebRequest.Create(interfaceUrl);
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();//请求连接,并反回数据
            Stream stream = webresponse.GetResponseStream();//把返回数据转换成流文件
            byte[] rsByte = new Byte[webresponse.ContentLength];  //把流文件转换为字节数组

            try
            {
                stream.Read(rsByte, 0, (int)webresponse.ContentLength);
                string strb = System.Text.Encoding.Default.GetString(rsByte, 0, rsByte.Length).ToString().Replace("{", "").Replace("}", "");

                if ((strb.ToString().IndexOf("\"errcode\":42001") != -1) || (strb.ToString().IndexOf("\"errcode\":40001") != -1) || (strb.ToString().IndexOf("\"errcode\":40014") != -1) || (strb.ToString().IndexOf("\"errcode\":41001") != -1))
                {
                    return "error";
                }
                else if (strb.ToString().IndexOf("\"errcode\":0,\"errmsg\":\"ok\"") != -1)
                {
                    string[] jsons = strb.Split(',');
                    if (jsons.Length == 4)
                    {
                        string[] param = jsons[2].Split(':');
                        if (param.Length == 2 && param[0] == "\"ticket\"")
                        {
                            string tempJsapiTicket = param[1].Replace("\"", "");

                            return tempJsapiTicket;
                        }
                        else
                        {
                            return "error";
                        }
                    }
                    return "error";
                }
                else
                {
                    return "error";
                }
            }
            catch
            {
                return "error";
            }
        }
    }
}
