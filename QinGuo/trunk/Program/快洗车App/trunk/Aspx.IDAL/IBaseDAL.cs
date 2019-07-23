using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Common;

namespace QINGUO.IDAL
{
    public interface IBaseDAL<T> where T : class,new()
    {

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        int Insert(T t);
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        int Update(T t);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        int Delete(object primaryKeyValue);

        /// <summary>
        /// 根据主键软删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        int DeleteStatus(object primaryKeyValue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="primaryKeyValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int Delete(T t);
        /// <summary>
        /// 获取分页查询集合,返回json
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        string QueryPageToJson(Search search);

        /// <summary>
        /// 获取分页查询集合,返回DataSet
        /// </summary>
        /// <param name="total">记录条数</param>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        DataSet QueryPageToDataSet(out int total, Search search);
        /// <summary>
        /// 获取查询集合,返回List
        /// </summary>
        /// <typeparam name="T">集合类</typeparam>
        /// <returns></returns>
        List<T> QueryToAll();
        /// <summary>
        /// 根据主键,获取类集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns></returns>
        T LoadData(object primaryKeyValue);

        /// <summary>
        /// 根据条件查询集合
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <param name="filedOrder">字段(filed1,filed2,filed2)或者（top 4 ,filed1,filed as name）...等sql语句就可以（""表示查询所有）</param>
        /// <param name="top">获取多少条,默认0</param>
        /// <returns>DataSet</returns>
        DataSet GetList(string tabName, string where, string filedOrder, int? top);
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="tabName">表名</param>
        /// <param name="where">条件（field = value and field=value）</param>
        /// <param name="count">返回的条数（共有几条数据）</param>
        /// <returns>返回ture or false</returns>
        bool Exists(string tabName, string where, out int count);
        /// <summary>
        /// 执行sql,返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteNonQueryByText(string sql);
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string sql);
    }
}
