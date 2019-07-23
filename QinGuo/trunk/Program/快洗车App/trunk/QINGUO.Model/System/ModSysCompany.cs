using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Company")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysCompany
    {

        /// <summary>
        /// 主键
        /// </summary>		
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 公司名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 公司名称简称
        /// </summary>		
        private string _nametitle;
        public string NameTitle
        {
            get { return _nametitle; }
            set { _nametitle = value; }
        }
        /// <summary>
        /// 公司地址
        /// </summary>		
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>		
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        /// <summary>
        /// 联系邮箱
        /// </summary>		
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        /// 公司编码
        /// </summary>		
        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// 节点深度
        /// </summary>		
        private int _level;
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        /// <summary>
        /// 节点路径
        /// </summary>		
        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        /// <summary>
        /// 0:开发者 1:超级管理员 2:集团 3:公司 4:分公司 5:子公司
        /// </summary>		
        private int _attribute;
        public int Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }
        /// <summary>
        /// 排序
        /// </summary>		
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }
        /// <summary>
        /// 成立时间
        /// </summary>		
        private DateTime? _regisitime;
        public DateTime? RegisiTime
        {
            get { return _regisitime; }
            set { _regisitime = value; }
        }
        /// <summary>
        /// 注册资本
        /// </summary>		
        private decimal _reegistmoney;
        public decimal ReegistMoney
        {
            get { return _reegistmoney; }
            set { _reegistmoney = value; }
        }
        /// <summary>
        /// 所在城市
        /// </summary>		
        private int _cityid;
        public int CityId
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        /// <summary>
        /// 联系人
        /// </summary>		
        private string _linkuser;
        public string LinkUser
        {
            get { return _linkuser; }
            set { _linkuser = value; }
        }
        /// <summary>
        /// 法人
        /// </summary>		
        private string _legalperson;
        public string LegalPerson
        {
            get { return _legalperson; }
            set { _legalperson = value; }
        }
        /// <summary>
        /// 类型 工程师 = 2,物业端 = 3,电梯公司 = 4,监管部门 = 5,生产厂家 = 6,
        /// </summary>		
        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 个体工商,私营独资,国营商家
        /// </summary>		
        private string _nature;
        public string Nature
        {
            get { return _nature; }
            set { _nature = value; }
        }
        /// <summary>
        /// 介绍
        /// </summary>		
        private string _introduction;
        public string Introduction
        {
            get { return _introduction; }
            set { _introduction = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>		
        private string _reark;
        public string Reark
        {
            get { return _reark; }
            set { _reark = value; }
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
        /// 每天开业时间(如：08:50)
        /// </summary>		
        private string _openedtime;
        public string OpenedTime
        {
            get { return _openedtime; }
            set { _openedtime = value; }
        }
        /// <summary>
        /// 每天停业时间(如：22:50)
        /// </summary>		
        private string _closedtime;
        public string ClosedTime
        {
            get { return _closedtime; }
            set { _closedtime = value; }
        }
        /// <summary>
        /// 当天营业状态
        /// </summary>		
        private bool _daystatus;
        public bool DayStatus
        {
            get { return _daystatus; }
            set { _daystatus = value; }
        }
        /// <summary>
        /// 停业原因
        /// </summary>		
        private string _closestorereason;
        public string CloseStoreReason
        {
            get { return _closestorereason; }
            set { _closestorereason = value; }
        }
        /// <summary>
        /// 创建公司编号
        /// </summary>		
        private string _createcompanyid;
        public string CreateCompanyId
        {
            get { return _createcompanyid; }
            set { _createcompanyid = value; }
        }
        /// <summary>
        /// 创建者
        /// </summary>		
        private string _createruserid;
        public string CreaterUserId
        {
            get { return _createruserid; }
            set { _createruserid = value; }
        }
        /// <summary>
        /// 所在位置经度
        /// </summary>		
        private string _complon;
        public string ComPLon
        {
            get { return _complon; }
            set { _complon = value; }
        }
        /// <summary>
        /// 所在位置纬度
        /// </summary>		
        private string _complat;
        public string CompLat
        {
            get { return _complat; }
            set { _complat = value; }
        }
        /// <summary>
        /// 浏览量
        /// </summary>		
        private long _hitnum;
        public long HitNum
        {
            get { return _hitnum; }
            set { _hitnum = value; }
        }
        /// <summary>
        /// 公司管理超级管理员编号
        /// </summary>		
        private string _masterid;
        public string MasterId
        {
            get { return _masterid; }
            set { _masterid = value; }
        }
        /// <summary>
        /// 标示图
        /// </summary>		
        private string _propic;
        public string ProPic
        {
            get { return _propic; }
            set { _propic = value; }
        }
        /// <summary>
        /// 传真
        /// </summary>		
        private string _fax;
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        /// <summary>
        /// 所在省
        /// </summary>		
        private string _province;
        public string Province
        {
            get { return _province; }
            set { _province = value; }
        }
        /// <summary>
        /// 邮政编码
        /// </summary>		
        private string _postalcode;
        public string PostalCode
        {
            get { return _postalcode; }
            set { _postalcode = value; }
        }
        /// <summary>
        /// 公司网址
        /// </summary>		
        private string _website;
        public string WebSite
        {
            get { return _website; }
            set { _website = value; }
        }
        /// <summary>
        /// 注册号
        /// </summary>		
        private string _registrationnumber;
        public string RegistrationNumber
        {
            get { return _registrationnumber; }
            set { _registrationnumber = value; }
        }
        /// <summary>
        /// 单位资质
        /// </summary>		
        private string _licensenumber;
        public string LicenseNumber
        {
            get { return _licensenumber; }
            set { _licensenumber = value; }
        }
        /// <summary>
        /// 律师名称 --(品牌名称)
        /// </summary>		
        private string _lawyername;
        public string LawyerName
        {
            get { return _lawyername; }
            set { _lawyername = value; }
        }
        /// <summary>
        /// 律师电话--(品牌主键)
        /// </summary>		
        private string _lawyerphone;
        public string LawyerPhone
        {
            get { return _lawyerphone; }
            set { _lawyerphone = value; }
        }
        /// <summary>
        /// 律师事务所
        /// </summary>		
        private string _lawfirm;
        public string LawFirm
        {
            get { return _lawfirm; }
            set { _lawfirm = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime? _createtime;
        public DateTime? CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }

        /// <summary>
        /// 公司合同文件
        /// </summary>
        public string Pact { get; set; }
        public int AreaId { get; set; }


        /// <summary>
        ///  电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 结账方式：0未设置 1月结 2日结
        /// </summary>
        public int CheckoutType { get; set; }
        /// <summary>
        /// 付款方式：1支付宝 2工行  3农行
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string AccountNum { get; set; }
    }
}
