using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace QINGUO.Common
{
    public class JsonHelper
    {
        public static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        #region===转换成json
        /// <summary>  
        /// 返回本对象的Json序列化  
        /// </summary>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static string ToJson(object obj)
        {
            string result = serializer.Serialize(obj);
            serializer.MaxJsonLength = Int32.MaxValue;
            result = Regex.Replace(result, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                //return dt.ToString("yyyy-MM-dd HH:mm:ss");
                return dt.ToString();
            });
            return result;
        }

        /// <summary>  
        /// 返回本对象的Json序列化  
        /// </summary>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static string ToJsonStr(object obj)
        {
            string result = serializer.Serialize(obj);
            return result;
        }


        public static string ToJsonForGridDataSuorce<T>(List<T> list)
        {
            return ToJsonForGridDataSuorce(list, list.Count);
        }

        public static string ToJsonForGridDataSuorce<T>(List<T> list, IEnumerable<string> listColumns) where T : new()
        {
            var jsonName = string.Format("\"total\":{0},\"rows\"", list.Count());
            var result = ToJson(list, listColumns);
            return "{" + jsonName + ":" + result + "}";
        }

        public static string ToJson<T>(IEnumerable<T> list, IEnumerable<string> listColumns) where T : new()
        {
            var toLowerColumnsList = listColumns.Select(item => item.ToLower()).ToList();
            var dic = new List<Dictionary<string, object>>();
            var t = new T();
            var pis = t.GetType().GetProperties();
            foreach (var item in list)
            {
                var result = new Dictionary<string, object>();
                foreach (var pi in pis)
                {
                    if (toLowerColumnsList.Contains(pi.Name.ToLower()))
                    {
                        var value = pi.GetValue(item, null);

                        if (pi.PropertyType == typeof(DateTime))
                        {
                            result.Add(pi.Name, value.ToString());
                        }
                        else if (pi.PropertyType == typeof(decimal) || pi.PropertyType == typeof(float))
                        {
                            result.Add(pi.Name, Convert.ToDecimal(value).ToString("#0.00"));
                        }
                        else if (pi.PropertyType != typeof(string))
                        {
                            result.Add(pi.Name, value);
                        }
                        else
                        {
                            if (value != null && value != DBNull.Value)
                            {

                                result.Add(pi.Name, HtmlEncode(value.ToString()));
                            }
                            else
                            {
                                result.Add(pi.Name, "");
                            }
                        }
                    }
                }
                dic.Add(result);
            }
            return ToJson(dic);
        }

        public static string ToJsonForGridDataSuorce<T>(List<T> list, int recordCount)
        {
            string result = ToJson(list);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":" + recordCount);
            sb.Append(",\"rows\":" + result + "}");
            return sb.ToString();
        }

        public static string DataTableToJsonForGridDataSuorce(DataTable dt)
        {
            string jsonName = string.Format("\"total\":{0},\"rows\"", dt.Rows.Count);
            return DataTableToJson(jsonName, dt);
        }

        public static string DataTableToJsonForGridDataSuorce(DataTable dt, int recordCount)
        {
            string jsonName = string.Format("\"dataSum\":{0},\"total\":{1},\"nowdate\":\"{2}\",\"rows\"",50, recordCount, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return DataTableToJson(jsonName, dt);
        }

        public static string DataTableToJson(DataTable dt)
        {
            List<Dictionary<string, object>> dic = new List<Dictionary<string, object>>();
            try
            {
                foreach (DataRow drIn in dt.Rows)
                {
                    Dictionary<string, object> result = new Dictionary<string, object>();
                    //Hashtable ht = new Hashtable();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.DataType == typeof(DateTime))
                        {
                            if (!string.IsNullOrEmpty(drIn[dc.ColumnName].ToString()))
                            {
                                string currentDt = Convert.ToDateTime(drIn[dc.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss");
                                result.Add(dc.ColumnName, currentDt);
                            }
                        }
                        else if (dc.DataType.Name.ToLower() == "decimal" || dc.DataType.Name.ToLower() == "float")
                        {
                            result.Add(dc.ColumnName, Convert.ToDecimal(drIn[dc.ColumnName].ToString() == "" ? "0" : drIn[dc.ColumnName]).ToString("#0.00"));
                        }
                        else if (dc.DataType != typeof(string))
                        {
                            result.Add(dc.ColumnName, drIn[dc.ColumnName]);
                        }

                        else
                        {
                            if (drIn[dc.ColumnName] != null && drIn[dc.ColumnName] != DBNull.Value)
                            {
                                result.Add(dc.ColumnName, HtmlEncode(drIn[dc.ColumnName].ToString()));
                            }
                            else
                            {
                                result.Add(dc.ColumnName, "");
                            }
                        }
                    }
                    dic.Add(result);
                }
            }
            catch (Exception a)
            { 
                
            }
            return ToJson(dic);
        }

        public static string DataTableToJsonExt(DataTable dt)
        {
            List<Dictionary<string, object>> dic = new List<Dictionary<string, object>>();
            foreach (DataRow drIn in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                //Hashtable ht = new Hashtable();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(DateTime))
                    {
                        if (!string.IsNullOrEmpty(drIn[dc.ColumnName].ToString()))
                        {
                            string currentDt = Convert.ToDateTime(drIn[dc.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss");
                            result.Add(dc.ColumnName, currentDt);
                        }
                    }
                    else if (dc.DataType.Name.ToLower() == "decimal" || dc.DataType.Name.ToLower() == "float")
                    {
                        result.Add(dc.ColumnName, Convert.ToDecimal(drIn[dc.ColumnName]).ToString("#0.00"));
                    }
                    else if (dc.DataType != typeof(string))
                    {
                        result.Add(dc.ColumnName, drIn[dc.ColumnName]);
                    }

                    else
                    {
                        if (drIn[dc.ColumnName] != null && drIn[dc.ColumnName] != DBNull.Value)
                        {
                            result.Add(dc.ColumnName, drIn[dc.ColumnName].ToString());
                        }
                        else
                        {
                            result.Add(dc.ColumnName, "");
                        }
                    }
                }
                dic.Add(result);
            }
            return ToJson(dic);
        }

        public static string DataTableToJson(string jsonName, DataTable dt)
        {
            return "{" + jsonName + ":" + DataTableToJson(dt) + "}";
        }

        public static string HtmlEncode(string text)
        {
            return text.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        /// <summary>
        /// 转换为combox json格式
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="id">combox的 value</param>
        /// <param name="txtName">combox的 text</param>
        /// <param name="Reamrk">描述</param>
        /// <returns></returns>
        public static string DataTableToComboxJson(DataTable dt, string id, string txtName, string Reamrk)
        {
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                sb.Append("[");
                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("{\"id\":\"" + row[id] + "\",\"text\":\"" + row[txtName] + "\",\"desc\":\"" + Reamrk + "\"");
                    sb.Append("},");
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return sb.ToString();
        }

        #endregion

      

    }
}
