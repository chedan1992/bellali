using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Common;
using QINGUO.Business;
using QINGUO.Model;
using ThoughtWorks.QRCode.Codec;
using System.Configuration;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;

namespace WebPortalAdmin.Controllers
{
    /*
     * 中秋佳节之季,为回馈新老客户
     * 
     * 凡是在咱们团队定做APP、微信、网站系统及其他业务的同志，
     * 
     * 都可亲眼看我们经理胸口碎大石~~！！
     * 
     * 联系Q：1101350716
     * 
     * 开发人:pocket163
     * 
     */
    public class BaseController<T> : BlankBaseController
    {
        public BllSysOperateLog OperateLog = new BllSysOperateLog();

        public string Output; //输出到页面
        public DataSet _mySet;//DataSet变量
        public string _where = " 1=1 ";//过滤条件   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentMaster == null)
            {
                return;
            }
        }

        #region ===页面取值

        /// <summary>
        /// 获取字符串参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetRequestQueryString(string key)
        {
            if (!string.IsNullOrEmpty(Request[key]))
            {
                return Request[key].Trim();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取Int类型参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected int GetRequestQueryInt(string key)
        {
            return Convert.ToInt32(GetRequestQueryString(key));
        }

        /// <summary>
        /// 获取DateTime类型参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected DateTime GetRequestQueryDateTime(string key)
        {
            return Convert.ToDateTime(GetRequestQueryString(key));
        }

        /// <summary>
        /// 是否修改
        /// </summary>
        protected bool IsModify
        {
            get
            {
                if (GetRequestQueryString("modify").Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取查询列表
        /// </summary>
        /// <returns></returns>
        protected Search GetSearch()
        {
            //获取用户查看类型
            string className = GetRequestQueryString("className");//页面类名
            if (!String.IsNullOrEmpty(className))
            {
                if (!String.IsNullOrEmpty(CurrentMaster.RoleIdList))
                {
                    CurrentMaster.LookPower = new BllSysMaster().GetLookPower(className, CurrentMaster.RoleIdList, CurrentMaster.Attribute);
                }
            }
            else
            {
                CurrentMaster.LookPower = 0;//不需要控权
            }


            string conditionField = GetRequestQueryString("conditionField");//查询字段
            string condition = GetRequestQueryString("condition");//查询比较符：包含、等于、大于、小于、不等于
            string conditionValue = GetRequestQueryString("conditionValue");//条件值

            string sortName = GetRequestQueryString("sort"); //排序字段名
            string order = GetRequestQueryString("dir");//排序方向，只有 DESC和ASC

            int start = int.Parse(Request["start"] == null ? "0" : Request["start"]);
            var _pageSize = int.Parse(Request["limit"] == null ? "20" : Request["limit"]);
            var _currentPage = start / _pageSize + 1;

            Search search = new Search
            {
                PageSize = _pageSize,
                CurrentPageIndex = _currentPage//当前页数
            };

            //排序处理
            if (sortName.Length > 0)
            {
                search.SortField = string.Format("{0} {1}", sortName, order);
            }
            else
            {
                
            }
            search.SortField = "IsTop desc,Sort ASC,CreateTime desc";//默认排序方式

            //条件处理
            if (conditionField.Length > 0 && conditionValue.Length > 0)
            {
                search.AddCondition(conditionField, condition, conditionValue);
            }

            //表名及主键处理
            EntityAttribute entityAtt = ReferanceAttribute.ReferanceEntityAttribute(typeof(T));
            if (entityAtt != null)
            {
                search.TableName = entityAtt.TableName;
                search.KeyFiled = entityAtt.PrimaryKey;
                search.StatusDefaultCondition = string.Format("{0}.Status <> -1", entityAtt.TableName);
            }

            return search;
        }
        #endregion


        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="jsonResult">信息</param>
        protected void WriteJsonToPage(string jsonResult)
        {
            if (jsonResult == "")
            {
                jsonResult = "{{'msg':'','success':false}}";
            }
            Response.Write(jsonResult);
            Response.End();
        }



        #region  ===获取客户端ip地址 IPAddressAll
        /// <summary>
        /// 获取客户端ip地址
        /// </summary>
        /// <returns></returns>
        public string IPAddressAll()
        {
            string result = String.Empty;
            try
            {
                result = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = Request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(result))
                {
                    return "127.0.0.1";
                }
            }
            catch
            {
                return "127.0.0.1";
            }
            return result;
        }
        #endregion


        #region ===添加日志操作
        /// <summary>
        /// 写入作业日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ColumnName"></param>
        /// <param name="Remark"></param>
        public void LogInsert(OperationTypeEnum type, string ColumnName, string Remark)
        {
            ModSysOperateLog t = new ModSysOperateLog();

            if (type == OperationTypeEnum.访问)
            {
                t.LogType ="0";
            }
            if (type == OperationTypeEnum.操作)
            {
                t.LogType = "1";
            }
            if (type == OperationTypeEnum.异常)
            {
                t.LogType = "2";
            }
            t.Id = Guid.NewGuid().ToString();
            t.UserName = CurrentMaster.UserName;
            t.LinkUrl = Request.Url.ToString();
            t.Remark = Remark;//备注
            t.Status = 1;//状态
            t.CreaterId = CurrentMaster.Id;
            t.IPAddress = IPAddressAll();
            t.ColumnName = ColumnName;
            t.CreateTime = DateTime.Now;
            ThreadPool.QueueUserWorkItem(new WaitCallback(WriteLogUsu), t);//放入异步执行
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="obSysLog"></param>
        private void WriteLogUsu(object obSysLog)
        {
            ModSysOperateLog VSysLog = (ModSysOperateLog)obSysLog;
            new BllSysOperateLog().Insert(VSysLog);
        }
        #endregion


        #region ===二维码生成 QrCode
        /// <summary>
        /// 二维码生成
        /// </summary>
        public string QrCode(string PrimaryKey)
        {
            try
            {
               
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                int scale = Convert.ToInt16(4);
                qrCodeEncoder.QRCodeScale = scale;
                qrCodeEncoder.QRCodeVersion = 5;
                qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
                System.Drawing.Image myimg = qrCodeEncoder.Encode("XF" + PrimaryKey, System.Text.Encoding.UTF8);
                string rootQROrderPath = ConfigurationManager.AppSettings["QROrder"] ?? "~/UploadFile/QRCode/";
                string path = rootQROrderPath + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                if (!string.IsNullOrEmpty(Server.MapPath(path)))
                {
                    if (!System.IO.File.Exists(Server.MapPath(path)))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(path));
                    }
                }
                path = path + PrimaryKey + ".png";//组成图片名称
                using (Bitmap bitmap = new Bitmap(250, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.FillRectangle(new SolidBrush(System.Drawing.Color.White), new Rectangle(0, 0, 250, 250));
                        g.DrawImage(myimg, new Rectangle(10, 10, 230, 230));
                        bitmap.Save(Server.MapPath(path), System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                path = path.Substring(1, path.Length - 1);
                return path;
            }
            catch (Exception ex)
            {
                LogErrorRecord.Debug("二维码生成错误" + ex.Message);
                return "";
            }
        }

        #endregion
    }
}
