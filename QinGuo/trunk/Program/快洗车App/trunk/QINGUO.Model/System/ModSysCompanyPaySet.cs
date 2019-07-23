#region Version Info
/* ======================================================================== 
* 【本类功能概述】 支付配置
* 
* 作者：zhangjian 时间：2014/1/10 9:45:46 
* 文件名： ModSysCompanyPaySet.cs
* 版本：V1.0.1 
* 
* 修改者： 时间： 
* 修改说明： 
* ======================================================================== 
*/
#endregion

using System;
using QINGUO.Common;
namespace QINGUO.Model
{

    //Sys_CompanyPaySet
    [Serializable]
    [Dapper.TableNameAttribute("Sys_CompanyPaySet")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysCompanyPaySet
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public string Id { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>		
        public string CompanyID { get; set; }
        /// <summary>
        /// 支付接口类型(1:支付宝，2：微信)
        /// </summary>		
        public int PayType { get; set; }
        /// <summary>
        /// 合作商户ID。用签约支付宝账号登录ms.alipay.com后，在账户信息页面获取
        /// </summary>		
        public string Partner { get; set; }
        /// <summary>
        /// 商户收款的支付宝账号（基本和商户编号同）
        /// </summary>		
        public string Seller { get; set; }
        /// <summary>
        /// 商户（RSA）私钥
        /// </summary>		
        public string RsaPrivate { get; set; }
        /// <summary>
        /// 支付宝（RSA）公钥 用签约支付宝账号登录ms.alipay.com后，在密钥管理页面获取。
        /// </summary>		
        public string RsaAlipayPublic { get; set; }

        //key
        public string Key { get; set; }
        
        /// <summary>
        /// 账号（支付宝，微信）
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 真实姓名 （开户名）
        /// </summary>
        public string Name { get; set; }


        private DateTime _addTime;
        /// <summary>
        /// 添加时间
        /// </summary>		
        public DateTime AddTime
        {
            get
            {

                if (_addTime.ToShortDateString().IndexOf("0001") > -1)
                    return DateTime.Now;
                else
                    return _addTime;
            }
            set
            {
                if (_addTime.ToShortDateString().IndexOf("0001") > -1)
                    _addTime = DateTime.Now;
                else
                    _addTime = value;
            }
        }
        /// <summary>
        /// -1:删除 0:禁用 1:正常 2:未审核
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string TypeName { get; set; }
        
    }
}