using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using System.Configuration;

namespace WebPortalAdmin.Controllers
{
    public class UserMoneyRecordController : BaseController<ModOrderUserMoneyRecord>
    {
        /// <summary>
        /// 取现管理
        /// </summary>
        /// <returns></returns>
        public ActionResult TakingCash()
        {
            return View();
        }


        #region==页面列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            var jsonResult = new BllOrderUserMoneyRecord().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region==页面查询员工流水列表 SearchTotalData
        /// <summary>
        /// 页面查询员工流水列表
        /// </summary>
        public void SearchTotalData()
        {
            var search = base.GetSearch();
            if (!string.IsNullOrEmpty(Request["Eid"]))
            {
                search.AddCondition("UserId='" + Request["Eid"].ToString() + "'");
            }
            search.AddCondition("Status=2");//状态(-1为删除，0为申请中,1:正在支付,2为已完成)
            var jsonResult = new BllOrderUserMoneyRecord().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region ==处理 TakingCashSubmit
        /// <summary>
        /// 处理
        /// </summary>
        public void TakingCashSubmit(string id)
        {
            try
            {

                //LogErrorRecord.ErrorFormat("支付宝开始提现操作=========================================================================id={0}", id);

                if (!string.IsNullOrEmpty(id))
                {

                    //查询公司支付账号信息
                    BllSysCompanyPaySet bllCompanyPaySet = new BllSysCompanyPaySet();
                    ModSysCompanyPaySet cpayset = bllCompanyPaySet.getByCId(CurrentMaster.Cid);
                    if (cpayset != null)
                    {
                        Config.Partner = cpayset.Partner;
                        Config.Key = cpayset.Key;
                        Config.Private_key = cpayset.RsaPrivate;
                        Config.Public_key = cpayset.RsaAlipayPublic;

                        List<ModOrderUserMoneyRecord> list = new BllOrderUserMoneyRecord().GetInId(id);
                        if (list != null && list.Count > 0)
                        {
                            //服务器异步通知页面路径
                            string notify_url = ConfigurationManager.AppSettings["notifyurlW"];
                            //需http://格式的完整路径，不允许加?id=123这类自定义参数

                            //付款账号
                            string email = cpayset.Seller;
                            //必填

                            //付款账户名
                            string account_name = cpayset.Name;//WIDaccount_name.Text.Trim();
                            //必填，个人支付宝账号是真实姓名公司支付宝账号是公司名称

                            //付款当天日期
                            string pay_date = DateTime.Now.ToString("yyyyMMdd");
                            //必填，格式：年[4位]月[2位]日[2位]，如：20100801

                            //批次号
                            string batch_no = DateTime.Now.ToString("yyyyMMdd") + CRC32.GetCRC32(Guid.NewGuid().ToString());
                            //必填，格式：当天日期[8位]+序列号[3至16位]，如：201008010000001


                            //付款总金额
                            string batch_fee = list.Sum(c => -c.MoneyNum).ToString("#.00");
                            //必填，即参数detail_data的值中所有金额的总和

                            //付款笔数
                            string batch_num = list.Count.ToString();
                            //必填，即参数detail_data的值中，“|”字符出现的数量加1，最大支持1000笔（即“|”字符出现的数量999个）

                            string detail_data = "";

                            //decimal bzj = Convert.ToDecimal(ConfigurationManager.AppSettings["bail"]);
                            foreach (var master in list)
                            {
                                string money = (-master.MoneyNum).ToString("#.00");
                                //订单ordernum
                                string ordernum = DateTime.Now.ToString("yyyyMMdd") + CRC32.GetCRC32(Guid.NewGuid().ToString());

                                //付款详细数据
                                detail_data += ordernum + "^" + master.UserName + "^" + master.Alipay + "^" + money + "^" + "用户支付宝提现操作！|";
                                //必填，格式：流水号1^收款方帐号1^真实姓名^付款金额1^备注说明1|流水号2^收款方帐号2^真实姓名^付款金额2^备注说明2....

                            }

                            detail_data = detail_data.Substring(0, detail_data.Length - 1);
                            //LogErrorRecord.ErrorFormat("支付宝日志-----------：detail_data={0},请求url:{1}", detail_data, Request.Url);

                            ////////////////////////////////////////////////////////////////////////////////////////////////

                            //把请求参数打包成数组
                            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                            sParaTemp.Add("partner", Config.Partner);
                            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                            sParaTemp.Add("service", "batch_trans_notify");
                            sParaTemp.Add("notify_url", notify_url);
                            sParaTemp.Add("email", email);
                            sParaTemp.Add("account_name", account_name);
                            sParaTemp.Add("pay_date", pay_date);
                            sParaTemp.Add("batch_no", batch_no);
                            sParaTemp.Add("batch_fee", batch_fee);
                            sParaTemp.Add("batch_num", batch_num);
                            sParaTemp.Add("detail_data", detail_data);


                            //新增金额记录
                            BllOrderUserMoneyRecord Bll = new BllOrderUserMoneyRecord();
                            if (Bll.Updatebatch_no(batch_no,id))
                            {
                                //建立请求
                                string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
                                Response.Write(sHtmlText);

                                //LogErrorRecord.ErrorFormat("支付宝日志-----------请求sHtmlText={0}", sHtmlText);
                            }
                            else
                            {

                                //LogErrorRecord.ErrorFormat("支付宝日志-----------提现失败002");
                            }
                        }
                        else
                        {

                            //LogErrorRecord.ErrorFormat("支付宝日志-----------提现失败,电梯公司支付账号信息未配置！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
            }
            finally
            {
                Response.End();
            }
        }
        #endregion

        #region ==删除 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                BllOrderUserMoneyRecord User = new BllOrderUserMoneyRecord();
                var Model = User.LoadData(id);
                if (Model != null)
                {
                    Model.Status = (int)StatusEnum.删除;
                    int result = User.Update(Model);
                    if (result > 0)
                    {
                        msg.success = true;
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "操作失败";
                    }
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion
    }
}
