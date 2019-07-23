using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    //产品表
    [Serializable]
    [Dapper.TableNameAttribute("Shop_Goods")]
    [Dapper.PrimaryKeyAttribute("ID", autoIncrement = false)]
    public class ModShopGoods
    {
        /// <summary>
        /// 产品主键
        /// </summary>		
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 产品编号
        /// </summary>		
        private string _goodscode;
        public string GoodsCode
        {
            get { return _goodscode; }
            set { _goodscode = value; }
        }
        /// <summary>
        /// 产品名称
        /// </summary>		
        private string _goodname;
        public string GoodName
        {
            get { return _goodname; }
            set { _goodname = value; }
        }
        /// <summary>
        /// 产品类别
        /// </summary>		
        private string _goodscategoryid;
        public string GoodsCategoryId
        {
            get { return _goodscategoryid; }
            set { _goodscategoryid = value; }
        }
        /// <summary>
        /// 产品简码
        /// </summary>		
        private string _brevitycode;
        public string BrevityCode
        {
            get { return _brevitycode; }
            set { _brevitycode = value; }
        }
        /// <summary>
        /// 产品封面图片全称
        /// </summary>		
        private string _goodspic;
        public string GoodsPic
        {
            get { return _goodspic; }
            set { _goodspic = value; }
        }
        /// <summary>
        /// 产品原价
        /// </summary>		
        private decimal _rawprice;
        public decimal RawPrice
        {
            get { return _rawprice; }
            set { _rawprice = value; }
        }
        /// <summary>
        /// 产品现价
        /// </summary>		
        private decimal _nowprice;
        public decimal NowPrice
        {
            get { return _nowprice; }
            set { _nowprice = value; }
        }
        /// <summary>
        /// 产品总点击量
        /// </summary>		
        private int _clicknum;
        public int ClickNum
        {
            get { return _clicknum; }
            set { _clicknum = value; }
        }
        /// <summary>
        /// 商品属性类型
        /// </summary>		
        private string _attributetypeid;
        public string AttributeTypeId
        {
            get { return _attributetypeid; }
            set { _attributetypeid = value; }
        }
        /// <summary>
        /// 商品类型(0:普通商品 1:虚拟商品 2:积分商品)
        /// </summary>		
        private int _goodtype;
        public int GoodType
        {
            get { return _goodtype; }
            set { _goodtype = value; }
        }
        /// <summary>
        /// 产品库存
        /// </summary>		
        private int _storenum;
        public int StoreNum
        {
            get { return _storenum; }
            set { _storenum = value; }
        }
        /// <summary>
        /// 产品摘要
        /// </summary>		
        private string _goodsummary;
        public string GoodSummary
        {
            get { return _goodsummary; }
            set { _goodsummary = value; }
        }
        /// <summary>
        /// 产品描述
        /// </summary>		
        private string _gooddescribe;
        public string GoodDescribe
        {
            get { return _gooddescribe; }
            set { _gooddescribe = value; }
        }
        /// <summary>
        /// 消费积分(消费商品使用)
        /// </summary>		
        private int _costpoints;
        public int CostPoints
        {
            get { return _costpoints; }
            set { _costpoints = value; }
        }
        /// <summary>
        /// 返送积分(默认0 不参与返送)
        /// </summary>		
        private int _returnpoints;
        public int ReturnPoints
        {
            get { return _returnpoints; }
            set { _returnpoints = value; }
        }
        /// <summary>
        /// 排序编号
        /// </summary>		
        private int _sort;
        public int Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }
        /// <summary>
        /// FreightNumber
        /// </summary>		
        private string _freightnumber;
        public string FreightNumber
        {
            get { return _freightnumber; }
            set { _freightnumber = value; }
        }
        /// <summary>
        /// Remark
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// Batch
        /// </summary>		
        private string _batch;
        public string Batch
        {
            get { return _batch; }
            set { _batch = value; }
        }
        /// <summary>
        /// StyleNumber
        /// </summary>		
        private string _stylenumber;
        public string StyleNumber
        {
            get { return _stylenumber; }
            set { _stylenumber = value; }
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
        /// 创建人编号
        /// </summary>		
        private string _creatorid;
        public string CreatorId
        {
            get { return _creatorid; }
            set { _creatorid = value; }
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
        /// 状态
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

    }
}
