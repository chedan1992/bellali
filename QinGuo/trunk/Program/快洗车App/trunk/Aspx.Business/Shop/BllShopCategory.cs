#region Version Info
/* ======================================================================== 
* 【本类功能概述】 产品分类业务层
* 
* 作者：张建 时间：2013/12/31 15:48:37 
* 文件名：BllShopCategory 
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
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Model;

namespace QINGUO.Business
{
    public class BllShopCategory : BllBase<ModShopCategory>
    {
        IShopCategory DAL = CreateDalFactory.CreateShopCategoryDAL();
        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 根据Id获取下一级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModShopCategory> GetChaildList(string id)
        {
            return DAL.GetChaildList(id);
        }
        /// <summary>
        /// 查询类别根节点
        /// </summary>
        /// <param name="parentCategoryId">父节点</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet SearchData(string parentCategoryId, string where)
        {
            return DAL.SearchData(parentCategoryId, where);
        }

         /// <summary>
        /// 获得树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            return DAL.GetTreeList(strWhere);
        }

        public List<ModShopCategory> SearchDataLikeName(string name)
        {
            return DAL.SearchDataLikeName(name);
        }

        /// <summary>
        /// 根据公司id查询分类（接口用到的行业标签和标签）
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="Depth">深度</param>
        /// <returns></returns>
        public List<string> SearchByCompany(string comId, string Depth)
        {
            return DAL.SearchByCompany( comId,  Depth);
        }
        /// <summary>
        /// 根据公司id查询分类（接口用到的行业标签和标签）
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="Depth">深度</param>
        /// <returns></returns>
        public DataSet SearchByCompanys(string comId, string Depth)
        {
            return DAL.SearchByCompanys(comId, Depth);
        }
        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateIsStatus(int status, string key)
        {
            return DAL.UpdateIsStatus(status, key);
        }

        /// <summary>
        /// 删除商品类别及子类别
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int DeleteDate(string key)
        {
            return DAL.DeleteDate(key);
        }

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int UpdateDate(ModShopCategory t)
        {
            return DAL.UpdateDate(t);
        }

        /// <summary>
        /// 新增类别信息
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int InsertDate(ModShopCategory t)
        {
            return DAL.InsertDate(t);
        }

        /// <summary>
        /// 获取公司商品分类
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<string> GetCompanySellProType(string cid)
        {
            return DAL.GetCompanySellProType(cid);
        }

          /// <summary>
        /// 更改商品类别,同步分公司开通管理类别信息
        /// </summary>
        /// <param name="NodeId"></param>
        public int UpdateSynchro(string NodeId)
        {
            return DAL.UpdateSynchro(NodeId); 
        }

        /// <summary>
        /// 获取全部正常分类
        /// </summary>
        public List<ModShopCategory> GetCategory()
        {
            return DAL.GetCategory(); 
        }
    }
}
