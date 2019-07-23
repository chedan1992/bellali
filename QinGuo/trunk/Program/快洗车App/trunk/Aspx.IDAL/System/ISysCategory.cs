using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ISysCategory : IBaseDAL<ModSysCategory>
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
        List<ModSysCategory> SearchDataLikeName(string name);
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
        int UpdateDate(ModSysCategory t);

        /// <summary>
        /// 新增类别信息
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        int InsertDate(ModSysCategory t);
        /// <summary>
        /// 根据Id获取下一级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ModSysCategory> GetChaildList(string id);
    }
}
