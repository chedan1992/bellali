using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.DataAccessBase;
using QINGUO.Common;
using Dapper;
using QINGUO.DataAccessBase;

namespace QINGUO.DAL
{
    public class BaseDAL<T> where T : class,new()
    {
        protected IDapperHelperSql<T> dabase = new DapperHelper<T>();

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Insert(T t)
        {

            try
            {
                int influenceCount = dabase.Insert(t);
                return influenceCount;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Update(T t)
        {
            try
            {
                int influenceCount = dabase.Update(t);
                return influenceCount;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        public virtual int Delete(object primaryKeyValue)
        {
            try
            {
                int influenceCount = dabase.Delete(primaryKeyValue);
                return influenceCount;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        public virtual int DeleteStatus(object primaryKeyValue)
        {
            try
            {
                int influenceCount = dabase.DeleteStatus(primaryKeyValue);
                return influenceCount;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="primaryKeyValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual int Delete(T t)
        {
            try
            {
                int influenceCount = dabase.Delete(t);
                return influenceCount;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        #region 轻量级分页
        /// <summary>
        /// 单表分页 单表查询
        /// </summary>
        /// <returns></returns>
        public Page<T> GetPage(int pageIndex, int pageSize)
        {
            return dabase.GetPage(pageIndex, pageSize);
        }
        /// <summary>
        /// 越过几条拿几条 单表查询
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">当前页容量</param>
        /// <returns></returns>
        public List<T> GetSkipTakePage(int pageIndex, int pageSize)
        {
            return dabase.GetSkipTakePage(pageIndex, pageSize);
        }

        /// <summary>
        /// 分页Fetch机制
        /// </summary>
        /// <returns></returns>
        public List<T> GetFetchPage(int pageIndex, int pageSize)
        {
            return dabase.GetFetchPage(pageIndex, pageSize);
        }
        #endregion

        /// <summary>
        /// 获取分页查询集合,返回json
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public virtual string QueryPageToJson(Search search)
        {
            return dabase.QueryPageToJson(search);
        }


        /// <summary>
        /// 获取分页查询集合,返回DataSet
        /// </summary>
        /// <param name="total">记录条数</param>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public virtual DataSet QueryPageToDataSet(out int total, Search search)
        {
            return dabase.QueryPageToDataSet(out total, search);
        }

        /// <summary>
        /// 获取查询集合,返回List
        /// </summary>
        /// <typeparam name="T">集合类</typeparam>
        /// <returns></returns>
        public virtual List<T> QueryToAll()
        {
            return dabase.QueryToAll();
        }

        /// <summary>
        /// 根据主键,获取类集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        public virtual T LoadData(object primaryKeyValue)
        {
            return dabase.LoadData(primaryKeyValue);
        }

        /// <summary>
        /// 根据条件查询集合
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <param name="filedOrder">字段(filed1,filed2,filed2)或者（top 4 ,filed1,filed as name）...等sql语句就可以（""表示查询所有）</param>
        /// <param name="top">获取多少条,默认0</param>
        /// <returns>DataSet</returns>
        public virtual DataSet GetList(string tabName, string where, string filedOrder, int? top)
        {
            object[] paras = new object[3];
            paras[0] = tabName;
            paras[1] = where;
            if (filedOrder == "" || string.IsNullOrEmpty(filedOrder))
            {
                filedOrder = @" * ";
            }
            if (top > 0)
            {
                filedOrder = " top " + top + filedOrder;
            }
            paras[2] = filedOrder;
            DataSet ds = dabase.ExecuteDataSetForMsSql("Get_Model", paras);
            return ds;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="where">条件（field = value and field=value）</param>
        /// <param name="count">返回的条数（共有几条数据）</param>
        /// <returns>返回ture or false</returns>
        public virtual bool Exists(string tabName, string where, out int count)
        {
            DataParameters pms = new DataParameters();
            pms.Add("@TabName", tabName);
            pms.Add("@Where", where);

            DataRow dr = dabase.ExecuteDataRow("Proc_Exists", pms, CommandType.StoredProcedure);
            count = Convert.ToInt32(dr[0]);
            if (count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="where">条件（field = value and field=value）</param>
        /// <param name="count">返回的条数（共有几条数据）</param>
        /// <returns>返回ture or false</returns>
        public virtual int ExecuteNonQueryByText(string sql)
        {
            return dabase.ExecuteNonQueryByText(sql);
        }
    }
}
