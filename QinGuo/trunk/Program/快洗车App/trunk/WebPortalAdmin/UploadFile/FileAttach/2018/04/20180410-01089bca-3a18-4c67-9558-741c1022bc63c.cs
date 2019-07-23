using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Common;
using DAL;
using Model;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Parameter;
using UFIDA.U8.U8MOMAPIFramework;

namespace Web.Service
{
    /// <summary>
    /// U8Service 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    //采购入库  新建+审   材料出库(红蓝字) 新建+审  产成品入库 新建+审 销售出库新建+审
    // [System.Web.Script.Services.ScriptService]
    public class U8Service : System.Web.Services.WebService
    {

        #region ===公共方法

        /// <summary>
        /// 模拟登录用户对象
        /// </summary>
        /// <returns></returns>
        public ViewLogin LoginView()
        {
            ViewLogin model = new ViewLogin();
            //model.sAccID = "[" + ConfigurationManager.AppSettings["SetBook"] + "]" + "(default)"; //套帐号(>10.0)
            model.sAccID = ConfigurationManager.AppSettings["SetBook"];//套帐号(v8.9)
            model.sYear = ConfigurationManager.AppSettings["Year"];   //年度
            model.sUserID = ConfigurationManager.AppSettings["LoginName"];//登录帐号
            model.sPassword = ConfigurationManager.AppSettings["LoginPwd"];//登录密码
            model.sServer = ConfigurationManager.AppSettings["sServer"];//U8服务器地址
            return model;
        }

        #region===模拟登录 Login
        /// <summary>
        /// 模拟登录
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "判断系统是否可以登录系统-模拟登录(Login)")]
        public string Login()
        {
            var msg = new ModJsonResult();
            try
            {
                //模拟登录用户
                ViewLogin login = LoginView();
                U8Login.clsLogin u8Login = new U8Login.clsLogin();
                String sSubId = "AS";//模块ID,
                String sAccID = login.sAccID;//账套号(你选择登录的账套号)
                String sYear = login.sYear;//选择的年份(截取选择登录日期的年份)
                String sUserID = login.sUserID;//用户名
                String sPassword = login.sPassword;//密码
                String sDate = DateTime.Now.ToShortDateString();//登录日期
                String sServer = login.sServer;//U8服务器地址
                String sSerial = "";//没有用到
                if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
                {
                    msg.success = false;
                    msg.msg = "登陆失败，原因：" + u8Login.ShareString;

                }
                else
                {
                    msg.success = true;
                    msg.msg = "登陆成功";
                }
                Marshal.FinalReleaseComObject(u8Login);//注销登录
            }
            catch (Exception e)
            {
                msg.success = false;
                msg.msg = "登录失败," + e.Message;
            }
            return msg.ToString();
        }

        #endregion

        #region ===返回用户登录对象 callLogin
        /// <summary>
        /// 返回用户登录对象
        /// </summary>
        /// <returns></returns>
        public ModJsonResult callLogin(ref U8Login.clsLogin loginInfo)
        {
            ViewLogin login = LoginView();
            var msg = new ModJsonResult();

            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            String sSubId = "AS";//模块ID,
            String sAccID = login.sAccID;//账套号(你选择登录的账套号)
            String sYear = login.sYear;//选择的年份(截取选择登录日期的年份)
            String sUserID = login.sUserID;//用户名
            String sPassword = login.sPassword;//密码
            String sDate = DateTime.Now.ToShortDateString();//登录日期
            String sServer = login.sServer;//U8服务器地址
            String sSerial = "";//没有用到
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {
                msg.success = false;
                msg.msg = "登陆失败，原因：" + u8Login.ShareString;
                Marshal.FinalReleaseComObject(u8Login);//注销登录
            }
            loginInfo = u8Login;
            return msg;
        }
        #endregion

        #region ===获取单号
        /// <summary>
        /// 最大单据号自动+1
        /// </summary>
        /// <param name="vouchType">单据类型</param>
        /// <param name="sqlConnection"></param>
        /// <param name="sqlTransaction"></param>
        public void UpdateMaxNumber(String vouchType)
        {
            String command = "select cNumber as Maxnumber From VoucherHistory with (ROWLOCK) Where CardNumber='"
                + vouchType + "' and cContent is NULL";
            DataTable numberDataTable = SqlHelper.ExcuteDataSet(command).Tables[0];
            String maxNumber = numberDataTable.Rows[0]["Maxnumber"].ToString();
            numberDataTable.Dispose();
            Int32 maxNumberInt = Int32.Parse(maxNumber) + 1;
            command = "update VoucherHistory set cNumber='" + maxNumberInt + "' Where CardNumber='" + vouchType + "' and cContent is NULL";
            SqlHelper.ExecuteNonQuery(command);
        }
        #endregion

        #region ===通用审核方法 Audit
        /// <summary>
        /// 通用审核方法
        /// </summary>
        /// <param name="key">单据编号</param>
        /// <param name="type">方法类型</param>
        /// <returns></returns>
        public ModJsonResult Audit(string key, string type, string url, string ufts)
        {
            var msg = new ModJsonResult();
            if (string.IsNullOrEmpty(key))
            {
                msg.success = false;
                msg.msg = "参数不能为空";
            }
            else
            {
                try
                {
                    //第一步：构造u8login对象并登陆(引用U8API类库中的Interop.U8Login.dll)
                    #region ===如果当前环境中有login对象则可以省去第一步
                    U8Login.clsLogin u8Login = new U8Login.clsLogin();
                    ModJsonResult IsLogin = callLogin(ref u8Login);
                    #endregion
                    if (IsLogin.success == false)
                    {
                        msg.success = false;
                        msg.msg = "登陆失败，原因：" + u8Login.ShareString;
                    }
                    else
                    {
                        //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                        U8EnvContext envContext = new U8EnvContext();
                        envContext.U8Login = u8Login;
                        //第三步：设置API地址标识(Url)
                        //当前API：审核单据的地址标识为：U8API/PuStoreIn/Audit
                        U8ApiAddress myApiAddress = new U8ApiAddress(url);
                        //第四步：构造APIBroker
                        U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);
                        //第五步：API参数赋值
                        //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：11
                        broker.AssignNormalValue("sVouchType", type);
                        //给普通参数VouchId赋值。此参数的数据类型为System.String，此参数按值传递，表示单据Id
                        broker.AssignNormalValue("VouchId", key);
                        //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值
                        //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象：调用方控制事务时需要传入连接对象
                        //broker.AssignNormalValue("cnnFrom", new ADODB.Connection());
                        //给普通参数TimeStamp赋值。此参数的数据类型为System.Object，此参数按值传递，表示单据时间戳，用于检查单据是否修改，空串时不检查

                        broker.AssignNormalValue("TimeStamp", ufts);//加上这个报错

                        //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                        //MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.IXMLDOMDocument2();
                        MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
                        broker.AssignNormalValue("domMsg", domMsg);
                        //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量
                        broker.AssignNormalValue("bCheck", false);
                        //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
                        broker.AssignNormalValue("bBeforCheckStock", false);
                        //给普通参数bList赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示传入空串
                        broker.AssignNormalValue("bList", false);
                        //给普通参数MakeWheres赋值。此参数的数据类型为VBA.Collection，此参数按值传递，表示传空
                        //broker.AssignNormalValue("MakeWheres", "");
                        //给普通参数sWebXml赋值。此参数的数据类型为System.String，此参数按值传递，表示传入空串
                        broker.AssignNormalValue("sWebXml", "");
                        //给普通参数oGenVouchIds赋值。此参数的数据类型为Scripting.IDictionary，此参数按值传递，表示返回审核时自动生成的单据的id列表,传空
                        //broker.AssignNormalValue("oGenVouchIds", "");
                        //第六步：调用API
                        if (!broker.Invoke())
                        {
                            //错误处理
                            Exception apiEx = broker.GetException();
                            if (apiEx != null)
                            {
                                if (apiEx is MomSysException)
                                {
                                    MomSysException sysEx = apiEx as MomSysException;
                                    msg.success = false;
                                    msg.msg = "系统异常：" + sysEx.Message;
                                }
                                else if (apiEx is MomBizException)
                                {
                                    MomBizException bizEx = apiEx as MomBizException;
                                    msg.success = false;
                                    msg.msg = "API异常：" + bizEx.Message;
                                }
                                //异常原因
                                String exReason = broker.GetExceptionString();
                                if (exReason.Length != 0)
                                {
                                    msg.success = false;
                                    msg.msg = "异常原因：" + exReason;
                                }
                            }
                        }
                        System.Boolean result = Convert.ToBoolean(broker.GetReturnValue());
                        if (result == false)
                        {
                            msg.success = false;
                            //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                            System.String errMsgRet = broker.GetResult("errMsg") as System.String;
                            if (!string.IsNullOrEmpty(errMsgRet))
                            {
                                msg.msg = errMsgRet;
                            }
                        }
                        else
                        {
                            //获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
                            MSXML2.IXMLDOMDocument2 domMsgRet = (MSXML2.IXMLDOMDocument2)(broker.GetResult("domMsg"));
                            //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));
                        }
                        //结束本次调用，释放API资源
                        broker.Release();
                    }
                }
                catch (Exception e)
                {
                    msg.success = false;
                    msg.msg = "操作失败:" + e.Message;
                }
            }
            return msg;
        }
        #endregion

        #region ===通用获取时间戳
        /// <summary>
        /// 通用获取时间戳
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string GetUfts(string key, string table, string columnName)
        {
            /*select BTQName,BWQName ,* from vouchers*/
            string sql = "select * from " + table + " where ID=" + key;
            DataSet ds = SqlHelper.ExcuteDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string tmpufts = ds.Tables[0].Rows[0][columnName].ToString();
                return tmpufts;
            }
            return "";
        }
        #endregion


        ///// <summary>
        ///// 获取表单ID
        ///// </summary>
        ///// <param name="cAcc_Id">帐套号</param>
        ///// <param name="iAmount">间隔数</param>
        ///// <param name="cVouchType">表单名称</param>
        ///// <returns></returns>
        //public static string[] UA_Identity(string cAcc_Id, int iAmount, string cVouchType)
        //{
        //    string[] iFiC = new string[2];
        //    // string iFatherId, iChildId;
        //    //1、获取ID

        //    //获取送货单主表和子表ID
        //    SqlParameter[] pmsGetID = new SqlParameter[]{
        //                 new SqlParameter("@RemoteId",SqlDbType.NVarChar),
        //                 new SqlParameter("@cAcc_Id",SqlDbType.NVarChar),
        //                 new SqlParameter("@cVouchType",SqlDbType.NVarChar),
        //                 new SqlParameter("@iAmount",SqlDbType.Int),
        //                 new SqlParameter("@iFatherId",SqlDbType.Int),
        //                 new SqlParameter("@iChildId",SqlDbType.Int)
        //               };
        //    pmsGetID[0].Value = "00";
        //    pmsGetID[1].Value = cAcc_Id;
        //    pmsGetID[2].Value = cVouchType;
        //    //间隔ID数
        //    pmsGetID[3].Value = iAmount;
        //    pmsGetID[4].Value = 0;
        //    pmsGetID[5].Value = 0;
        //    pmsGetID[4].Direction = ParameterDirection.Output;
        //    pmsGetID[5].Direction = ParameterDirection.Output;
        //    SqlHelper.ExecuteNonQuery("sp_getID", CommandType.StoredProcedure, pmsGetID);

        //    iFiC[0] = pmsGetID[4].Value.ToString();
        //    iFiC[1] = pmsGetID[5].Value.ToString();

        //    return iFiC;
        //}

        /// <summary>
        /// 获取单据号
        /// </summary>
        /// <cardNumber>单据类型编码</cardNumber>
        /// <returns>返回单据号</returns>
        public string GetVoucherNumnber(string cardNumber)
        {
            string resultNumber = "";
            //VoucherNumber 单据编号规则设置表
            string sel_VoucherNumber = string.Format("select *  from VoucherNumber Where CardNumber='{0}'", cardNumber);
            DataTable dt_VoucherNumber = SqlHelper.ExcuteTable(sel_VoucherNumber, CommandType.Text);

            if (dt_VoucherNumber.Rows.Count == 1)
            {
                for (int i = 0; i < dt_VoucherNumber.Rows.Count; i++)
                {
                    //第一前缀是否为空,不为空时
                    if (!IsEmpty(dt_VoucherNumber.Rows[i]["Prefix1"].ToString().Trim()))
                    {
                        resultNumber += JudgeVoucherRule(dt_VoucherNumber, "Prefix1", dt_VoucherNumber.Rows[i]["Prefix1"].ToString().Trim());
                    }

                    //第二前缀不为空时
                    if (!IsEmpty(dt_VoucherNumber.Rows[i]["Prefix2"].ToString().Trim()))
                    {
                        resultNumber += JudgeVoucherRule(dt_VoucherNumber, "Prefix2", dt_VoucherNumber.Rows[i]["Prefix2"].ToString().Trim());
                    }

                    //第三前缀不为空时
                    if (!IsEmpty(dt_VoucherNumber.Rows[i]["Prefix3"].ToString().Trim()))
                    {
                        resultNumber += JudgeVoucherRule(dt_VoucherNumber, "Prefix3", dt_VoucherNumber.Rows[i]["Prefix3"].ToString().Trim());
                    }

                    //流水号依据不为空时
                    if (!IsEmpty(dt_VoucherNumber.Rows[i]["Glide"].ToString().Trim()))
                    {
                        int newcNumber = 0;

                        //流水依据
                        string glide = dt_VoucherNumber.Rows[i]["Glide"].ToString().Trim();

                        //流水规则
                        string glideRule = dt_VoucherNumber.Rows[i]["GlideRule"].ToString().Trim();
                        //流水号长度
                        int glideLen = int.Parse(dt_VoucherNumber.Rows[i]["GlideLen"].ToString().Trim());
                        //遍历列，获取流水依据对应的列
                        string prefixRule = "";
                        for (int k = dt_VoucherNumber.Columns.Count - 1; k > 0; k--)
                        {
                            if (glide == dt_VoucherNumber.Rows[i][k].ToString().Trim())
                            {
                                //获取列对应的规则
                                prefixRule = dt_VoucherNumber.Rows[i][k + 2].ToString().Trim();
                                break;
                            }
                        }

                        //获取日期规则，用于后面判断
                        string dataRule = GetDateRule(prefixRule, glideLen);

                        //VoucherHistory 单据流水号历史表    查询系统中是否有满足上面流规则的 记录
                        string sel_VoucherHistory = string.Format("select * from VoucherHistory where  AutoId = (select MAX(AutoId) from VoucherHistory where CardNumber ='{0}' and cContentRule='{1}' and cSeed='{2}')", cardNumber, glideRule, dataRule);
                        DataTable dt_VoucherHistory = SqlHelper.ExcuteTable(sel_VoucherHistory, CommandType.Text);
                        // 当表中有历史流水号时
                        if (dt_VoucherHistory.Rows.Count > 0)
                        {
                            //流水规则                           流水依据     历史流水号
                            string cContentRule = "", cSeed = ""; int autoId = 0, cNumber = 0;
                            for (int j = 0; j < dt_VoucherHistory.Rows.Count; j++)
                            {
                                cContentRule = dt_VoucherHistory.Rows[j]["cContentRule"].ToString();
                                cSeed = dt_VoucherHistory.Rows[j]["cSeed"].ToString();

                                //流水ID
                                autoId = int.Parse(dt_VoucherHistory.Rows[j]["autoId"].ToString());
                                //原流水号
                                cNumber = int.Parse(dt_VoucherHistory.Rows[j]["cNumber"].ToString());
                            }

                            //新号=老号+1
                            newcNumber = cNumber + 1;

                            //更新 单据流水号历史表
                            string up_VoucherHistory = string.Format("update  VoucherHistory set cNumber={0} where AutoId = {1}", newcNumber, autoId);
                            int upReslut = SqlHelper.ExecuteNonQuery(up_VoucherHistory);
                            if (upReslut > 0)
                            {
                            }
                        }
                        else
                        {
                            //当系统中不存在相同规流水号时，新增加一行
                            string in_VoucherHistory = string.Format("insert  into VoucherHistory (CardNumber,cContent,cContentRule,cSeed,cNumber)values('{0}','{1}','{2}','{3}','{4}')", cardNumber, glide, glideRule, dataRule, 1);
                            int inResult = SqlHelper.ExecuteNonQuery(in_VoucherHistory);
                            //增加成功时
                            if (inResult > 0)
                            {
                                //新流水号等于1
                                newcNumber = 1;
                            }
                        }

                        if (newcNumber.ToString().Length <= glideLen && newcNumber != 0)
                        {
                            resultNumber += newcNumber.ToString().PadLeft(glideLen, '0');
                        }
                        else
                        {
                            return "Error:无法获取字符串长度";
                        }
                    }
                }
            }

            return resultNumber;
        }

        /// <summary>
        /// 判断单据规则
        /// </summary>
        /// <param name="voucherRule">单据规则名称</param>
        /// <returns></returns>

        private string JudgeVoucherRule(DataTable dataTable, string prefixNumber, string voucherRule)
        {
            string strResult = "";

            if (voucherRule == "手工输入")
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    return strResult = dataTable.Rows[i][prefixNumber + "Rule"].ToString().Trim();
                }
            }
            if (voucherRule == "制单日期" || voucherRule == "单据日期" || voucherRule == "日期" || voucherRule == "年" || voucherRule == "年月" || voucherRule == "年月日")
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    //规则
                    string prefixRule = dataTable.Rows[i][prefixNumber + "Rule"].ToString().Trim();
                    //规则的长度
                    int prefixLength = int.Parse(dataTable.Rows[i][prefixNumber + "len"].ToString().Trim());

                    //获取当前日期

                    //规则 分为  年   年月  年月日
                    strResult = GetDateRule(prefixRule, prefixLength);
                }
            }

            return strResult;
        }

        /// <summary>
        /// 按日期规则返回日期
        /// </summary>
        /// <param name="prefixRule">日期规则，年，年月，年月日</param>
        /// <param name="prefixLength">长度</param>
        /// <returns></returns>
        private string GetDateRule(string prefixRule, int prefixLength)
        {
            string strResult = "";
            //规则 分为  年   年月  年月日
            switch (prefixRule)
            {
                case "年":
                    switch (prefixLength)  //年的长度有两种
                    {
                        case 2: //年的后两位
                            strResult = DateTime.Now.ToString("yy", DateTimeFormatInfo.InvariantInfo);
                            break;

                        case 4:  //完整的年份
                            strResult = DateTime.Now.ToString("yyyy", DateTimeFormatInfo.InvariantInfo);
                            break;
                    }
                    break;

                case "年月":
                    switch (prefixLength)  //年月的长度有两种
                    {
                        case 4: //年的后两位+月
                            strResult = DateTime.Now.ToString("yyMM", DateTimeFormatInfo.InvariantInfo);
                            break;

                        case 6://完整的年+月
                            strResult = DateTime.Now.ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);
                            break;
                    }
                    break;

                case "年月日":
                    switch (prefixLength)  //年月日的长度有两种
                    {
                        case 6: //年的后两位+月
                            strResult = DateTime.Now.ToString("yyMMdd", DateTimeFormatInfo.InvariantInfo);
                            break;

                        case 8://完整的年+月
                            strResult = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                            break;
                    }
                    break;
            }

            return strResult;
        }

        /// <summary>
        /// 判断值是否为空
        /// </summary>
        /// <param name="value">要判断的值</param>
        /// <returns>true 值为空，false 不为空</returns>
        public bool IsEmpty(string value)
        {
            if (value == "" || value == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region===[采购管理]采购入库单

        #region ===[采购管理]采购入库单-添加 InsertInOrder
        /// <summary>
        /// 采购入库 帐套：999
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "[采购管理]采购入库单-添加 InsertInOrder")]
        public string InsertInOrder(string record, string records)
        {
            var msg = new ModJsonResult();

            ////调用方法事例
            ////主表字段
            //V121RdRecord mainModel = new V121RdRecord();
            ////类型1，根据采购到货单添加
            //mainModel.cARVCode = "CQ-1801002075"; //采购到货单号(填了单号可以不用填到货单ID,二选1)
            //mainModel.ipurarriveid = 0;//采购到货单ID，string类型(填到货单ID可以不用填单号,二选1)

            //mainModel.dDate = DateTime.Now; //入库日期，DateTime类型(可选)
            //mainModel.cpersoncode = ""; //业务员编码，string类型(可选)
            //mainModel.cMemo = "Webservice生成";//备注信息(可选)
            //mainModel.cWhCode = "";//仓库编码  (可选)
            //mainModel.VT_ID = 27; //默认 模版号，int类型 (固定)
            //mainModel.cRdCode = "10";//入库类别编码 (固定)
            //mainModel.cBusType = "0";//默认 业务类型，int类型 0:普通采购  1直运采购,2受托代销
            //mainModel.cVouchType = "01";//默认 单据类型，string类型
            //mainModel.bIsRedVouch = false;//红蓝区分 false:蓝 true:红

            ////转换成json
            //record = "[" + JsonHelper.ToJson(mainModel) + "]";//转换成json

            ////子表字段
            //List<V121RdRecords> listDetail = new List<V121RdRecords>();
            //V121RdRecords detailModel = new V121RdRecords();
            //detailModel.AutoID =0;//主键ID(必传)
            //detailModel.cInvCode = "3030110002";//存货编码
            //detailModel.iQuantity = 1;//数量
            //detailModel.iUnitCost = 0; //本币单价，double类型
            //listDetail.Add(detailModel);

            //V121RdRecords detailModel2 = new V121RdRecords();
            //detailModel2.cInvCode = "3030110005";//存货编码
            //detailModel2.iQuantity = 5;//数量
            //detailModel2.iUnitCost = 0; //本币单价，double类型
            //listDetail.Add(detailModel2);

            ////转换成json
            //records = JsonHelper.ToJson(listDetail);//转换成json

            if (string.IsNullOrEmpty(record))
            {
                msg.success = false;
                msg.msg = "主单据参数不能为空";
            }
            else
            {
                try
                {
                    //第一步：构造u8login对象并登陆(引用U8API类库中的Interop.U8Login.dll)
                    #region ===如果当前环境中有login对象则可以省去第一步
                    U8Login.clsLogin u8Login = new U8Login.clsLogin();
                    ModJsonResult IsLogin = callLogin(ref u8Login);
                    #endregion
                    if (IsLogin.success == false)
                    {
                        msg.success = false;
                        msg.msg = "登陆失败，原因：" + u8Login.ShareString;
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        //主表
                        List<V121RdRecord> ListRdRecord = serializer.Deserialize<List<V121RdRecord>>(record).ToList();
                        //子表
                        List<V121RdRecords> ListRdRecords = serializer.Deserialize<List<V121RdRecords>>(records).ToList();
                        if (ListRdRecord.Count == 1)
                        {
                            int recordId = 0;
                            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                            U8EnvContext envContext = new U8EnvContext();
                            envContext.U8Login = u8Login;
                            //第三步：设置API地址标识(Url)
                            //当前API：添加新单据的地址标识为：U8API/PuStoreIn/Add
                            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/PuStoreIn/Add");
                            //第四步：构造APIBroker
                            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);
                            //第五步：API参数赋值
                            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：01
                            broker.AssignNormalValue("sVouchType", "01");
                            //给BO表头参数DomHead赋值，此BO参数的业务类型为采购入库单，属表头参数。BO参数均按引用传递
                            //提示：给BO表头参数DomHead赋值有两种方法
                            //方法一是直接传入MSXML2.DOMDocumentClass对象
                            //broker.AssignNormalValue("DomHead", new MSXML2.DOMDocumentClass());
                            //方法二是构造BusinessObject对象，具体方法如下：
                            BusinessObject DomHead = broker.GetBoParam("DomHead");
                            DomHead.RowCount = ListRdRecord.Count; //设置BO对象(表头)行数，只能为一行
                            //给BO对象(表头)的字段赋值，值可以是真实类型，也可以是无类型字符串
                            //以下代码示例只设置第一行值。各字段定义详见API服务接口定义
                            /****************************** 以下是必输字段 ****************************/
                            #region ===根据到货单编号获取到货单主键
                            U8EnvContext envContext1 = new U8EnvContext();
                            envContext1.U8Login = u8Login;

                            //设置上下文参数
                            envContext1.SetApiContext("VoucherType", 2); //上下文数据类型：int，含义：单据类型， 采购到货单 2 
                            envContext1.SetApiContext("bPositive", true); //上下文数据类型：bool，含义：红蓝标识：True,蓝字；False,红字
                            envContext1.SetApiContext("sBillType", "0"); //上下文数据类型：string，含义：到货单类型， 到货单 0 退货单 1
                            envContext1.SetApiContext("sBusType", Enum.GetName(typeof(EnumBusType), int.Parse(ListRdRecord[0].cBusType))); //上下文数据类型：string，含义：业务类型：普通采购,直运采购,受托代销
                            U8ApiAddress myApiAddress1 = new U8ApiAddress("U8API/ArrivedGoods/GetVoucherData");
                            U8ApiBroker broker1 = new U8ApiBroker(myApiAddress1, envContext1);
                            broker1.AssignNormalValue("strWhere", null);

                            if (string.IsNullOrEmpty(ListRdRecord[0].iarriveid))
                            {
                                //根据采购到货单号获取采购到货单ID
                                DataSet ArrivalVouch = SqlHelper.ExcuteDataSet("select ID from PU_ArrivalVouch(nolock) where cCode='" + ListRdRecord[0].cARVCode + "';");
                                ListRdRecord[0].iarriveid = ArrivalVouch.Tables[0].Rows[0]["ID"].ToString();
                            }
                            broker1.AssignNormalValue("varVoucherID", ListRdRecord[0].iarriveid);

                            broker1.AssignNormalValue("strLocateWhere", null);
                            //第六步：调用API
                            if (!broker1.Invoke())
                            {
                                //错误处理
                                Exception apiEx = broker1.GetException();
                                if (apiEx != null)
                                {
                                    if (apiEx is MomSysException)
                                    {
                                        MomSysException sysEx = apiEx as MomSysException;
                                        msg.success = false;
                                        msg.msg = "系统异常：" + sysEx.Message;
                                    }
                                    else if (apiEx is MomBizException)
                                    {
                                        MomBizException bizEx = apiEx as MomBizException;
                                        msg.success = false;
                                        msg.msg = "API异常：" + bizEx.Message;
                                    }
                                    //异常原因
                                    String exReason = broker1.GetExceptionString();
                                    if (exReason.Length != 0)
                                    {
                                        msg.success = false;
                                        msg.msg = "异常原因：" + exReason;
                                    }
                                }
                                //结束本次调用，释放API资源
                                broker1.Release();
                            }
                            #endregion
                            //获取返回值
                            //获取普通返回值。此参数的数据类型为System.String，此参数按引用传递，表示错误描述：空，正确；非空，错误
                            System.String GetResult = broker1.GetReturnValue() as System.String;

                            if (string.IsNullOrEmpty(GetResult))
                            {
                                int mainId = new Random().Next(100);//临时存储主键
                                BusinessObject DomHeadRet = broker1.GetBoParam("DomHead"); //发货单信息
                                BusinessObject domBodyRet = broker1.GetBoParam("domBody");
                                #region===入库主单据
                                DomHead[0]["id"] = mainId; //主关键字段，int类型
                                DomHead[0]["bomfirst"] = "0"; //委外期初标志，string类型

                                string ccode = GetVoucherNumnber("24");//自动生成入库单号

                                DomHead[0]["ccode"] = ccode;//string类型
                                DomHead[0]["ddate"] = ListRdRecord[0].dDate == null ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(ListRdRecord[0].dDate).ToShortDateString(); //入库日期，DateTime类型
                                DomHead[0]["cpersoncode"] = string.IsNullOrEmpty(ListRdRecord[0].cpersoncode) == true ? Convert.ToString(DomHeadRet[0]["cpersoncode"]) : ListRdRecord[0].cpersoncode; //业务员编码，string类型
                                DomHead[0]["crdcode"] = ListRdRecord[0].cRdCode; //入库类别编码，string类型
                                DomHead[0]["csysbarcode"] = "||st01|" + ccode; ; //单据条码，string类型
                                DomHead[0]["cvouchtype"] = ListRdRecord[0].cVouchType;//单据类型，string类型


                                DomHead[0]["darvdate"] = Convert.ToDateTime(DomHeadRet[0]["ddate"]).ToShortDateString(); //到货日期，DateTime类型

                                DomHead[0]["ipurarriveid"] = ListRdRecord[0].ipurarriveid.ToString(); //采购到货单ID，string类型
                                DomHead[0]["carvcode"] = ListRdRecord[0].cARVCode; //到货单号，string类型
                                DomHead[0]["cvencode"] = Convert.ToString(DomHeadRet[0]["cvencode"]); //供货单位编码，string类型
                                DomHead[0]["cordercode"] = Convert.ToString(DomHeadRet[0]["cpocode"]); //订单号，string类型
                                DomHead[0]["cdepcode"] = Convert.ToString(DomHeadRet[0]["cdepcode"]); //部门编码，string类型
                                DomHead[0]["iexchrate"] = Convert.ToDouble(DomHeadRet[0]["iexchrate"]); //汇率，double类型
                                DomHead[0]["cexch_name"] = Convert.ToString(DomHeadRet[0]["cexch_name"]);  //币种，string类型
                                DomHead[0]["ufts"] = Convert.ToString(DomHeadRet[0]["ufts"]); //时间戳，string类型
                                DomHead[0]["cwhcode"] = string.IsNullOrEmpty(Convert.ToString(domBodyRet[0]["cwhcode"])) == true ? ListRdRecord[0].cWhCode : Convert.ToString(domBodyRet[0]["cwhcode"]); //仓库编码，string类型
                                DomHead[0]["cptcode"] = Convert.ToString(DomHeadRet[0]["cptcode"]); //采购类型编码，string类型
                                DomHead[0]["cptname"] = Convert.ToString(DomHeadRet[0]["cptname"]);//采购类型，string类型
                                DomHead[0]["cbustype"] = DomHeadRet[0]["cbustype"];// ListRdRecord[0].cBusType; //业务类型，int类型  普通采购
                                DomHead[0]["cmaker"] = u8Login.cUserName; //制单人，string类型
                                DomHead[0]["itaxrate"] = Convert.ToDouble(DomHeadRet[0]["itaxrate"]); //税率，double类型
                                DomHead[0]["cmemo"] = string.IsNullOrEmpty(Convert.ToString(DomHeadRet[0]["cmemo"])) == true ? ListRdRecord[0].cMemo : Convert.ToString(DomHeadRet[0]["cmemo"]);//备注，string类型
                                DomHead[0]["idiscounttaxtype"] = Convert.ToInt32(DomHeadRet[0]["idiscounttaxtype"]);//扣税类别，int类型

                                DomHead[0]["bpufirst"] = "false"; //采购期初标志，string类型
                                DomHead[0]["brdflag"] = 1; //收发标志，int类型
                                DomHead[0]["csource"] = "采购到货单";// ListRdRecord[0].cSource; //单据来源，int类型  采购订单
                                DomHead[0]["iflowid"] = 0; //流程模式ID，string类型
                                DomHead[0]["cflowname"] = ""; //流程模式描述，string类型
                                DomHead[0]["cdefine5"] = 0; //表头自定义项5，int类型
                                DomHead[0]["cdefine15"] = 0; //表头自定义项15，int类型
                                DomHead[0]["bisstqc"] = "false"; //库存期初标记，string类型
                                DomHead[0]["vt_id"] = ListRdRecord[0].VT_ID; //模版号，int类型
                                DomHead[0]["dnmaketime"] = DateTime.Now; //制单时间，DateTime类型
                                DomHead[0]["ipurorderid"] = ""; //采购订单ID，string类型
                                DomHead[0]["bcredit"] = 0; //是否为立账单据，int类型
                                DomHead[0]["cdefine1"] = "青果科技" + ListRdRecord[0].cCode; //表头自定义项1，string类型(区分用友还是光码添加的数据)
                                DomHead[0]["cdefine2"] = "gomrit" + recordId.ToString(); //表头自定义项2，int类型                 
                                DomHead[0]["cmodifyperson"] = ""; //修改人，string类型
                                DomHead[0]["dmodifydate"] = ""; //修改日期，DateTime类型
                                DomHead[0]["ireturncount"] = 0; //ireturncount，string类型
                                DomHead[0]["isalebillid"] = 0; //发票号，string类型
                                DomHead[0]["iverifystate"] = 0; //工作流审批状态 ，int类型
                                DomHead[0]["iswfcontrolled"] = 0; //是否工作流控制，int类型

                                /***************************** 以下是非必输字段 ****************************/

                                //DomHead[0]["chinvsn"] = ListRdRecord[0].chinvsn; //序列号，string类型
                                //DomHead[0]["gspcheck"] = ListRdRecord[0].gspcheck; //gsp复核标志，string类型
                                // DomHead[0]["iarriveid"] = ListRdRecord[0].iarriveid; //到货单ID，string类型
                                //DomHead[0]["dnmodifytime"] = ListRdRecord[0].dnmodifytime; //修改时间，DateTime类型
                                //DomHead[0]["dnverifytime"] = ListRdRecord[0].dnverifytime; //审核时间，DateTime类型
                                //DomHead[0]["cwhname"] = ""; //仓库，string类型
                                //DomHead[0]["cbuscode"] = ListRdRecord[0].cbuscode; //业务号，string类型
                                //DomHead[0]["cdepname"] = ListRdRecord[0].cdepname; //部门，string类型
                                //DomHead[0]["cpersonname"] = ListRdRecord[0].cpersonname; //业务员，string类型
                                //DomHead[0]["crdname"] = ListRdRecord[0].crdname; //入库类别，string类型
                                //DomHead[0]["dveridate"] = ListRdRecord[0].dveridate; //审核日期，DateTime类型
                                //DomHead[0]["cvenpuomprotocol"] = ListRdRecord[0].cvenpuomprotocol; //收付款协议编码，string类型
                                //DomHead[0]["cvenpuomprotocolname"] = ListRdRecord[0].cvenpuomprotocolname; //收付款协议名称，string类型
                                //DomHead[0]["dcreditstart"] = ListRdRecord[0].dcreditstart; //立账日，DateTime类型
                                //DomHead[0]["icreditperiod"] = ListRdRecord[0].icreditperiod; //账期，int类型
                                //DomHead[0]["dgatheringdate"] = ListRdRecord[0].dgatheringdate; //到期日，DateTime类型
                                //DomHead[0]["cvenfullname"] = ListRdRecord[0].cvenfullname; //供应商全称，string类型
                                //DomHead[0]["chandler"] = ListRdRecord[0].chandler; //审核人，string类型
                                //DomHead[0]["caccounter"] = ListRdRecord[0].caccounter; //记账人，string类型
                                //DomHead[0]["ipresent"] = ListRdRecord[0].ipresent; //现存量，string类型
                                //DomHead[0]["dchkdate"] = ListRdRecord[0].dchkdate; //检验日期，DateTime类型
                                //DomHead[0]["iavaquantity"] = ListRdRecord[0].iavaquantity; //可用量，string类型
                                //DomHead[0]["iavanum"] = ListRdRecord[0].iavanum; //可用件数，string类型
                                //DomHead[0]["ipresentnum"] = ListRdRecord[0].ipresentnum; //现存件数，string类型
                                //DomHead[0]["cchkperson"] = ListRdRecord[0].cchkperson; //检验员，string类型
                                //DomHead[0]["cchkcode"] = ListRdRecord[0].cchkcode; //检验单号，string类型
                                //DomHead[0]["cbillcode"] = ListRdRecord[0].cbillcode; //发票id，string类型
                                //DomHead[0]["isafesum"] = ListRdRecord[0].isafesum; //安全库存量，string类型
                                //DomHead[0]["ilowsum"] = ListRdRecord[0].ilowsum; //最低库存量，string类型
                                //DomHead[0]["cdefine16"] = ListRdRecord[0].cDefine16; //表头自定义项16，double类型
                                //DomHead[0]["itopsum"] = ListRdRecord[0].itopsum; //最高库存量，string类型
                                //DomHead[0]["iproorderid"] = ListRdRecord[0].iproorderid; //生产订单Id，string类型
                                //DomHead[0]["cdefine3"] = ListRdRecord[0].cDefine3; //表头自定义项3，string类型
                                //DomHead[0]["cdefine4"] = ListRdRecord[0].cDefine4; //表头自定义项4，DateTime类型
                                //DomHead[0]["cdefine6"] = ListRdRecord[0].cDefine6; //表头自定义项6，DateTime类型
                                //DomHead[0]["cdefine7"] = ListRdRecord[0].cDefine7; //表头自定义项7，double类型
                                //DomHead[0]["cdefine8"] = ListRdRecord[0].cDefine8; //表头自定义项8，string类型
                                //DomHead[0]["cdefine9"] = ListRdRecord[0].cDefine9; //表头自定义项9，string类型
                                //DomHead[0]["cdefine10"] = ListRdRecord[0].cDefine10; //表头自定义项10，string类型
                                //DomHead[0]["cdefine11"] = ListRdRecord[0].cDefine11; //表头自定义项11，string类型
                                //DomHead[0]["cdefine12"] = ListRdRecord[0].cDefine12; //表头自定义项12，string类型
                                //DomHead[0]["cdefine13"] = ListRdRecord[0].cDefine13; //表头自定义项13，string类型
                                //DomHead[0]["cdefine14"] = ListRdRecord[0].cDefine14; //表头自定义项14，string类型
                                #endregion
                                //给BO表体参数domBody赋值，此BO参数的业务类型为采购入库单，属表体参数。BO参数均按引用传递
                                //提示：给BO表体参数domBody赋值有两种方法
                                //方法一是直接传入MSXML2.DOMDocumentClass对象
                                //broker.AssignNormalValue("domBody", new MSXML2.DOMDocumentClass())
                                //方法二是构造BusinessObject对象，具体方法如下：
                                BusinessObject domBody = broker.GetBoParam("domBody");
                                if (ListRdRecords.Count > 0)
                                {
                                    int RowCount = 0;
                                    #region==获取行号
                                    for (int j = 0; j < ListRdRecords.Count; j++)
                                    {
                                        for (int i = 0; i < domBodyRet.RowCount; i++)
                                        {
                                            if (ListRdRecords[j].cInvCode == domBodyRet[i]["cinvcode"].ToString())
                                            {
                                                RowCount++;
                                            }
                                        }
                                    }
                                    #endregion

                                    domBody.RowCount = RowCount; //设置BO对象行数

                                    var indexRowCount = 0; 
                                    for (int j = 0; j < ListRdRecords.Count; j++)
                                    {
                                        for (int i = 0; i < domBodyRet.RowCount; i++)
                                        {
                                            if (ListRdRecords[j].cInvCode == domBodyRet[i]["cinvcode"].ToString() && ListRdRecords[j].AutoID == domBodyRet[i]["autoid"].ToString())
                                            {
                                                domBody[indexRowCount]["autoid"] = i; //主关键字段，int类型
                                                domBody[indexRowCount]["id"] = mainId; //与收发记录主表关联项，int类型
                                                domBody[indexRowCount]["cinvcode"] = Convert.ToString(domBodyRet[i]["cinvcode"]); //存货编码，string类型
                                                domBody[indexRowCount]["cinvm_unit"] = Convert.ToString(domBodyRet[i]["cinvm_unit"]);//主计量单位，string类型
                                                var iquantity = ListRdRecords[j].iQuantity <= 0 ? Convert.ToDouble(domBodyRet[i]["iquantity"]) : Convert.ToDouble(ListRdRecords[j].iQuantity);
                                                domBody[indexRowCount]["iquantity"] = iquantity;//数量，double类型
                                                domBody[indexRowCount]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                                                domBody[indexRowCount]["iMatSettleState"] = 0; //结算状态，int类型
                                                domBody[indexRowCount]["irowno"] = i + 1; //行号，string类型
                                                //domBody[indexRowCount]["strowguid"] = ListRdRecords[i].strowguid; //rowguid，string类型

                                                //domBody[indexRowCount]["cassunit"] = drArr[0]["cUnitID"].ToString(); //辅计量单位编码，string类型
                                                //domBody[indexRowCount]["cbatch"] = ListRdRecords[i].cbatch; //批号，string类型
                                                //domBody[indexRowCount]["cbmemo"] = ListRdRecords[i].cbMemo; //备注，string类型
                                                //domBody[indexRowCount]["iFaQty"] = ListRdRecords[i].iFaQty; //转资产数量，string类型
                                                //domBody[indexRowCount]["isTax"] = ListRdRecords[i].isTax; //累计结算税额，string类型
                                                //domBody[indexRowCount]["cbinvsn"] = ListRdRecords[i].cbinvsn; //序列号，string类型
                                                //domBody[indexRowCount]["cplanlotcode"] = ListRdRecords[i].cplanlotcode; //计划批号，string类型
                                                //domBody[indexRowCount]["taskguid"] = ListRdRecords[i].taskguid; //taskguid，string类型
                                                //domBody[indexRowCount]["bgift"] = ListRdRecords[i].bgift; //赠品，string类型
                                                //domBody[indexRowCount]["creworkmocode"] = ListRdRecords[i].creworkmocode; //返工订单号，string类型
                                                //domBody[indexRowCount]["ireworkmodetailsid"] = ""; //返工订单子表标识，string类型
                                                //domBody[indexRowCount]["iproducttype"] = ListRdRecords[i].iproducttype; //产出品类型，string类型
                                                //domBody[indexRowCount]["cmaininvcode"] = ListRdRecords[i].cmaininvcode; //对应主产品，string类型
                                                //domBody[indexRowCount]["imainmodetailsid"] = ListRdRecords[i].imainmodetailsid; //主产品订单子表标识，string类型
                                                //domBody[indexRowCount]["isharematerialfee"] = ListRdRecords[i].isharematerialfee; //分摊材料费，string类型
                                                //domBody[indexRowCount]["cinvouchtype"] = ListRdRecords[i].cinvouchtype; //对应入库单类型，string类型
                                                //domBody[indexRowCount]["idebitids"] = ListRdRecords[i].idebitids; //借入借出单子表id，string类型
                                                //domBody[indexRowCount]["imergecheckautoid"] = ListRdRecords[i].imergecheckautoid; //检验单子表ID，string类型
                                                //domBody[indexRowCount]["outcopiedquantity"] = ListRdRecords[i].outcopiedquantity; //已复制数量，string类型
                                                //domBody[indexRowCount]["iOldPartId"] = ListRdRecords[i].iOldPartId; //降级前物料编码，string类型
                                                //domBody[indexRowCount]["fOldQuantity"] = ListRdRecords[i].fOldQuantity; //降级前数量，string类型
                                                //domBody[indexRowCount]["cbsysbarcode"] = ListRdRecords[i].cbsysbarcode; //单据行条码，string类型

                                                /***************************** 以下是非必输字段 ****************************/
                                                #region===入库子单据
                                                var iunitcost = ListRdRecords[j].iUnitCost <= 0 ? Convert.ToDouble(domBodyRet[i]["icost"]) : Convert.ToDouble(ListRdRecords[j].iUnitCost);
                                                domBody[indexRowCount]["iunitcost"] = iunitcost; //本币单价，double类型

                                                var iprice = Convert.ToDouble(Convert.ToDouble(iunitcost) * Convert.ToDouble(iquantity));//本币金额
                                                domBody[indexRowCount]["iprice"] = iprice;//本币金额，double类型
                                                domBody[indexRowCount]["iaprice"] = iprice; //暂估金额，double类型
                                                domBody[indexRowCount]["cfree1"] = Convert.ToString(domBodyRet[i]["cfree1"]); //存货自由项1，string类型
                                                domBody[indexRowCount]["inum"] = Convert.ToDouble(domBodyRet[i]["inum"]); //件数，double类型
                                                domBody[indexRowCount]["cassunit"] = Convert.ToString(domBodyRet[i]["cunitid"]); //库存单位码，string类型
                                                domBody[indexRowCount]["facost"] = domBody[indexRowCount]["iunitcost"]; //暂估单价，double类型
                                                domBody[indexRowCount]["iarrsid"] = Convert.ToInt32(domBodyRet[i]["autoid"]); //采购到货单子表标识，string类型
                                                domBody[indexRowCount]["inquantity"] = Math.Round(Convert.ToDouble(domBodyRet[i]["iquantity"]), 2).ToString();//应收数量，double类型
                                                domBody[indexRowCount]["iflag"] = 0; //标志，string类型
                                                domBody[indexRowCount]["isnum"] = 0; //累计结算件数，double类型
                                                domBody[indexRowCount]["isquantity"] = 0; //累计结算数量，double类型


                                                domBody[indexRowCount]["ioritaxcost"] = Convert.ToDouble(domBodyRet[i]["ioritaxcost"]); //原币含税单价，double类型
                                                domBody[indexRowCount]["ioricost"] = Convert.ToDouble(domBodyRet[i]["ioricost"]);  //原币单价，double类型
                                                domBody[indexRowCount]["iorimoney"] = Convert.ToDouble(domBodyRet[i]["iorimoney"]);  //原币金额，double类型
                                                domBody[indexRowCount]["ioritaxprice"] = Convert.ToDouble(domBodyRet[i]["ioritaxprice"]); //原币税额，double类型
                                                domBody[indexRowCount]["iorisum"] = Convert.ToDouble(domBodyRet[i]["iorisum"]); //原币价税合计，double类型
                                                domBody[indexRowCount]["itaxrate"] = Convert.ToDouble(domBodyRet[i]["itaxrate"]); //税率，double类型
                                                domBody[indexRowCount]["itaxprice"] = Convert.ToDouble(domBodyRet[i]["itaxprice"]); //本币税额，double类型
                                                domBody[indexRowCount]["isum"] = Convert.ToDouble(domBodyRet[i]["isum"]); //本币价税合计，double类型
                                                domBody[indexRowCount]["btaxcost"] = Convert.ToString(domBodyRet[i]["btaxcost"]);//单价标准，double类型
                                                domBody[indexRowCount]["cpoid"] = Convert.ToString(domBodyRet[i]["isourcemocode"]);  //订单号，string类型
                                                domBody[indexRowCount]["iposid"] = Convert.ToInt32(domBodyRet[i]["iposid"]);//订单子表ID，int类型
                                                domBody[indexRowCount]["cinvaddcode"] = Convert.ToString(domBodyRet[i]["cinvaddcode"]);//存货代码，string类型
                                                domBody[indexRowCount]["cinvstd"] = Convert.ToString(domBodyRet[i]["cinvstd"]);//规格型号，string类型
                                                domBody[indexRowCount]["cinva_unit"] = Convert.ToString(domBodyRet[i]["cinva_unit"]); //库存单位，string类型
                                                domBody[indexRowCount]["iinvexchrate"] = Convert.ToDouble(domBodyRet[i]["iinvexchrate"]); //换算率，double类型


                                                //domBody[indexRowCount]["innum"] = Math.Round(count, 2).ToString();//应收件数，double类型
                                                //Decimal count = Convert.ToDecimal(Convert.ToDouble(domBodyRet[0]["iquantity"])) - Convert.ToDecimal(drArr[0]["iReceivedQTY"].ToString() == "" ? "0" : drArr[0]["iReceivedQTY"].ToString());
                                                //domBody[indexRowCount]["binvbatch"] = ListRdRecords[i].binvbatch; //批次管理，string类型
                                                //domBody[indexRowCount]["cposname"] = ListRdRecords[i].cposname; //货位，string类型
                                                //domBody[indexRowCount]["cinvname"] = ListRdRecords[i].cinvname; //存货名称，string类型
                                                //domBody[indexRowCount]["bservice"] = ListRdRecords[i].bservice; //是否费用，string类型
                                                //domBody[indexRowCount]["cinvccode"] = ListRdRecords[i].cinvccode; //所属分类码，string类型
                                                // domBody[indexRowCount]["cbatchproperty1"] = ListRdRecords[i].cBatchProperty1; //属性1，double类型
                                                // domBody[indexRowCount]["cbatchproperty2"] = ListRdRecords[i].cBatchProperty2; //属性2，double类型
                                                // domBody[indexRowCount]["cfree2"] = ListRdRecords[i].cFree2; //存货自由项2，string类型
                                                // domBody[indexRowCount]["ipunitcost"] = ListRdRecords[i].iPUnitCost.ToString() == "0" ? "" : ListRdRecords[i].iPUnitCost.ToString(); //计划单价/售价，double类型
                                                // domBody[indexRowCount]["ipprice"] = ListRdRecords[i].iPPrice.ToString() == "0" ? "" : ListRdRecords[i].iPPrice.ToString(); //计划金额/售价金额，double类型
                                                // domBody[indexRowCount]["dvdate"] = ListRdRecords[i].dVDate; //失效日期，DateTime类型
                                                // domBody[indexRowCount]["cvouchcode"] = ListRdRecords[i].cVouchCode; //对应入库单id，string类型
                                                //domBody[indexRowCount]["imassdate"] = 0; //保质期，int类型
                                                //domBody[indexRowCount]["dsdate"] = ListRdRecords[i].dSDate; //结算日期，DateTime类型
                                                //domBody[indexRowCount]["itax"] = ListRdRecords[i].iTax; //税额，double类型
                                                //domBody[indexRowCount]["imoney"] = ListRdRecords[i].iMoney; //累计结算金额，double类型
                                                //domBody[indexRowCount]["isoutquantity"] = ListRdRecords[i].iSOutQuantity; //累计出库数量，double类型
                                                //domBody[indexRowCount]["isoutnum"] = ListRdRecords[i].iSOutNum; //累计出库件数，double类型
                                                //domBody[indexRowCount]["ifnum"] = ListRdRecords[i].iFNum; //实际件数，double类型
                                                //domBody[indexRowCount]["ifquantity"] = ListRdRecords[i].iFQuantity; //实际数量，double类型
                                                //domBody[indexRowCount]["binvtype"] = ListRdRecords[i].binvtype; //折扣类型，string类型
                                                //domBody[indexRowCount]["cdefine22"] = ListRdRecords[i].cDefine22; //表体自定义项1，string类型
                                                //domBody[indexRowCount]["cdefine23"] = ListRdRecords[i].cDefine23; //表体自定义项2，string类型
                                                //domBody[indexRowCount]["cdefine24"] = ListRdRecords[i].cDefine24; //表体自定义项3，string类型
                                                //domBody[indexRowCount]["cdefine25"] = ListRdRecords[i].cDefine25; //表体自定义项4，string类型
                                                //domBody[indexRowCount]["cdefine26"] = ListRdRecords[i].cDefine26; //表体自定义项5，double类型
                                                //domBody[indexRowCount]["cdefine27"] = ListRdRecords[i].cDefine27; //表体自定义项6，double类型
                                                //domBody[indexRowCount]["citemcode"] = ListRdRecords[i].cItemCode; //项目编码，string类型
                                                //domBody[indexRowCount]["citem_class"] = ListRdRecords[i].cItem_class; //项目大类编码，string类型
                                                //domBody[indexRowCount]["cbatchproperty3"] = ListRdRecords[i].cBatchProperty3; //属性3，double类型
                                                //domBody[indexRowCount]["cfree3"] = ListRdRecords[i].cFree3; //存货自由项3，string类型
                                                //domBody[indexRowCount]["cfree4"] = ListRdRecords[i].cFree4; //存货自由项4，string类型
                                                //domBody[indexRowCount]["cbatchproperty4"] = ListRdRecords[i].cBatchProperty4; //属性4，double类型
                                                //domBody[indexRowCount]["cbatchproperty5"] = ListRdRecords[i].cBatchProperty5; //属性5，double类型
                                                //domBody[indexRowCount]["cfree5"] = ListRdRecords[i].cFree5; //存货自由项5，string类型
                                                //domBody[indexRowCount]["cfree6"] = ListRdRecords[i].cFree6; //存货自由项6，string类型
                                                //domBody[indexRowCount]["cbatchproperty6"] = ListRdRecords[i].cBatchProperty6; //属性6，string类型
                                                //domBody[indexRowCount]["cbatchproperty7"] = ListRdRecords[i].cBatchProperty7; //属性7，string类型
                                                //domBody[indexRowCount]["cfree7"] = ListRdRecords[i].cFree7; //存货自由项7，string类型
                                                //domBody[indexRowCount]["cfree8"] = ListRdRecords[i].cFree8; //存货自由项8，string类型
                                                //domBody[indexRowCount]["cbatchproperty8"] = ListRdRecords[i].cBatchProperty8; //属性8，string类型
                                                //domBody[indexRowCount]["cbatchproperty9"] = ListRdRecords[i].cBatchProperty9; //属性9，string类型
                                                //domBody[indexRowCount]["cfree9"] = ListRdRecords[i].cFree9; //存货自由项9，string类型
                                                //domBody[indexRowCount]["cfree10"] = ListRdRecords[i].cFree10; //存货自由项10，string类型
                                                //domBody[indexRowCount]["cbatchproperty10"] = ListRdRecords[i].cBatchProperty10; //属性10，DateTime类型
                                                //domBody[indexRowCount]["cdefine28"] = ListRdRecords[i].cDefine28; //表体自定义项7，string类型
                                                //domBody[indexRowCount]["cdefine29"] = ListRdRecords[i].cDefine29; //表体自定义项8，string类型
                                                //domBody[indexRowCount]["cdefine30"] = ListRdRecords[i].cDefine30; //表体自定义项9，string类型
                                                //domBody[indexRowCount]["cdefine31"] = ListRdRecords[i].cDefine31; //表体自定义项10，string类型
                                                //domBody[indexRowCount]["cdefine32"] = ListRdRecords[i].cDefine32; //表体自定义项11，string类型
                                                //domBody[indexRowCount]["cdefine33"] = ListRdRecords[i].cDefine33; //表体自定义项12，string类型
                                                //domBody[indexRowCount]["cdefine34"] = ListRdRecords[i].cDefine34.ToString() == "0" ? "" : ListRdRecords[i].cDefine34.ToString(); //表体自定义项13，int类型
                                                //domBody[indexRowCount]["cdefine35"] = ListRdRecords[i].cDefine35.ToString() == "0" ? "" : ListRdRecords[i].cDefine35.ToString(); //表体自定义项14，int类型
                                                //domBody[indexRowCount]["cdefine36"] = ListRdRecords[i].cDefine36; //表体自定义项15，DateTime类型
                                                //domBody[indexRowCount]["cdefine37"] = ListRdRecords[i].cDefine37; //表体自定义项16，DateTime类型
                                                //domBody[indexRowCount]["cinvdefine4"] = ListRdRecords[i].cinv; //存货自定义项4，string类型
                                                //domBody[indexRowCount]["cinvdefine5"] = ""; //存货自定义项5，string类型
                                                //domBody[indexRowCount]["cinvdefine6"] = ""; //存货自定义项6，string类型
                                                //domBody[indexRowCount]["cinvdefine7"] = ""; //存货自定义项7，string类型
                                                //domBody[indexRowCount]["cinvdefine8"] = ""; //存货自定义项8，string类型
                                                //domBody[indexRowCount]["cinvdefine9"] = ""; //存货自定义项9，string类型
                                                //domBody[indexRowCount]["cinvdefine10"] = ""; //存货自定义项10，string类型
                                                //domBody[indexRowCount]["cinvdefine11"] = ""; //存货自定义项11，string类型
                                                //domBody[indexRowCount]["cinvdefine12"] = ""; //存货自定义项12，string类型
                                                //domBody[indexRowCount]["cinvdefine13"] = ""; //存货自定义项13，string类型
                                                //domBody[indexRowCount]["cinvdefine14"] = ""; //存货自定义项14，string类型
                                                //domBody[indexRowCount]["cinvdefine15"] = ""; //存货自定义项15，string类型
                                                //domBody[indexRowCount]["cinvdefine16"] = ""; //存货自定义项16，string类型
                                                //domBody[indexRowCount]["cdemandmemo"] = ListRdRecords[i].cdemandmemo; //需求分类代号说明，string类型
                                                //domBody[indexRowCount]["cinvdefine1"] = ListRdRecords[i].; //存货自定义项1，string类型
                                                //domBody[indexRowCount]["cinvdefine2"] = ""; //存货自定义项2，string类型
                                                //domBody[indexRowCount]["cinvdefine3"] = ""; //存货自定义项3，string类型
                                                //domBody[indexRowCount]["scrapufts"] = ListRdRecords[i].scrapufts; //不合格品时间戳，string类型
                                                //domBody[indexRowCount]["creplaceitem"] = ListRdRecords[i].creplaceitem; //替换件，string类型

                                                //domBody[indexRowCount]["cbarcode"] = ListRdRecords[i].cBarCode; //条形码，string类型
                                                //domBody[indexRowCount]["impoids"] = ListRdRecords[i].iMPoIds.ToString() == "0" ? "" : ListRdRecords[i].iMPoIds.ToString(); //生产订单子表ID，int类型
                                                //domBody[indexRowCount]["icheckids"] = ListRdRecords[i].iCheckIds.ToString() == "0" ? "" : ListRdRecords[i].iCheckIds.ToString(); //检验单子表ID，int类型
                                                //domBody[indexRowCount]["iomodid"] = ListRdRecords[i].iOMoDID.ToString() == "0" ? "" : ListRdRecords[i].iOMoDID.ToString(); //委外订单子表ID，int类型
                                                //domBody[indexRowCount]["isodid"] = ListRdRecords[i].isodid; //销售订单子表ID，string类型
                                                //domBody[indexRowCount]["cbvencode"] = ListRdRecords[i].cBVencode; //供应商编码，string类型
                                                //domBody[indexRowCount]["cvenname"] = ListRdRecords[i].cvenname; //供应商，string类型

                                                //domBody[indexRowCount]["dmadedate"] = ListRdRecords[i].dMadeDate; //生产日期，DateTime类型
                                                //
                                                //domBody[indexRowCount]["corufts"] = ListRdRecords[i].corufts; //时间戳，string类型
                                                //domBody[indexRowCount]["cgspstate"] = ListRdRecords[i].cGspState; //检验状态，double类型
                                                //domBody[indexRowCount]["icheckidbaks"] = ListRdRecords[i].iCheckIdBaks; //检验单子表id，string类型
                                                //domBody[indexRowCount]["irejectids"] = ListRdRecords[i].iRejectIds; //不良品处理单id，string类型
                                                //domBody[indexRowCount]["dcheckdate"] = ListRdRecords[i].dCheckDate; //检验日期，DateTime类型
                                                //domBody[indexRowCount]["dmsdate"] = ListRdRecords[i].dMSDate; //核销日期，DateTime类型
                                                //domBody[indexRowCount]["cmassunit"] = ListRdRecords[i].cMassUnit.ToString() == "0" ? "" : ListRdRecords[i].cMassUnit.ToString(); //保质期单位，int类型
                                                //domBody[indexRowCount]["ccheckcode"] = ListRdRecords[i].cCheckCode; //检验单号，string类型
                                                //domBody[indexRowCount]["crejectcode"] = ListRdRecords[i].cRejectCode; //不良品处理单号，string类型
                                                //domBody[indexRowCount]["csocode"] = ListRdRecords[i].csocode; //需求跟踪号，string类型
                                                //domBody[indexRowCount]["cvmivencode"] = ListRdRecords[i].cvmivencode; //代管商代码，string类型
                                                //domBody[indexRowCount]["cvmivenname"] = ListRdRecords[i].cvmivenname; //代管商，string类型
                                                //domBody[indexRowCount]["bvmiused"] = ListRdRecords[i].bVMIUsed.ToString() == "0" ? "" : ListRdRecords[i].bVMIUsed.ToString(); //代管消耗标识，int类型
                                                //domBody[indexRowCount]["ivmisettlequantity"] = ListRdRecords[i].iVMISettleQuantity; //代管挂账确认单数量，double类型
                                                //domBody[indexRowCount]["ivmisettlenum"] = ListRdRecords[i].iVMISettleNum; //代管挂账确认单件数，double类型
                                                ////---------/
                                                //domBody[indexRowCount]["cbarvcode"] = ListRdRecords[i].cbarvcode; //到货单号，string类型
                                                //domBody[indexRowCount]["dbarvdate"] = ListRdRecords[i].dbarvdate; //到货日期，DateTime类型
                                                //domBody[indexRowCount]["iordertype"] = ListRdRecords[i].iordertype.ToString() == "0" ? "" : ListRdRecords[i].iordertype.ToString(); //销售订单类别，int类型
                                                //domBody[indexRowCount]["iorderdid"] = ListRdRecords[i].iorderdid.ToString() == "0" ? "" : ListRdRecords[i].iorderdid.ToString(); //iorderdid，int类型
                                                //domBody[indexRowCount]["iordercode"] = ListRdRecords[i].iordercode; //销售订单号，string类型
                                                //domBody[indexRowCount]["iorderseq"] = ListRdRecords[i].iorderseq.ToString() == "0" ? "" : ListRdRecords[i].iorderseq.ToString(); //销售订单行号，string类型
                                                //domBody[indexRowCount]["iexpiratdatecalcu"] = ListRdRecords[i].iExpiratDateCalcu.ToString() == "0" ? "" : ListRdRecords[i].iExpiratDateCalcu.ToString(); //有效期推算方式，int类型
                                                //domBody[indexRowCount]["cexpirationdate"] = ListRdRecords[i].cExpirationdate; //有效期至，string类型
                                                //domBody[indexRowCount]["dexpirationdate"] = ListRdRecords[i].dExpirationdate; //有效期计算项，string类型
                                                //domBody[indexRowCount]["cciqbookcode"] = ListRdRecords[i].cciqbookcode; //手册号，string类型
                                                //domBody[indexRowCount]["ibondedsumqty"] = ListRdRecords[i].iBondedSumQty.ToString() == "0" ? "" : ListRdRecords[i].iBondedSumQty.ToString(); //累计保税处理抽取数量，string类型
                                                //domBody[indexRowCount]["iimosid"] = ListRdRecords[i].iIMOSID.ToString() == "0" ? "" : ListRdRecords[i].iIMOSID.ToString(); //iimosid，string类型
                                                //domBody[indexRowCount]["iimbsid"] = ListRdRecords[i].iIMBSID.ToString() == "0" ? "" : ListRdRecords[i].iIMBSID.ToString(); //iimbsid，string类型
                                                //domBody[indexRowCount]["ccheckpersonname"] = ListRdRecords[i].ccheckpersonname; //检验员，string类型
                                                //domBody[indexRowCount]["ccheckpersoncode"] = ListRdRecords[i].cCheckPersonCode; //检验员编码，string类型
                                                //domBody[indexRowCount]["strcontractid"] = ListRdRecords[i].strContractId; //合同号，string类型
                                                //domBody[indexRowCount]["strcode"] = ListRdRecords[i].strCode; //合同标的编码，string类型
                                                //domBody[indexRowCount]["cveninvcode"] = ListRdRecords[i].cveninvcode; //供应商存货编码，string类型
                                                //domBody[indexRowCount]["cveninvname"] = ListRdRecords[i].cveninvname; //供应商存货名称，string类型
                                                //domBody[indexRowCount]["isotype"] = ListRdRecords[i].isotype.ToString() == "0" ? "" : ListRdRecords[i].isotype.ToString(); //需求跟踪方式，int类型
                                                //domBody[indexRowCount]["isumbillquantity"] = ListRdRecords[i].iSumBillQuantity.ToString() == "0" ? "" : ListRdRecords[i].iSumBillQuantity.ToString(); //累计开票数量，double类型
                                                //domBody[indexRowCount]["cbaccounter"] = ListRdRecords[i].cbaccounter; //记账人，string类型
                                                //// domBody[indexRowCount]["bcosting"] = ListRdRecords[i].bCosting; //是否核算，string类型
                                                //domBody[indexRowCount]["impcost"] = ListRdRecords[i].impcost.ToString() == "0" ? "" : ListRdRecords[i].impcost.ToString(); //最高进价，string类型
                                                //domBody[indexRowCount]["ioricost"] = ListRdRecords[i].iOriCost.ToString() == "0" ? "" : ListRdRecords[i].iOriCost.ToString(); //原币单价，double类型
                                                //
                                                //
                                                //

                                                //domBody[indexRowCount]["imaterialfee"] = ListRdRecords[i].iMaterialFee.ToString() == "0" ? "" : ListRdRecords[i].iMaterialFee.ToString(); //材料费，double类型
                                                //domBody[indexRowCount]["iprocesscost"] = ListRdRecords[i].iProcessCost.ToString() == "0" ? "" : ListRdRecords[i].iProcessCost.ToString(); //加工费单价，double类型
                                                //domBody[indexRowCount]["iprocessfee"] = ListRdRecords[i].iProcessFee.ToString() == "0" ? "" : ListRdRecords[i].iProcessFee.ToString(); //加工费，double类型
                                                //domBody[indexRowCount]["ismaterialfee"] = ListRdRecords[i].iSMaterialFee.ToString() == "0" ? "" : ListRdRecords[i].iSMaterialFee.ToString(); //累计结算材料费，double类型
                                                //domBody[indexRowCount]["isprocessfee"] = ListRdRecords[i].iSProcessFee.ToString() == "0" ? "" : ListRdRecords[i].iSProcessFee.ToString(); //累计结算加工费，double类型
                                                //domBody[indexRowCount]["isoseq"] = ListRdRecords[i].isoseq.ToString() == "0" ? "" : ListRdRecords[i].isoseq.ToString(); //需求跟踪行号，string类型
                                                //domBody[indexRowCount]["cposition"] = ListRdRecords[i].cPosition; //货位编码，string类型
                                                //domBody[indexRowCount]["itrids"] = ListRdRecords[i].iTrIds.ToString() == "0" ? "" : ListRdRecords[i].iTrIds.ToString(); //特殊单据子表标识，double类型
                                                //domBody[indexRowCount]["cname"] = ListRdRecords[i].cName; //项目名称，string类型
                                                //domBody[indexRowCount]["citemcname"] = ListRdRecords[i].cItemCName; //项目大类名称，string类型
                                                //domBody[indexRowCount]["cinvouchcode"] = ListRdRecords[i].cInVouchCode; //对应入库单号，string类型
                                                //domBody[indexRowCount]["iinvsncount"] = ListRdRecords[i].iInvSNCount.ToString() == "0" ? "" : ListRdRecords[i].iInvSNCount.ToString(); //存库序列号，int类型

                                                indexRowCount++;
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    domBody.RowCount = domBodyRet.RowCount; //设置BO对象行数

                                    for (int i = 0; i < domBodyRet.RowCount; i++)
                                    {

                                        /****************************** 以下是必输字段 ****************************/
                                        domBody[i]["autoid"] = i; //主关键字段，int类型
                                        domBody[i]["id"] = mainId; //与收发记录主表关联项，int类型
                                        domBody[i]["cinvcode"] = Convert.ToString(domBodyRet[i]["cinvcode"]); //存货编码，string类型
                                        domBody[i]["cinvm_unit"] = Convert.ToString(domBodyRet[i]["cinvm_unit"]);//主计量单位，string类型
                                        domBody[i]["iquantity"] = Convert.ToDouble(domBodyRet[i]["iquantity"]);//数量，double类型
                                        domBody[i]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                                        domBody[i]["iMatSettleState"] = 0; //结算状态，int类型
                                        domBody[i]["irowno"] = i + 1; //行号，string类型
                                        //domBody[i]["strowguid"] = ListRdRecords[i].strowguid; //rowguid，string类型

                                        //domBody[i]["cassunit"] = drArr[0]["cUnitID"].ToString(); //辅计量单位编码，string类型
                                        //domBody[i]["cbatch"] = ListRdRecords[i].cbatch; //批号，string类型
                                        //domBody[i]["cbmemo"] = ListRdRecords[i].cbMemo; //备注，string类型
                                        //domBody[i]["iFaQty"] = ListRdRecords[i].iFaQty; //转资产数量，string类型
                                        //domBody[i]["isTax"] = ListRdRecords[i].isTax; //累计结算税额，string类型
                                        //domBody[i]["cbinvsn"] = ListRdRecords[i].cbinvsn; //序列号，string类型
                                        //domBody[i]["cplanlotcode"] = ListRdRecords[i].cplanlotcode; //计划批号，string类型
                                        //domBody[i]["taskguid"] = ListRdRecords[i].taskguid; //taskguid，string类型
                                        //domBody[i]["bgift"] = ListRdRecords[i].bgift; //赠品，string类型
                                        //domBody[i]["creworkmocode"] = ListRdRecords[i].creworkmocode; //返工订单号，string类型
                                        //domBody[i]["ireworkmodetailsid"] = ""; //返工订单子表标识，string类型
                                        //domBody[i]["iproducttype"] = ListRdRecords[i].iproducttype; //产出品类型，string类型
                                        //domBody[i]["cmaininvcode"] = ListRdRecords[i].cmaininvcode; //对应主产品，string类型
                                        //domBody[i]["imainmodetailsid"] = ListRdRecords[i].imainmodetailsid; //主产品订单子表标识，string类型
                                        //domBody[i]["isharematerialfee"] = ListRdRecords[i].isharematerialfee; //分摊材料费，string类型
                                        //domBody[i]["cinvouchtype"] = ListRdRecords[i].cinvouchtype; //对应入库单类型，string类型
                                        //domBody[i]["idebitids"] = ListRdRecords[i].idebitids; //借入借出单子表id，string类型
                                        //domBody[i]["imergecheckautoid"] = ListRdRecords[i].imergecheckautoid; //检验单子表ID，string类型
                                        //domBody[i]["outcopiedquantity"] = ListRdRecords[i].outcopiedquantity; //已复制数量，string类型
                                        //domBody[i]["iOldPartId"] = ListRdRecords[i].iOldPartId; //降级前物料编码，string类型
                                        //domBody[i]["fOldQuantity"] = ListRdRecords[i].fOldQuantity; //降级前数量，string类型
                                        //domBody[i]["cbsysbarcode"] = ListRdRecords[i].cbsysbarcode; //单据行条码，string类型

                                        /***************************** 以下是非必输字段 ****************************/
                                        #region===入库子单据

                                        domBody[i]["iunitcost"] = Convert.ToDouble(domBodyRet[i]["icost"]); //本币单价，double类型

                                        var iprice = Convert.ToDouble(Convert.ToDouble(domBodyRet[i]["icost"]) * Convert.ToDouble(domBodyRet[i]["iquantity"]));//本币金额
                                        domBody[i]["iprice"] = iprice;//本币金额，double类型
                                        domBody[i]["iaprice"] = iprice; //暂估金额，double类型
                                        domBody[i]["cfree1"] = Convert.ToString(domBodyRet[i]["cfree1"]); //存货自由项1，string类型
                                        domBody[i]["inum"] = Convert.ToDouble(domBodyRet[i]["inum"]); //件数，double类型
                                        domBody[i]["cassunit"] = Convert.ToString(domBodyRet[i]["cunitid"]); //库存单位码，string类型
                                        domBody[i]["facost"] = domBody[i]["iunitcost"]; //暂估单价，double类型
                                        domBody[i]["iarrsid"] = Convert.ToInt32(domBodyRet[i]["autoid"]); //采购到货单子表标识，string类型
                                        domBody[i]["inquantity"] = Math.Round(Convert.ToDouble(domBodyRet[i]["iquantity"]), 2).ToString();//应收数量，double类型
                                        domBody[i]["iflag"] = 0; //标志，string类型
                                        domBody[i]["isnum"] = 0; //累计结算件数，double类型
                                        domBody[i]["isquantity"] = 0; //累计结算数量，double类型


                                        domBody[i]["ioritaxcost"] = Convert.ToDouble(domBodyRet[i]["ioritaxcost"]); //原币含税单价，double类型
                                        domBody[i]["ioricost"] = Convert.ToDouble(domBodyRet[i]["ioricost"]);  //原币单价，double类型
                                        domBody[i]["iorimoney"] = Convert.ToDouble(domBodyRet[i]["iorimoney"]);  //原币金额，double类型
                                        domBody[i]["ioritaxprice"] = Convert.ToDouble(domBodyRet[i]["ioritaxprice"]); //原币税额，double类型
                                        domBody[i]["iorisum"] = Convert.ToDouble(domBodyRet[i]["iorisum"]); //原币价税合计，double类型
                                        domBody[i]["itaxrate"] = Convert.ToDouble(domBodyRet[i]["itaxrate"]); //税率，double类型
                                        domBody[i]["itaxprice"] = Convert.ToDouble(domBodyRet[i]["itaxprice"]); //本币税额，double类型
                                        domBody[i]["isum"] = Convert.ToDouble(domBodyRet[i]["isum"]); //本币价税合计，double类型
                                        domBody[i]["btaxcost"] = Convert.ToString(domBodyRet[i]["btaxcost"]);//单价标准，double类型
                                        domBody[i]["cpoid"] = Convert.ToString(domBodyRet[i]["isourcemocode"]);  //订单号，string类型
                                        domBody[i]["iposid"] = Convert.ToInt32(domBodyRet[i]["iposid"]);//订单子表ID，int类型
                                        domBody[i]["cinvaddcode"] = Convert.ToString(domBodyRet[i]["cinvaddcode"]);//存货代码，string类型
                                        domBody[i]["cinvstd"] = Convert.ToString(domBodyRet[i]["cinvstd"]);//规格型号，string类型
                                        domBody[i]["cinva_unit"] = Convert.ToString(domBodyRet[i]["cinva_unit"]); //库存单位，string类型
                                        domBody[i]["iinvexchrate"] = Convert.ToDouble(domBodyRet[i]["iinvexchrate"]); //换算率，double类型


                                        //domBody[i]["innum"] = Math.Round(count, 2).ToString();//应收件数，double类型
                                        //Decimal count = Convert.ToDecimal(Convert.ToDouble(domBodyRet[0]["iquantity"])) - Convert.ToDecimal(drArr[0]["iReceivedQTY"].ToString() == "" ? "0" : drArr[0]["iReceivedQTY"].ToString());
                                        //domBody[i]["binvbatch"] = ListRdRecords[i].binvbatch; //批次管理，string类型
                                        //domBody[i]["cposname"] = ListRdRecords[i].cposname; //货位，string类型
                                        //domBody[i]["cinvname"] = ListRdRecords[i].cinvname; //存货名称，string类型
                                        //domBody[i]["bservice"] = ListRdRecords[i].bservice; //是否费用，string类型
                                        //domBody[i]["cinvccode"] = ListRdRecords[i].cinvccode; //所属分类码，string类型
                                        // domBody[i]["cbatchproperty1"] = ListRdRecords[i].cBatchProperty1; //属性1，double类型
                                        // domBody[i]["cbatchproperty2"] = ListRdRecords[i].cBatchProperty2; //属性2，double类型
                                        // domBody[i]["cfree2"] = ListRdRecords[i].cFree2; //存货自由项2，string类型
                                        // domBody[i]["ipunitcost"] = ListRdRecords[i].iPUnitCost.ToString() == "0" ? "" : ListRdRecords[i].iPUnitCost.ToString(); //计划单价/售价，double类型
                                        // domBody[i]["ipprice"] = ListRdRecords[i].iPPrice.ToString() == "0" ? "" : ListRdRecords[i].iPPrice.ToString(); //计划金额/售价金额，double类型
                                        // domBody[i]["dvdate"] = ListRdRecords[i].dVDate; //失效日期，DateTime类型
                                        // domBody[i]["cvouchcode"] = ListRdRecords[i].cVouchCode; //对应入库单id，string类型
                                        //domBody[i]["imassdate"] = 0; //保质期，int类型
                                        //domBody[i]["dsdate"] = ListRdRecords[i].dSDate; //结算日期，DateTime类型
                                        //domBody[i]["itax"] = ListRdRecords[i].iTax; //税额，double类型
                                        //domBody[i]["imoney"] = ListRdRecords[i].iMoney; //累计结算金额，double类型
                                        //domBody[i]["isoutquantity"] = ListRdRecords[i].iSOutQuantity; //累计出库数量，double类型
                                        //domBody[i]["isoutnum"] = ListRdRecords[i].iSOutNum; //累计出库件数，double类型
                                        //domBody[i]["ifnum"] = ListRdRecords[i].iFNum; //实际件数，double类型
                                        //domBody[i]["ifquantity"] = ListRdRecords[i].iFQuantity; //实际数量，double类型
                                        //domBody[i]["binvtype"] = ListRdRecords[i].binvtype; //折扣类型，string类型
                                        //domBody[i]["cdefine22"] = ListRdRecords[i].cDefine22; //表体自定义项1，string类型
                                        //domBody[i]["cdefine23"] = ListRdRecords[i].cDefine23; //表体自定义项2，string类型
                                        //domBody[i]["cdefine24"] = ListRdRecords[i].cDefine24; //表体自定义项3，string类型
                                        //domBody[i]["cdefine25"] = ListRdRecords[i].cDefine25; //表体自定义项4，string类型
                                        //domBody[i]["cdefine26"] = ListRdRecords[i].cDefine26; //表体自定义项5，double类型
                                        //domBody[i]["cdefine27"] = ListRdRecords[i].cDefine27; //表体自定义项6，double类型
                                        //domBody[i]["citemcode"] = ListRdRecords[i].cItemCode; //项目编码，string类型
                                        //domBody[i]["citem_class"] = ListRdRecords[i].cItem_class; //项目大类编码，string类型
                                        //domBody[i]["cbatchproperty3"] = ListRdRecords[i].cBatchProperty3; //属性3，double类型
                                        //domBody[i]["cfree3"] = ListRdRecords[i].cFree3; //存货自由项3，string类型
                                        //domBody[i]["cfree4"] = ListRdRecords[i].cFree4; //存货自由项4，string类型
                                        //domBody[i]["cbatchproperty4"] = ListRdRecords[i].cBatchProperty4; //属性4，double类型
                                        //domBody[i]["cbatchproperty5"] = ListRdRecords[i].cBatchProperty5; //属性5，double类型
                                        //domBody[i]["cfree5"] = ListRdRecords[i].cFree5; //存货自由项5，string类型
                                        //domBody[i]["cfree6"] = ListRdRecords[i].cFree6; //存货自由项6，string类型
                                        //domBody[i]["cbatchproperty6"] = ListRdRecords[i].cBatchProperty6; //属性6，string类型
                                        //domBody[i]["cbatchproperty7"] = ListRdRecords[i].cBatchProperty7; //属性7，string类型
                                        //domBody[i]["cfree7"] = ListRdRecords[i].cFree7; //存货自由项7，string类型
                                        //domBody[i]["cfree8"] = ListRdRecords[i].cFree8; //存货自由项8，string类型
                                        //domBody[i]["cbatchproperty8"] = ListRdRecords[i].cBatchProperty8; //属性8，string类型
                                        //domBody[i]["cbatchproperty9"] = ListRdRecords[i].cBatchProperty9; //属性9，string类型
                                        //domBody[i]["cfree9"] = ListRdRecords[i].cFree9; //存货自由项9，string类型
                                        //domBody[i]["cfree10"] = ListRdRecords[i].cFree10; //存货自由项10，string类型
                                        //domBody[i]["cbatchproperty10"] = ListRdRecords[i].cBatchProperty10; //属性10，DateTime类型
                                        //domBody[i]["cdefine28"] = ListRdRecords[i].cDefine28; //表体自定义项7，string类型
                                        //domBody[i]["cdefine29"] = ListRdRecords[i].cDefine29; //表体自定义项8，string类型
                                        //domBody[i]["cdefine30"] = ListRdRecords[i].cDefine30; //表体自定义项9，string类型
                                        //domBody[i]["cdefine31"] = ListRdRecords[i].cDefine31; //表体自定义项10，string类型
                                        //domBody[i]["cdefine32"] = ListRdRecords[i].cDefine32; //表体自定义项11，string类型
                                        //domBody[i]["cdefine33"] = ListRdRecords[i].cDefine33; //表体自定义项12，string类型
                                        //domBody[i]["cdefine34"] = ListRdRecords[i].cDefine34.ToString() == "0" ? "" : ListRdRecords[i].cDefine34.ToString(); //表体自定义项13，int类型
                                        //domBody[i]["cdefine35"] = ListRdRecords[i].cDefine35.ToString() == "0" ? "" : ListRdRecords[i].cDefine35.ToString(); //表体自定义项14，int类型
                                        //domBody[i]["cdefine36"] = ListRdRecords[i].cDefine36; //表体自定义项15，DateTime类型
                                        //domBody[i]["cdefine37"] = ListRdRecords[i].cDefine37; //表体自定义项16，DateTime类型
                                        //domBody[i]["cinvdefine4"] = ListRdRecords[i].cinv; //存货自定义项4，string类型
                                        //domBody[i]["cinvdefine5"] = ""; //存货自定义项5，string类型
                                        //domBody[i]["cinvdefine6"] = ""; //存货自定义项6，string类型
                                        //domBody[i]["cinvdefine7"] = ""; //存货自定义项7，string类型
                                        //domBody[i]["cinvdefine8"] = ""; //存货自定义项8，string类型
                                        //domBody[i]["cinvdefine9"] = ""; //存货自定义项9，string类型
                                        //domBody[i]["cinvdefine10"] = ""; //存货自定义项10，string类型
                                        //domBody[i]["cinvdefine11"] = ""; //存货自定义项11，string类型
                                        //domBody[i]["cinvdefine12"] = ""; //存货自定义项12，string类型
                                        //domBody[i]["cinvdefine13"] = ""; //存货自定义项13，string类型
                                        //domBody[i]["cinvdefine14"] = ""; //存货自定义项14，string类型
                                        //domBody[i]["cinvdefine15"] = ""; //存货自定义项15，string类型
                                        //domBody[i]["cinvdefine16"] = ""; //存货自定义项16，string类型
                                        //domBody[i]["cdemandmemo"] = ListRdRecords[i].cdemandmemo; //需求分类代号说明，string类型
                                        //domBody[i]["cinvdefine1"] = ListRdRecords[i].; //存货自定义项1，string类型
                                        //domBody[i]["cinvdefine2"] = ""; //存货自定义项2，string类型
                                        //domBody[i]["cinvdefine3"] = ""; //存货自定义项3，string类型
                                        //domBody[i]["scrapufts"] = ListRdRecords[i].scrapufts; //不合格品时间戳，string类型
                                        //domBody[i]["creplaceitem"] = ListRdRecords[i].creplaceitem; //替换件，string类型

                                        //domBody[i]["cbarcode"] = ListRdRecords[i].cBarCode; //条形码，string类型
                                        //domBody[i]["impoids"] = ListRdRecords[i].iMPoIds.ToString() == "0" ? "" : ListRdRecords[i].iMPoIds.ToString(); //生产订单子表ID，int类型
                                        //domBody[i]["icheckids"] = ListRdRecords[i].iCheckIds.ToString() == "0" ? "" : ListRdRecords[i].iCheckIds.ToString(); //检验单子表ID，int类型
                                        //domBody[i]["iomodid"] = ListRdRecords[i].iOMoDID.ToString() == "0" ? "" : ListRdRecords[i].iOMoDID.ToString(); //委外订单子表ID，int类型
                                        //domBody[i]["isodid"] = ListRdRecords[i].isodid; //销售订单子表ID，string类型
                                        //domBody[i]["cbvencode"] = ListRdRecords[i].cBVencode; //供应商编码，string类型
                                        //domBody[i]["cvenname"] = ListRdRecords[i].cvenname; //供应商，string类型

                                        //domBody[i]["dmadedate"] = ListRdRecords[i].dMadeDate; //生产日期，DateTime类型
                                        //
                                        //domBody[i]["corufts"] = ListRdRecords[i].corufts; //时间戳，string类型
                                        //domBody[i]["cgspstate"] = ListRdRecords[i].cGspState; //检验状态，double类型
                                        //domBody[i]["icheckidbaks"] = ListRdRecords[i].iCheckIdBaks; //检验单子表id，string类型
                                        //domBody[i]["irejectids"] = ListRdRecords[i].iRejectIds; //不良品处理单id，string类型
                                        //domBody[i]["dcheckdate"] = ListRdRecords[i].dCheckDate; //检验日期，DateTime类型
                                        //domBody[i]["dmsdate"] = ListRdRecords[i].dMSDate; //核销日期，DateTime类型
                                        //domBody[i]["cmassunit"] = ListRdRecords[i].cMassUnit.ToString() == "0" ? "" : ListRdRecords[i].cMassUnit.ToString(); //保质期单位，int类型
                                        //domBody[i]["ccheckcode"] = ListRdRecords[i].cCheckCode; //检验单号，string类型
                                        //domBody[i]["crejectcode"] = ListRdRecords[i].cRejectCode; //不良品处理单号，string类型
                                        //domBody[i]["csocode"] = ListRdRecords[i].csocode; //需求跟踪号，string类型
                                        //domBody[i]["cvmivencode"] = ListRdRecords[i].cvmivencode; //代管商代码，string类型
                                        //domBody[i]["cvmivenname"] = ListRdRecords[i].cvmivenname; //代管商，string类型
                                        //domBody[i]["bvmiused"] = ListRdRecords[i].bVMIUsed.ToString() == "0" ? "" : ListRdRecords[i].bVMIUsed.ToString(); //代管消耗标识，int类型
                                        //domBody[i]["ivmisettlequantity"] = ListRdRecords[i].iVMISettleQuantity; //代管挂账确认单数量，double类型
                                        //domBody[i]["ivmisettlenum"] = ListRdRecords[i].iVMISettleNum; //代管挂账确认单件数，double类型
                                        ////---------/
                                        //domBody[i]["cbarvcode"] = ListRdRecords[i].cbarvcode; //到货单号，string类型
                                        //domBody[i]["dbarvdate"] = ListRdRecords[i].dbarvdate; //到货日期，DateTime类型
                                        //domBody[i]["iordertype"] = ListRdRecords[i].iordertype.ToString() == "0" ? "" : ListRdRecords[i].iordertype.ToString(); //销售订单类别，int类型
                                        //domBody[i]["iorderdid"] = ListRdRecords[i].iorderdid.ToString() == "0" ? "" : ListRdRecords[i].iorderdid.ToString(); //iorderdid，int类型
                                        //domBody[i]["iordercode"] = ListRdRecords[i].iordercode; //销售订单号，string类型
                                        //domBody[i]["iorderseq"] = ListRdRecords[i].iorderseq.ToString() == "0" ? "" : ListRdRecords[i].iorderseq.ToString(); //销售订单行号，string类型
                                        //domBody[i]["iexpiratdatecalcu"] = ListRdRecords[i].iExpiratDateCalcu.ToString() == "0" ? "" : ListRdRecords[i].iExpiratDateCalcu.ToString(); //有效期推算方式，int类型
                                        //domBody[i]["cexpirationdate"] = ListRdRecords[i].cExpirationdate; //有效期至，string类型
                                        //domBody[i]["dexpirationdate"] = ListRdRecords[i].dExpirationdate; //有效期计算项，string类型
                                        //domBody[i]["cciqbookcode"] = ListRdRecords[i].cciqbookcode; //手册号，string类型
                                        //domBody[i]["ibondedsumqty"] = ListRdRecords[i].iBondedSumQty.ToString() == "0" ? "" : ListRdRecords[i].iBondedSumQty.ToString(); //累计保税处理抽取数量，string类型
                                        //domBody[i]["iimosid"] = ListRdRecords[i].iIMOSID.ToString() == "0" ? "" : ListRdRecords[i].iIMOSID.ToString(); //iimosid，string类型
                                        //domBody[i]["iimbsid"] = ListRdRecords[i].iIMBSID.ToString() == "0" ? "" : ListRdRecords[i].iIMBSID.ToString(); //iimbsid，string类型
                                        //domBody[i]["ccheckpersonname"] = ListRdRecords[i].ccheckpersonname; //检验员，string类型
                                        //domBody[i]["ccheckpersoncode"] = ListRdRecords[i].cCheckPersonCode; //检验员编码，string类型
                                        //domBody[i]["strcontractid"] = ListRdRecords[i].strContractId; //合同号，string类型
                                        //domBody[i]["strcode"] = ListRdRecords[i].strCode; //合同标的编码，string类型
                                        //domBody[i]["cveninvcode"] = ListRdRecords[i].cveninvcode; //供应商存货编码，string类型
                                        //domBody[i]["cveninvname"] = ListRdRecords[i].cveninvname; //供应商存货名称，string类型
                                        //domBody[i]["isotype"] = ListRdRecords[i].isotype.ToString() == "0" ? "" : ListRdRecords[i].isotype.ToString(); //需求跟踪方式，int类型
                                        //domBody[i]["isumbillquantity"] = ListRdRecords[i].iSumBillQuantity.ToString() == "0" ? "" : ListRdRecords[i].iSumBillQuantity.ToString(); //累计开票数量，double类型
                                        //domBody[i]["cbaccounter"] = ListRdRecords[i].cbaccounter; //记账人，string类型
                                        //// domBody[i]["bcosting"] = ListRdRecords[i].bCosting; //是否核算，string类型
                                        //domBody[i]["impcost"] = ListRdRecords[i].impcost.ToString() == "0" ? "" : ListRdRecords[i].impcost.ToString(); //最高进价，string类型
                                        //domBody[i]["ioricost"] = ListRdRecords[i].iOriCost.ToString() == "0" ? "" : ListRdRecords[i].iOriCost.ToString(); //原币单价，double类型
                                        //
                                        //
                                        //

                                        //domBody[i]["imaterialfee"] = ListRdRecords[i].iMaterialFee.ToString() == "0" ? "" : ListRdRecords[i].iMaterialFee.ToString(); //材料费，double类型
                                        //domBody[i]["iprocesscost"] = ListRdRecords[i].iProcessCost.ToString() == "0" ? "" : ListRdRecords[i].iProcessCost.ToString(); //加工费单价，double类型
                                        //domBody[i]["iprocessfee"] = ListRdRecords[i].iProcessFee.ToString() == "0" ? "" : ListRdRecords[i].iProcessFee.ToString(); //加工费，double类型
                                        //domBody[i]["ismaterialfee"] = ListRdRecords[i].iSMaterialFee.ToString() == "0" ? "" : ListRdRecords[i].iSMaterialFee.ToString(); //累计结算材料费，double类型
                                        //domBody[i]["isprocessfee"] = ListRdRecords[i].iSProcessFee.ToString() == "0" ? "" : ListRdRecords[i].iSProcessFee.ToString(); //累计结算加工费，double类型
                                        //domBody[i]["isoseq"] = ListRdRecords[i].isoseq.ToString() == "0" ? "" : ListRdRecords[i].isoseq.ToString(); //需求跟踪行号，string类型
                                        //domBody[i]["cposition"] = ListRdRecords[i].cPosition; //货位编码，string类型
                                        //domBody[i]["itrids"] = ListRdRecords[i].iTrIds.ToString() == "0" ? "" : ListRdRecords[i].iTrIds.ToString(); //特殊单据子表标识，double类型
                                        //domBody[i]["cname"] = ListRdRecords[i].cName; //项目名称，string类型
                                        //domBody[i]["citemcname"] = ListRdRecords[i].cItemCName; //项目大类名称，string类型
                                        //domBody[i]["cinvouchcode"] = ListRdRecords[i].cInVouchCode; //对应入库单号，string类型
                                        //domBody[i]["iinvsncount"] = ListRdRecords[i].iInvSNCount.ToString() == "0" ? "" : ListRdRecords[i].iInvSNCount.ToString(); //存库序列号，int类型

                                        //sb.Append("update rdrecords01 set itaxrate='" + itaxrate + "',iUnitCost='" + iUnitPrice + "',fACost='" + iUnitPrice + "',iOriCost='" + iUnitPrice + "',cpoid='" + cpoid + "' where cpoid='gomrit" + recordsId + "';");
                                        //sb.AppendLine();
                                        #endregion

                                    }
                                }
                                //可以自由设置BO对象行数为大于零的整数，也可以不设置而自动增加行数
                                //给BO对象的字段赋值，值可以是真实类型，也可以是无类型字符串
                                //以下代码示例只设置第一行值。各字段定义详见API服务接口定义

                                //给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
                                //broker.AssignNormalValue("domPosition", null);
                                //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值
                                //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
                                //broker.AssignNormalValue("cnnFrom", null);
                                //该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
                                //broker.AssignNormalValue("VouchId","");
                                ////该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                                MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
                                broker.AssignNormalValue("domMsg", domMsg);
                                ////给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
                                ////broker.AssignNormalValue("bCheck", new System.Boolean());
                                broker.AssignNormalValue("bCheck", false);
                                ////给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
                                ////broker.AssignNormalValue("bBeforCheckStock", new System.Boolean());
                                broker.AssignNormalValue("bBeforCheckStock", false);
                                ////给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
                                ////broker.AssignNormalValue("bIsRedVouch", new System.Boolean());
                                broker.AssignNormalValue("bIsRedVouch", ListRdRecord[0].bIsRedVouch);
                                ////给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
                                broker.AssignNormalValue("sAddedState", "");
                                ////给普通参数bReMote赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否远程：转入false
                                ////broker.AssignNormalValue("bReMote", new System.Boolean());
                                broker.AssignNormalValue("bReMote", false);
                                //第六步：调用API
                                #region===调用API
                                if (!broker.Invoke())
                                {
                                    //错误处理
                                    Exception apiEx = broker.GetException();
                                    if (apiEx != null)
                                    {
                                        if (apiEx is MomSysException)
                                        {
                                            MomSysException sysEx = apiEx as MomSysException;
                                            msg.success = false;
                                            msg.msg = "系统异常：" + sysEx.Message;
                                            //todo:异常处理
                                        }
                                        else if (apiEx is MomBizException)
                                        {
                                            MomBizException bizEx = apiEx as MomBizException;
                                            msg.success = false;
                                            msg.msg = "API异常：" + bizEx.Message;
                                            //todo:异常处理
                                        }
                                        //异常原因
                                        String exReason = broker.GetExceptionString();
                                        if (exReason.Length != 0)
                                        {
                                            msg.success = false;
                                            msg.msg = "异常原因：" + exReason;
                                        }
                                    }
                                    //结束本次调用，释放API资源
                                    broker.Release();
                                }
                                #endregion

                                //第七步：获取返回结果
                                //获取返回值
                                //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
                                System.Boolean result = Convert.ToBoolean(broker.GetReturnValue());
                                //获取out/inout参数值
                                //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                                System.String errMsgRet = broker.GetResult("errMsg") as System.String;
                                if (!string.IsNullOrEmpty(errMsgRet))
                                {
                                    msg.success = false;
                                    msg.msg = errMsgRet;
                                }
                                else
                                {
                                    //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                                    System.String VouchIdRet = broker.GetResult("VouchId") as System.String;
                                    //获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
                                    //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));
                                    MSXML2.IXMLDOMDocument2 domMsgRet = (MSXML2.IXMLDOMDocument2)(broker.GetResult("domMsg"));
                                    msg.success = true;
                                    msg.data = VouchIdRet;
                                    //保存成功,同步用友操作
                                    //sb.AppendLine();
                                    //sb.Append("update RdRecord01 set cBusType='普通采购',cSource='采购订单',cdefine2=null where cdefine2='gomrit" + recordId + "';");
                                    //int resSql =SqlHelper.ExecuteNonQuery(sb.ToString());
                                    //if (resSql <= 0)
                                    //{
                                    //    msg.success = false;
                                    //    msg.msg = "单据信息同步失败";
                                    //}
                                }
                                //结束本次调用，释放API资源
                                broker.Release();
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    msg.success = false;
                    msg.msg = "操作失败:" + e.Message;
                }
            }
            return JsonHelper.ToJson(msg).ToString();
        }
        #endregion

        #region ===[采购管理]采购入库单-审核 PuStoreInAudit
        /// <summary>
        /// 采购单审核
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "[采购管理]采购入库单-审核 PuStoreInAudit")]
        public string PuStoreInAudit(string VouchId)
        {
            //试图查询为时间戳
            string ufts = GetUfts(VouchId, "zpurRkdHead", "ufts");
            ModJsonResult msg = Audit(VouchId, "01", "U8API/PuStoreIn/Audit", ufts);
            return JsonHelper.ToJson(msg).ToString();
        }
        #endregion

        #endregion

        #region===[生产管理]材料出库单

        #region ===[生产管理]材料出库单-添加 MaterialOutAdd
        /// <summary>
        /// 材料出库单-添加 MaterialOutAdd
        /// </summary>
        /// <param name="record11">主单据</param>
        /// <param name="records11">子单据</param>
        /// <returns></returns>
        [WebMethod(Description = "[生产管理]材料出库单-添加 MaterialOutAdd")]
        public string MaterialOutAdd(string rdrecord11, string rdrecords11)
        {
            var msg = new ModJsonResult();
            //调用方法事例

            ////主表字段
            //V121RdRecord11 mainModel = new V121RdRecord11();

            //mainModel.AddType =1;//添加类型 0：空白单据 1：引用生产订单
            //mainModel.bIsRedVouch = false;//false:蓝字单据 true:红字单据
            //mainModel.cWhCode = "01"; //仓库编码，string类型(空单必填)
            //mainModel.cMPoCode = "VPU_SMT3_20180315";//生产订单号(添加类型为2,必填)
            //mainModel.cRdCode = "23"; //出库类别编码，string类型(必填)
            //mainModel.dDate = DateTime.Now; //出库日期，DateTime类型
            //mainModel.cDepCode = "0102"; //部门编码，string类型(可选)
            //mainModel.cMemo = "";//备注（可选）
            //mainModel.VT_ID = 65; //默认 模版号，int类型(默认)

            ////转换成json
            //rdrecord11 = "[" + JsonHelper.ToJson(mainModel) + "]";//转换成json

            ////子表字段
            //List<V121RdRecords11> listDetail = new List<V121RdRecords11>();
            //V121RdRecords11 detailModel = new V121RdRecords11();
            //detailModel.AutoID =0;//主键ID(必传)
            //detailModel.cInvCode = "SW01100551";//存货编码
            //detailModel.iQuantity = 1;//数量(必填)
            //detailModel.iUnitCost = 0; //空单必传 单价，double类型
            //detailModel.cBatch = "";//批次号（可选）
            //listDetail.Add(detailModel);
            ////转换成json
            //rdrecords11 = JsonHelper.ToJson(listDetail);//转换成json

            try
            {
                if (string.IsNullOrEmpty(rdrecord11))
                {
                    msg.success = false;
                    msg.msg = "rdrecord11参数不能为空";
                    return JsonHelper.ToJson(msg).ToString();
                }

                #region ===如果当前环境中有login对象则可以省去第一步
                U8Login.clsLogin u8Login = new U8Login.clsLogin();
                ModJsonResult IsLogin = callLogin(ref u8Login);
                #endregion
                if (IsLogin.success == false)
                {
                    msg.success = false;
                    msg.msg = "登陆失败，原因：" + u8Login.ShareString;
                    return JsonHelper.ToJson(msg).ToString();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<V121RdRecord11> Listdispatchlist = serializer.Deserialize<List<V121RdRecord11>>(rdrecord11).ToList();
                    List<V121RdRecords11> Listdispatchslist = serializer.Deserialize<List<V121RdRecords11>>(rdrecords11).ToList();


                    //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                    U8EnvContext envContext = new U8EnvContext();
                    envContext.U8Login = u8Login;
                    //当前API：添加新单据的地址标识为：U8API/saleout/Add
                    U8ApiAddress myApiAddress = new U8ApiAddress("U8API/MaterialOut/Add");
                    //第四步：构造APIBroker
                    U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);
                    //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：11
                    broker.AssignNormalValue("sVouchType", "11");
                    //给BO表头参数DomHead赋值，此BO参数的业务类型为采购入库单，属表头参数。BO参数均按引用传递
                    //提示：给BO表头参数DomHead赋值有两种方法
                    //方法一是直接传入MSXML2.DOMDocumentClass对象
                    //broker.AssignNormalValue("DomHead", new MSXML2.DOMDocumentClass());
                    //方法二是构造BusinessObject对象，具体方法如下：

                    BusinessObject DomHead = broker.GetBoParam("DomHead");
                    DomHead.RowCount = Listdispatchlist.Count; //设置BO对象(表头)行数，只能为一行

                    int mainId = new Random().Next(100);//临时存储主键
                    string ccode = GetVoucherNumnber("0412");//获取单号
                    DomHead[0]["id"] = mainId; //主关键字段，int类型
                    DomHead[0]["ddate"] = Listdispatchlist[0].dDate == null ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(Listdispatchlist[0].dDate).ToShortDateString(); //出库日期，DateTime类型
                    DomHead[0]["ccode"] = ccode; //出库单号，string类型
                    DomHead[0]["vt_id"] = Listdispatchlist[0].VT_ID; //模版号，int类型
                    DomHead[0]["brdflag"] = "0"; //收发标志，string类型 1：入库 0：出库
                    DomHead[0]["cmemo"] = Listdispatchlist[0].cMemo; //备注，string类型
                    DomHead[0]["iswfcontrolled"] = "0"; //iswfcontrolled，string类型
                    DomHead[0]["iverifystate"] = "0"; //iverifystate，string类型
                    DomHead[0]["ireturncount"] = "0"; //ireturncount，string类型
                    DomHead[0]["csysbarcode"] = "||st11|" + ccode; //单据条码，string类型
                    DomHead[0]["dnmaketime"] = DateTime.Now; //制单时间，DateTime类型
                    DomHead[0]["cmaker"] = u8Login.cUserName; //制单人，string类型
                  
                    DomHead[0]["cbustype"] = "领料"; //业务类型，int类型

                    switch (Listdispatchlist[0].AddType)
                    {
                        case 0://添加空白单据
                            if (string.IsNullOrEmpty(rdrecord11) || string.IsNullOrEmpty(rdrecords11))
                            {
                                msg.success = false;
                                msg.msg = "rdrecords11参数不能为空";
                                return JsonHelper.ToJson(msg).ToString();
                            }
                            DomHead[0]["csource"] = "库存"; //单据来源，int类型
                            DomHead[0]["cwhcode"] = Listdispatchlist[0].cWhCode; //仓库编码，string类型
                            DomHead[0]["crdcode"] = Listdispatchlist[0].cRdCode; //出库类别编码，string类型
                            DomHead[0]["cdepcode"] = Listdispatchlist[0].cDepCode; //部门编码，string类型

                            #region== 子单据
                            //方法二是构造BusinessObject对象，具体方法如下：
                            BusinessObject domBody = broker.GetBoParam("domBody");
                            domBody.RowCount = Listdispatchslist.Count; //设置BO对象行数
                            //根据存货编码查询货物具体信息
                            for (int i = 0; i < Listdispatchslist.Count; i++)
                            {
                                DataSet Details = new SqlDAL().GetGoodByCode(Listdispatchslist[i].cInvCode);
                                if (Details.Tables[0].Rows.Count > 0)
                                {
                                    /****************************** 以下是必输字段 ****************************/
                                    domBody[i]["autoid"] = i; //主关键字段，int类型
                                    domBody[i]["id"] = mainId; //与主表关联项，int类型
                                    domBody[i]["cinvcode"] = Listdispatchslist[i].cInvCode; //存货编码，string类型
                                    domBody[i]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                                    domBody[i]["iquantity"] = Listdispatchslist[i].iQuantity; //数量，double类型
                                    domBody[i]["iunitcost"] = Listdispatchslist[i].iUnitCost; //单价，double类型
                                    domBody[i]["iprice"] = Listdispatchslist[i].iQuantity * Listdispatchslist[i].iUnitCost; //金额，double类型
                                    domBody[i]["cbatch"] = Listdispatchslist[i].cBatch; //批号，string类型
                                    domBody[i]["iexpiratdatecalcu"] = "0"; //有效期推算方式，int类型
                                    domBody[i]["bcosting"] = "1"; //是否核算，string类型
                                    domBody[i]["iordertype"] = "0"; //销售订单类别，int类型
                                    domBody[i]["isotype"] = "0"; //需求跟踪方式，int类型
                                    domBody[i]["irowno"] = i + 1; //行号，string类型
                                    domBody[i]["cbsysbarcode"] = "||st11|" + ccode + "|" + domBody[i]["irowno"]; //单据行条码，string类型
                                    domBody[i]["cbmemo"] = Listdispatchslist[i].cbMemo; //备注，string类型
                                    domBody[i]["iinvexchrate"] = Details.Tables[0].Rows[0]["iChangRate"].ToString();//换算率，double类型
                                    domBody[i]["cinva_unit"] = Details.Tables[0].Rows[0]["cinva_unit"].ToString(); //销售单位,辅计量单位名称，string类型
                                    domBody[i]["cinvm_unit"] = Details.Tables[0].Rows[0]["cComUnitCode"].ToString(); //主计量单位，string类型
                                    domBody[i]["iavaquantity"] = ""; //可用量，string类型
                                    domBody[i]["iavanum"] = ""; //可用件数，string类型
                                    domBody[i]["ipresent"] = ""; //现存量，string类型
                                    domBody[i]["iinvsncount"] = ""; //序列号个数，string类型
                                    domBody[i]["imaids"] = ""; //领料申请单子表id，string类型
                                    domBody[i]["csourcemocode"] = ""; //源订单号，string类型
                                    domBody[i]["isourcemodetailsid"] = ""; //源订单子表标识，string类型
                                    domBody[i]["invstd"] = ""; //产品规格，string类型
                                    domBody[i]["applycode"] = ""; //子件补料申请单号，string类型
                                    domBody[i]["applydid"] = ""; //applydid，string类型
                                    domBody[i]["cbinvsn"] = ""; //序列号，string类型
                                    domBody[i]["strowguid"] = ""; //rowguid，string类型
                                    domBody[i]["cservicecode"] = ""; //服务单号，string类型
                                    domBody[i]["cinvouchtype"] = ""; //对应入库单类型，string类型
                                    domBody[i]["coutvouchid"] = ""; //对应蓝字出库单id，string类型
                                    domBody[i]["coutvouchtype"] = ""; //对应蓝字出库单类型，string类型
                                    domBody[i]["isredoutquantity"] = ""; //对应蓝字出库单退回数量，string类型
                                    domBody[i]["isredoutnum"] = ""; //对应蓝字出库单退回件数，string类型
                                    domBody[i]["ipesotype"] = ""; //需求跟踪方式，string类型
                                    domBody[i]["ipesodid"] = ""; //销售订单子表，string类型
                                    domBody[i]["cpesocode"] = ""; //需求跟踪号，string类型
                                    domBody[i]["ipesoseq"] = ""; //需求跟踪行号，string类型
                                    domBody[i]["bsupersede"] = ""; //替代料，string类型
                                    domBody[i]["isupersedeqty"] = ""; //替代数量，string类型
                                    domBody[i]["isupersedempoids"] = ""; //被替代料生产订单子表id，string类型
                                    domBody[i]["imoallocatesubid"] = ""; //替代料子表subid，string类型
                                    domBody[i]["cinvoucherlineid"] = ""; //源单行ID，string类型
                                    domBody[i]["cinvouchercode"] = ""; //源单号，string类型
                                    domBody[i]["cinvouchertype"] = ""; //源单类型，string类型
                                    domBody[i]["ipresentnum"] = ""; //现存件数，string类型
                                    domBody[i]["cplanlotcode"] = ""; //计划批号，string类型
                                    domBody[i]["bcanreplace"] = ""; //可替代，string类型
                                    domBody[i]["taskguid"] = ""; //taskguid，string类型
                                    /***************************** 以下是非必输字段 ****************************/
                                    domBody[i]["cinvaddcode"] = ""; //材料代码，string类型
                                    domBody[i]["cinvname"] = ""; //材料名称，string类型
                                    domBody[i]["cinvstd"] = ""; //规格型号，string类型
                                    domBody[i]["creplaceitem"] = ""; //替换件，string类型
                                    domBody[i]["cposition"] = ""; //货位编码，string类型
                                    domBody[i]["cinvdefine1"] = ""; //存货自定义项1，string类型
                                    domBody[i]["cinvdefine2"] = ""; //存货自定义项2，string类型
                                    domBody[i]["cinvdefine3"] = ""; //存货自定义项3，string类型
                                    domBody[i]["cfree1"] = ""; //存货自由项1，string类型
                                    domBody[i]["cbatchproperty1"] = ""; //批次属性1，double类型
                                    domBody[i]["cbatchproperty2"] = ""; //批次属性2，double类型
                                    domBody[i]["cfree2"] = ""; //存货自由项2，string类型
                                    domBody[i]["inum"] = ""; //件数，double类型
                                    domBody[i]["ipunitcost"] = ""; //计划单价，double类型
                                    domBody[i]["ipprice"] = ""; //计划金额，double类型
                                    domBody[i]["dvdate"] = ""; //失效日期，DateTime类型
                                    domBody[i]["cobjcode"] = ""; //成本对象编码，string类型
                                    domBody[i]["cname"] = ""; //项目，string类型
                                    domBody[i]["isoutquantity"] = ""; //累计出库数量，double类型
                                    domBody[i]["isoutnum"] = ""; //累计出库件数，double类型
                                    domBody[i]["dsdate"] = ""; //结算日期，DateTime类型
                                    domBody[i]["ifquantity"] = ""; //实际数量，double类型
                                    domBody[i]["ifnum"] = ""; //实际件数，double类型
                                    domBody[i]["cvouchcode"] = ""; //对应入库单id，string类型
                                    domBody[i]["cbatchproperty3"] = ""; //批次属性3，double类型
                                    domBody[i]["cfree3"] = ""; //存货自由项3，string类型
                                    domBody[i]["cfree4"] = ""; //存货自由项4，string类型
                                    domBody[i]["cbatchproperty4"] = ""; //批次属性4，double类型
                                    domBody[i]["cbatchproperty5"] = ""; //批次属性5，double类型
                                    domBody[i]["cfree5"] = ""; //存货自由项5，string类型
                                    domBody[i]["cfree6"] = ""; //存货自由项6，string类型
                                    domBody[i]["cbatchproperty6"] = ""; //批次属性6，string类型
                                    domBody[i]["cbatchproperty7"] = ""; //批次属性7，string类型
                                    domBody[i]["cfree7"] = ""; //存货自由项7，string类型
                                    domBody[i]["cfree8"] = ""; //存货自由项8，string类型
                                    domBody[i]["cbatchproperty8"] = ""; //批次属性8，string类型
                                    domBody[i]["cbatchproperty9"] = ""; //批次属性9，string类型
                                    domBody[i]["cfree9"] = ""; //存货自由项9，string类型
                                    domBody[i]["cfree10"] = ""; //存货自由项10，string类型
                                    domBody[i]["cbatchproperty10"] = ""; //批次属性10，DateTime类型
                                    domBody[i]["cdefine36"] = ""; //表体自定义项15，DateTime类型
                                    domBody[i]["cdefine37"] = ""; //表体自定义项16，DateTime类型
                                    domBody[i]["cinvdefine13"] = ""; //存货自定义项13，string类型
                                    domBody[i]["cinvdefine14"] = ""; //存货自定义项14，string类型
                                    domBody[i]["cinvdefine15"] = ""; //存货自定义项15，string类型
                                    domBody[i]["cinvdefine16"] = ""; //存货自定义项16，string类型
                                    domBody[i]["inquantity"] = ""; //应发数量，double类型
                                    domBody[i]["innum"] = ""; //应发件数，double类型
                                    domBody[i]["dmadedate"] = ""; //生产日期，DateTime类型
                                    domBody[i]["impoids"] = ""; //生产订单子表ID，int类型
                                    domBody[i]["isodid"] = ""; //销售订单子表ID，string类型
                                    domBody[i]["iomomid"] = ""; //委外用料表ID，int类型
                                    domBody[i]["iomodid"] = ""; //委外订单子表ID，int类型
                                    domBody[i]["cbvencode"] = ""; //供应商编码，string类型
                                    domBody[i]["cinvouchcode"] = ""; //对应入库单号，string类型
                                    domBody[i]["imassdate"] = ""; //保质期，int类型
                                    domBody[i]["cassunit"] = ""; //库存单位码，string类型
                                    domBody[i]["cvenname"] = ""; //供应商，string类型
                                    domBody[i]["cposname"] = ""; //货位，string类型
                                    domBody[i]["corufts"] = ""; //对应单据时间戳，string类型
                                    domBody[i]["cmolotcode"] = ""; //生产批号，string类型
                                    domBody[i]["dmsdate"] = ""; //核销日期，DateTime类型
                                    domBody[i]["cmassunit"] = ""; //保质期单位，int类型
                                    domBody[i]["csocode"] = ""; //需求跟踪号，string类型
                                    domBody[i]["cmocode"] = ""; //生产订单号，string类型
                                    domBody[i]["comcode"] = ""; //委外订单号，string类型
                                    domBody[i]["cvmivencode"] = ""; //代管商代码，string类型
                                    domBody[i]["cvmivenname"] = ""; //代管商，string类型
                                    domBody[i]["bvmiused"] = ""; //代管消耗标识，int类型
                                    domBody[i]["ivmisettlequantity"] = ""; //代管挂账确认单数量，double类型
                                    domBody[i]["ivmisettlenum"] = ""; //代管挂账确认单件数，double类型
                                    domBody[i]["productinids"] = ""; //productinids，int类型
                                    domBody[i]["crejectcode"] = ""; //在库不良品处理单号，string类型
                                    domBody[i]["cdemandmemo"] = ""; //需求分类代号说明，string类型
                                    domBody[i]["iorderdid"] = ""; //iorderdid，int类型
                                    domBody[i]["iordercode"] = ""; //销售订单号，string类型
                                    domBody[i]["iorderseq"] = ""; //销售订单行号，string类型
                                    domBody[i]["cexpirationdate"] = ""; //有效期至，string类型
                                    domBody[i]["dexpirationdate"] = ""; //有效期计算项，string类型
                                    domBody[i]["cciqbookcode"] = ""; //手册号，string类型
                                    domBody[i]["ibondedsumqty"] = ""; //累计保税处理抽取数量，string类型
                                    domBody[i]["copdesc"] = ""; //工序说明，string类型
                                    domBody[i]["cmworkcentercode"] = ""; //工作中心编码，string类型
                                    domBody[i]["cmworkcenter"] = ""; //工作中心，string类型
                                    domBody[i]["invcode"] = ""; //产品编码，string类型
                                    domBody[i]["invname"] = ""; //产品，string类型
                                    domBody[i]["cwhpersonname"] = ""; //库管员名称，string类型
                                    domBody[i]["cbaccounter"] = ""; //记账人，string类型
                                    domBody[i]["isoseq"] = ""; //需求跟踪行号，string类型
                                    domBody[i]["imoseq"] = ""; //生产订单行号，string类型
                                    domBody[i]["iopseq"] = ""; //工序行号，string类型
                                    domBody[i]["isquantity"] = ""; //累计核销数量，double类型
                                    domBody[i]["ismaterialfee"] = ""; //累计核销金额，double类型
                                    domBody[i]["cdefine34"] = ""; //表体自定义项13，int类型
                                    domBody[i]["cdefine35"] = ""; //表体自定义项14，int类型
                                    domBody[i]["cwhpersoncode"] = ""; //库管员编码，string类型
                                    domBody[i]["cdefine22"] = ""; //表体自定义项1，string类型
                                    domBody[i]["cdefine28"] = ""; //表体自定义项7，string类型
                                    domBody[i]["cdefine29"] = ""; //表体自定义项8，string类型
                                    domBody[i]["cdefine30"] = ""; //表体自定义项9，string类型
                                    domBody[i]["cdefine31"] = ""; //表体自定义项10，string类型
                                    domBody[i]["cdefine32"] = ""; //表体自定义项11，string类型
                                    domBody[i]["cdefine33"] = ""; //表体自定义项12，string类型
                                    domBody[i]["cinvdefine4"] = ""; //存货自定义项4，string类型
                                    domBody[i]["cinvdefine5"] = ""; //存货自定义项5，string类型
                                    domBody[i]["cinvdefine6"] = ""; //存货自定义项6，string类型
                                    domBody[i]["cinvdefine7"] = ""; //存货自定义项7，string类型
                                    domBody[i]["cinvdefine8"] = ""; //存货自定义项8，string类型
                                    domBody[i]["cinvdefine9"] = ""; //存货自定义项9，string类型
                                    domBody[i]["cinvdefine10"] = ""; //存货自定义项10，string类型
                                    domBody[i]["cinvdefine11"] = ""; //存货自定义项11，string类型
                                    domBody[i]["cinvdefine12"] = ""; //存货自定义项12，string类型
                                    domBody[i]["cbarcode"] = ""; //条形码，string类型
                                    domBody[i]["cdefine23"] = ""; //表体自定义项2，string类型
                                    domBody[i]["cdefine24"] = ""; //表体自定义项3，string类型
                                    domBody[i]["itrids"] = ""; //特殊单据子表标识，double类型
                                    domBody[i]["cdefine25"] = ""; //表体自定义项4，string类型
                                    domBody[i]["cdefine26"] = ""; //表体自定义项5，double类型
                                    domBody[i]["cdefine27"] = ""; //表体自定义项6，double类型
                                    domBody[i]["citemcode"] = ""; //项目编码，string类型
                                    domBody[i]["citem_class"] = ""; //项目大类编码，string类型
                                    domBody[i]["citemcname"] = ""; //项目大类名称，string类型
                                }
                            }
                            #endregion

                            break;
                        case 1://根据生产订单号添加单据
                            if (string.IsNullOrEmpty(rdrecord11))
                            {
                                msg.success = false;
                                msg.msg = "rdrecord11参数不能为空";
                                return JsonHelper.ToJson(msg).ToString();
                            }
                            if (string.IsNullOrEmpty(Listdispatchlist[0].cMPoCode))
                            {
                                msg.success = false;
                                msg.msg = "cMPoCode参数不能为空";
                                return JsonHelper.ToJson(msg).ToString();
                            }
                            DomHead[0]["csource"] = "生产订单"; //单据来源，int类型
                            #region====根据生产订单编号获取信息
                            DataSet mom_order = new SqlDAL().GetMomOrder(Listdispatchlist[0].cMPoCode);
                            DataSet mom_orderDetail = new SqlDAL().GetMomOrderDetails(Listdispatchlist[0].cMPoCode);
                            if (mom_order.Tables[0].Rows.Count > 0)
                            {
                                if (mom_orderDetail.Tables[0].Rows.Count <= 0)
                                {
                                    msg.success = false;
                                    msg.msg = "单据明细数据为空。";
                                    return JsonHelper.ToJson(msg).ToString();
                                }

                                //获取仓库
                                string cwhcode = mom_orderDetail.Tables[0].Rows[0]["whcode"].ToString();
                                string cdepcode = mom_orderDetail.Tables[0].Rows[0]["Dept"].ToString();

                                DomHead[0]["cwhcode"] = string.IsNullOrEmpty(Listdispatchlist[0].cWhCode) == true ? cwhcode : Listdispatchlist[0].cWhCode; //仓库编码，string类型
                                DomHead[0]["crdcode"] = Listdispatchlist[0].cRdCode; //出库类别编码，string类型
                                DomHead[0]["cdepcode"] = string.IsNullOrEmpty(Listdispatchlist[0].cDepCode) == true ? cdepcode : Listdispatchlist[0].cDepCode; //部门编码，string类型

                                //DomHead[0]["cdefine2"] = mom_order.Tables[0].Rows[0]["define2"].ToString(); //表头自定义项2，string类型
                                //DomHead[0]["cdefine3"] = mom_order.Tables[0].Rows[0]["define3"].ToString(); //表头自定义项3，string类型
                                //DomHead[0]["cdefine4"] = mom_order.Tables[0].Rows[0]["define4"].ToString();
                                //DomHead[0]["cdefine5"] = mom_order.Tables[0].Rows[0]["define5"].ToString();
                                //DomHead[0]["cdefine6"] = mom_order.Tables[0].Rows[0]["define6"].ToString();
                                //DomHead[0]["cdefine7"] = mom_order.Tables[0].Rows[0]["define7"].ToString();
                                //DomHead[0]["cdefine8"] = mom_order.Tables[0].Rows[0]["define8"].ToString();
                                //DomHead[0]["cdefine9"] = mom_order.Tables[0].Rows[0]["define9"].ToString();
                                //DomHead[0]["cdefine10"] = mom_order.Tables[0].Rows[0]["define10"].ToString();
                                DomHead[0]["imquantity"] = mom_order.Tables[0].Rows[0]["impqty"].ToString(); //产量，double类型
                                DomHead[0]["cpspcode"] = mom_order.Tables[0].Rows[0]["invcode"].ToString(); ; //产品编码，string类型
                                DomHead[0]["cmpocode"] = mom_order.Tables[0].Rows[0]["MoCode"].ToString(); //订单号，string类型
                                DomHead[0]["iproorderid"] = mom_orderDetail.Tables[0].Rows[0]["MoDid"].ToString(); ; //生产订单ID，string类型

                                #region== 子单据
                                //方法二是构造BusinessObject对象，具体方法如下：
                                domBody = broker.GetBoParam("domBody");
                                DataSet MomDetail = new SqlDAL().GetMomDetail(Listdispatchlist[0].cMPoCode);
                                if (Listdispatchslist.Count > 0)
                                {
                                    int RowCount = 0;
                                    #region==获取行号
                                    for (int i = 0; i < Listdispatchslist.Count; i++)
                                    {
                                        for (int j = 0; j < mom_orderDetail.Tables[0].Rows.Count; j++)
                                        {
                                            if (Listdispatchslist[i].cInvCode == mom_orderDetail.Tables[0].Rows[j]["invcode"].ToString())
                                            {
                                                RowCount++;
                                            }
                                        }
                                    }
                                    #endregion

                                    domBody.RowCount = RowCount; //设置BO对象行数

                                    var inRowCount = 0; 
                                    for (int i = 0; i < Listdispatchslist.Count; i++)
                                    {
                                        for (int j = 0; j < mom_orderDetail.Tables[0].Rows.Count; j++)
                                        {
                                            if (Listdispatchslist[i].cInvCode == mom_orderDetail.Tables[0].Rows[j]["invcode"].ToString() && Listdispatchslist[i].AutoID == long.Parse(mom_orderDetail.Tables[0].Rows[j]["MoDId"].ToString()))
                                            {
                                                /****************************** 以下是必输字段 ****************************/
                                                domBody[inRowCount]["autoid"] = i; //主关键字段，int类型
                                                domBody[inRowCount]["id"] = mainId; //与主表关联项，int类型
                                                domBody[inRowCount]["cinvcode"] = Listdispatchslist[i].cInvCode; //存货编码，string类型
                                                domBody[inRowCount]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                                                domBody[inRowCount]["iquantity"] = Listdispatchslist[i].iQuantity; //数量，double类型
                                                domBody[inRowCount]["inquantity"] = Listdispatchslist[i].iQuantity; //应发数量，double类型
                                                domBody[inRowCount]["impoids"] = mom_orderDetail.Tables[0].Rows[j]["AllocateId"].ToString(); //生产订单子表ID，int类型
                                                domBody[inRowCount]["cmocode"] = Listdispatchlist[0].cMPoCode; //生产订单号，string类型
                                                domBody[inRowCount]["iunitcost"] = Listdispatchslist[i].iUnitCost; //单价，double类型
                                                domBody[inRowCount]["iprice"] = Listdispatchslist[i].iQuantity * Listdispatchslist[i].iUnitCost; //金额，double类型
                                                domBody[inRowCount]["cbatch"] = Listdispatchslist[i].cBatch; //批号，string类型
                                                domBody[inRowCount]["cinvstd"] = mom_orderDetail.Tables[0].Rows[j]["invstd"].ToString(); //规格型号，string类型
                                                string invcode = "";
                                                if (MomDetail.Tables[0].Rows.Count > 0)
                                                {
                                                    invcode = MomDetail.Tables[0].Rows[0]["InvCode"].ToString();
                                                }
                                                domBody[inRowCount]["invcode"] = invcode; //产品编码，string类型
                                                domBody[inRowCount]["imoseq"] = i + 1; //生产订单行号，string类型
                                                domBody[inRowCount]["iunitcost"] = "";// Listdispatchslist[i].iUnitCost; //单价，double类型
                                                domBody[inRowCount]["iprice"] = "";// Listdispatchslist[i].iQuantity * Listdispatchslist[i].iUnitCost; //金额，double类型
                                                domBody[inRowCount]["cbatch"] = "";// Listdispatchslist[i].cBatch; //批号，string类型
                                                domBody[inRowCount]["iexpiratdatecalcu"] = "0"; //有效期推算方式，int类型
                                                domBody[inRowCount]["bcosting"] = "1"; //是否核算，string类型
                                                domBody[inRowCount]["iordertype"] = "0"; //销售订单类别，int类型
                                                domBody[inRowCount]["isotype"] = "0"; //需求跟踪方式，int类型
                                                domBody[inRowCount]["irowno"] = i + 1; //行号，string类型
                                                domBody[inRowCount]["cbsysbarcode"] = "||st11|" + ccode + "|" + domBody[inRowCount]["irowno"]; //单据行条码，string类型
                                                domBody[inRowCount]["cbmemo"] = Listdispatchslist[i].cbMemo; //备注，string类型

                                                inRowCount++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    domBody.RowCount = mom_orderDetail.Tables[0].Rows.Count;
                                    for (int i = 0; i < mom_orderDetail.Tables[0].Rows.Count; i++)
                                    {
                                        /****************************** 以下是必输字段 ****************************/
                                        domBody[i]["autoid"] = i; //主关键字段，int类型
                                        domBody[i]["id"] = mainId; //与主表关联项，int类型
                                        domBody[i]["cinvcode"] = mom_orderDetail.Tables[0].Rows[i]["invcode"].ToString(); //存货编码，string类型
                                        domBody[i]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                                        domBody[i]["iquantity"] = mom_orderDetail.Tables[0].Rows[i]["Qty"].ToString(); //数量，double类型
                                        domBody[i]["inquantity"] = mom_orderDetail.Tables[0].Rows[i]["Qty"].ToString(); //应发数量，double类型
                                        domBody[i]["impoids"] = mom_orderDetail.Tables[0].Rows[i]["AllocateId"].ToString(); //生产订单子表ID，int类型
                                        domBody[i]["cmocode"] = Listdispatchlist[0].cMPoCode; //生产订单号，string类型
                                        string invcode = "";
                                        if (MomDetail.Tables[0].Rows.Count > 0)
                                        {
                                            invcode = MomDetail.Tables[0].Rows[0]["InvCode"].ToString();
                                        }
                                        domBody[i]["cinvstd"] = mom_orderDetail.Tables[0].Rows[i]["invstd"].ToString(); //规格型号，string类型

                                        domBody[i]["invcode"] = invcode; //产品编码，string类型
                                        domBody[i]["imoseq"] = i+1; //生产订单行号，string类型
                                        domBody[i]["iunitcost"] = "";// Listdispatchslist[i].iUnitCost; //单价，double类型
                                        domBody[i]["iprice"] = "";// Listdispatchslist[i].iQuantity * Listdispatchslist[i].iUnitCost; //金额，double类型
                                        domBody[i]["cbatch"] = "";// Listdispatchslist[i].cBatch; //批号，string类型
                                        domBody[i]["iexpiratdatecalcu"] = "0"; //有效期推算方式，int类型
                                        domBody[i]["bcosting"] = "1"; //是否核算，string类型
                                        domBody[i]["iordertype"] = "0"; //销售订单类别，int类型
                                        domBody[i]["isotype"] = "0"; //需求跟踪方式，int类型
                                        domBody[i]["irowno"] = i + 1; //行号，string类型
                                        domBody[i]["cbsysbarcode"] = "||st11|" + ccode + "|" + domBody[i]["irowno"]; //单据行条码，string类型
                                        domBody[i]["cbmemo"] = ""; //备注，string类型
                                        domBody[i]["iinvexchrate"] = "";// Details.Tables[0].Rows[0]["iChangRate"].ToString();//换算率，double类型
                                        domBody[i]["cinva_unit"] = "";//Details.Tables[0].Rows[0]["cinva_unit"].ToString(); //销售单位,辅计量单位名称，string类型
                                        domBody[i]["cinvm_unit"] = "";// Details.Tables[0].Rows[0]["cComUnitCode"].ToString(); //主计量单位，string类型
                                        domBody[i]["iavaquantity"] = ""; //可用量，string类型
                                        domBody[i]["iavanum"] = ""; //可用件数，string类型
                                        domBody[i]["ipresent"] = ""; //现存量，string类型
                                        domBody[i]["iinvsncount"] = ""; //序列号个数，string类型
                                        domBody[i]["imaids"] = ""; //领料申请单子表id，string类型
                                        domBody[i]["csourcemocode"] = ""; //源订单号，string类型
                                        domBody[i]["isourcemodetailsid"] = ""; //源订单子表标识，string类型
                                        domBody[i]["invstd"] = ""; //产品规格，string类型
                                        domBody[i]["applycode"] = ""; //子件补料申请单号，string类型
                                        domBody[i]["applydid"] = ""; //applydid，string类型
                                        domBody[i]["cbinvsn"] = ""; //序列号，string类型
                                        domBody[i]["strowguid"] = ""; //rowguid，string类型
                                        domBody[i]["cservicecode"] = ""; //服务单号，string类型
                                        domBody[i]["cinvouchtype"] = ""; //对应入库单类型，string类型
                                        domBody[i]["coutvouchid"] = ""; //对应蓝字出库单id，string类型
                                        domBody[i]["coutvouchtype"] = ""; //对应蓝字出库单类型，string类型
                                        domBody[i]["isredoutquantity"] = ""; //对应蓝字出库单退回数量，string类型
                                        domBody[i]["isredoutnum"] = ""; //对应蓝字出库单退回件数，string类型
                                        domBody[i]["ipesotype"] = ""; //需求跟踪方式，string类型
                                        domBody[i]["ipesodid"] = ""; //销售订单子表，string类型
                                        domBody[i]["cpesocode"] = ""; //需求跟踪号，string类型
                                        domBody[i]["ipesoseq"] = ""; //需求跟踪行号，string类型
                                        domBody[i]["bsupersede"] = ""; //替代料，string类型
                                        domBody[i]["isupersedeqty"] = ""; //替代数量，string类型
                                        domBody[i]["isupersedempoids"] = ""; //被替代料生产订单子表id，string类型
                                        domBody[i]["imoallocatesubid"] = ""; //替代料子表subid，string类型
                                        domBody[i]["cinvoucherlineid"] = ""; //源单行ID，string类型
                                        domBody[i]["cinvouchercode"] = ""; //源单号，string类型
                                        domBody[i]["cinvouchertype"] = ""; //源单类型，string类型
                                        domBody[i]["ipresentnum"] = ""; //现存件数，string类型
                                        domBody[i]["cplanlotcode"] = ""; //计划批号，string类型
                                        domBody[i]["bcanreplace"] = ""; //可替代，string类型
                                        domBody[i]["taskguid"] = ""; //taskguid，string类型
                                        /***************************** 以下是非必输字段 ****************************/
                                        domBody[i]["cinvaddcode"] = ""; //材料代码，string类型
                                        domBody[i]["cinvname"] = ""; //材料名称，string类型
                                     
                                        domBody[i]["creplaceitem"] = ""; //替换件，string类型
                                        domBody[i]["cposition"] = ""; //货位编码，string类型
                                        domBody[i]["cinvdefine1"] = ""; //存货自定义项1，string类型
                                        domBody[i]["cinvdefine2"] = ""; //存货自定义项2，string类型
                                        domBody[i]["cinvdefine3"] = ""; //存货自定义项3，string类型
                                        domBody[i]["cfree1"] = ""; //存货自由项1，string类型
                                        domBody[i]["cbatchproperty1"] = ""; //批次属性1，double类型
                                        domBody[i]["cbatchproperty2"] = ""; //批次属性2，double类型
                                        domBody[i]["cfree2"] = ""; //存货自由项2，string类型
                                        domBody[i]["inum"] = ""; //件数，double类型
                                        domBody[i]["ipunitcost"] = ""; //计划单价，double类型
                                        domBody[i]["ipprice"] = ""; //计划金额，double类型
                                        domBody[i]["dvdate"] = ""; //失效日期，DateTime类型
                                        domBody[i]["cobjcode"] = ""; //成本对象编码，string类型
                                        domBody[i]["cname"] = ""; //项目，string类型
                                        domBody[i]["isoutquantity"] = ""; //累计出库数量，double类型
                                        domBody[i]["isoutnum"] = ""; //累计出库件数，double类型
                                        domBody[i]["dsdate"] = ""; //结算日期，DateTime类型
                                        domBody[i]["ifquantity"] = ""; //实际数量，double类型
                                        domBody[i]["ifnum"] = ""; //实际件数，double类型
                                        domBody[i]["cvouchcode"] = ""; //对应入库单id，string类型
                                        domBody[i]["cbatchproperty3"] = ""; //批次属性3，double类型
                                        domBody[i]["cfree3"] = ""; //存货自由项3，string类型
                                        domBody[i]["cfree4"] = ""; //存货自由项4，string类型
                                        domBody[i]["cbatchproperty4"] = ""; //批次属性4，double类型
                                        domBody[i]["cbatchproperty5"] = ""; //批次属性5，double类型
                                        domBody[i]["cfree5"] = ""; //存货自由项5，string类型
                                        domBody[i]["cfree6"] = ""; //存货自由项6，string类型
                                        domBody[i]["cbatchproperty6"] = ""; //批次属性6，string类型
                                        domBody[i]["cbatchproperty7"] = ""; //批次属性7，string类型
                                        domBody[i]["cfree7"] = ""; //存货自由项7，string类型
                                        domBody[i]["cfree8"] = ""; //存货自由项8，string类型
                                        domBody[i]["cbatchproperty8"] = ""; //批次属性8，string类型
                                        domBody[i]["cbatchproperty9"] = ""; //批次属性9，string类型
                                        domBody[i]["cfree9"] = ""; //存货自由项9，string类型
                                        domBody[i]["cfree10"] = ""; //存货自由项10，string类型
                                        domBody[i]["cbatchproperty10"] = ""; //批次属性10，DateTime类型
                                        domBody[i]["cdefine36"] = ""; //表体自定义项15，DateTime类型
                                        domBody[i]["cdefine37"] = ""; //表体自定义项16，DateTime类型
                                        domBody[i]["cinvdefine13"] = ""; //存货自定义项13，string类型
                                        domBody[i]["cinvdefine14"] = ""; //存货自定义项14，string类型
                                        domBody[i]["cinvdefine15"] = ""; //存货自定义项15，string类型
                                        domBody[i]["cinvdefine16"] = ""; //存货自定义项16，string类型
                                      
                                        domBody[i]["innum"] = ""; //应发件数，double类型
                                        domBody[i]["dmadedate"] = ""; //生产日期，DateTime类型
                                      
                                        domBody[i]["isodid"] = ""; //销售订单子表ID，string类型
                                        domBody[i]["iomomid"] = ""; //委外用料表ID，int类型
                                        domBody[i]["iomodid"] = ""; //委外订单子表ID，int类型
                                        domBody[i]["cbvencode"] = ""; //供应商编码，string类型
                                        domBody[i]["cinvouchcode"] = ""; //对应入库单号，string类型
                                        domBody[i]["imassdate"] = ""; //保质期，int类型
                                        domBody[i]["cassunit"] = ""; //库存单位码，string类型
                                        domBody[i]["cvenname"] = ""; //供应商，string类型
                                        domBody[i]["cposname"] = ""; //货位，string类型
                                        domBody[i]["corufts"] = ""; //对应单据时间戳，string类型
                                        domBody[i]["cmolotcode"] = ""; //生产批号，string类型
                                        domBody[i]["dmsdate"] = ""; //核销日期，DateTime类型
                                        domBody[i]["cmassunit"] = ""; //保质期单位，int类型
                                        domBody[i]["csocode"] = ""; //需求跟踪号，string类型
                                    
                                        domBody[i]["comcode"] = ""; //委外订单号，string类型
                                        domBody[i]["cvmivencode"] = ""; //代管商代码，string类型
                                        domBody[i]["cvmivenname"] = ""; //代管商，string类型
                                        domBody[i]["bvmiused"] = ""; //代管消耗标识，int类型
                                        domBody[i]["ivmisettlequantity"] = ""; //代管挂账确认单数量，double类型
                                        domBody[i]["ivmisettlenum"] = ""; //代管挂账确认单件数，double类型
                                        domBody[i]["productinids"] = ""; //productinids，int类型
                                        domBody[i]["crejectcode"] = ""; //在库不良品处理单号，string类型
                                        domBody[i]["cdemandmemo"] = ""; //需求分类代号说明，string类型
                                        domBody[i]["iorderdid"] = ""; //iorderdid，int类型
                                        domBody[i]["iordercode"] = ""; //销售订单号，string类型
                                        domBody[i]["iorderseq"] = ""; //销售订单行号，string类型
                                        domBody[i]["cexpirationdate"] = ""; //有效期至，string类型
                                        domBody[i]["dexpirationdate"] = ""; //有效期计算项，string类型
                                        domBody[i]["cciqbookcode"] = ""; //手册号，string类型
                                        domBody[i]["ibondedsumqty"] = ""; //累计保税处理抽取数量，string类型
                                        domBody[i]["copdesc"] = ""; //工序说明，string类型
                                        domBody[i]["cmworkcentercode"] = ""; //工作中心编码，string类型
                                        domBody[i]["cmworkcenter"] = ""; //工作中心，string类型
                                        domBody[i]["invname"] = ""; //产品，string类型
                                        domBody[i]["cwhpersonname"] = ""; //库管员名称，string类型
                                        domBody[i]["cbaccounter"] = ""; //记账人，string类型
                                        domBody[i]["isoseq"] = ""; //需求跟踪行号，string类型
                                        domBody[i]["iopseq"] = ""; //工序行号，string类型
                                        domBody[i]["isquantity"] = ""; //累计核销数量，double类型
                                        domBody[i]["ismaterialfee"] = ""; //累计核销金额，double类型
                                        domBody[i]["cdefine34"] = ""; //表体自定义项13，int类型
                                        domBody[i]["cdefine35"] = ""; //表体自定义项14，int类型
                                        domBody[i]["cwhpersoncode"] = ""; //库管员编码，string类型
                                        domBody[i]["cdefine22"] = ""; //表体自定义项1，string类型
                                        domBody[i]["cdefine28"] = ""; //表体自定义项7，string类型
                                        domBody[i]["cdefine29"] = ""; //表体自定义项8，string类型
                                        domBody[i]["cdefine30"] = ""; //表体自定义项9，string类型
                                        domBody[i]["cdefine31"] = ""; //表体自定义项10，string类型
                                        domBody[i]["cdefine32"] = ""; //表体自定义项11，string类型
                                        domBody[i]["cdefine33"] = ""; //表体自定义项12，string类型
                                        domBody[i]["cinvdefine4"] = ""; //存货自定义项4，string类型
                                        domBody[i]["cinvdefine5"] = ""; //存货自定义项5，string类型
                                        domBody[i]["cinvdefine6"] = ""; //存货自定义项6，string类型
                                        domBody[i]["cinvdefine7"] = ""; //存货自定义项7，string类型
                                        domBody[i]["cinvdefine8"] = ""; //存货自定义项8，string类型
                                        domBody[i]["cinvdefine9"] = ""; //存货自定义项9，string类型
                                        domBody[i]["cinvdefine10"] = ""; //存货自定义项10，string类型
                                        domBody[i]["cinvdefine11"] = ""; //存货自定义项11，string类型
                                        domBody[i]["cinvdefine12"] = ""; //存货自定义项12，string类型
                                        domBody[i]["cbarcode"] = ""; //条形码，string类型
                                        domBody[i]["cdefine23"] = ""; //表体自定义项2，string类型
                                        domBody[i]["cdefine24"] = ""; //表体自定义项3，string类型
                                        domBody[i]["itrids"] = ""; //特殊单据子表标识，double类型
                                        domBody[i]["cdefine25"] = ""; //表体自定义项4，string类型
                                        domBody[i]["cdefine26"] = ""; //表体自定义项5，double类型
                                        domBody[i]["cdefine27"] = ""; //表体自定义项6，double类型
                                        domBody[i]["citemcode"] = ""; //项目编码，string类型
                                        domBody[i]["citem_class"] = ""; //项目大类编码，string类型
                                        domBody[i]["citemcname"] = ""; //项目大类名称，string类型
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                msg.success = false;
                                msg.msg = "订单信息获取失败";
                                return JsonHelper.ToJson(msg).ToString();
                            }
                            #endregion

                            break;
                    }

                    #region===调用API
                    //给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
                    //broker.AssignNormalValue("domPosition", null);
                    //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值
                    //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
                    //broker.AssignNormalValue("cnnFrom", null);
                    //该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
                    //broker.AssignNormalValue("VouchId","");
                    ////该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                    MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
                    broker.AssignNormalValue("domMsg", domMsg);
                    ////给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
                    ////broker.AssignNormalValue("bCheck", new System.Boolean());
                    broker.AssignNormalValue("bCheck", false);
                    ////给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
                    ////broker.AssignNormalValue("bBeforCheckStock", new System.Boolean());
                    broker.AssignNormalValue("bBeforCheckStock", false);
                    ////给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
                    ////broker.AssignNormalValue("bIsRedVouch", new System.Boolean());
                    broker.AssignNormalValue("bIsRedVouch", false);
                    ////给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
                    broker.AssignNormalValue("sAddedState", "");
                    ////给普通参数bReMote赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否远程：转入false
                    ////broker.AssignNormalValue("bReMote", new System.Boolean());
                    broker.AssignNormalValue("bReMote", false);
                    //第六步：调用API
                    if (!broker.Invoke())
                    {
                        //错误处理
                        Exception apiEx = broker.GetException();
                        if (apiEx != null)
                        {
                            if (apiEx is MomSysException)
                            {
                                MomSysException sysEx = apiEx as MomSysException;
                                msg.success = false;
                                msg.msg = "系统异常：" + sysEx.Message;
                            }
                            else if (apiEx is MomBizException)
                            {
                                MomBizException bizEx = apiEx as MomBizException;
                                msg.success = false;
                                msg.msg = "API异常：" + bizEx.Message;
                            }
                            //异常原因
                            String exReason = broker.GetExceptionString();
                            if (exReason.Length != 0)
                            {
                                msg.success = false;
                                msg.msg = "异常原因：" + exReason;
                            }
                        }
                        //结束本次调用，释放API资源
                        broker.Release();
                    }
                    #endregion
                    //第七步：获取返回结果
                    //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功返回空串
                    //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
                    System.Boolean result = Convert.ToBoolean(broker.GetReturnValue());
                    if (result == false)
                    {
                        msg.success = false;
                        //获取out/inout参数值
                        //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                        System.String errMsgRet = broker.GetResult("errMsg") as System.String;
                        if (!string.IsNullOrEmpty(errMsgRet))
                        {
                            msg.msg = errMsgRet;
                        }
                    }
                    else
                    {
                        ////获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                        System.String VouchIdRet = broker.GetResult("VouchId") as System.String;
                        msg.data = VouchIdRet;
                        ////获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
                        //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));
                    }
                    //结束本次调用，释放API资源
                    broker.Release();

                }
            }
            catch (Exception e)
            {
                msg.success = false;
                msg.msg = "操作失败:" + e.Message;
            }
            return JsonHelper.ToJson(msg).ToString();
        }
        #endregion

        #region ===[生产管理]材料出库单-审核 MaterialOutAudit
        /// <summary>
        /// 材料出库单审核
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "[生产管理]材料出库单-审核 MaterialOutAudit")]
        public string MaterialOutAudit(string VouchId)
        {
            //试图查询为时间戳
            string ufts = GetUfts(VouchId, "RecordOutQ", "ufts");
            ModJsonResult msg = Audit(VouchId, "11", "U8API/MaterialOut/Audit", ufts);
            return JsonHelper.ToJson(msg).ToString();
        }
        #endregion


        #endregion

        #region===[生产管理]产成品入库单

        #region ===[生产管理]产成品入库单-添加 ProductInAdd
        /// <summary>
        /// 产成品入库单-添加 ProductInAdd
        /// </summary>
        /// <param name="dispatchlist">主单据</param>
        /// <param name="dispatchlists">子单据</param>
        /// <returns></returns>
        [WebMethod(Description = "[生产管理]产成品入库单-添加 ProductInAdd")]
        public string ProductInAdd(string rdrecord10, string rdrecords10)
        {
            var msg = new ModJsonResult();
            ////调用方法事例
            ////主表字段
            //V121RdRecord10 mainModel = new V121RdRecord10();
            //mainModel.AddType = 1;//添加类型 0：空白单据 1：引用生产订单(蓝或红)
            //mainModel.bIsRedVouch = false;//false:蓝字单据 true:红字单据
            //mainModel.cMPoCode = "VPU_SMT1_20180123"; //生产订单号，string类型
            //mainModel.VT_ID = 63; //默认 模版号，int类型
            //mainModel.cWhCode = ""; //仓库编码，string类型
            //mainModel.cDepCode = ""; //部门编码，string类型
            //mainModel.cRdCode = "10"; //入库类别编码(必填)
            //mainModel.dDate = DateTime.Now; //入库日期，DateTime类型
            //mainModel.cMemo = "WebSerivce自动生成"; //备注，string类型
            ////转换成json
            //rdrecord10 = "[" + JsonHelper.ToJson(mainModel) + "]";//转换成json

            ////子表字段
            //List<V121RdRecords10> listDetail = new List<V121RdRecords10>();
            //V121RdRecords10 detailModel = new V121RdRecords10();
            //detailModel.AutoID = 0;//主键ID(必传)
            //detailModel.cInvCode = "2016102001";//存货编码
            //detailModel.iQuantity = 1;//数量
            //detailModel.iUnitCost =0; //单价(可选)
            //detailModel.cBatch = "";//批次号(可选)
            //listDetail.Add(detailModel);
            ////转换成json
            //rdrecords10 = JsonHelper.ToJson(listDetail);//转换成json

            if (string.IsNullOrEmpty(rdrecord10))
            {
                msg.success = false;
                msg.msg = "rdrecord10参数不能为空";
                return JsonHelper.ToJson(msg).ToString();
            }
            try
            {
                #region ===如果当前环境中有login对象则可以省去第一步
                U8Login.clsLogin u8Login = new U8Login.clsLogin();
                ModJsonResult IsLogin = callLogin(ref u8Login);
                #endregion
                if (IsLogin.success == false)
                {
                    msg.success = false;
                    msg.msg = "登陆失败，原因：" + u8Login.ShareString;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<V121RdRecord10> Listdispatchlist = serializer.Deserialize<List<V121RdRecord10>>(rdrecord10).ToList();
                    List<V121RdRecords10> Listdispatchslist = serializer.Deserialize<List<V121RdRecords10>>(rdrecords10).ToList();

                    //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                    U8EnvContext envContext = new U8EnvContext();
                    envContext.U8Login = u8Login;
                    //当前API：添加新单据的地址标识为：U8API/saleout/Add
                    U8ApiAddress myApiAddress = new U8ApiAddress("U8API/ProductIn/Add");
                    //第四步：构造APIBroker
                    U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);
                    //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：10
                    broker.AssignNormalValue("sVouchType", "10");
                    //给BO表头参数DomHead赋值，此BO参数的业务类型为采购入库单，属表头参数。BO参数均按引用传递
                    //提示：给BO表头参数DomHead赋值有两种方法
                    //方法一是直接传入MSXML2.DOMDocumentClass对象
                    //broker.AssignNormalValue("DomHead", new MSXML2.DOMDocumentClass());
                    //方法二是构造BusinessObject对象，具体方法如下：
                    BusinessObject DomHead = broker.GetBoParam("DomHead");
                    DomHead.RowCount = Listdispatchlist.Count; //设置BO对象(表头)行数，只能为一行

                    int mainId = new Random().Next(100);//临时存储主键
                    string ccode = GetVoucherNumnber("0411");
                    DomHead[0]["id"] = mainId; //主关键字段，int类型
                    DomHead[0]["ddate"] = Listdispatchlist[0].dDate == null ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(Listdispatchlist[0].dDate).ToShortDateString(); //入库日期，DateTime类型
                    DomHead[0]["ccode"] = ccode; //单据号，string类型
                    DomHead[0]["vt_id"] = Listdispatchlist[0].VT_ID; //模版号，int类型                                              
                    DomHead[0]["cmemo"] = Listdispatchlist[0].cMemo; //备注，string类型
                    DomHead[0]["cbustype"] = "成品入库"; //业务类型，int类型
                    DomHead[0]["brdflag"] = "1"; //收发标志，string类型
                    DomHead[0]["iswfcontrolled"] = "0"; //iswfcontrolled，string类型
                    DomHead[0]["iverifystate"] = "0"; //iverifystate，string类型
                    DomHead[0]["ireturncount"] = "0"; //ireturncount，string类型
                    DomHead[0]["csysbarcode"] = "||st10|" + ccode; //单据条码，string类型
                    DomHead[0]["dnmaketime"] = DateTime.Now; //制单时间，DateTime类型
                    DomHead[0]["cmaker"] = u8Login.cUserName; //制单人，string类型
                    DomHead[0]["csource"] = "生产订单";// Enum.GetName(typeof(EnumCsource), int.Parse(Listdispatchlist[0].cSource)); ;//单据来源，int类型

                    switch (Listdispatchlist[0].AddType)
                    {
                        case 0://空单

                            /****************************** 以下是必输字段 ****************************/
                            DomHead[0]["cwhcode"] = Listdispatchlist[0].cWhCode; //仓库编码，string类型
                            DomHead[0]["crdcode"] = Listdispatchlist[0].cRdCode; //入库类别编码，string类型
                            DomHead[0]["cdepcode"] = Listdispatchlist[0].cDepCode; //部门编码，string类型

                            #region== 子单据
                            //方法二是构造BusinessObject对象，具体方法如下：
                            BusinessObject domBody = broker.GetBoParam("domBody");
                            domBody.RowCount = Listdispatchslist.Count; //设置BO对象行数
                            //根据存货编码查询货物具体信息
                            for (int i = 0; i < Listdispatchslist.Count; i++)
                            {
                                DataSet Details = new SqlDAL().GetGoodByCode(Listdispatchslist[i].cInvCode);
                                if (Details.Tables[0].Rows.Count > 0)
                                {
                                    /****************************** 以下是必输字段 ****************************/
                                    domBody[i]["autoid"] = i; //主关键字段，int类型
                                    domBody[i]["id"] = mainId; //与主表关联项，int类型
                                    domBody[i]["cinvcode"] = Listdispatchslist[i].cInvCode; //存货编码，string类型
                                    domBody[i]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                                    domBody[i]["iquantity"] = Listdispatchslist[i].iQuantity; //数量，double类型
                                    domBody[i]["iunitcost"] = Listdispatchslist[i].iUnitCost; //单价，double类型
                                    domBody[i]["iprice"] = Listdispatchslist[i].iQuantity * Listdispatchslist[i].iUnitCost; //金额，double类型
                                    domBody[i]["cbatch"] = Listdispatchslist[i].cBatch; //批号，string类型
                                    domBody[i]["iexpiratdatecalcu"] = "0"; //有效期推算方式，int类型
                                    domBody[i]["bcosting"] = "1"; //是否核算，string类型
                                    domBody[i]["iordertype"] = "0"; //销售订单类别，int类型
                                    domBody[i]["isotype"] = "0"; //需求跟踪方式，int类型
                                    domBody[i]["irowno"] = i + 1; //行号，string类型
                                    domBody[i]["cbsysbarcode"] = "||st10|" + ccode + "|" + domBody[i]["irowno"]; //单据行条码，string类型
                                    domBody[i]["cbmemo"] =""; //备注，string类型
                                    domBody[i]["iinvexchrate"] = Details.Tables[0].Rows[0]["iChangRate"].ToString();//换算率，double类型
                                    domBody[i]["cinva_unit"] = Details.Tables[0].Rows[0]["cinva_unit"].ToString(); //销售单位,辅计量单位名称，string类型
                                    domBody[i]["cinvm_unit"] = Details.Tables[0].Rows[0]["cComUnitCode"].ToString(); //主计量单位，string类型

                                    domBody[i]["ccheckcode"] = ""; //检验单号，string类型
                                    domBody[i]["icheckidbaks"] = ""; //检验单子表id，string类型
                                    domBody[i]["crejectcode"] = ""; //不良品处理单号，string类型
                                    domBody[i]["irejectids"] = ""; //不良品处理单id，string类型
                                    domBody[i]["ccheckpersonname"] = ""; //检验员，string类型
                                    domBody[i]["dcheckdate"] = ""; //检验日期，string类型
                                    domBody[i]["ccheckpersoncode"] = ""; //检验员编码，string类型
                                    domBody[i]["cinvouchtype"] = ""; //对应入库单类型，string类型
                                    domBody[i]["outcopiedquantity"] = ""; //已复制数量，string类型
                                    domBody[i]["cbinvsn"] = ""; //序列号，string类型
                                    domBody[i]["strowguid"] = ""; //rowguid，string类型
                                    domBody[i]["cservicecode"] = ""; //服务单号，string类型
                                    domBody[i]["cplanlotcode"] = ""; //计划批号，string类型
                                    domBody[i]["taskguid"] = ""; //taskguid，string类型
                                    domBody[i]["iinvsncount"] = ""; //已指定序列号量，string类型

                                    /***************************** 以下是非必输字段 ****************************/

                                    domBody[i]["cinvaddcode"] = ""; //产品代码，string类型
                                    domBody[i]["cinvname"] = ""; //产品名称，string类型
                                    domBody[i]["cinvstd"] = ""; //规格型号，string类型
                                    domBody[i]["creplaceitem"] = ""; //替换件，string类型
                                    domBody[i]["cposition"] = ""; //货位编码，string类型
                                    domBody[i]["cinvdefine1"] = ""; //存货自定义项1，string类型
                                    domBody[i]["cinvdefine2"] = ""; //存货自定义项2，string类型
                                    domBody[i]["cinvdefine3"] = ""; //存货自定义项3，string类型
                                    domBody[i]["cfree1"] = ""; //存货自由项1，string类型
                                    domBody[i]["cbatchproperty1"] = ""; //批次属性1，double类型
                                    domBody[i]["cbatchproperty2"] = ""; //批次属性2，double类型
                                    domBody[i]["cfree2"] = ""; //存货自由项2，string类型
                                    domBody[i]["inum"] = ""; //件数，double类型
                                    domBody[i]["ipunitcost"] = ""; //计划单价，double类型
                                    domBody[i]["ipprice"] = ""; //计划金额，double类型
                                    domBody[i]["dvdate"] = ""; //失效日期，DateTime类型
                                    domBody[i]["isoutquantity"] = ""; //累计出库数量，double类型
                                    domBody[i]["isoutnum"] = ""; //累计出库件数，double类型
                                    domBody[i]["dsdate"] = ""; //结算日期，DateTime类型
                                    domBody[i]["ifquantity"] = ""; //实际数量，double类型
                                    domBody[i]["ifnum"] = ""; //实际件数，double类型
                                    domBody[i]["cvouchcode"] = ""; //对应入库单id，string类型
                                    domBody[i]["cfree3"] = ""; //存货自由项3，string类型
                                    domBody[i]["cbatchproperty3"] = ""; //批次属性3，double类型
                                    domBody[i]["cbatchproperty4"] = ""; //批次属性4，double类型
                                    domBody[i]["cfree4"] = ""; //存货自由项4，string类型
                                    domBody[i]["cfree5"] = ""; //存货自由项5，string类型
                                    domBody[i]["cbatchproperty5"] = ""; //批次属性5，double类型
                                    domBody[i]["cbatchproperty6"] = ""; //批次属性6，string类型
                                    domBody[i]["cfree6"] = ""; //存货自由项6，string类型
                                    domBody[i]["cfree7"] = ""; //存货自由项7，string类型
                                    domBody[i]["cbatchproperty7"] = ""; //批次属性7，string类型
                                    domBody[i]["cbatchproperty8"] = ""; //批次属性8，string类型
                                    domBody[i]["cfree8"] = ""; //存货自由项8，string类型
                                    domBody[i]["cfree9"] = ""; //存货自由项9，string类型
                                    domBody[i]["cbatchproperty9"] = ""; //批次属性9，string类型
                                    domBody[i]["cbatchproperty10"] = ""; //批次属性10，DateTime类型
                                    domBody[i]["cfree10"] = ""; //存货自由项10，string类型
                                    domBody[i]["cdefine36"] = ""; //表体自定义项15，DateTime类型
                                    domBody[i]["cdefine37"] = ""; //表体自定义项16，DateTime类型
                                    domBody[i]["cinvdefine13"] = ""; //存货自定义项13，string类型
                                    domBody[i]["cinvdefine14"] = ""; //存货自定义项14，string类型
                                    domBody[i]["cinvdefine15"] = ""; //存货自定义项15，string类型
                                    domBody[i]["cinvdefine16"] = ""; //存货自定义项16，string类型
                                    domBody[i]["inquantity"] = ""; //应收数量，double类型
                                    domBody[i]["innum"] = ""; //应收件数，double类型
                                    domBody[i]["dmadedate"] = ""; //生产日期，DateTime类型
                                    domBody[i]["impoids"] = ""; //生产订单子表ID，int类型
                                    domBody[i]["icheckids"] = ""; //检验单子表ID，int类型
                                    domBody[i]["isodid"] = ""; //销售订单子表ID，string类型
                                    domBody[i]["brelated"] = ""; //是否联副产品，int类型
                                    domBody[i]["cbvencode"] = ""; //供应商编码，string类型
                                    domBody[i]["cinvouchcode"] = ""; //对应入库单号，string类型
                                    domBody[i]["cvenname"] = ""; //供应商，string类型
                                    domBody[i]["imassdate"] = ""; //保质期，int类型
                                    domBody[i]["cassunit"] = ""; //库存单位码，string类型
                                    domBody[i]["corufts"] = ""; //对应单据时间戳，string类型
                                    domBody[i]["cposname"] = ""; //货位，string类型
                                    domBody[i]["cmolotcode"] = ""; //生产批号，string类型
                                    domBody[i]["cmassunit"] = ""; //保质期单位，int类型
                                    domBody[i]["csocode"] = ""; //需求跟踪号，string类型
                                    domBody[i]["cmocode"] = ""; //生产订单号，string类型
                                    domBody[i]["cvmivencode"] = ""; //代管商代码，string类型
                                    domBody[i]["cvmivenname"] = ""; //代管商，string类型
                                    domBody[i]["bvmiused"] = ""; //代管消耗标识，int类型
                                    domBody[i]["ivmisettlequantity"] = ""; //代管挂账确认单数量，double类型
                                    domBody[i]["ivmisettlenum"] = ""; //代管挂账确认单件数，double类型
                                    domBody[i]["cdemandmemo"] = ""; //需求分类代号说明，string类型
                                    domBody[i]["iorderdid"] = ""; //iorderdid，int类型
                                    domBody[i]["iordercode"] = ""; //销售订单号，string类型
                                    domBody[i]["iorderseq"] = ""; //销售订单行号，string类型
                                    domBody[i]["cexpirationdate"] = ""; //有效期至，string类型
                                    domBody[i]["dexpirationdate"] = ""; //有效期计算项，string类型
                                    domBody[i]["cciqbookcode"] = ""; //手册号，string类型
                                    domBody[i]["ibondedsumqty"] = ""; //累计保税处理抽取数量，string类型
                                    domBody[i]["copdesc"] = ""; //工序说明，string类型
                                    domBody[i]["cmworkcentercode"] = ""; //工作中心编码，string类型
                                    domBody[i]["cmworkcenter"] = ""; //工作中心，string类型
                                    domBody[i]["cbaccounter"] = ""; //记账人，string类型
                                    domBody[i]["isoseq"] = ""; //需求跟踪行号，string类型
                                    domBody[i]["imoseq"] = ""; //生产订单行号，string类型
                                    domBody[i]["iopseq"] = ""; //工序行号，string类型
                                    domBody[i]["cdefine34"] = ""; //表体自定义项13，int类型
                                    domBody[i]["cdefine35"] = ""; //表体自定义项14，int类型
                                    domBody[i]["cdefine22"] = ""; //表体自定义项1，string类型
                                    domBody[i]["cdefine28"] = ""; //表体自定义项7，string类型
                                    domBody[i]["cdefine29"] = ""; //表体自定义项8，string类型
                                    domBody[i]["cdefine30"] = ""; //表体自定义项9，string类型
                                    domBody[i]["cdefine31"] = ""; //表体自定义项10，string类型
                                    domBody[i]["cdefine32"] = ""; //表体自定义项11，string类型
                                    domBody[i]["cdefine33"] = ""; //表体自定义项12，string类型
                                    domBody[i]["cinvdefine4"] = ""; //存货自定义项4，string类型
                                    domBody[i]["cinvdefine5"] = ""; //存货自定义项5，string类型
                                    domBody[i]["cinvdefine6"] = ""; //存货自定义项6，string类型
                                    domBody[i]["cinvdefine7"] = ""; //存货自定义项7，string类型
                                    domBody[i]["cinvdefine8"] = ""; //存货自定义项8，string类型
                                    domBody[i]["cinvdefine9"] = ""; //存货自定义项9，string类型
                                    domBody[i]["cinvdefine10"] = ""; //存货自定义项10，string类型
                                    domBody[i]["cinvdefine11"] = ""; //存货自定义项11，string类型
                                    domBody[i]["cinvdefine12"] = ""; //存货自定义项12，string类型
                                    domBody[i]["cbarcode"] = ""; //条形码，string类型
                                    domBody[i]["cdefine23"] = ""; //表体自定义项2，string类型
                                    domBody[i]["cdefine24"] = ""; //表体自定义项3，string类型
                                    domBody[i]["cdefine25"] = ""; //表体自定义项4，string类型
                                    domBody[i]["itrids"] = ""; //特殊单据子表标识，double类型
                                    domBody[i]["cdefine26"] = ""; //表体自定义项5，double类型
                                    domBody[i]["cdefine27"] = ""; //表体自定义项6，double类型
                                    domBody[i]["citemcode"] = ""; //项目编码，string类型
                                    domBody[i]["cname"] = ""; //项目，string类型
                                    domBody[i]["citem_class"] = ""; //项目大类编码，string类型
                                    domBody[i]["citemcname"] = ""; //项目大类名称，string类型
                                }
                            }
                            #endregion


                            break;
                        case 1:

                            #region==存在生产订单号的情况

                            DataSet mom_order = new SqlDAL().GetMomOrderByProductIn(Listdispatchlist[0].cMPoCode);
                            DataSet mom_orderDetail = new SqlDAL().GetMomOrderDetailsByProductIn(Listdispatchlist[0].cMPoCode);
                            if (mom_order.Tables[0].Rows.Count > 0)
                            {
                                if (mom_orderDetail.Tables[0].Rows.Count <= 0)
                                {
                                    msg.success = false;
                                    msg.msg = "单据明细数据为空。";
                                    return JsonHelper.ToJson(msg).ToString();
                                }
                                #region===主单据
                                //获取仓库
                                string cwhcode = mom_orderDetail.Tables[0].Rows[0]["whcode"].ToString();
                                string cdepcode = mom_orderDetail.Tables[0].Rows[0]["MDEPT"].ToString();

                                DomHead[0]["cdefine2"] = mom_order.Tables[0].Rows[0]["define2"].ToString(); //表头自定义项2，string类型
                                DomHead[0]["cdefine3"] = mom_order.Tables[0].Rows[0]["define3"].ToString(); //表头自定义项3，string类型

                                DomHead[0]["cwhcode"] = string.IsNullOrEmpty(Listdispatchlist[0].cWhCode) == true ? cwhcode : Listdispatchlist[0].cWhCode; //仓库编码，string类型
                                DomHead[0]["crdcode"] = Listdispatchlist[0].cRdCode; //出库类别编码，string类型
                                DomHead[0]["cdepcode"] = string.IsNullOrEmpty(Listdispatchlist[0].cDepCode) == true ? cdepcode : Listdispatchlist[0].cDepCode; //部门编码，string类型

                                /****************************** 以下是必输字段 ****************************/
                                //DomHead[0]["chinvsn"] = ""; //序列号，string类型
                                DomHead[0]["cmpocode"] = Listdispatchlist[0].cMPoCode; //生产订单号，string类型
                                DomHead[0]["iproorderid"] = mom_order.Tables[0].Rows[0]["MoId"].ToString(); //生产订单ID，string类型
                                DomHead[0]["csysbarcode"] = "||st10|" + ccode; //单据条码，string类型
                                /***************************** 以下是非必输字段 ****************************/
                                DomHead[0]["cmodifyperson"] = ""; //修改人，string类型
                                DomHead[0]["dmodifydate"] = ""; //修改日期，DateTime类型
                                DomHead[0]["dnmodifytime"] = ""; //修改时间，DateTime类型
                                DomHead[0]["dnverifytime"] = ""; //审核时间，DateTime类型
                                DomHead[0]["dchkdate"] = ""; //检验日期，DateTime类型
                                DomHead[0]["iavaquantity"] = ""; //可用量，string类型
                                DomHead[0]["iavanum"] = ""; //可用件数，string类型
                                DomHead[0]["ipresentnum"] = ""; //现存件数，string类型
                                DomHead[0]["ufts"] = ""; //时间戳，string类型
                                DomHead[0]["cpspcode"] = ""; //产品，string类型
                                DomHead[0]["cprobatch"] = ""; //生产批号，string类型
                                DomHead[0]["cdepname"] = ""; //部门，string类型
                                DomHead[0]["crdname"] = ""; //入库类别，string类型
                                DomHead[0]["dveridate"] = ""; //审核日期，DateTime类型
                                DomHead[0]["cchkperson"] = ""; //检验员，string类型
                                DomHead[0]["chandler"] = ""; //审核人，string类型
                                DomHead[0]["itopsum"] = ""; //最高库存量，string类型
                                DomHead[0]["caccounter"] = ""; //记账人，string类型
                                DomHead[0]["ilowsum"] = ""; //最低库存量，string类型
                                DomHead[0]["ipresent"] = ""; //现存量，string类型
                                DomHead[0]["isafesum"] = ""; //安全库存量，string类型
                                DomHead[0]["cpersonname"] = ""; //业务员，string类型
                                DomHead[0]["cdefine1"] = ""; //表头自定义项1，string类型
                                DomHead[0]["cdefine11"] = ""; //表头自定义项11，string类型
                                DomHead[0]["cdefine12"] = ""; //表头自定义项12，string类型
                                DomHead[0]["cdefine13"] = ""; //表头自定义项13，string类型
                                DomHead[0]["cdefine14"] = ""; //表头自定义项14，string类型
                             
                                DomHead[0]["cdefine5"] = ""; //表头自定义项5，int类型
                                DomHead[0]["cdefine15"] = ""; //表头自定义项15，int类型
                                DomHead[0]["cdefine6"] = ""; //表头自定义项6，DateTime类型
                                DomHead[0]["cdefine7"] = ""; //表头自定义项7，double类型
                                DomHead[0]["cdefine16"] = ""; //表头自定义项16，double类型
                                DomHead[0]["cdefine8"] = ""; //表头自定义项8，string类型
                                DomHead[0]["cdefine9"] = ""; //表头自定义项9，string类型
                                DomHead[0]["cdefine10"] = ""; //表头自定义项10，string类型
                                DomHead[0]["cvouchtype"] = ""; //单据类型，string类
                                DomHead[0]["cpersoncode"] = ""; //业务员编码，string类型
                                DomHead[0]["cdefine4"] = ""; //表头自定义项4，DateTime类型
                                #endregion


                                #region== 子单据
                                //方法二是构造BusinessObject对象，具体方法如下：
                                domBody = broker.GetBoParam("domBody");
                                DataSet MomDetail = new SqlDAL().GetMomDetail(Listdispatchlist[0].cMPoCode);
                                if (Listdispatchslist.Count > 0)
                                {
                                    int RowCount = 0;
                                    #region==获取行号
                                    for (int j = 0; j < Listdispatchslist.Count; j++)
                                    {
                                        for (int i = 0; i < mom_orderDetail.Tables[0].Rows.Count; i++)
                                        {
                                            if (Listdispatchslist[i].cInvCode == mom_orderDetail.Tables[0].Rows[i]["invcode"].ToString())
                                            {
                                                RowCount++;
                                            }
                                        }
                                    }
                                    #endregion

                                    domBody.RowCount = RowCount; //设置BO对象行数

                                    var inRowCount = 0; 
                                    for (int j = 0; j < Listdispatchslist.Count; j++)
                                    {
                                        for (int i = 0; i < mom_orderDetail.Tables[0].Rows.Count; i++)
                                        {
                                            if (Listdispatchslist[i].cInvCode == mom_orderDetail.Tables[0].Rows[i]["invcode"].ToString() && Listdispatchslist[i].AutoID == long.Parse(mom_orderDetail.Tables[0].Rows[i]["autoid"].ToString()))
                                            {
                                                /****************************** 以下是必输字段 ****************************/
                                                domBody[inRowCount]["autoid"] = i; //主关键字段，int类型
                                                domBody[inRowCount]["id"] = mainId; //与主表关联项，int类型
                                                domBody[inRowCount]["cinvcode"] = mom_orderDetail.Tables[0].Rows[i]["invcode"].ToString();  //存货编码，string类型
                                                domBody[inRowCount]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                                                domBody[inRowCount]["iquantity"] = Listdispatchslist[i].iQuantity > 0 ? Listdispatchslist[i].iQuantity.ToString() : mom_orderDetail.Tables[0].Rows[i]["Qty"].ToString(); //数量，double类型
                                                domBody[inRowCount]["inquantity"] = Listdispatchslist[i].iQuantity > 0 ? Listdispatchslist[i].iQuantity.ToString() : mom_orderDetail.Tables[0].Rows[i]["Qty"].ToString(); //应收数量，double类型
                                                domBody[inRowCount]["iunitcost"] = Listdispatchslist[i].iUnitCost; //单价，double类型
                                                domBody[inRowCount]["impoids"] = mom_orderDetail.Tables[0].Rows[i]["MODID"].ToString(); //生产订单子表ID，int类型
                                                domBody[inRowCount]["cmocode"] = Listdispatchlist[0].cMPoCode; //生产订单号，string类型
                                                string invcode = "";
                                                if (MomDetail.Tables[0].Rows.Count > 0)
                                                {
                                                    invcode = MomDetail.Tables[0].Rows[0]["InvCode"].ToString();
                                                }
                                                domBody[inRowCount]["cinvm_unit"] = ""; //主计量单位，string类型
                                                domBody[inRowCount]["cinvstd"] = mom_orderDetail.Tables[0].Rows[i]["invstd"].ToString(); //规格型号，
                                                //domBody[inRowCount]["iprice"] = Listdispatchslist[i].iQuantity * Listdispatchslist[i].iUnitCost; //金额，double类型
                                                domBody[inRowCount]["cbatch"] = Listdispatchslist[i].cBatch; //批号，string类型
                                                domBody[inRowCount]["iexpiratdatecalcu"] = "0"; //有效期推算方式，int类型
                                                domBody[inRowCount]["bcosting"] = "1"; //是否核算，string类型
                                                domBody[inRowCount]["iordertype"] = "0"; //销售订单类别，int类型
                                                domBody[inRowCount]["isotype"] = "0"; //需求跟踪方式，int类型
                                                domBody[inRowCount]["irowno"] = i + 1; //行号，string类型
                                                domBody[inRowCount]["cbsysbarcode"] = "||st10|" + ccode + "|" + domBody[inRowCount]["irowno"]; //单据行条码，string类型
                                                domBody[inRowCount]["cbmemo"] = ""; //备注，string类型
                                                domBody[inRowCount]["iinvexchrate"] = "";//换算率，double类型
                                                domBody[inRowCount]["cinva_unit"] = ""; //销售单位,辅计量单位名称，string类型
                                                domBody[inRowCount]["cinvm_unit"] = ""; //主计量单位，string类型
                                                domBody[inRowCount]["ccheckcode"] = ""; //检验单号，string类型
                                                domBody[inRowCount]["icheckidbaks"] = ""; //检验单子表id，string类型
                                                domBody[inRowCount]["crejectcode"] = ""; //不良品处理单号，string类型
                                                domBody[inRowCount]["irejectids"] = ""; //不良品处理单id，string类型
                                                domBody[inRowCount]["ccheckpersonname"] = ""; //检验员，string类型
                                                domBody[inRowCount]["dcheckdate"] = ""; //检验日期，string类型
                                                domBody[inRowCount]["ccheckpersoncode"] = ""; //检验员编码，string类型
                                                domBody[inRowCount]["cinvouchtype"] = ""; //对应入库单类型，string类型
                                                domBody[inRowCount]["outcopiedquantity"] = ""; //已复制数量，string类型
                                                domBody[inRowCount]["cbinvsn"] = ""; //序列号，string类型
                                                domBody[inRowCount]["strowguid"] = ""; //rowguid，string类型
                                                domBody[inRowCount]["cservicecode"] = ""; //服务单号，string类型
                                                domBody[inRowCount]["cplanlotcode"] = ""; //计划批号，string类型
                                                domBody[inRowCount]["taskguid"] = ""; //taskguid，string类型
                                                domBody[inRowCount]["iinvsncount"] = ""; //已指定序列号量，string类型

                                                inRowCount++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    domBody.RowCount = mom_orderDetail.Tables[0].Rows.Count; //设置BO对象行数
                                    for (int i = 0; i < mom_orderDetail.Tables[0].Rows.Count; i++)
                                    {
                                        /****************************** 以下是必输字段 ****************************/
                                        domBody[i]["autoid"] = i; //主关键字段，int类型
                                        domBody[i]["id"] = mainId; //与主表关联项，int类型
                                        domBody[i]["cinvcode"] = mom_orderDetail.Tables[0].Rows[i]["invcode"].ToString();  //存货编码，string类型
                                        domBody[i]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                                        domBody[i]["iquantity"] = mom_orderDetail.Tables[0].Rows[i]["Qty"].ToString(); //数量，double类型
                                        domBody[i]["inquantity"] = mom_orderDetail.Tables[0].Rows[i]["Qty"].ToString(); //应收数量，double类型
                                        //domBody[i]["iunitcost"] = Listdispatchslist[i].iUnitCost; //单价，double类型
                                        domBody[i]["impoids"] = mom_orderDetail.Tables[0].Rows[i]["MODID"].ToString(); //生产订单子表ID，int类型
                                        domBody[i]["cmocode"] = Listdispatchlist[0].cMPoCode; //生产订单号，string类型
                                        string invcode = "";
                                        if (MomDetail.Tables[0].Rows.Count > 0)
                                        {
                                            invcode = MomDetail.Tables[0].Rows[0]["InvCode"].ToString();
                                        }
                                        domBody[i]["cinvstd"] = mom_orderDetail.Tables[0].Rows[i]["invstd"].ToString(); //规格型号，
                                        //domBody[i]["iprice"] = Listdispatchslist[i].iQuantity * Listdispatchslist[i].iUnitCost; //金额，double类型
                                        domBody[i]["cbatch"] =""; //批号，string类型
                                        domBody[i]["iexpiratdatecalcu"] = "0"; //有效期推算方式，int类型
                                        domBody[i]["bcosting"] = "1"; //是否核算，string类型
                                        domBody[i]["iordertype"] = "0"; //销售订单类别，int类型
                                        domBody[i]["isotype"] = "0"; //需求跟踪方式，int类型
                                        domBody[i]["irowno"] = i + 1; //行号，string类型
                                        domBody[i]["cbsysbarcode"] = "||st10|" + ccode + "|" + domBody[i]["irowno"]; //单据行条码，string类型
                                        domBody[i]["cbmemo"] = ""; //备注，string类型
                                        domBody[i]["iinvexchrate"] = "";//换算率，double类型
                                        domBody[i]["cinva_unit"] =""; //销售单位,辅计量单位名称，string类型
                                        domBody[i]["cinvm_unit"] =""; //主计量单位，string类型
                                        domBody[i]["ccheckcode"] = ""; //检验单号，string类型
                                        domBody[i]["icheckidbaks"] = ""; //检验单子表id，string类型
                                        domBody[i]["crejectcode"] = ""; //不良品处理单号，string类型
                                        domBody[i]["irejectids"] = ""; //不良品处理单id，string类型
                                        domBody[i]["ccheckpersonname"] = ""; //检验员，string类型
                                        domBody[i]["dcheckdate"] = ""; //检验日期，string类型
                                        domBody[i]["ccheckpersoncode"] = ""; //检验员编码，string类型
                                        domBody[i]["cinvouchtype"] = ""; //对应入库单类型，string类型
                                        domBody[i]["outcopiedquantity"] = ""; //已复制数量，string类型
                                        domBody[i]["cbinvsn"] = ""; //序列号，string类型
                                        domBody[i]["strowguid"] = ""; //rowguid，string类型
                                        domBody[i]["cservicecode"] = ""; //服务单号，string类型
                                        domBody[i]["cplanlotcode"] = ""; //计划批号，string类型
                                        domBody[i]["taskguid"] = ""; //taskguid，string类型
                                        domBody[i]["iinvsncount"] = ""; //已指定序列号量，string类型
                                        /***************************** 以下是非必输字段 ****************************/

                                        domBody[i]["cinvaddcode"] = ""; //产品代码，string类型
                                        domBody[i]["cinvname"] = ""; //产品名称，string类型                                    
                                        domBody[i]["creplaceitem"] = ""; //替换件，string类型
                                        domBody[i]["cposition"] = ""; //货位编码，string类型
                                        domBody[i]["cinvdefine1"] = ""; //存货自定义项1，string类型
                                        domBody[i]["cinvdefine2"] = ""; //存货自定义项2，string类型
                                        domBody[i]["cinvdefine3"] = ""; //存货自定义项3，string类型
                                        domBody[i]["cfree1"] = ""; //存货自由项1，string类型
                                        domBody[i]["cbatchproperty1"] = ""; //批次属性1，double类型
                                        domBody[i]["cbatchproperty2"] = ""; //批次属性2，double类型
                                        domBody[i]["cfree2"] = ""; //存货自由项2，string类型
                                        domBody[i]["inum"] = ""; //件数，double类型
                                        domBody[i]["ipunitcost"] = ""; //计划单价，double类型
                                        domBody[i]["ipprice"] = ""; //计划金额，double类型
                                        domBody[i]["dvdate"] = ""; //失效日期，DateTime类型
                                        domBody[i]["isoutquantity"] = ""; //累计出库数量，double类型
                                        domBody[i]["isoutnum"] = ""; //累计出库件数，double类型
                                        domBody[i]["dsdate"] = ""; //结算日期，DateTime类型
                                        domBody[i]["ifquantity"] = ""; //实际数量，double类型
                                        domBody[i]["ifnum"] = ""; //实际件数，double类型
                                        domBody[i]["cvouchcode"] = ""; //对应入库单id，string类型
                                        domBody[i]["cfree3"] = ""; //存货自由项3，string类型
                                        domBody[i]["cbatchproperty3"] = ""; //批次属性3，double类型
                                        domBody[i]["cbatchproperty4"] = ""; //批次属性4，double类型
                                        domBody[i]["cfree4"] = ""; //存货自由项4，string类型
                                        domBody[i]["cfree5"] = ""; //存货自由项5，string类型
                                        domBody[i]["cbatchproperty5"] = ""; //批次属性5，double类型
                                        domBody[i]["cbatchproperty6"] = ""; //批次属性6，string类型
                                        domBody[i]["cfree6"] = ""; //存货自由项6，string类型
                                        domBody[i]["cfree7"] = ""; //存货自由项7，string类型
                                        domBody[i]["cbatchproperty7"] = ""; //批次属性7，string类型
                                        domBody[i]["cbatchproperty8"] = ""; //批次属性8，string类型
                                        domBody[i]["cfree8"] = ""; //存货自由项8，string类型
                                        domBody[i]["cfree9"] = ""; //存货自由项9，string类型
                                        domBody[i]["cbatchproperty9"] = ""; //批次属性9，string类型
                                        domBody[i]["cbatchproperty10"] = ""; //批次属性10，DateTime类型
                                        domBody[i]["cfree10"] = ""; //存货自由项10，string类型
                                        domBody[i]["cdefine36"] = ""; //表体自定义项15，DateTime类型
                                        domBody[i]["cdefine37"] = ""; //表体自定义项16，DateTime类型
                                        domBody[i]["cinvdefine13"] = ""; //存货自定义项13，string类型
                                        domBody[i]["cinvdefine14"] = ""; //存货自定义项14，string类型
                                        domBody[i]["cinvdefine15"] = ""; //存货自定义项15，string类型
                                        domBody[i]["cinvdefine16"] = ""; //存货自定义项16，string类型                                     
                                        domBody[i]["innum"] = ""; //应收件数，double类型
                                        domBody[i]["dmadedate"] = ""; //生产日期，DateTime类型                                    
                                        domBody[i]["icheckids"] = ""; //检验单子表ID，int类型
                                        domBody[i]["isodid"] = ""; //销售订单子表ID，string类型
                                        domBody[i]["brelated"] = ""; //是否联副产品，int类型
                                        domBody[i]["cbvencode"] = ""; //供应商编码，string类型
                                        domBody[i]["cinvouchcode"] = ""; //对应入库单号，string类型
                                        domBody[i]["cvenname"] = ""; //供应商，string类型
                                        domBody[i]["imassdate"] = ""; //保质期，int类型
                                        domBody[i]["cassunit"] = ""; //库存单位码，string类型
                                        domBody[i]["corufts"] = ""; //对应单据时间戳，string类型
                                        domBody[i]["cposname"] = ""; //货位，string类型
                                        domBody[i]["cmolotcode"] = ""; //生产批号，string类型
                                        domBody[i]["cmassunit"] = ""; //保质期单位，int类型
                                        domBody[i]["csocode"] = ""; //需求跟踪号，string类型
                                        domBody[i]["cvmivencode"] = ""; //代管商代码，string类型
                                        domBody[i]["cvmivenname"] = ""; //代管商，string类型
                                        domBody[i]["bvmiused"] = ""; //代管消耗标识，int类型
                                        domBody[i]["ivmisettlequantity"] = ""; //代管挂账确认单数量，double类型
                                        domBody[i]["ivmisettlenum"] = ""; //代管挂账确认单件数，double类型
                                        domBody[i]["cdemandmemo"] = ""; //需求分类代号说明，string类型
                                        domBody[i]["iorderdid"] = ""; //iorderdid，int类型
                                        domBody[i]["iordercode"] = ""; //销售订单号，string类型
                                        domBody[i]["iorderseq"] = ""; //销售订单行号，string类型
                                        domBody[i]["cexpirationdate"] = ""; //有效期至，string类型
                                        domBody[i]["dexpirationdate"] = ""; //有效期计算项，string类型
                                        domBody[i]["cciqbookcode"] = ""; //手册号，string类型
                                        domBody[i]["ibondedsumqty"] = ""; //累计保税处理抽取数量，string类型
                                        domBody[i]["copdesc"] = ""; //工序说明，string类型
                                        domBody[i]["cmworkcentercode"] = ""; //工作中心编码，string类型
                                        domBody[i]["cmworkcenter"] = ""; //工作中心，string类型
                                        domBody[i]["cbaccounter"] = ""; //记账人，string类型
                                        domBody[i]["isoseq"] = ""; //需求跟踪行号，string类型
                                        domBody[i]["imoseq"] = ""; //生产订单行号，string类型
                                        domBody[i]["iopseq"] = ""; //工序行号，string类型
                                        domBody[i]["cdefine34"] = ""; //表体自定义项13，int类型
                                        domBody[i]["cdefine35"] = ""; //表体自定义项14，int类型
                                        domBody[i]["cdefine22"] = ""; //表体自定义项1，string类型
                                        domBody[i]["cdefine28"] = ""; //表体自定义项7，string类型
                                        domBody[i]["cdefine29"] = ""; //表体自定义项8，string类型
                                        domBody[i]["cdefine30"] = ""; //表体自定义项9，string类型
                                        domBody[i]["cdefine31"] = ""; //表体自定义项10，string类型
                                        domBody[i]["cdefine32"] = ""; //表体自定义项11，string类型
                                        domBody[i]["cdefine33"] = ""; //表体自定义项12，string类型
                                        domBody[i]["cinvdefine4"] = ""; //存货自定义项4，string类型
                                        domBody[i]["cinvdefine5"] = ""; //存货自定义项5，string类型
                                        domBody[i]["cinvdefine6"] = ""; //存货自定义项6，string类型
                                        domBody[i]["cinvdefine7"] = ""; //存货自定义项7，string类型
                                        domBody[i]["cinvdefine8"] = ""; //存货自定义项8，string类型
                                        domBody[i]["cinvdefine9"] = ""; //存货自定义项9，string类型
                                        domBody[i]["cinvdefine10"] = ""; //存货自定义项10，string类型
                                        domBody[i]["cinvdefine11"] = ""; //存货自定义项11，string类型
                                        domBody[i]["cinvdefine12"] = ""; //存货自定义项12，string类型
                                        domBody[i]["cbarcode"] = ""; //条形码，string类型
                                        domBody[i]["cdefine23"] = ""; //表体自定义项2，string类型
                                        domBody[i]["cdefine24"] = ""; //表体自定义项3，string类型
                                        domBody[i]["cdefine25"] = ""; //表体自定义项4，string类型
                                        domBody[i]["itrids"] = ""; //特殊单据子表标识，double类型
                                        domBody[i]["cdefine26"] = ""; //表体自定义项5，double类型
                                        domBody[i]["cdefine27"] = ""; //表体自定义项6，double类型
                                        domBody[i]["citemcode"] = ""; //项目编码，string类型
                                        domBody[i]["cname"] = ""; //项目，string类型
                                        domBody[i]["citem_class"] = ""; //项目大类编码，string类型
                                        domBody[i]["citemcname"] = ""; //项目大类名称，string类型
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                msg.success = false;
                                msg.msg = "订单信息获取失败。";
                                return JsonHelper.ToJson(msg).ToString();
                            }
                            #endregion
                            break;
                    }
                    #region===调用API

                    //给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
                    //broker.AssignNormalValue("domPosition", null);
                    //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值
                    //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
                    //broker.AssignNormalValue("cnnFrom", null);
                    //该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
                    //broker.AssignNormalValue("VouchId","");
                    ////该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                    MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
                    broker.AssignNormalValue("domMsg", domMsg);
                    ////给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
                    ////broker.AssignNormalValue("bCheck", new System.Boolean());
                    broker.AssignNormalValue("bCheck", false);
                    ////给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
                    ////broker.AssignNormalValue("bBeforCheckStock", new System.Boolean());
                    broker.AssignNormalValue("bBeforCheckStock", false);
                    ////给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
                    broker.AssignNormalValue("bIsRedVouch", Listdispatchlist[0].bIsRedVouch);
                    ////给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
                    broker.AssignNormalValue("sAddedState", "");
                    ////给普通参数bReMote赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否远程：转入false
                    ////broker.AssignNormalValue("bReMote", new System.Boolean());
                    broker.AssignNormalValue("bReMote", false);
                    //第六步：调用API
                    if (!broker.Invoke())
                    {
                        //错误处理
                        Exception apiEx = broker.GetException();
                        if (apiEx != null)
                        {
                            if (apiEx is MomSysException)
                            {
                                MomSysException sysEx = apiEx as MomSysException;
                                msg.success = false;
                                msg.msg = "系统异常：" + sysEx.Message;
                            }
                            else if (apiEx is MomBizException)
                            {
                                MomBizException bizEx = apiEx as MomBizException;
                                msg.success = false;
                                msg.msg = "API异常：" + bizEx.Message;
                            }
                            //异常原因
                            String exReason = broker.GetExceptionString();
                            if (exReason.Length != 0)
                            {
                                msg.success = false;
                                msg.msg = "异常原因：" + exReason;
                            }
                        }
                        //结束本次调用，释放API资源
                        broker.Release();
                    }
                    #endregion
                    //第七步：获取返回结果
                    //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
                    System.Boolean result = Convert.ToBoolean(broker.GetReturnValue());
                    if (result == false)
                    {
                        msg.success = false;
                        //获取out/inout参数值
                        //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                        System.String errMsgRet = broker.GetResult("errMsg") as System.String;
                        if (!string.IsNullOrEmpty(errMsgRet))
                        {
                            msg.msg = errMsgRet;
                        }
                    }
                    else
                    {
                        ////获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                        System.String VouchIdRet = broker.GetResult("VouchId") as System.String;
                        msg.data = VouchIdRet;
                        ////获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
                        //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));
                    }
                    //结束本次调用，释放API资源
                    broker.Release();
                }


            }
            catch (Exception e)
            {
                msg.success = false;
                msg.msg = "操作失败:" + e.Message;
            }
            return JsonHelper.ToJson(msg).ToString();
        }
        #endregion

        #region ===[生产管理]产成品入库单-审核 ProductInAudit
        /// <summary>
        ///产成品入库单审核
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "[生产管理]产成品入库单-审核 ProductInAudit")]
        public string ProductInAudit(string VouchId)
        {
            //试图查询为时间戳
            string ufts = GetUfts(VouchId, "RecordInQ", "ufts");
            ModJsonResult msg = Audit(VouchId, "10", "U8API/ProductIn/Audit", ufts);
            return JsonHelper.ToJson(msg).ToString();
        }
        #endregion

        #endregion

        #region===[销售管理]销售出库单

        #region ===[销售管理]销售出库单-添加 SaleoutAdd
        /// <summary>
        /// 销售出库单-添加 SaleoutAdd
        /// </summary>
        /// <param name="record">主单据</param>
        /// <param name="records">子单据</param>
        /// <returns></returns>
        [WebMethod(Description = "[销售管理]销售出库单-添加 SaleoutAdd")]
        public string SaleoutAdd(string record, string records)
        {
            var msg = new ModJsonResult();

            ////调用方法事例
            ////主表字段
            //V121RdRecord32 mainModel = new V121RdRecord32();
            //mainModel.AddType = 1;//添加类型  1：引用销售发货单
            //mainModel.bIsRedVouch = false;//false:蓝字单据 true:红字单据
            //mainModel.cBusCode = "CQ-180300001";//引用销售发货单
            //mainModel.cWhCode = "";//仓库编码(可选)
            //mainModel.cMemo=""; //备注，string类型
            //mainModel.cRdCode = "23";//出库类别(默认)
            //mainModel.cCusCode = ""; //客户编码(可选)
            //mainModel.VT_ID = 87; //默认 模版号，int类型

            ////转换成json
            //record = "[" + JsonHelper.ToJson(mainModel) + "]";//转换成json

            ////子表字段
            //List<V121RdRecords32> listDetail = new List<V121RdRecords32>();
            //V121RdRecords32 detailModel = new V121RdRecords32();
            //detailModel.AutoID = 0;//主键ID(必传)
            //detailModel.cInvCode = "MY00000001";//存货编码
            //detailModel.iQuantity =5;//数量
            //detailModel.iUnitCost = 0; //单价，double类型(可选)
          
            //listDetail.Add(detailModel);
            ////转换成json
            //records = JsonHelper.ToJson(listDetail);//转换成json

            if (string.IsNullOrEmpty(record))
            {
                msg.success = false;
                msg.msg = "record参数不能为空";
                return JsonHelper.ToJson(msg).ToString();
            }

            try
            {
                #region ===如果当前环境中有login对象则可以省去第一步
                U8Login.clsLogin u8Login = new U8Login.clsLogin();
                ModJsonResult IsLogin = callLogin(ref u8Login);
                if (IsLogin.success == false)
                {
                    msg.success = false;
                    msg.msg = "登陆失败，原因：" + u8Login.ShareString;
                    return JsonHelper.ToJson(msg).ToString();
                }
                #endregion

                StringBuilder sb = new StringBuilder();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<V121RdRecord32> ListRdRecord = serializer.Deserialize<List<V121RdRecord32>>(record).ToList();
                List<V121RdRecords32> ListRdRecords = serializer.Deserialize<List<V121RdRecords32>>(records).ToList();
                int recordId = 0;//主单据编号

                if (string.IsNullOrEmpty(ListRdRecord[0].cBusCode))
                {
                    msg.success = false;
                    msg.msg = "cBusCode参数不能为空";
                    return JsonHelper.ToJson(msg).ToString();
                }

                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = u8Login;
                //第三步：设置API地址标识(Url)
                //当前API：添加新单据的地址标识为：U8API/saleout/Add
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/saleout/Add");
                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);
                //第五步：API参数赋值
                //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：32
                broker.AssignNormalValue("sVouchType", "32");
                //给BO表头参数DomHead赋值，此BO参数的业务类型为采购入库单，属表头参数。BO参数均按引用传递
                //提示：给BO表头参数DomHead赋值有两种方法
                //方法一是直接传入MSXML2.DOMDocumentClass对象
                //broker.AssignNormalValue("DomHead", new MSXML2.DOMDocumentClass());
                //方法二是构造BusinessObject对象，具体方法如下：
                BusinessObject DomHead = broker.GetBoParam("DomHead");
                DomHead.RowCount = ListRdRecord.Count; //设置BO对象(表头)行数，只能为一行

                #region===根据cBusCode获取销售发货单详细信息
                U8EnvContext envContext1 = new U8EnvContext();
                envContext1.U8Login = u8Login;
                //设置上下文参数
                envContext1.SetApiContext("VoucherType",9); //上下文数据类型：int，含义：单据类型：9
                //第三步：设置API地址标识(Url)
                //当前API：装载单据的地址标识为：U8API/Consignment/Load
                U8ApiAddress myApiAddress1 = new U8ApiAddress("U8API/Consignment/Load");
                //第四步：构造APIBroker
                U8ApiBroker broker1 = new U8ApiBroker(myApiAddress1, envContext1);
                //第五步：API参数赋值
                //给普通参数VouchID赋值。此参数的数据类型为string，此参数按值传递，表示单据号
               
                //根据销售发货单号获取销售发货单ID
                DataSet ArrivalVouch = SqlHelper.ExcuteDataSet("select DLID from DispatchList(nolock) where cDLCode='" + ListRdRecord[0].cBusCode + "';");
                if (ArrivalVouch.Tables[0].Rows.Count == 0)
                {
                    msg.success = false;
                    msg.msg = "销售发货单信息不存在。";
                    return JsonHelper.ToJson(msg).ToString();
                }
                broker1.AssignNormalValue("VouchID", ArrivalVouch.Tables[0].Rows[0]["DLID"].ToString());//销售发货单主键ID
                //给普通参数blnAuth赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制权限：true
                broker1.AssignNormalValue("blnAuth",false);
                //第六步：调用API
                if (!broker1.Invoke())
                {
                    //错误处理
                    Exception apiEx = broker1.GetException();
                    if (apiEx != null)
                    {
                        if (apiEx is MomSysException)
                        {
                            MomSysException sysEx = apiEx as MomSysException;
                            Console.WriteLine("系统异常：" + sysEx.Message);
                            //todo:异常处理
                        }
                        else if (apiEx is MomBizException)
                        {
                            MomBizException bizEx = apiEx as MomBizException;
                            Console.WriteLine("API异常：" + bizEx.Message);
                            //todo:异常处理
                        }
                        //异常原因
                        String exReason = broker1.GetExceptionString();
                        if (exReason.Length != 0)
                        {
                            Console.WriteLine("异常原因：" + exReason);
                        }
                    }
                    //结束本次调用，释放API资源
                    broker1.Release();
                }
                #endregion
                //第七步：获取返回结果
                //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功返回空串
                System.String result = broker1.GetReturnValue() as System.String;
                if (string.IsNullOrEmpty(result))
                {
                    BusinessObject domHeadRet = broker1.GetBoParam("domHead");
                    BusinessObject domBodyRet = broker1.GetBoParam("domBody");


                    //给BO对象(表头)的字段赋值，值可以是真实类型，也可以是无类型字符串
                    //以下代码示例只设置第一行值。各字段定义详见API服务接口定义
                    /****************************** 以下是必输字段 ****************************/
                    int mainId = new Random().Next(100);//临时存储主键
                    DomHead[0]["id"] = mainId; //主关键字段，int类型
                    DomHead[0]["brdflag"] = 0; //收发标志，int类型
                    DomHead[0]["cvouchtype"] = "32"; //单据类型，string类型
                    DomHead[0]["cbuscode"] = ListRdRecord[0].cBusCode; //业务号,发货单号，string类型
                    string ccode= GetVoucherNumnber("0303");
                    DomHead[0]["ccode"] =ccode; //自动生成出库单号,自动生成，string类型
                    DomHead[0]["ddate"] = ListRdRecord[0].dDate == null ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(ListRdRecord[0].dDate).ToShortDateString(); //出库日期，DateTime类型
                    DomHead[0]["cwhcode"] = string.IsNullOrEmpty(ListRdRecord[0].cWhCode)==true?Convert.ToString(domBodyRet[0]["cwhcode"]):ListRdRecord[0].cWhCode; //仓库编码，string类型
                    DomHead[0]["crdcode"] =ListRdRecord[0].cRdCode; //出库类别编码，string类型
                    DomHead[0]["cdepcode"] = string.IsNullOrEmpty(ListRdRecord[0].cDepCode) == true ? Convert.ToString(domHeadRet[0]["cdepcode"]): ListRdRecord[0].cDepCode; //部门编码，string类型
                    DomHead[0]["cstcode"] = string.IsNullOrEmpty(ListRdRecord[0].cSTCode) == true ? Convert.ToString(domHeadRet[0]["cstcode"]) : ListRdRecord[0].cSTCode; //销售类型编码，string类型
                    DomHead[0]["cdlcode"] = Convert.ToInt32(domHeadRet[0]["dlid"]); //发货单id，string类型
                    DomHead[0]["ccuscode"] = string.IsNullOrEmpty(ListRdRecord[0].cCusCode) == true ? Convert.ToString(domHeadRet[0]["ccuscode"]) : ListRdRecord[0].cCusCode.ToString(); //客户编码，string类型
                    DomHead[0]["iverifystate"] = 0; //工作流审批状态 ，int类型
                    DomHead[0]["iswfcontrolled"] = 0; //是否工作流控制，int类型
                    DomHead[0]["cmaker"] = u8Login.cUserName; //制单人，string类型
                    DomHead[0]["bisstqc"] = "false"; //库存期初标记，string类型
                    DomHead[0]["cbustype"] = "普通销售"; //业务类型，int类型  普通采购
                    DomHead[0]["csource"] = "发货单"; //单据来源，int类型  采购订单
                    DomHead[0]["cwhname"] = ""; //仓库名称，string类型
                    DomHead[0]["ccusabbname"] = ""; //客户名称，string类型
                    DomHead[0]["ufts"] = Convert.ToString(domHeadRet[0]["ufts"]); //时间戳，string类型
                    DomHead[0]["ccuscode"] = string.IsNullOrEmpty(ListRdRecord[0].cCusCode) == true ? Convert.ToString(domHeadRet[0]["ccuscode"]) : ListRdRecord[0].cCusCode; //客户编码，string类型

                    /***************************** 以下是非必输字段 ****************************/
                    #region===入库主单据
                    DomHead[0]["cmemo"] = (string.IsNullOrEmpty(ListRdRecord[0].cMemo) == true ? Convert.ToString(domHeadRet[0]["cmemo"]) : ListRdRecord[0].cMemo);//备注，string类型
                    DomHead[0]["cdefine10"] = Convert.ToString(domHeadRet[0]["cdefine10"]); //表头自定义项10，联系电话,string类型
                    DomHead[0]["cdefine11"] = Convert.ToString(domHeadRet[0]["cdefine11"]); //表头自定义项11,联系人名称，string类型
                    DomHead[0]["cshipaddress"] = Convert.ToString(domHeadRet[0]["cshipaddress"]); //发货地址，string类型

                    DomHead[0]["iarriveid"] = ListRdRecord[0].cBusCode; //发货单号，string类型
                    DomHead[0]["ireturncount"] = "0"; //打回次数，string类型
                    DomHead[0]["iverifystate"] = 0; //工作流审批状态 ，int类型
                    DomHead[0]["iswfcontrolled"] = 0; //是否工作流控制 ，int类型
                    DomHead[0]["vt_id"] = ListRdRecord[0].VT_ID; //模版号，int类型
                    DomHead[0]["dnmaketime"] = DateTime.Now; //制单时间，DateTime类型
                    DomHead[0]["cpersoncode"] = string.IsNullOrEmpty(ListRdRecord[0].cPersonCode) == true ? Convert.ToString(domHeadRet[0]["cpersoncode"]) : ListRdRecord[0].cPersonCode; //业务员编码，string类型
                    DomHead[0]["dnmaketime"] = DateTime.Now; //制单时间，DateTime类型

                    //自定义列
                    DomHead[0]["cdefine1"] = Convert.ToString(domHeadRet[0]["cdefine1"]); //表头自定义项1，string类型(区分用友还是光码添加的数据)
                    DomHead[0]["cdefine2"] = Convert.ToString(domHeadRet[0]["cdefine2"]); //表头自定义项2，int类型
                    #endregion
                    //给BO表体参数domBody赋值，此BO参数的业务类型为采购入库单，属表体参数。BO参数均按引用传递
                    //提示：给BO表体参数domBody赋值有两种方法
                    //方法一是直接传入MSXML2.DOMDocumentClass对象
                    //broker.AssignNormalValue("domBody", new MSXML2.DOMDocumentClass())
                    //方法二是构造BusinessObject对象，具体方法如下：
                    BusinessObject domBody = broker.GetBoParam("domBody");
                    //可以自由设置BO对象行数为大于零的整数，也可以不设置而自动增加行数
                    //给BO对象的字段赋值，值可以是真实类型，也可以是无类型字符串
                    //以下代码示例只设置第一行值。各字段定义详见API服务接口定义

                    //获取单据详细信息
                    #region===添加出库子单据
                    if (ListRdRecords.Count > 0)
                    {

                        int RowCount = 0;
                        #region==获取行号
                        for (int i = 0; i < ListRdRecords.Count; i++)
                        {
                            for (int j = 0; j < domBodyRet.RowCount; j++)
                            {
                                if (ListRdRecords[i].cInvCode == Convert.ToString(domBodyRet[j]["cinvcode"]))
                                {
                                    RowCount++;
                                }
                            }
                        }
                        #endregion

                        domBody.RowCount = RowCount; //设置BO对象行数
                        var inRowCount = 0; 
                        for (int i = 0; i < ListRdRecords.Count; i++)
                        {
                            for (int j = 0; j < domBodyRet.RowCount; j++)
                            {
                                if (ListRdRecords[i].cInvCode == Convert.ToString(domBodyRet[j]["cinvcode"]) && ListRdRecords[i].AutoID == long.Parse(domBodyRet[j]["autoid"].ToString()))
                                {
                                    domBody[inRowCount]["idlsid"] = domBodyRet[j]["idlsid"]; ;//发货单子表ID，int类型
                                    domBody[inRowCount]["inum"] = ""; //件数，double类型
                                    domBody[inRowCount]["cassunit"] = Convert.ToString(domBodyRet[j]["cunitid"]); //库存单位码，string类型
                                    domBody[inRowCount]["iunitcost"] = ListRdRecords[i].iUnitCost; //单价，double类型
                                    domBody[inRowCount]["iprice"] = ListRdRecords[i].iUnitCost * ListRdRecords[i].iQuantity; //金额，double类型
                                    domBody[inRowCount]["autoid"] = i; //主关键字段，int类型
                                    domBody[inRowCount]["id"] = mainId; //与收发记录主表关联项，int类型
                                    domBody[inRowCount]["cinvcode"] = Convert.ToString(domBodyRet[j]["cinvcode"]); //存货编码，string类型
                                    domBody[inRowCount]["iquantity"] = ListRdRecords[i].iQuantity; //数量，double类型
                                    domBody[inRowCount]["cinvm_unit"] = "";//ListRdRecords[i].cinvm_unit; //主计量单位，string类型
                                    domBody[inRowCount]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                                    /***************************** 以下是非必输字段 ****************************/
                                    #region===入库子单据
                                    domBody[inRowCount]["inquantity"] = Convert.ToDouble(domBodyRet[j]["iquantity"]);//应收数量，double类型
                                    domBody[inRowCount]["innum"] = "";//应收件数，double类型
                                    domBody[inRowCount]["iorderdid"] = Convert.ToInt32(domBodyRet[j]["isosid"]); //销售订单子单表，int类型
                                    domBody[inRowCount]["iordertype"] = 1; //销售订单类别，int类型
                                    domBody[inRowCount]["bcosting"] = "1"; //是否核算，string类型
                                    domBody[inRowCount]["cbdlcode"] = ListRdRecord[i].cBusCode; //发货单号，string类型
                                    domBody[inRowCount]["iinvexchrate"] = Math.Round(Convert.ToDouble(domBodyRet[j]["iinvexchrate"]), 2).ToString(); //换算率，double类型
                                    domBody[inRowCount]["iordercode"] = Convert.ToString(domBodyRet[j]["csocode"]); //销售订单号，string类型
                                    domBody[inRowCount]["iorderseq"] = (i + 1).ToString(); //销售订单行号，string类型
                                    domBody[inRowCount]["isodid"] = Convert.ToInt32(domBodyRet[j]["isosid"]); //销售订单子表ID，string类型
                                    domBody[inRowCount]["isotype"] = 1; //订单类型，int类型
                                    domBody[inRowCount]["isoseq"] = (i + 1).ToString(); //需求跟踪行号，string类型
                                    domBody[inRowCount]["csocode"] = Convert.ToString(domHeadRet[j]["csocode"]); //销售订单号，string类型
                                    domBody[inRowCount]["irowno"] = i + 1; //需求跟踪行号，string类型
                                    domBody[inRowCount]["cbsysbarcode"] = "||st32|" + ccode + "|" + i + 1; //单据行条码，string类型
                                    domBody[inRowCount]["isoseq"] = i + 1; //需求跟踪行号，string类型
                                    #endregion

                                    inRowCount++;
                                }
                            }
                        }
                    }
                    else {
                        domBody.RowCount = domBodyRet.RowCount; //设置BO对象行数

                        for (int i = 0; i < domBodyRet.RowCount; i++)
                        {
                            /****************************** 以下是必输字段 ****************************/
                            domBody[i]["idlsid"] = domBodyRet[i]["idlsid"]; ;//发货单子表ID，int类型
                            domBody[i]["inum"] =""; //件数，double类型
                            domBody[i]["cassunit"] = Convert.ToString(domBodyRet[i]["cunitid"]); //库存单位码，string类型
                            domBody[i]["iunitcost"] = ""; //单价，double类型
                            domBody[i]["iprice"] = ""; //金额，double类型
                            domBody[i]["autoid"] = i; //主关键字段，int类型
                            domBody[i]["id"] = mainId; //与收发记录主表关联项，int类型
                            domBody[i]["cinvcode"] = Convert.ToString(domBodyRet[i]["cinvcode"]); //存货编码，string类型
                            domBody[i]["iquantity"] = Convert.ToDouble(domBodyRet[i]["iquantity"]); //数量，double类型
                            domBody[i]["cinvm_unit"] = "";//ListRdRecords[i].cinvm_unit; //主计量单位，string类型
                            domBody[i]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
                            /***************************** 以下是非必输字段 ****************************/
                            #region===入库子单据
                            domBody[i]["inquantity"] = Convert.ToDouble(domBodyRet[i]["iquantity"]);//应收数量，double类型
                            domBody[i]["innum"] ="";//应收件数，double类型
                            domBody[i]["iorderdid"] = Convert.ToInt32(domBodyRet[i]["isosid"]); //销售订单子单表，int类型
                            domBody[i]["iordertype"] = 1; //销售订单类别，int类型
                            domBody[i]["bcosting"] = "1"; //是否核算，string类型
                            domBody[i]["cbdlcode"] = ListRdRecord[i].cBusCode; //发货单号，string类型
                            domBody[i]["iinvexchrate"] = Math.Round(Convert.ToDouble(domBodyRet[i]["iinvexchrate"]), 2).ToString(); //换算率，double类型
                            domBody[i]["iordercode"] = Convert.ToString(domBodyRet[i]["csocode"]); //销售订单号，string类型
                            domBody[i]["iorderseq"] = (i + 1).ToString(); //销售订单行号，string类型
                            domBody[i]["isodid"] = Convert.ToInt32(domBodyRet[i]["isosid"]); //销售订单子表ID，string类型
                            domBody[i]["isotype"] = 1; //订单类型，int类型
                            domBody[i]["isoseq"] = (i + 1).ToString(); //需求跟踪行号，string类型
                            domBody[i]["csocode"] = Convert.ToString(domHeadRet[i]["csocode"]); //销售订单号，string类型
                            domBody[i]["irowno"] = i + 1; //需求跟踪行号，string类型
                            domBody[i]["cbsysbarcode"] = "||st32|" + ccode + "|" + i + 1; //单据行条码，string类型
                            domBody[i]["isoseq"] = i + 1; //需求跟踪行号，string类型
                            #endregion
                        }
                    }
                   
                
                    #endregion
                }
                else
                {
                    msg.success = false;
                    msg.msg = "发货单信息获取为空。";
                    return JsonHelper.ToJson(msg).ToString();
                }
                //给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
                // broker.AssignNormalValue("domPosition", new System.Object());
                //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值
                //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
                //broker.AssignNormalValue("cnnFrom", new ADODB.Connection());
                //该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
                // broker.AssignNormalValue("VouchId", "");
                //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
                broker.AssignNormalValue("domMsg", domMsg);
                //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
                //broker.AssignNormalValue("bCheck", new System.Boolean());
                broker.AssignNormalValue("bCheck", false);
                //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
                //broker.AssignNormalValue("bBeforCheckStock", new System.Boolean());
                broker.AssignNormalValue("bBeforCheckStock", false);
                //给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
                broker.AssignNormalValue("bIsRedVouch", ListRdRecord[0].bIsRedVouch);
                //给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
                broker.AssignNormalValue("sAddedState", "");
                //给普通参数bReMote赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否远程：转入false
                //broker.AssignNormalValue("bReMote", new System.Boolean());
                broker.AssignNormalValue("bReMote", false);
                //第六步：调用API
                #region===调用API
                if (!broker.Invoke())
                {
                    //错误处理
                    Exception apiEx = broker.GetException();
                    if (apiEx != null)
                    {
                        if (apiEx is MomSysException)
                        {
                            MomSysException sysEx = apiEx as MomSysException;
                            msg.success = false;
                            msg.msg = "系统异常：" + sysEx.Message;
                        }
                        else if (apiEx is MomBizException)
                        {
                            MomBizException bizEx = apiEx as MomBizException;
                            msg.success = false;
                            msg.msg = "API异常：" + bizEx.Message;
                        }
                        //异常原因
                        String exReason = broker.GetExceptionString();
                        if (exReason.Length != 0)
                        {
                            msg.success = false;
                            msg.msg = "异常原因：" + exReason;
                        }
                    }
                    //结束本次调用，释放API资源
                    broker.Release();
                }
                #endregion

                //第七步：获取返回结果
                //获取返回值
                //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
                System.Boolean GetResult = Convert.ToBoolean(broker.GetReturnValue());
                if (GetResult == false)
                {
                    //获取out/inout参数值
                    //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                    System.String errMsgRet = broker.GetResult("errMsg") as System.String;
                    if (!string.IsNullOrEmpty(errMsgRet))
                    {
                        msg.success = false;
                        msg.msg = errMsgRet;
                    }
                }
                else
                {
                    //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                    System.String VouchIdRet = broker.GetResult("VouchId") as System.String;
                    //获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
                    //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));
                    //MSXML2.IXMLDOMDocument2 domMsgRet = (MSXML2.IXMLDOMDocument2)(broker.GetResult("domMsg"));
                    msg.success = true;
                    msg.data = VouchIdRet;
                    ////保存成功,同步用友操作
                    //if (sb.ToString().Length > 0)
                    //{
                    //    int resSql = new BllPopomain().InsertSql(sb.ToString());
                    //    if (resSql <= 0)
                    //    {
                    //        msg.success = false;
                    //        msg.msg = "单据信息同步失败";
                    //    }
                    //}
                }
                //结束本次调用，释放API资源
                broker.Release();
            }
            catch (Exception e)
            {
                msg.success = false;
                msg.msg = "操作失败:" + e.Message;
            }
            return JsonHelper.ToJson(msg).ToString();
        }
        #endregion

        #region ===[销售管理]销售出库单-审核 SaleoutAudit
        /// <summary>
        /// 销售出库单-审核 SaleoutAudit
        /// </summary>
        /// <param name="SaleoutId">出库单主键</param>
        /// <returns></returns>
        [WebMethod(Description = "[销售管理]销售出库单-审核 SaleoutAudit")]
        public string SaleoutAudit(string VouchId)
        {
            //试图查询为时间戳
            string ufts = GetUfts(VouchId, "KCSaleOutH", "ufts");
            ModJsonResult msg = Audit(VouchId, "32", "U8API/saleout/Audit", ufts);
            return JsonHelper.ToJson(msg).ToString();
        }
        #endregion
        #endregion
    }
}
