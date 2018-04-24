using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace QINGUO.Common
{
    /// <summary>
    /// html操作类
    /// </summary>
    public class Html
    {
        /// <summary>
        /// 除去html标签
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public string StripHtml(string strHtml)
        {
            string[] aryReg ={ 
                      @"<script[^>]*?>.*?</script>", 
                      @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>", 
                      @"([\r\n])[\s]+", 
                      @"&(quot|#34);", 
                      @"&(amp|#38);", 
                      @"&(lt|#60);", 
                      @"&(gt|#62);",    
                      @"&(nbsp|#160);",    
                      @"&(iexcl|#161);", 
                      @"&(cent|#162);", 
                      @"&(pound|#163);", 
                      @"&(copy|#169);", 
                      @"&#(\d+);", 
                      @"-->", 
                      @"<!--.*\n" 
                    };

            string[] aryRep =   { 
                        "", 
                        "", 
                        "", 
                        "\"", 
                        "&", 
                        "<", 
                        ">", 
                        "   ", 
                        "\xa1",//chr(161), 
                        "\xa2",//chr(162), 
                        "\xa3",//chr(163), 
                        "\xa9",//chr(169), 
                        "", 
                        "\r\n", 
                        "" 
                      };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");
            return strOutput;
        }

        //Get请求方式
        public string RequestGet(string Url)
        {
            string strResult = "";
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    HttpWebResponse HttpWResp = (HttpWebResponse)myRequest.GetResponse();
                    Stream myStream = HttpWResp.GetResponseStream();
                    StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
                    StringBuilder strBuilder = new StringBuilder();
                    while (-1 != sr.Peek())
                    {
                        strBuilder.Append(sr.ReadLine());
                    }
                    strResult = strBuilder.ToString();
                }
                catch (Exception exp)
                {
                    strResult = "错误：" + exp.Message;
                }
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
            }
            return strResult;
        }
    }
}
