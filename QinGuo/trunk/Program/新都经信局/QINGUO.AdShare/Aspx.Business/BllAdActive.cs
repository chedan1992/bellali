using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.Business
{
    public class BllAdActive : BllBase<ModAdActive>
    {
        private IAdActive DAL = CreateDalFactory.CreateAdActive();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(string where)
        {
            return DAL.Exists(where);
        }

        /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModAdActiveView GetModelByWhere(string where)
        {
            var model=DAL.GetModelByWhere(where);
            return model;
        }

        /// <summary>
        /// 获取公告分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            //更新过期广告
            Expired();
            search.TableName = @"Cor_Ad_Active";//表名
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status!=" + (int)StatusEnum.删除);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 获取最新的公告列表
        /// </summary>
        /// <param name="IwillId"></param>
        /// <returns></returns>
        public List<ModAdActive> QueryAll(int? top)
        {
            return DAL.QueryAll(top);
        }

        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateStatus(int flag, string key)
        {
            return DAL.UpdateStatus(flag, key);
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Dapper.Page<ModAdActiveView> Search(Search search)
        {
            search.TableName = @"Cor_Ad_Active";//表名
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "IsTop desc,CreateTime desc";//排序
            return DAL.Search(search);
        }


        /// <summary>
        /// 广告过期
        /// </summary>
        /// <returns></returns>
        public int Expired()
        {
            return DAL.Expired();
        }

         /// <summary>
        /// 根据用户编号获取用户名称
        /// </summary>
        /// <param name="Idlist"></param>
        /// <returns></returns>
        public DataSet GetPersonalName(string Idlist)
        {
            return DAL.GetPersonalName(Idlist);
        }
    }
}
