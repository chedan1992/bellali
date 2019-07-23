using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Common;


namespace QINGUO.Business
{
    public abstract class BllBase<T> where T : class,new()
    {
        public BllBase()
        {
            SetCurrentReposiotry();
        }

        /// <summary>
        /// 当前DAL
        /// </summary>
        protected IBaseDAL<T> CurrentDAL
        {
            get;
            set;
        }
        public abstract void SetCurrentReposiotry();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Insert(T t)
        {
            try
            {
                return CurrentDAL.Insert(t);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Update(T t)
        {
            try
            {
                return CurrentDAL.Update(t);
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="primaryKeyValue"></param>
        /// <returns></returns>
        public virtual int Delete(object primaryKeyValue)
        {
            try
            {
                return CurrentDAL.Delete(primaryKeyValue);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// 数据软删除
        /// </summary>
        /// <param name="primaryKeyValue"></param>
        /// <returns></returns>
        public virtual int DeleteStatus(object primaryKeyValue)
        {
            try
            {
                return CurrentDAL.DeleteStatus(primaryKeyValue);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据主键获取集合
        /// </summary>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        public virtual T LoadData(object primaryKeyValue)
        {
            return CurrentDAL.LoadData(primaryKeyValue);
        }

        /// <summary>
        /// 获取集合,返回List
        /// </summary>
        /// <returns></returns>
        public virtual List<T> QueryToAll()
        {
            return CurrentDAL.QueryToAll();
        }

        /// <summary>
        /// 获取分页查询集合,返回DataSet
        /// </summary>
        /// <param name="total">记录条数</param>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public virtual DataSet QueryPageToDataSet(out int total, Search search)
        {
            return CurrentDAL.QueryPageToDataSet(out total, search);
        }

        /// <summary>
        /// 获取分页查询集合,返回json
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public virtual string QueryPageToJson(Search search)
        {
            return CurrentDAL.QueryPageToJson(search);
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
            return CurrentDAL.GetList(tabName, where, filedOrder, top);
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
            return CurrentDAL.Exists(tabName, where, out count);
        }


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="where">条件（field = value and field=value）</param>
        /// <param name="count">返回的条数（共有几条数据）</param>
        /// <returns>返回ture or false</returns>
        public virtual  int ExecuteNonQueryByText(string sql)
        {
            return CurrentDAL.ExecuteNonQueryByText(sql);
        }

        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="where">条件（field = value and field=value）</param>
        /// <param name="count">返回的条数（共有几条数据）</param>
        /// <returns>返回ture or false</returns>
        public virtual DataSet ExecuteDataSet(string sql)
        {
            return CurrentDAL.ExecuteDataSet(sql);
        } 
    }
}
