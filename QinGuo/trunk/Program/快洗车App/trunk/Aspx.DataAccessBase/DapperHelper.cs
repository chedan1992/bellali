using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using QINGUO.Common;
using QINGUO.DapperAccessBase;

namespace QINGUO.DataAccessBase
{
    public class DapperHelper<T> : IDapperHelperSql<T> where T : class,new()
    {
        DapperRepository<T> dapper = new DapperRepository<T>();

        #region DataBase
        /// <summary>
        /// 主库进行插入修改删除
        /// </summary>
        public Database WriteDataBase
        {
            get
            {
                return dapper.WriteDataBase;
            }
        }
        /// <summary>
        /// 从库进行读取
        /// </summary>
        public Database ReadDataBase
        {
            get
            {
                return dapper.ReadDataBase;
            }
        }
        #endregion

        #region 数据库链接
        /// <summary>
        /// 打开数据库链接
        /// </summary>
        public void OpenConnection()
        {
            dapper.WriteDataBase.OpenSharedConnection();
        }
        /// <summary>
        /// 关闭数据库链接
        /// </summary>
        public void CloseConnection()
        {
            dapper.WriteDataBase.CloseSharedConnection();
        }
        #endregion

        #region 数据库事务
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            dapper.WriteDataBase.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            dapper.WriteDataBase.CompleteTransaction();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            dapper.WriteDataBase.AbortTransaction();
        }
        #endregion

        #region 数据库操作公用方法
        public object ExecuteScalar(string sql)
        {
            return dapper.ExecuteScalar(sql, CommandType.Text);
        }
        public object ExecuteScalar(string procedureName, DataParameters parameters)
        {
            SqlParameter[] pms = GetParameter(parameters);
            return dapper.ExecuteScalar(procedureName, CommandType.StoredProcedure, pms);
        }
        public System.Data.DataSet ExecuteDataSetForMsSql(string spName, object[] paraValues)
        {
            SqlParameter[] pms = 
            {
                new SqlParameter("@TabName",paraValues[0]),
                new SqlParameter("@Where",paraValues[1]),
                new SqlParameter("@FiledOrder",paraValues[2])
            };
            return dapper.QueryFillDataSet(spName, CommandType.StoredProcedure, pms);
        }
        #endregion

        #region 数据库 ExecuteNonQuery 集合
        public int ExecuteNonQuery(string sqlString)
        {
            return dapper.ExecuteNoquery(sqlString, CommandType.Text);
        }
        public int ExecuteNonQueryByStore(string procedureName, DataParameters cmdParms)
        {
            SqlParameter[] pms = GetParameter(cmdParms);
            return dapper.ExecuteNoquery(procedureName, CommandType.StoredProcedure, pms);
        }
        public int ExecuteNonQueryByText(string sql, DataParameters paraValues)
        {
            SqlParameter[] pms = GetParameter(paraValues);
            return dapper.ExecuteNoquery(sql, CommandType.Text, pms);
        }
        public int ExecuteNonQueryByText(string sql)
        {
            SqlParameter[] pms = new SqlParameter[0];
            return dapper.ExecuteNoquery(sql, CommandType.Text, pms);
        }
        #endregion

        #region 数据库 DataSet 集合

        public System.Data.DataSet ExecuteDataSet(string sql)
        {
            return dapper.QueryFillDataSet(sql, CommandType.Text);
        }

        public System.Data.DataSet ExecuteDataSet(string procedureName, System.Data.CommandType ctype, DataParameters parameters)
        {
            SqlParameter[] pms = GetParameter(parameters);
            return dapper.QueryFillDataSet(procedureName, ctype, pms);
        }


        #endregion

        #region 数据库 DataRow 集合

        public System.Data.DataRow ExecuteDataRow(string sql)
        {
            return dapper.QueryDataRow(sql, CommandType.Text);
        }

        public System.Data.DataRow ExecuteDataRow(string sql, DataParameters parameters, CommandType cmdType = CommandType.Text)
        {
            SqlParameter[] pms = GetParameter(parameters);
            return dapper.QueryDataRow(sql, cmdType, pms);
        }

        #endregion

        #region 存储过程执行 ByProc 集合

        public System.Data.DataSet ExecuteDataSetByProc(string storedProcName, DataParameters parameters)
        {
            SqlParameter[] pms = GetParameter(parameters);
            return dapper.QueryFillDataSet(storedProcName, CommandType.StoredProcedure, pms);
        }


        #endregion

        #region ===页面查询类 QueryPage ===
        public string QueryPageToJson(Search search)
        {
            int total = 0;
            DataSet ds = QueryPageList(search, out total);
            string jsonResult = JsonHelper.DataTableToJsonForGridDataSuorce(ds.Tables[0], total);
            return jsonResult;

        }
        public System.Data.DataSet QueryPageToDataSet(out int total,Search search)
        {
            return QueryPageList(search, out total);
        }
        public List<T> QueryToAll()
        {
            return dapper.GetAll();
        }
        public T LoadData(object primaryKeyValue)
        {
            return dapper.Get(primaryKeyValue);
        }
        #endregion


        #region MyRegion
        /// <summary>
        /// 单表分页 单表查询
        /// </summary>
        /// <returns></returns>
        public Page<T> GetPage(int pageIndex, int pageSize)
        {
            return dapper.GetPage(pageIndex, pageSize);
        }
        /// <summary>
        /// 越过几条拿几条 单表查询
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">当前页容量</param>
        /// <returns></returns>
        public List<T> GetSkipTakePage(int pageIndex, int pageSize)
        {
            return dapper.GetSkipTakePage(pageIndex, pageSize);
        }

        /// <summary>
        /// 分页Fetch机制
        /// </summary>
        /// <returns></returns>
        public List<T> GetFetchPage(int pageIndex, int pageSize)
        {
            return dapper.GetFetchPage(pageIndex, pageSize);
        }
        #endregion

        #region ===页面新增===
        public int Insert(T t)
        {
            return dapper.Save(t) ? 1 : 0;
        }
        #endregion

        #region ===页面修改===
        public int Update(T t)
        {
            return dapper.Update(t) ? 1 : 0;
        }
        #endregion

        #region ===页面删除===
        public int Delete(object primaryKeyValue)
        {
            return dapper.Delete(primaryKeyValue) ? 1 : 0;
        }

        public int Delete(T t)
        {
            return dapper.Delete(t) ? 1 : 0;
        }

        public int DeleteStatus(object primaryKeyValue)
        {
            return dapper.DeleteStatus(primaryKeyValue) ? 1 : 0;
        }
        #endregion

        #region 扩展方法
        public SqlParameter[] GetParameter(DataParameters cmdParms)
        {
            SqlParameter[] pms = new SqlParameter[cmdParms.Items.Count];
            for (int i = 0; i < cmdParms.Items.Count; i++)
            {
                pms[i] = new SqlParameter();
                if (!string.IsNullOrEmpty(cmdParms.Items[i].ParameterName))
                {
                    pms[i].ParameterName = cmdParms.Items[i].ParameterName;
                    pms[i].Value = cmdParms.Items[i].ParameterValue == null ? "" : cmdParms.Items[i].ParameterValue;
                }
            }
            return pms;
        }

        public DataSet QueryPageList(Search search, out int total)
        {
            var str = search.SortField;
            if(str.ToLower().IndexOf("remark")>-1)
            {
                string[] newstr = str.Split(' ');
                search.SortField = "cast(" + newstr[0] + " as nvarchar(max))" + " " + newstr[newstr.Length-1];  
            }
            total = 0;
            SqlParameter[] pms = 
            {
                new SqlParameter("@FieldKey",search.KeyFiled),
                new SqlParameter("@FieldShow",search.SelectedColums),
                new SqlParameter("@tbname",search.TableName),
                new SqlParameter("@Where",search.GetConditon()),
                new SqlParameter("@FieldOrder",search.SortField),
                new SqlParameter("@PageCurrent",search.CurrentPageIndex),
                new SqlParameter("@PageSize",search.PageSize)
            };
            DataSet ds = dapper.QueryFillDataSet("SP_PageList", CommandType.StoredProcedure, pms);
            if (ds.Tables.Count != 2 || ds.Tables[1].Rows.Count == 0)
            {
                return null;
            }
            total = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]);
            return ds;
        }
        #endregion



    }
}
