using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;

namespace QINGUO.Business
{
    public class BllSysCategory : BllBase<ModSysCategory>
    {

        ISysCategory DAL = CreateDalFactory.CreateSysCategory();
        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
        /// <summary>
        /// 根据Id获取下一级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModSysCategory> GetChaildList(string id)
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

        public List<ModSysCategory> SearchDataLikeName(string name)
        {
            return DAL.SearchDataLikeName(name);
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
        public int UpdateDate(ModSysCategory t)
        {
            return DAL.UpdateDate(t);
        }

        /// <summary>
        /// 新增类别信息
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int InsertDate(ModSysCategory t)
        {
            return DAL.InsertDate(t);
        }

   
    }
}
