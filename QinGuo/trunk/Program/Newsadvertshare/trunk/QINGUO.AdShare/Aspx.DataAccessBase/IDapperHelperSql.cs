using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using QINGUO.Common;

namespace QINGUO.DataAccessBase
{
    public interface IDapperHelperSql<T> where T : class,new()
    {

        Dapper.Database WriteDataBase { get; }

        Dapper.Database ReadDataBase { get; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        void CloseConnection();
        /// <summary>
        /// 提交事物
        /// </summary>
        void CommitTransaction();
        void BeginTransaction();

        System.Data.DataSet ExecuteDataSet(string procedureName, System.Data.CommandType ctype, DataParameters parameters);
        System.Data.DataSet ExecuteDataSet(string sql);

        System.Data.DataSet ExecuteDataSetForMsSql(string spName, object[] paraValues);

        int ExecuteNonQuery(string sqlString);
        int ExecuteNonQueryByStore(string procedureName, DataParameters cmdParms);
        int ExecuteNonQueryByText(string sql, DataParameters paraValues);
        int ExecuteNonQueryByText(string sql);

        void OpenConnection();
        System.Data.DataSet QueryPageToDataSet(out int total,Search search);
        string QueryPageToJson(Search search);

        void RollbackTransaction();


        object ExecuteScalar(string sql);
        object ExecuteScalar(string procedureName, DataParameters parameters);


        #region 数据库 DataRow 集合
        System.Data.DataRow ExecuteDataRow(string sql);
        System.Data.DataRow ExecuteDataRow(string sql, DataParameters parameters, CommandType cmdType = CommandType.Text);
        #endregion

        #region 存储过程执行 ByProc 集合
        System.Data.DataSet ExecuteDataSetByProc(string storedProcName, DataParameters parameters);
        #endregion

        #region 页面级获取数据
        System.Collections.Generic.List<T> QueryToAll();

        T LoadData(object primaryKeyValue);
        #endregion




        #region 轻量级分页
        Page<T> GetPage(int pageIndex, int pageSize);
        List<T> GetSkipTakePage(int pageIndex, int pageSize);
        List<T> GetFetchPage(int pageIndex, int pageSize);
        #endregion

        #region 增加 修改 删除 查询
        int Insert(T t);

        int Update(T t);

        int Delete(object primaryKeyValue);

        int Delete(T t);

        int DeleteStatus(object primaryKeyValue);
        #endregion


    }
}
