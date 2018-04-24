#region Version Info
/* ======================================================================== 
* 【本类功能概述】 商品数据层
* 
* 作者：张建 时间：2014/1/2 15:51:43 
* 文件名：DalShopGoods 
* 版本：V1.0.1 
* 
* 修改者： 时间： 
* 修改说明： 
* ======================================================================== 
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;
using QINGUO.Model;
using QINGUO.Common;

namespace QINGUO.DAL
{
    public class ShopGoodsDAL : BaseDAL<ModShopGoods>, IShopGoods
    {
        /// <summary>
        /// 分页获取活动商品
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="_category"></param>
        /// <param name="_starttime"></param>
        /// <param name="_endtime"></param>
        /// <returns></returns>
        public Page<ModShopGoods> QueryList(int pageindex, int pagesize, string companyId, string name, string _category, DateTime _starttime, DateTime _endtime, string ActiveID)
        {
            string stime = _starttime.ToString("yyyy-MM-dd HH:mm");
            string etime = _endtime.ToString("yyyy-MM-dd HH:mm");

            string pdtIds = " and Id not in(select * from GetProduct('" + companyId + "','" + _starttime + "','" + _endtime + "'))";
            if (ActiveID != "")
            {
                pdtIds = " and (id in(select ProductID from Shop_ActiveProduct where ActiveID='" + ActiveID + "') or Id not in(select * from GetProduct('" + companyId + "','" + _starttime + "','" + _endtime + "')))";
            }

            string category = _category == "" ? "" : "and CategoryID in('" + _category + "')";

            string sql = "select * from Shop_Goods where companyID='" + companyId + "' and ProName like '%" + name + "%' " + pdtIds + " " + category + " and status!=-1 order by AddTime desc";

            return dabase.ReadDataBase.Page<ModShopGoods>(pageindex, pagesize, sql);
        }
        /// <summary>
        /// 获取全部活动商品
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public List<ModShopGoods> QueryAll(string companyId, DateTime _starttime, DateTime _endtime)
        {
            string stime = _starttime.ToString("yyyy-MM-dd HH:mm");
            string etime = _endtime.ToString("yyyy-MM-dd HH:mm");

            string pdtIds = " and Id not in(select * from GetProduct('" + companyId + "','" + _starttime + "','" + _endtime + "'))";

            string sql = "select * from Shop_Goods where companyID='" + companyId + "' " + pdtIds + " and status!=-1 order by AddTime desc";

            return dabase.ReadDataBase.Query<ModShopGoods>(sql).ToList();
        }
        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModShopGoods> QueryList(string where)
        {
            return dabase.ReadDataBase.Query<ModShopGoods>("select * from Shop_Goods", null).ToList();
        }

        /// <summary>
        /// 商品状态修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool ChangeStatus(string id, int Status)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", id);
            paras.Add("@Status", Status);
            int result = dabase.ExecuteNonQueryByText("update Shop_Goods set Status=@Status where Id=@Id", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ModShopGoods GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" * ");
            strSql.Append(" from v_Shop_Goods ");
            strSql.Append(" where Id='" + id + "' ");
            ModShopGoods model = new ModShopGoods();
            DataSet ds = dabase.ExecuteDataSet(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ModShopGoods DataRowToModel(DataRow row)
        {
            ModShopGoods model = new ModShopGoods();
            if (row != null)
            {
                if (row["Id"] != null)
                {
                    model.Id = row["Id"].ToString();
                }
                if (row["CategoryId"] != null)
                {
                    model.CategoryId = row["CategoryId"].ToString();
                }
                if (row["ProdectTypeId"] != null)
                {
                    model.ProdectTypeId = row["ProdectTypeId"].ToString();
                }
                if (row["ProName"] != null)
                {
                    model.ProName = row["ProName"].ToString();
                }
                if (row["ProPic"] != null)
                {
                    model.ProPic = row["ProPic"].ToString();
                }
                if (row["ProOldPrice"] != null && row["ProOldPrice"].ToString() != "")
                {
                    model.ProOldPrice = decimal.Parse(row["ProOldPrice"].ToString());
                }
                if (row["IsDiscount"] != null && row["IsDiscount"].ToString() != "")
                {
                    if ((row["IsDiscount"].ToString() == "1") || (row["IsDiscount"].ToString().ToLower() == "true"))
                    {
                        model.IsDiscount = true;
                    }
                    else
                    {
                        model.IsDiscount = false;
                    }
                }
                if (row["ProNowPrice"] != null && row["ProNowPrice"].ToString() != "")
                {
                    model.ProNowPrice = decimal.Parse(row["ProNowPrice"].ToString());
                }
                if (row["ProInfo"] != null)
                {
                    model.ProInfo = row["ProInfo"].ToString();
                }
                if (row["ProStock"] != null && row["ProStock"].ToString() != "")
                {
                    model.ProStock = int.Parse(row["ProStock"].ToString());
                }
                if (row["ProHit"] != null && row["ProHit"].ToString() != "")
                {
                    model.ProHit = int.Parse(row["ProHit"].ToString());
                }
                if (row["ProBrief"] != null)
                {
                    model.ProBrief = row["ProBrief"].ToString();
                }
                if (row["CompanyId"] != null)
                {
                    model.CompanyId = row["CompanyId"].ToString();
                }
                if (row["ProKey"] != null)
                {
                    model.ProKey = row["ProKey"].ToString();
                }
                if (row["DiscountRate"] != null && row["DiscountRate"].ToString() != "")
                {
                    model.DiscountRate = int.Parse(row["DiscountRate"].ToString());
                }
                if (row["IsRecommend"] != null && row["IsRecommend"].ToString() != "")
                {
                    model.IsRecommend = int.Parse(row["IsRecommend"].ToString());
                }
                if (row["IsSpecial"] != null && row["IsSpecial"].ToString() != "")
                {
                    model.IsSpecial = int.Parse(row["IsSpecial"].ToString());
                }
                if (row["Status"] != null)
                {
                    model.Status = Convert.ToInt32(row["Status"]);
                }
                //if (row["playType"] != null)
                //{
                //    model.ProAllowSellType = row["playType"].ToString();
                //}
            }
            return model;
        }

        /// <summary>
        /// 获取该公司下面的所有商品
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public DataSet GetGoodsByCompanyId(string companyID)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@companyId", companyID);
            DataSet ds = dabase.ExecuteDataSetByProc("GetGoodsCompany", paras);
            return ds;
        }


        /// <summary>
        /// 获取该公司下面的所有商品
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public DataSet GetGoodsByProName(string companyID, string proName)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@companyId", companyID);
            paras.Add("@proName", "%" + proName + "%");
            DataSet ds = dabase.ExecuteDataSetByProc("GetGoodsByProName", paras);
            return ds;
        }
        /// <summary>
        /// 修改商品库存
        /// </summary>
        /// <param name="Id">商品Id</param>
        /// <param name="ProStock">商品购买数量</param>
        /// <returns></returns>
        public bool UpdateProStock(string Id, int ProStock)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", Id);
            paras.Add("@ProStock", ProStock);
            int result = dabase.ExecuteNonQueryByText("update Shop_Goods set ProStock=ProStock-@ProStock where Id=@Id", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 获取商家商品数量或者商家商品上线数量
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="status"></param>
        public int GetShopNumOrShopOnNum(string cid, int status)
        {
            //获取商家上级商品数量
            string sql = "select COUNT(*) from Shop_Goods where Status=@0 and CompanyId=@1 and IsTemplate=0";
            int goodGifCount = 0;
            if (status == (int)StatusEnum.删除)//获取商家商品总数量
            {
                sql = "select COUNT(*) from Shop_Goods where Status<>@0 and CompanyId=@1 and IsTemplate=0";
                string sqlGif = "select COUNT(*) from Shop_ProGIF where Status<>@0 and CompanyId=@1";
                goodGifCount = dabase.ReadDataBase.ExecuteScalar<int>(sqlGif, status, cid);
            }
            int goodCount = dabase.ReadDataBase.ExecuteScalar<int>(sql, status, cid);

            return goodCount + goodGifCount;
        }

        /// <summary>
        /// 手机接口商品分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Page<ModShopGoods> Search(Search search)
        {
            return dabase.ReadDataBase.Page<ModShopGoods>(search.CurrentPageIndex, search.PageSize, search.SqlString);
        }
    }
}
