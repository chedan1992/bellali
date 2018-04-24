using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Common
{
    public class SqlAttackHelper
    {
        #region SQL注入式攻击代码分析
        /**/
        /// <summary>
        /// 处理用户提交的请求
        /// </summary>
        public void StartProcessRequest()
        {
            try
            {
                string getkeys = "";
                string sqlErrorPage = "~/PublicPage/Error.htm";
                if (System.Web.HttpContext.Current.Request.QueryString != null)
                {

                    for (int i = 0; i < System.Web.HttpContext.Current.Request.QueryString.Count; i++)
                    {
                        getkeys = System.Web.HttpContext.Current.Request.QueryString.Keys[i];
                        if (!ProcessSqlStr(System.Web.HttpContext.Current.Request.QueryString[getkeys].ToLower()))
                        {
                            System.Web.HttpContext.Current.Response.Redirect(sqlErrorPage);
                            System.Web.HttpContext.Current.Response.End();
                        }
                    }
                }

                if (System.Web.HttpContext.Current.Request.Form != null)
                {
                    for (int i = 0; i < System.Web.HttpContext.Current.Request.Form.Count; i++)
                    {
                        getkeys = System.Web.HttpContext.Current.Request.Form.Keys[i];
                        if (!ProcessSqlStrA(System.Web.HttpContext.Current.Request.Form[getkeys].ToLower()))
                        {
                            System.Web.HttpContext.Current.Response.Redirect(sqlErrorPage);
                            System.Web.HttpContext.Current.Response.End();
                        }
                    }
                }

            }
            catch
            {
                // 错误处理: 处理用户提交信息!
            }
        }
        /**/
        /// <summary>
        /// 分析用户请求是否正常
        /// </summary>
        /// <param name="Str">传入用户提交数据</param>
        /// <returns>返回是否含有SQL注入式攻击代码</returns>
        private bool ProcessSqlStr(string Str)
        {
            bool ReturnValue = true;
            try
            {
                if (Str != "" && Str != null)
                {
                    string SqlStr = "";
                    if (SqlStr == "" || SqlStr == null)
                    {
                        SqlStr = "'|and|exec|insert|select|delete|update|count|mid|master|truncate|char|declare";
                    }
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.IndexOf(ss) >= 0)
                        {
                            ReturnValue = false;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }
        /**/
        /// <summary>
        /// 分析用户请求是否正常
        /// </summary>
        /// <param name="Str">传入用户提交数据</param>
        /// <returns>返回是否含有SQL注入式攻击代码</returns>
        private bool ProcessSqlStrA(string Str)
        {
            bool ReturnValue = true;
            try
            {
                if (Str != "" && Str != null)
                {
                    string SqlStr = "";
                    if (SqlStr == "" || SqlStr == null)
                    {
                        SqlStr = "'|and|exec|insert|select|delete|update|count|*|chr|mid|master|truncate|char|declare";
                    }
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (ss != "")
                        {
                            if (Str.IndexOf(ss) >= 0)
                            {
                                ReturnValue = false;
                            }
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }
        #endregion       
    }
}
