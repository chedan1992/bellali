#region Version Info
/* ======================================================================== 
* 【本类功能概述】 商品接口
* 
* 作者：张建 时间：2014/1/2 15:49:06 
* 文件名：IShopGoods 
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
using QINGUO.Common;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface IShopGoods : IBaseDAL<ModShopGoods>
    {
        /// <summary>
        /// 获取该公司下面的所有商品
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        DataSet GetGoodsByCompanyId(string companyID);
        /// <summary>
        /// 获取该公司下面的所有商品
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        DataSet GetGoodsByProName(string companyID, string proName);

        /// <summary>
        /// 获取商品详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ModShopGoods GetModel(string id);

        /// <summary>
        /// 商品状态修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        bool ChangeStatus(string id, int Status);
        /// <summary>
        /// 修改商品库存
        /// </summary>
        /// <param name="ID">商品ID</param>
        /// <param name="ProStock">商品购买数量</param>
        /// <returns></returns>
        bool UpdateProStock(string Id, int ProStock);
         /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ModShopGoods> QueryList(string where);
        /// <summary>
        /// 分页获取商品列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        Page<ModShopGoods> QueryList(int pageindex, int pagesize, string companyId, string name, string category, DateTime starttime, DateTime endtime, string ActiveID);

        /// <summary>
        /// 手机接口商品分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Page<ModShopGoods> Search(Search search);
    }
}
