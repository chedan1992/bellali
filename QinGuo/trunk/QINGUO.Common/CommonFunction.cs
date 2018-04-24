using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace QINGUO.Common
{
    /// <summary>
    /// 公共方法类
    /// </summary>
    public class CommonFunction
    {

        #region ==DataSet转化为list对象
        /// <summary>
        /// 表格转化为实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSet"></param>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        public static List<T> DataSetToList<T>(DataSet dataSet, int tableIndex)
        {
            var list = new List<T>();
            if (dataSet == null || dataSet.Tables.Count <= 0 || tableIndex < 0 || dataSet.Tables.Count - 1 < tableIndex)
                return null;

            var dt = dataSet.Tables[tableIndex];

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                var t = Activator.CreateInstance<T>();
                //获取对象所有属性
                PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (var info in propertyInfo)
                    {
                        //属性名称和列名相同时赋值
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                if (dt.Rows[i][j].GetType() == typeof(string))
                                {
                                    info.SetValue(t, dt.Rows[i][j].ToString(), null);
                                }
                                else if (dt.Rows[i][j].GetType() == typeof(int))
                                {
                                    info.SetValue(t, int.Parse(dt.Rows[i][j].ToString()), null);
                                }
                                else if (dt.Rows[i][j].GetType() == typeof(DateTime))
                                {
                                    info.SetValue(t, DateTime.Parse(dt.Rows[i][j].ToString()), null);
                                }
                                else if (dt.Rows[i][j].GetType() == typeof(float))
                                {
                                    info.SetValue(t, float.Parse(dt.Rows[i][j].ToString()), null);
                                }
                                else if (dt.Rows[i][j].GetType() == typeof(double))
                                {
                                    info.SetValue(t, double.Parse(dt.Rows[i][j].ToString()), null);
                                }
                                else if (dt.Rows[i][j].GetType() == typeof(decimal))
                                {
                                    info.SetValue(t, decimal.Parse(dt.Rows[i][j].ToString()), null);
                                }
                                else if (dt.Rows[i][j].GetType() == typeof(bool))
                                {
                                    info.SetValue(t, Convert.ToBoolean(dt.Rows[i][j].ToString()), null);
                                }
                                else
                                {
                                    info.SetValue(t, Convert.ChangeType(dt.Columns[j], info.PropertyType), null);
                                }
                            }
                            else
                            {
                                info.SetValue(t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }
        #endregion

        #region ==DataRow转化为model对象
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T DataRowToList<T>(DataRow dr) where T : new()
        {
            var t = new T();
            if (dr != null)
            {
                var pis = t.GetType().GetProperties();
                var columeNameMapping = dr.Table.Columns.Cast<DataColumn>().ToDictionary(column => column.ColumnName.ToLower(), column => column.ColumnName);

                foreach (var pi in pis)
                {
                    string propertiesName = pi.Name.ToLower();
                    if (columeNameMapping.ContainsKey(propertiesName))
                    {
                        object value = dr[columeNameMapping[propertiesName]];
                        if (value != DBNull.Value)
                        {
                            if (value.GetType() == pi.PropertyType)
                            {
                                pi.SetValue(t, value, null);
                            }
                            else
                            {
                                pi.SetValue(t, Convert.ChangeType(dr[columeNameMapping[propertiesName]], pi.PropertyType), null);
                            }
                        }
                        else
                        {
                            pi.SetValue(t, null, null);
                        }
                    }

                }
                return t;
            }
            else
            {
                return default(T);
            }

        }
        #endregion

        #region 通过反射获取对象属性值
        /// <summary>
        /// 通过反射获取对象属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            var type = typeof(T);
            var property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            var o = property.GetValue(t, null);
            return o == null ? string.Empty : o.ToString();
        }

        public static void SetObjectPropertyValue<T>(T t, string propertyname, string value)
        {
            try
            {
                var type = typeof(T);
                var property = type.GetProperty(propertyname);
                if (property == null) return;
                property.SetValue(t, value, null);
            }
            catch
            {
                ;
            }
        }
        #endregion

        #region ===随机生成数字文件名
        /// <summary>
        /// 随机生成数字文件名
        /// </summary>
        /// <returns></returns>
        public static string MakeFileRndName()
        {
            return (DateTime.Now.ToString("yyyyMMddHHmmss") + MakeRandomString("0123456789", 4));
        }

        private static string MakeRandomString(string pwdchars, int pwdlen)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                int num = random.Next(pwdchars.Length);
                builder.Append(pwdchars[num]);
            }
            return builder.ToString();
        }

        #endregion

        #region  ====加密解密
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string Decrypt(string decryptString)
        {
            return new DEncryptHelp().DecryptString(decryptString);
            //return SecurityData.Decrypt(decryptString);
            // string temp = decryptString.Trim();

            //string result = MyEntry(temp);
            // return result;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string Encrypt(string encryptString)
        {
            return new DEncryptHelp().EncryptString(encryptString);
            //return SecurityData.HA_Encrypt(encryptString);
            //return MyEntry(encryptString);
        }











        #endregion

        /// <summary>
        /// 生成6位数字验证码
        /// </summary>
        /// <returns></returns>
        public static string GetValidateCode()
        {
            var vcode = "";
            var r = new Random();
            for (var i = 0; i < 6; i++)
            {
                vcode += Convert.ToString(r.Next(0, 10));
            }
            return vcode;
        }

        /// <summary>
        /// 获取网站页面内容
        /// </summary>
        /// <param name="sUrl"></param>
        /// <returns></returns>
        public static string GetWebContent(string sUrl)
        {
            string strResult = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                //声明一个HttpWebRequest请求
                request.Timeout = 3000000;
                //设置连接超时时间
                request.Headers.Set("Pragma", "no-cache");
                var response = (HttpWebResponse)request.GetResponse();
                if (response.ToString() != "")
                {
                    var streamReceive = response.GetResponseStream();
                    var encoding = Encoding.GetEncoding("UTF-8");
                    if (streamReceive != null)
                    {
                        var streamReader = new StreamReader(streamReceive, encoding);
                        strResult = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception exp)
            {
                strResult = "";
            }
            return strResult;
        }


        public static string MyEntry(string orginString)
        {
            StringBuilder sb = new StringBuilder();

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(orginString));

            for (int i = 0; i < s.Length; i++)
            {

                sb.Append(s[i].ToString("X2"));

            }

            string md5Str = sb.ToString();//md5Str就是最后得到加密后的字符串 
            return md5Str;
        }

        /// <summary>
        /// 树形组织生成
        /// </summary>
        /// <param name="nds">树形字符串</param>
        /// <param name="parentId">上级编号</param>
        /// <param name="bl">是否顶级</param>
        /// <param name="_treeDataTable">数据</param>
        /// <returns>树形字符串</returns>
        public static string BindTree(string nds, string parentId, bool bl, DataTable _treeDataTable)
        {
            string runStr = nds;
            if (_treeDataTable != null)
            {
                foreach (DataRow dr in _treeDataTable.Select(" ParentCategoryId ='" + parentId + "'"))
                {
                    if (_treeDataTable.Select(" ParentCategoryId ='" + dr["id"].ToString() + "'").Count() > 0)
                    {
                        runStr += @" {id:'" + dr["id"].ToString() + "',pId:'" + parentId + "',name:\"" + dr["Name"] + "\",open:true, nocheck:true},";
                    }
                    else
                    {
                        runStr += @" {id:'" + dr["id"].ToString() + "',pId:'" + parentId + "',name:\"" + dr["Name"] + "\"},";
                    }

                    runStr = BindTree(runStr, dr["id"].ToString(), false, _treeDataTable);
                }
            }
            return runStr;
        }
    }
}
