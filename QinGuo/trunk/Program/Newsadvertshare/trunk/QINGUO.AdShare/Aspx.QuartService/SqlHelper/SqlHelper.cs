using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace QINGUO.QuartService
{
    //写成静态类，当调用该类方法的时候不用在创建一个新对象
    public static class SqlHelper
    {
        private static readonly string constr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

        /// <summary>
        /// 执行Insert，delete，Update时返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType">SqlCommand命令类型（存储过程，T—Sql语句，等等）</param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, CommandType cmdType, params SqlParameter[] pms)
        {

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }

        }
        /// <summary>
        /// 返回查询的首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType">SqlCommand命令类型（存储过程，T—Sql语句，等等）</param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }

        }
        /// <summary>
        /// 执行select语句时返回查询的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType">SqlCommand命令类型（存储过程，T—Sql语句，等等）</param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuDataReader(string sql, CommandType cmdType, params SqlParameter[] pms)
        {

            //因为ExecuteReader是一行一行的读，所以必须保证con是连接的，因此这里不能用using自动释放资源
            SqlConnection con = new SqlConnection(constr);
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);//当ExecuteReader方法发生异常时，CommandBehavior.CloseConnection会关闭连接
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
                throw;
            }
        }
        public static DataTable ExecuteDataTable(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, constr);
            da.SelectCommand.CommandType = cmdType;
            if (pms != null)
            {
                da.SelectCommand.Parameters.AddRange(pms);
            }
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }
    }
}
