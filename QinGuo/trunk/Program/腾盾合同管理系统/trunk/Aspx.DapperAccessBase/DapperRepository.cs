using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Dapper;
using QINGUO.Common;
namespace QINGUO.DapperAccessBase
{
    public class DapperRepository<T> : DapperView where T : class,new()
    {

        public static SharePresentationLog log
        {
            get
            {
                return new SharePresentationLog();
            }
        }
        #region 表相关
        /// <summary>
        /// 获取表名称
        /// </summary>
        public string TableName
        {
            get
            {
                Type type = typeof(T);
                object[] primaryKeyObj = type.GetCustomAttributes(typeof(TableNameAttribute), true);
                if (primaryKeyObj.Count() > 0)//取第一个
                {
                    TableNameAttribute primaryKeyAttr = (TableNameAttribute)primaryKeyObj[0];
                    return primaryKeyAttr.Value;
                }
                return type.Name;
            }
        }
        /// <summary>
        /// 获取主键名称
        /// </summary>
        public string PrimaryKey
        {
            get
            {
                Type type = typeof(T);
                object[] tableNameObj = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (tableNameObj.Count() > 0)//取第一个
                {
                    PrimaryKeyAttribute tableNameAttr = (PrimaryKeyAttribute)tableNameObj[0];
                    return tableNameAttr.Value;
                }
                return type.Name + "ID";
            }
        }
        #endregion

        #region Get()
        /// <summary>
        /// 根据ID获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(object id)
        {

            try
            {
                return ReadDataBase.SingleOrDefault<T>(Sql.Builder.Select("*").From(TableName).Where(PrimaryKey + "=@0", id.ToString().Trim().Trim('\'')));
            }
            catch (Exception ex)
            {
                log.Info("Get:" + ex);
            }
            finally
            {
                ReadDataBase.CloseSharedConnection();
            }
            return null;
        }

        #endregion

        #region GetAll()
        /// <summary>
        /// 获取该表的所有实体对象
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            try
            {
                return ReadDataBase.Query<T>(Sql.Builder.Select("*").From(TableName)).ToList<T>();
            }
            catch (Exception ex)
            {
                log.Info("GetAll:" + ex);
            }
            finally
            {
                ReadDataBase.CloseSharedConnection();
            }
            return null;
        }
        #endregion

        #region Save()
        /// <summary>
        /// 保存该实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Save(T t)
        {
            bool isSuccess = true;
            try
            {
                WriteDataBase.BeginTransaction();
                object obj = WriteDataBase.Insert(t);
                WriteDataBase.CompleteTransaction();
            }
            catch (Exception ex)
            {
                log.Info("Save:" + ex);
                isSuccess = false;
                WriteDataBase.AbortTransaction();
                WriteDataBase.CloseSharedConnection();
            }
            finally
            {
                WriteDataBase.CloseSharedConnection();
            }
            return isSuccess;
        }
        #endregion

        #region Update()
        /// <summary>
        /// 根据实体进行更新操作   
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Update(T t)
        {
            bool isSuccess = true;
            try
            {
                WriteDataBase.BeginTransaction();
                isSuccess = WriteDataBase.Update(t) > 0;
                WriteDataBase.CompleteTransaction();
            }
            catch (Exception ex)
            {
                log.Info("Update:" + ex);
                isSuccess = false;
                WriteDataBase.AbortTransaction();
                WriteDataBase.CloseSharedConnection();
            }
            finally
            {
                WriteDataBase.CloseSharedConnection();
            }
            return isSuccess;
        }
        #endregion

        #region Delete(T t)
        /// <summary>
        /// 根据实体进行删除
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Delete(T t)
        {

            bool isSuccess = true;
            try
            {
                WriteDataBase.BeginTransaction();
                isSuccess = WriteDataBase.Delete(t) > 0;
                WriteDataBase.CompleteTransaction();
            }
            catch (Exception ex)
            {
                log.Info("Delete(T):" + ex);
                isSuccess = false;
                WriteDataBase.AbortTransaction();
                WriteDataBase.CloseSharedConnection();
            }
            finally
            {
                WriteDataBase.CloseSharedConnection();
            }
            return isSuccess;
        }
        #endregion

        #region DeleteStatus(T t)
        /// <summary>
        /// 根据实体进行软删除
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool DeleteStatus(T t)
        {
            bool isSuccess = true;
            try
            {
                WriteDataBase.BeginTransaction();
                isSuccess = WriteDataBase.DeleteStatus(t) > 0;
                WriteDataBase.CompleteTransaction();
            }
            catch (Exception ex)
            {
                log.Info("Delete(T):" + ex);
                isSuccess = false;
                WriteDataBase.AbortTransaction();
                WriteDataBase.CloseSharedConnection();
            }
            finally
            {
                WriteDataBase.CloseSharedConnection();
            }
            return isSuccess;
        }
        #endregion

        #region Delete(object id)
        /// <summary>
        /// 根据主键进行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(object id)
        {
            try
            {
                T t = Get(id);
                if (t != null)
                    return Delete(t);
                else
                    return false;
            }
            catch (Exception ex)
            {
                log.Info("Delete(id):" + ex);

            }
            return false;

        }
        #endregion

        #region DeleteStatus(object id)
        /// <summary>
        /// 根据主键进行软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteStatus(object id)
        {
            try
            {
                T t = Get(id);
                if (t != null)
                    return DeleteStatus(t);
                else
                    return false;
            }
            catch (Exception ex)
            {
                log.Info("Delete(id):" + ex);

            }
            return false;

        }
        #endregion

        #region SaveOrUpdate(T t)
        /// <summary>
        /// 更新或保存记录
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool SaveOrUpdate(T t)
        {
            try
            {
                Type type = typeof(T);
                object objPrimary = type.GetProperty(PrimaryKey).GetValue(t, null);
                if (objPrimary == null)
                    return Save(t);
                else
                    return Update(t);
            }
            catch (Exception ex)
            {
                log.Info("SaveOrUpdate(T):" + ex);
            }
            return false;
        }
        #endregion

        #region Merge(T t)
        /// <summary>
        /// 多表关联保存
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Merge(T t)
        {
            SharePresentationLog log = new SharePresentationLog();
            log.Info("调用了");
            return false;
        }
        #endregion

        #region 单表分页
        /// <summary>
        /// 单表分页 单表查询
        /// </summary>
        /// <returns></returns>
        public Page<T> GetPage(int pageIndex, int pageSize)
        {
            try
            {
                return ReadDataBase.Page<T>(1, 10, Sql.Builder.Select("*").From(TableName));
            }
            catch (Exception ex)
            {
                log.Info("GetPage(pageIndex,pageSize):" + ex);
            }
            finally
            {
                ReadDataBase.CloseSharedConnection();
            }
            return null;
        }
        /// <summary>
        /// 越过几条拿几条 单表查询
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">当前页容量</param>
        /// <returns></returns>
        public List<T> GetSkipTakePage(int pageIndex, int pageSize)
        {
            try
            {
                return ReadDataBase.SkipTake<T>((pageIndex - 1) * pageSize, pageSize, Sql.Builder.Select("*").From(TableName));
            }
            catch (Exception ex)
            {
                log.Info("GetSkipTakePage(pageIndex,pageSize):" + ex);
                return null;
            }
            finally
            {
                ReadDataBase.CloseSharedConnection();
            }
        }
        /// <summary>
        /// 分页Fetch机制
        /// </summary>
        /// <returns></returns>
        public List<T> GetFetchPage(int pageIndex, int pageSize)
        {
            try
            {
                return ReadDataBase.Fetch<T>(pageIndex, pageSize, Sql.Builder.Select("*").From(TableName));
            }
            catch (Exception ex)
            {
                log.Info("GetFetchPage(pageIndex,pageSize)" + ex);
            }
            finally
            {
                ReadDataBase.CloseSharedConnection();
            }
            return null;


        }
        #endregion

        #region 扩展方法

        #region 查询DataSet:QueryFillDataSet()
        /// <summary>
        /// 查询或则DataSet
        /// </summary>
        /// <param name="sql">存储过程名称或则纯sql语句</param>
        /// <param name="cmdType">CommandType 【SQL脚本Text】  【简单存储过程StoredProcedure】</param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataSet QueryFillDataSet(string sql, CommandType cmdType, params SqlParameter[] pms)
        {

            try
            {
                ReadDataBase.OpenSharedConnection();
                DataSet ds = new DataSet();
                using (var cmd = ReadDataBase.CreateCommand(ReadDataBase._sharedConnection, sql, true, pms))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null && pms.Count() > 0)
                    {
                        cmd.Parameters.Clear();
                        foreach (SqlParameter item in pms)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }
                    using (DbDataAdapter dbDataAdapter = ReadDataBase._factory.CreateDataAdapter())
                    {

                        dbDataAdapter.SelectCommand = (DbCommand)cmd;

                        dbDataAdapter.Fill(ds);
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("QueryFillDataSet:" + ex);
                //todu.....异常日志,关闭连接
                ReadDataBase.CloseSharedConnection();
                return null;
            }
            finally
            {
                //关闭连接
                ReadDataBase.CloseSharedConnection();
            }

        }
        #endregion

        #region 查询DataTable:QueryFillDataTable()
        /// <summary>
        /// 查询获取DataTable
        /// </summary>
        /// <param name="sql">存储过程名称或则纯sql语句</param>
        /// <param name="cmdType">CommandType 【SQL脚本Text】  【简单存储过程StoredProcedure】</param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataTable QueryFillDataTable(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            try
            {
                ReadDataBase.OpenSharedConnection();
                DataTable dt = new DataTable();
                using (var cmd = ReadDataBase.CreateCommand(ReadDataBase._sharedConnection, sql, true, pms))
                {
                    cmd.CommandType = cmdType;

                    if (pms != null && pms.Count() > 0)
                    {
                        cmd.Parameters.Clear();
                        foreach (SqlParameter item in pms)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }
                    using (DbDataAdapter dbDataAdapter = ReadDataBase._factory.CreateDataAdapter())
                    {
                        dbDataAdapter.SelectCommand = (DbCommand)cmd;
                        dbDataAdapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                //todu.....异常日志,关闭连接
                ReadDataBase.CloseSharedConnection();
                log.Info("QueryFillDataTable:" + ex);
                return null;
            }
            finally
            {
                ReadDataBase.CloseSharedConnection();
            }

        }
        #endregion

        #region 返回单一结果集:ExecuteDataRow()
        /// <summary>
        /// 返回单一结果集
        /// </summary>
        /// <param name="sql">存储过程名称或则纯sql语句</param>
        /// <param name="cmdType">CommandType 【SQL脚本Text】  【简单存储过程StoredProcedure】</param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataRow QueryDataRow(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            try
            {
                DataTable dt = QueryFillDataTable(sql, cmdType, pms);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                log.Info("QueryDataRow:" + ex);

            }
            return null;

        }

        #endregion

        #region SQL或则存储过程的增删改:ExecuteNoquery()
        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="sql">存储过程名称或则纯sql语句</param>
        /// <param name="cmdType">CommandType 【SQL脚本Text】  【简单存储过程StoredProcedure】</param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public int ExecuteNoquery(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            try
            {
                WriteDataBase.OpenSharedConnection();
                int flentCount = 0;
                try
                {
                    using (var cmd = WriteDataBase.CreateCommand(WriteDataBase._sharedConnection, sql, true, pms))
                    {
                        cmd.CommandType = cmdType;

                        if (pms != null && pms.Count() > 0)
                        {
                            cmd.Parameters.Clear();
                            foreach (SqlParameter item in pms)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        flentCount = cmd.ExecuteNonQuery();
                        return flentCount;
                    }
                }
                catch (Exception ex)
                {
                    //todu.....异常日志,关闭连接
                    log.Info("ExecuteNoquery:" + ex);
                }
                finally
                {
                    WriteDataBase.CloseSharedConnection();
                }
                return flentCount;
            }
            catch (Exception ex)
            {
                log.Info("ExecuteNoquery:" + ex);
            }
            finally
            {
                //todu.....异常日志,关闭连接
                WriteDataBase.CloseSharedConnection();
            }
            return 0;
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="sql">存储过程名称或则纯sql语句</param>
        /// <param name="cmdType">CommandType 【SQL脚本Text】  【简单存储过程StoredProcedure】</param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            try
            {
                ReadDataBase.OpenSharedConnection();
                object flentCount = null;
                try
                {
                    using (var cmd = WriteDataBase.CreateCommand(ReadDataBase._sharedConnection, sql, true, pms))
                    {
                        cmd.CommandType = cmdType;

                        if (pms != null && pms.Count() > 0)
                        {
                            cmd.Parameters.Clear();
                            foreach (SqlParameter item in pms)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        flentCount = cmd.ExecuteScalar();
                        return flentCount;
                    }
                }
                catch (Exception ex)
                {
                    //todu.....异常日志,关闭连接
                    ReadDataBase.CloseSharedConnection();
                    log.Info("ExecuteScalar:" + ex);
                }
                finally
                {
                    ReadDataBase.CloseSharedConnection();
                }
                return flentCount;
            }
            catch (Exception ex)
            {
                log.Info("ExecuteScalar:" + ex);
            }
            finally
            {
                ReadDataBase.CloseSharedConnection();
            }
            return null;
        }

        #endregion

        #endregion

    }
}
