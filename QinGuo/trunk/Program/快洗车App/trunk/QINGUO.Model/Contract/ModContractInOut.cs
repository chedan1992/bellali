using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 收入/支出合同表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Con_ContractInOut")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
   public class ModContractInOut
    {
        /// <summary>
        /// Id
        /// </summary>		
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
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
        /// 创建人编号
        /// </summary>		
        private string _createrid;
        public string CreaterId
        {
            get { return _createrid; }
            set { _createrid = value; }
        }
        /// <summary>
        /// 数据状态(-1: 删除 0:禁用 1:正常)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 公司编号
        /// </summary>		
        private string _companyid;
        public string CompanyId
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        /// <summary>
        /// 现行有效合同编号
        /// </summary>		
        private string _numcode;
        public string NumCode
        {
            get { return _numcode; }
            set { _numcode = value; }
        }
        /// <summary>
        /// 原合同编号
        /// </summary>		
        private string _oldnumcode;
        public string OldNumCode
        {
            get { return _oldnumcode; }
            set { _oldnumcode = value; }
        }
        /// <summary>
        /// 合同名称
        /// </summary>		
        private string _contracename;
        public string ContraceName
        {
            get { return _contracename; }
            set { _contracename = value; }
        }
        /// <summary>
        /// 合同性质
        /// </summary>		
        private string _nature;
        public string Nature
        {
            get { return _nature; }
            set { _nature = value; }
        }
        /// <summary>
        /// 发起人
        /// </summary>		
        private string _initiatoruser;
        public string InitiatorUser
        {
            get { return _initiatoruser; }
            set { _initiatoruser = value; }
        }
        /// <summary>
        /// 发起部门
        /// </summary>		
        private string _initiatordept;
        public string InitiatorDept
        {
            get { return _initiatordept; }
            set { _initiatordept = value; }
        }
        /// <summary>
        /// 经办人
        /// </summary>		
        private string _agent;
        public string Agent
        {
            get { return _agent; }
            set { _agent = value; }
        }
        /// <summary>
        /// 审批人
        /// </summary>		
        private string _approver;
        public string Approver
        {
            get { return _approver; }
            set { _approver = value; }
        }
        /// <summary>
        /// 项目
        /// </summary>		
        private string _project;
        public string Project
        {
            get { return _project; }
            set { _project = value; }
        }
        /// <summary>
        /// 子系统名称
        /// </summary>		
        private string _subsystem;
        public string Subsystem
        {
            get { return _subsystem; }
            set { _subsystem = value; }
        }
        /// <summary>
        /// 产品说明
        /// </summary>		
        private string _productdescription;
        public string ProductDescription
        {
            get { return _productdescription; }
            set { _productdescription = value; }
        }
        /// <summary>
        /// DeliveriesDetail
        /// </summary>		
        private string _deliveriesdetail;
        public string DeliveriesDetail
        {
            get { return _deliveriesdetail; }
            set { _deliveriesdetail = value; }
        }
        /// <summary>
        /// DeliveriesNum
        /// </summary>		
        private string _deliveriesnum;
        public string DeliveriesNum
        {
            get { return _deliveriesnum; }
            set { _deliveriesnum = value; }
        }
        /// <summary>
        /// Price
        /// </summary>		
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }
        /// <summary>
        /// PriceRemark
        /// </summary>		
        private string _priceremark;
        public string PriceRemark
        {
            get { return _priceremark; }
            set { _priceremark = value; }
        }
        /// <summary>
        /// 交付物及数量
        /// </summary>		
        private string _deliveriesquantities;
        public string DeliveriesQuantities
        {
            get { return _deliveriesquantities; }
            set { _deliveriesquantities = value; }
        }
        /// <summary>
        /// 交付地点
        /// </summary>		
        private string _deliveriesaddress;
        public string DeliveriesAddress
        {
            get { return _deliveriesaddress; }
            set { _deliveriesaddress = value; }
        }
        /// <summary>
        /// 项目阶段
        /// </summary>		
        private string _projectphase;
        public string ProjectPhase
        {
            get { return _projectphase; }
            set { _projectphase = value; }
        }
        /// <summary>
        /// 合同状态阶段
        /// </summary>		
        private string _contractstate;
        public string ContractState
        {
            get { return _contractstate; }
            set { _contractstate = value; }
        }
        /// <summary>
        /// 合同执行情况
        /// </summary>		
        private string _contractiimplementation;
        public string ContractIimplementation
        {
            get { return _contractiimplementation; }
            set { _contractiimplementation = value; }
        }
        /// <summary>
        /// 签订日期
        /// </summary>		
        private DateTime? _signingdate;
        public DateTime? SigningDate
        {
            get { return _signingdate; }
            set { _signingdate = value; }
        }
        /// <summary>
        /// 交付日期
        /// </summary>		
        private DateTime? _deliverdate;
        public DateTime? DeliverDate
        {
            get { return _deliverdate; }
            set { _deliverdate = value; }
        }
        /// <summary>
        /// 有效期
        /// </summary>		
        private string _validitydate;
        public string ValidityDate
        {
            get { return _validitydate; }
            set { _validitydate = value; }
        }
        /// <summary>
        /// 合同金额币种
        /// </summary>		
        private string _currency;
        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }
        /// <summary>
        /// 金额单位
        /// </summary>		
        private string _currencyunit;
        public string CurrencyUnit
        {
            get { return _currencyunit; }
            set { _currencyunit = value; }
        }
        /// <summary>
        /// 合同总金额
        /// </summary>		
        private decimal _totalcontractamount;
        public decimal TotalContractAmount
        {
            get { return _totalcontractamount; }
            set { _totalcontractamount = value; }
        }
        /// <summary>
        /// WarrantyMoney
        /// </summary>		
        private decimal _warrantymoney;
        public decimal WarrantyMoney
        {
            get { return _warrantymoney; }
            set { _warrantymoney = value; }
        }
        /// <summary>
        /// WarrantyTime
        /// </summary>		
        private string _warrantytime;
        public string WarrantyTime
        {
            get { return _warrantytime; }
            set { _warrantytime = value; }
        }
        /// <summary>
        /// 已开票总金额(开票节点金额合计)
        /// </summary>		
        private decimal _totalinvoice;
        public decimal TotalInvoice
        {
            get { return _totalinvoice; }
            set { _totalinvoice = value; }
        }
        /// <summary>
        /// 待收款金额(合同总金额-财务收款总金额)
        /// </summary>		
        private decimal _receivablesamount;
        public decimal ReceivablesAmount
        {
            get { return _receivablesamount; }
            set { _receivablesamount = value; }
        }
        /// <summary>
        /// 财务收款/财务支付总金额(财务实收节点合计)
        /// </summary>		
        private decimal _receiptstotalamount;
        public decimal ReceiptsTotalAmount
        {
            get { return _receiptstotalamount; }
            set { _receiptstotalamount = value; }
        }
        /// <summary>
        /// 约定收款金额1
        /// </summary>		
        private decimal _agreedmoney1;
        public decimal AgreedMoney1
        {
            get { return _agreedmoney1; }
            set { _agreedmoney1 = value; }
        }
        /// <summary>
        /// 约定收款时间1
        /// </summary>		
        private DateTime? _agreedtime1;
        public DateTime? AgreedTime1
        {
            get { return _agreedtime1; }
            set { _agreedtime1 = value; }
        }
        /// <summary>
        /// 约定收款金额2
        /// </summary>		
        private decimal _agreedmoney2;
        public decimal AgreedMoney2
        {
            get { return _agreedmoney2; }
            set { _agreedmoney2 = value; }
        }
        /// <summary>
        /// 约定收款时间2
        /// </summary>		
        private DateTime? _agreedtime2;
        public DateTime? AgreedTime2
        {
            get { return _agreedtime2; }
            set { _agreedtime2 = value; }
        }
        /// <summary>
        /// 约定收款金额3
        /// </summary>		
        private decimal _agreedmoney3;
        public decimal AgreedMoney3
        {
            get { return _agreedmoney3; }
            set { _agreedmoney3 = value; }
        }
        /// <summary>
        /// 约定收款时间3
        /// </summary>		
        private DateTime? _agreedtime3;
        public DateTime? AgreedTime3
        {
            get { return _agreedtime3; }
            set { _agreedtime3 = value; }
        }
        /// <summary>
        /// 约定收款金额4
        /// </summary>		
        private decimal _agreedmoney4;
        public decimal AgreedMoney4
        {
            get { return _agreedmoney4; }
            set { _agreedmoney4 = value; }
        }
        /// <summary>
        /// 约定收款时间4
        /// </summary>		
        private DateTime? _agreedtime4;
        public DateTime? AgreedTime4
        {
            get { return _agreedtime4; }
            set { _agreedtime4 = value; }
        }
        /// <summary>
        /// 约定收款金额5
        /// </summary>		
        private decimal _agreedmoney5;
        public decimal AgreedMoney5
        {
            get { return _agreedmoney5; }
            set { _agreedmoney5 = value; }
        }
        /// <summary>
        /// 约定收款时间5
        /// </summary>		
        private DateTime? _agreedtime5;
        public DateTime? AgreedTime5
        {
            get { return _agreedtime5; }
            set { _agreedtime5 = value; }
        }
        /// <summary>
        /// 财务实收金额1
        /// </summary>		
        private decimal _financialamount1;
        public decimal FinancialAmount1
        {
            get { return _financialamount1; }
            set { _financialamount1 = value; }
        }
        /// <summary>
        /// 财务实收时间1
        /// </summary>		
        private DateTime? _financialtime1;
        public DateTime? FinancialTime1
        {
            get { return _financialtime1; }
            set { _financialtime1 = value; }
        }
        /// <summary>
        /// 财务实收金额2
        /// </summary>		
        private decimal _financialamount2;
        public decimal FinancialAmount2
        {
            get { return _financialamount2; }
            set { _financialamount2 = value; }
        }
        /// <summary>
        /// 财务实收时间2
        /// </summary>		
        private DateTime? _financialtime2;
        public DateTime? FinancialTime2
        {
            get { return _financialtime2; }
            set { _financialtime2 = value; }
        }
        /// <summary>
        /// 财务实收金额3
        /// </summary>		
        private decimal _financialamount3;
        public decimal FinancialAmount3
        {
            get { return _financialamount3; }
            set { _financialamount3 = value; }
        }
        /// <summary>
        /// 财务实收时间3
        /// </summary>		
        private DateTime? _financialtime3;
        public DateTime? FinancialTime3
        {
            get { return _financialtime3; }
            set { _financialtime3 = value; }
        }
        /// <summary>
        /// 财务实收金额4
        /// </summary>		
        private decimal _financialamount4;
        public decimal FinancialAmount4
        {
            get { return _financialamount4; }
            set { _financialamount4 = value; }
        }
        /// <summary>
        /// 财务实收时间4
        /// </summary>		
        private DateTime? _financialtime4;
        public DateTime? FinancialTime4
        {
            get { return _financialtime4; }
            set { _financialtime4 = value; }
        }
        /// <summary>
        /// 财务实收金额5
        /// </summary>		
        private decimal _financialamount5;
        public decimal FinancialAmount5
        {
            get { return _financialamount5; }
            set { _financialamount5 = value; }
        }
        /// <summary>
        /// 财务实收时间5
        /// </summary>		
        private DateTime? _financialtime5;
        public DateTime? FinancialTime5
        {
            get { return _financialtime5; }
            set { _financialtime5 = value; }
        }
        /// <summary>
        /// 开票节点金额1
        /// </summary>		
        private decimal _ticketmoney1;
        public decimal TicketMoney1
        {
            get { return _ticketmoney1; }
            set { _ticketmoney1 = value; }
        }
        /// <summary>
        /// 开票节点时间1
        /// </summary>		
        private DateTime? _tickettime1;
        public DateTime? TicketTime1
        {
            get { return _tickettime1; }
            set { _tickettime1 = value; }
        }
        /// <summary>
        /// 开票节点金额2
        /// </summary>		
        private decimal _ticketmoney2;
        public decimal TicketMoney2
        {
            get { return _ticketmoney2; }
            set { _ticketmoney2 = value; }
        }
        /// <summary>
        /// 开票节点时间2
        /// </summary>		
        private DateTime? _tickettime2;
        public DateTime? TicketTime2
        {
            get { return _tickettime2; }
            set { _tickettime2 = value; }
        }
        /// <summary>
        /// 开票节点金额3
        /// </summary>		
        private decimal _ticketmoney3;
        public decimal TicketMoney3
        {
            get { return _ticketmoney3; }
            set { _ticketmoney3 = value; }
        }
        /// <summary>
        /// 开票节点时间3
        /// </summary>		
        private DateTime? _tickettime3;
        public DateTime? TicketTime3
        {
            get { return _tickettime3; }
            set { _tickettime3 = value; }
        }
        /// <summary>
        /// 开票节点金额4
        /// </summary>		
        private decimal _ticketmoney4;
        public decimal TicketMoney4
        {
            get { return _ticketmoney4; }
            set { _ticketmoney4 = value; }
        }
        /// <summary>
        /// 开票节点时间4
        /// </summary>		
        private DateTime? _tickettime4;
        public DateTime? TicketTime4
        {
            get { return _tickettime4; }
            set { _tickettime4 = value; }
        }
        /// <summary>
        /// 开票节点金额5
        /// </summary>		
        private decimal _ticketmoney5;
        public decimal TicketMoney5
        {
            get { return _ticketmoney5; }
            set { _ticketmoney5 = value; }
        }
        /// <summary>
        /// 开票节点时间5
        /// </summary>		
        private DateTime? _tickettime5;
        public DateTime? TicketTime5
        {
            get { return _tickettime5; }
            set { _tickettime5 = value; }
        }
        /// <summary>
        /// 计划收款/计划支付总金额
        /// </summary>		
        private decimal _plantotalamount;
        public decimal PlanTotalAmount
        {
            get { return _plantotalamount; }
            set { _plantotalamount = value; }
        }
        /// <summary>
        /// 是否有附件
        /// </summary>		
        private bool _hasfileattach;
        public bool HasFileAttach
        {
            get { return _hasfileattach; }
            set { _hasfileattach = value; }
        }
        /// <summary>
        /// 协作单位名称
        /// </summary>		
        private string _unitname;
        public string UnitName
        {
            get { return _unitname; }
            set { _unitname = value; }
        }
        /// <summary>
        /// 协作单位地址
        /// </summary>		
        private string _unitaddress;
        public string UnitAddress
        {
            get { return _unitaddress; }
            set { _unitaddress = value; }
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
        /// 联系方式
        /// </summary>		
        private string _lintype;
        public string LinType
        {
            get { return _lintype; }
            set { _lintype = value; }
        }
        /// <summary>
        /// 协作单位开户行
        /// </summary>		
        private string _openingbank;
        public string OpeningBank
        {
            get { return _openingbank; }
            set { _openingbank = value; }
        }
        /// <summary>
        /// 协作单位开户行账号
        /// </summary>		
        private string _openingaccount;
        public string OpeningAccount
        {
            get { return _openingaccount; }
            set { _openingaccount = value; }
        }
        /// <summary>
        /// 发票类型
        /// </summary>		
        private string _invoicetype;
        public string InvoiceType
        {
            get { return _invoicetype; }
            set { _invoicetype = value; }
        }
        /// <summary>
        /// 发票税率
        /// </summary>		
        private string _taxrate;
        public string TaxRate
        {
            get { return _taxrate; }
            set { _taxrate = value; }
        }
        /// <summary>
        /// 应开发票金额
        /// </summary>		
        private decimal _invoicevaluebe;
        public decimal InvoiceValueBe
        {
            get { return _invoicevaluebe; }
            set { _invoicevaluebe = value; }
        }
        /// <summary>
        /// 已开发票金额（开票节点金额合计）
        /// </summary>		
        private decimal _invoicevaluehas;
        public decimal InvoiceValueHas
        {
            get { return _invoicevaluehas; }
            set { _invoicevaluehas = value; }
        }
        /// <summary>
        /// 待开发票金额
        /// </summary>		
        private decimal _invoicevaluebefore;
        public decimal InvoiceValueBefore
        {
            get { return _invoicevaluebefore; }
            set { _invoicevaluebefore = value; }
        }
        /// <summary>
        /// 预算情况
        /// </summary>		
        private string _budgetsituation;
        public string BudgetSituation
        {
            get { return _budgetsituation; }
            set { _budgetsituation = value; }
        }
        /// <summary>
        /// 依据文件
        /// </summary>		
        private string _accordingdocument;
        public string AccordingDocument
        {
            get { return _accordingdocument; }
            set { _accordingdocument = value; }
        }
        /// <summary>
        /// 归档情况
        /// </summary>		
        private string _filingsituation;
        public string FilingSituation
        {
            get { return _filingsituation; }
            set { _filingsituation = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// 合同类型(1:收入合同 2：支出合同)
        /// </summary>		
        private int _ctype;
        public int CType
        {
            get { return _ctype; }
            set { _ctype = value; }
        }
        /// <summary>
        /// 扩展字段1
        /// </summary>		
        private decimal _cdefine1;
        public decimal cdefine1
        {
            get { return _cdefine1; }
            set { _cdefine1 = value; }
        }
        /// <summary>
        /// 扩展字段2
        /// </summary>		
        private decimal _cdefine2;
        public decimal cdefine2
        {
            get { return _cdefine2; }
            set { _cdefine2 = value; }
        }
        /// <summary>
        /// 扩展字段3
        /// </summary>		
        private decimal _cdefine3;
        public decimal cdefine3
        {
            get { return _cdefine3; }
            set { _cdefine3 = value; }
        }
        /// <summary>
        /// 扩展字段4
        /// </summary>		
        private string _cdefine4;
        public string cdefine4
        {
            get { return _cdefine4; }
            set { _cdefine4 = value; }
        }
        /// <summary>
        /// 扩展字段5
        /// </summary>		
        private string _cdefine5;
        public string cdefine5
        {
            get { return _cdefine5; }
            set { _cdefine5 = value; }
        }
        /// <summary>
        /// 扩展字段6
        /// </summary>		
        private string _cdefine6;
        public string cdefine6
        {
            get { return _cdefine6; }
            set { _cdefine6 = value; }
        }
        /// <summary>
        /// 扩展字段7
        /// </summary>		
        private string _cdefine7;
        public string cdefine7
        {
            get { return _cdefine7; }
            set { _cdefine7 = value; }
        }
        /// <summary>
        /// 扩展字段8
        /// </summary>		
        private string _cdefine8;
        public string cdefine8
        {
            get { return _cdefine8; }
            set { _cdefine8 = value; }
        }
        /// <summary>
        /// 扩展字段9
        /// </summary>		
        private string _cdefine9;
        public string cdefine9
        {
            get { return _cdefine9; }
            set { _cdefine9 = value; }
        }
        /// <summary>
        /// 扩展字段10
        /// </summary>		
        private string _cdefine10;
        public string cdefine10
        {
            get { return _cdefine10; }
            set { _cdefine10 = value; }
        }

        public string attribute1 { get; set; }
        public string attribute2 { get; set; }
        public string attribute3 { get; set; }
        public string attribute4 { get; set; }
        public string attribute5 { get; set; }
    }
}
