using System;
using System.Data;
using QINGUO.Common;

namespace QINGUO.DataAccessBase
{
    public interface IDbHelperSql
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        void CloseConnection();
        /// <summary>
        /// 提交事物
        /// </summary>
        void CommitTransaction();
        System.Data.DbType ConvertToDbType(object o);
        int Delete(object primaryKeyValue, Type type);
        int Delete<T>(object primaryKeyValue) where T : new();
        System.Data.DataRow ExecuteDataRow(string spName, object[] parms);
        System.Data.DataRow ExecuteDataRow(string sql);
        System.Data.DataRow ExecuteDataRow(string sql, DataParameters parameters, CommandType cmdType = CommandType.Text);
        System.Data.DataSet ExecuteDataSet(string procedureName, System.Data.CommandType ctype, DataParameters parameters);
        System.Data.DataSet ExecuteDataSet(string sql);
        System.Data.DataSet ExecuteDataSetByProc(string storedProcName, DataParameters parameters);
        System.Data.DataSet ExecuteDataSetForMsSql(string spName, object[] paraValues);
        int ExecuteNonQuery(System.Data.Common.DbCommand command);
        int ExecuteNonQuery(string sqlString);
        int ExecuteNonQueryByProc(string storedProcName, object[] paraValues, out int rowsAffected);
        int ExecuteNonQueryByStore(string procedureName, DataParameters cmdParms);
        int ExecuteNonQueryByStored(string procedureName, params object[] paraValues);
        int ExecuteNonQueryByText(string sql, DataParameters paraValues);
        int ExecuteNonQueryByText(string sql);
        int Insert(string tbName, System.Collections.Generic.Dictionary<string, object> columnValues);
        int Insert<T>(T t);
        T LoadData<T>(object primaryKeyValue) where T : new();
        void OpenConnection();
        System.Data.DataSet QueryPageToDataSet(out int total,Search search);
        string QueryPageToJson(Search search);
        System.Collections.Generic.List<T> QueryToAll<T>() where T : new();
        void RollbackTransaction();
        int UpDate(string tbName, System.Collections.Generic.Dictionary<string, object> editColumnValues, System.Collections.Generic.Dictionary<string, object> whereColumnValues);
        int Update<T>(T t);
        object ExecuteScalar(string sql);
        object ExecuteScalar(string procedureName, DataParameters parameters);
    }
}
