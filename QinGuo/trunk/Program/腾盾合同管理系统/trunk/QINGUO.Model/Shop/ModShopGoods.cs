#region Version Info
/* ======================================================================== 
* 【本类功能概述】 商品表
* 
* 作者：zhangjian 时间：2014/1/2 15:47:23 
* 文件名： ModShop_Goods.cs
* 版本：V1.0.1 
* 
* 修改者： 时间： 
* 修改说明： 
* ======================================================================== 
*/
#endregion

using System;
using System.Collections.Generic;
using QINGUO.Common;
namespace QINGUO.Model
{

    //Shop_Goods
    [Serializable]
    [Dapper.TableNameAttribute("Shop_Goods")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModShopGoods
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>		
        public string ProdectTypeId { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>		
        public string CategoryId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>		
        public string ProName { get; set; }
        /// <summary>
        /// 商品标示图
        /// </summary>		
        public string ProPic { get; set; }
        /// <summary>
        /// 商品原价
        /// </summary>		
        public decimal ProOldPrice { get; set; }
        /// <summary>
        /// 是否折扣
        /// </summary>		
        public bool IsDiscount { get; set; }
        /// <summary>
        /// 商品现价
        /// </summary>		
        public decimal ProNowPrice { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>		
        public string ProInfo { get; set; }
        /// <summary>
        /// 商品库存
        /// </summary>		
        public int ProStock { get; set; }
        /// <summary>
        /// 商品点击量
        /// </summary>		
        public int ProHit { get; set; }
        /// <summary>
        /// 商品简介
        /// </summary>		
        public string ProBrief { get; set; }
        /// <summary>
        /// 商品所属店铺
        /// </summary>		
        public string CompanyId { get; set; }
        /// <summary>
        /// 商品关键字
        /// </summary>		
        public string ProKey { get; set; }
        /// <summary>
        /// 折扣率
        /// </summary>		
        public int DiscountRate { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>		
        public int IsRecommend { get; set; }
        /// <summary>
        /// 是否特价
        /// </summary>		
        public int IsSpecial { get; set; }
      
        /// <summary>
        /// 商品状态(1为正常，0为下架，-1为删除)
        /// </summary>		
        public int Status { get; set; }
          /// <summary>
        /// 商品编码
        /// </summary>		
        public string Code { get; set; }
        private DateTime? CreateTime{get;set;}
        /// <summary>
        /// 助记码
        /// </summary>		
        private string _shorthand;
        public string shorthand
        {
            get { return _shorthand; }
            set { _shorthand = value; }
        }
        /// <summary>
        /// 规格型号
        /// </summary>		
        private string _specification;
        public string specification
        {
            get { return _specification; }
            set { _specification = value; }
        }



    }
}