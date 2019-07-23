using Common;
using Microsoft.AspNetCore.Hosting;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Commonlication
{
    public class Common
    {
        public static string ReadStream2String(Stream stream)
        {
            if (null == stream)
            {
                return string.Empty;
            }
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static Dictionary<string, string> Xml2Dict(string xml)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement root = doc.DocumentElement;
            foreach (XmlNode node in root.ChildNodes)
            {
                result.Add(node.Name, node.InnerText);
            }
            return result;
        }
        public static string GetTimeStamp()
        {

            #region linux系统时间转换
            /*
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return Convert.ToInt64(ts.TotalSeconds * 1000); 
             * */
            #endregion
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }

        public static string Valid(string token, string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature(token, signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echostr))
                {
                    return echostr;
                }
            }

            return "";
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        public static bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp); //字典排序
            string tmpStr = string.Join("", ArrTmp);

            var sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(tmpStr));
            tmpStr = BitConverter.ToString(hash).Replace("-", "").ToLower();

            Log.WriteLog(signature);
            Log.WriteLog(tmpStr);
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static HttpResponseMessage ToHttpMsgForWeChat(string strMsg)
        {
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(strMsg, Encoding.GetEncoding("UTF-8"), "application/x-www-form-urlencoded") };
            return result;
        }

        public static byte[] Output(DataTable dataTable, string[] tableTitle)
        {
            NPOI.SS.UserModel.IWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("sheet");
            IRow Title = null;
            IRow rows = null;
            for (int i = 1; i <= dataTable.Rows.Count; i++)
            {
                //创建表头
                if (i - 1 == 0)
                {
                    Title = sheet.CreateRow(0);
                    for (int k = 1; k < tableTitle.Length + 1; k++)
                    {
                        Title.CreateCell(0).SetCellValue("序号");
                        Title.CreateCell(k).SetCellValue(tableTitle[k - 1]);
                    }
                    continue;
                }
                else
                {
                    rows = sheet.CreateRow(i - 1);
                    for (int j = 1; j <= dataTable.Columns.Count; j++)
                    {
                        rows.CreateCell(0).SetCellValue(i - 1);
                        rows.CreateCell(j).SetCellValue(dataTable.Rows[i - 1][j - 1].ToString());
                    }
                }
            }

            byte[] buffer = new byte[1024 * 5];
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.GetBuffer();
                ms.Close();
            }
            return buffer;
        }
        public static byte[] OutputExcel(List<Order> entitys, List<string> title, string webRootPath)
        {
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet");
            IRow Title = null;
            IRow rows = null;
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            for (int i = 0; i <= entitys.Count; i++)
            {
                if (i == 0)
                {
                    Title = sheet.CreateRow(0);
                    for (int k = 1; k < title.Count + 1; k++)
                    {
                        Title.CreateCell(0).SetCellValue("序号");
                        Title.CreateCell(k).SetCellValue(title[k - 1]);
                    }
                    continue;
                }
                else
                {
                    rows = sheet.CreateRow(i);
                    rows.Height = 60 * 20;
                    Order entity = (Order)entitys[i - 1];

                    rows.CreateCell(0).SetCellValue(i);

                    rows.CreateCell(1).SetCellValue(entity.Payno);
                    rows.CreateCell(2).SetCellValue(entity.RealName);
                    rows.CreateCell(3).SetCellValue(entity.Sex);
                    rows.CreateCell(4).SetCellValue(entity.Phone);
                    rows.CreateCell(5).SetCellValue(entity.Nat);
                    rows.CreateCell(6).SetCellValue(entity.Natv);
                    rows.CreateCell(7).SetCellValue(entity.SchoolName);
                    rows.CreateCell(8).SetCellValue(entity.Card);
                    rows.CreateCell(9).SetCellValue(entity.School);
                    rows.CreateCell(10).SetCellValue(entity.Grade);
                    rows.CreateCell(11).SetCellValue(entity.Money.ToString());
                    //rows.CreateCell(12).SetCellValue(entity.Czimg);
                    if (entity.PayTime != null)
                        rows.CreateCell(13).SetCellValue(entity.PayTime.ToString());

                    HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                    //HSSFPatriarch patriarch = new HSSFPatriarch.CreatePatriarch(sheet, EscherAggregate.); //(HSSFPatriarch)sheet.CreateDrawingPatriarch();
                    setPic(workbook, patriarch, entity.Czimg, sheet, i, 12, webRootPath);
                }
            }

            byte[] buffer = new byte[1024 * 100];
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.GetBuffer();
                ms.Close();
            }
            return buffer;
        }

        private static void setPic(IWorkbook workbook, HSSFPatriarch patriarch, string path, ISheet sheet, int rowline, int col, string webRootPath)
        {
            if (string.IsNullOrEmpty(path)) return;
            byte[] bytes = System.IO.File.ReadAllBytes(webRootPath + path);
            int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);
            // 插图片的位置  HSSFClientAnchor（dx1,dy1,dx2,dy2,col1,row1,col2,row2) 后面再作解释
            HSSFClientAnchor anchor = new HSSFClientAnchor(70, 10, 0, 0, col, rowline, col + 1, rowline + 1);
            //把图片插到相应的位置
            HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
        }
    }
}