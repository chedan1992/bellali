#region Version Info
/* ======================================================================== 
* 【本类功能概述】 商品业务逻辑层
* 
* 作者：张建 时间：2014/1/2 15:54:08 
* 文件名：BllShopGoods 
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
using QINGUO.Factory;
using QINGUO.Common;
using QINGUO.Model;

namespace QINGUO.Business
{
    public class BllShopGoods : BllBase<ModShopGoods>
    {
        IShopGoods DAL = CreateDalFactory.CreateShopGoodsDAL();
        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
        /// <summary>
        /// 获取活动商品
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public Page<ModShopGoods> QueryList(int pageindex, int pagesize, string companyId, string name, string category, DateTime starttime, DateTime endtime, string ActiveID)
        {
            return DAL.QueryList(pageindex, pagesize, companyId, name, category, starttime, endtime, ActiveID);
        }
        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModShopGoods> QueryList(string where)
        {
            return DAL.QueryList(where);
        }
        /// <summary>
        /// 获取商品详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ModShopGoods GetModel(string id)
        {
            return DAL.GetModel(id);
        }
        /// <summary>
        /// 修改商品状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool ChangeStatus(string id, int Status)
        {

            return DAL.ChangeStatus(id, Status);
        }

        /// <summary>
        /// 获取该公司下面的所有商品
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public List<ModShopGoods> GetGoodsByCompanyId(string companyID, string GoodsName)
        {
            List<ModShopGoods> list = null;
            DataSet ds = null;
            if (string.IsNullOrEmpty(GoodsName))
                ds = DAL.GetGoodsByCompanyId(companyID);
            else
                ds = DAL.GetGoodsByProName(companyID, GoodsName);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        list = new List<ModShopGoods>();
                        foreach (DataRow row in dt.Rows)
                        {
                            ModShopGoods goods = new ModShopGoods();
                            goods.Id = row["Id"] == null ? "" : Convert.ToString(row["Id"]);
                            goods.ProName = row["ProName"] == null ? "" : Convert.ToString(row["ProName"]);
                            goods.ProNowPrice = row["ProNowPrice"] == null ? Convert.ToDecimal("0.0") : Convert.ToDecimal(row["ProNowPrice"]);
                            goods.ProOldPrice = row["ProOldPrice"] == null ? Convert.ToDecimal("0.0") : Convert.ToDecimal(row["ProOldPrice"]);
                            list.Add(goods);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        public IList<ModShopGoods> PageSizeList(int pageSize, int pageIndex, string where, out int total)
        {
            Search search = new Search();
            search.PageSize = pageSize;
            search.CurrentPageIndex = pageIndex;
            search.TableName = @"v_Shop_Goods"; //表名
            search.SelectedColums = "[Id],[ProdectTypeId],[ChildId],[hyId],[ProName],[ProPic],[ProOldPrice],[IsDiscount],[ProNowPrice],[ProStock],[ProHit],[CompanyId],[AddTime],[ProKey],[DiscountRate],[IsRecommend],[IsSpecial],[IsWaitConfirm],[CName],[ParentId],[Status],[IsTemplate]";
            search.KeyFiled = "Id"; //主键
            search.SortField = " AddTime desc";
            if (!string.IsNullOrEmpty(where))
            {
                search.AddCondition(where); //过滤条件
            }

            // search.StatusDefaultCondition = "";
            DataTable dt = base.QueryPageToDataSet(out total, search).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {

                return DataTableToList(dt);
            }
            return new List<ModShopGoods>();
        }

        /// <summary>
        /// 修改商品库存
        /// </summary>
        /// <param name="Id">商品Id</param>
        /// <param name="ProStock">商品购买数量</param>
        /// <returns></returns>
        public bool UpdateProStock(string Id, int ProStock)
        {
            return DAL.UpdateProStock(Id, ProStock);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public IList<ModShopGoods> DataTableToList(DataTable dt)
        {
            IList<ModShopGoods> modelList = new List<ModShopGoods>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                ModShopGoods model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        public ModShopGoods DataRowToModel(DataRow row)
        {

            var model = new ModShopGoods();
            if (row != null)
            {
                if (row["Id"] != null)
                {
                    model.Id = row["Id"].ToString();
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

                if (row["ProStock"] != null && row["ProStock"].ToString() != "")
                {
                    model.ProStock = int.Parse(row["ProStock"].ToString());
                }
                if (row["ProHit"] != null && row["ProHit"].ToString() != "")
                {
                    model.ProHit = int.Parse(row["ProHit"].ToString());
                }
                if (row["CompanyId"] != null)
                {
                    model.CompanyId = row["CompanyId"].ToString();
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
                    model.Status = Convert.ToInt32(row["Status"].ToString());
                }

            }
            return model;
        }


        /// <summary>
        /// 获取商品分页数据--商品选择
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"Shop_Goods";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.StatusDefaultCondition = "";
            search.SortField = "Code asc";//排序
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 手机接口商品分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Page<ModShopGoods> Search(Search search)
        {
            search.TableName = @"Shop_Goods";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.StatusDefaultCondition = "Status=" + (int)StatusEnum.正常;
            return DAL.Search(search);
        }
    }
}
