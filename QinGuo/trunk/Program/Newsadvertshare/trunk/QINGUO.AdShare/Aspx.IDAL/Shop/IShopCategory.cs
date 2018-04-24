#region Version Info
/* ======================================================================== 
* 【本类功能概述】 产品分类接口
* 
* 作者：张建 时间：2013/12/31 15:26:17 
* 文件名：IShopCategory 
* 版本：V1.0.1 
* 
* 修改者： 时间： 
* 修改说明： 
* ======================================================================== 
*/
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface IShopCategory : IBaseDAL<ModShopCategory>
    {
        /// <summary>
        /// 查询类别根节点
        /// </summary>
        /// <param name="parentCategoryId">父节点</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        DataSet SearchData(string parentCategoryId, string where);
        /// <summary>
        /// 获得树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        DataSet GetTreeList(string strWhere);
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<ModShopCategory> SearchDataLikeName(string name);
        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        int UpdateIsStatus(int status, string key);

        /// <summary>
        /// 删除商品类别及子类别
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        int DeleteDate(string key);

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        int UpdateDate(ModShopCategory t);

        /// <summary>
        /// 新增类别信息
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        int InsertDate(ModShopCategory t);
        /// <summary>
        /// 根据商家Id和节点深度获取
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="Depth"></param>
        /// <returns></returns>
        List<string> SearchByCompany(string comId, string Depth);
        /// <summary>
        /// 根据商家Id和节点深度获取
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="Depth"></param>
        /// <returns></returns>
        DataSet SearchByCompanys(string comId, string Depth);

        List<string> GetCompanySellProType(string cid);
        /// <summary>
        /// 根据Id获取下一级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ModShopCategory> GetChaildList(string id);

           /// <summary>
        /// 更改商品类别,同步分公司开通管理类别信息
        /// </summary>
        /// <param name="NodeId"></param>
        int UpdateSynchro(string NodeId);

        /// <summary>
        /// 获取全部正常分类
        /// </summary>
        /// <returns></returns>
        List<ModShopCategory> GetCategory();
    }
}
