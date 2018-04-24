using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using System.Data.SqlClient;
using System.Configuration;
using QINGUO.DapperAccessBase;

namespace QINGUO.DAL
{
    public class SysDataBaseBackDAL : BaseDAL<ModSysDataBaseBack>, ISysDataBaseBack
    {

        /// <summary>
        /// 数据库备份
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int BackDataBase(string path)
        {
            
            string DataBaseName = ConfigurationManager.AppSettings["DataBaseName"];
            string str = ConfigurationManager.ConnectionStrings["WriteConnectionString"].ConnectionString;
            string strSql2 = string.Format("backup database [{0}] to disk='{1}'", DataBaseName, path);
            using (SqlConnection conn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand(strSql2, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return 1;
                    }
                    catch (Exception er)
                    {
                        return 0;
                    }

                }
            }
        }

        /// <summary>
        /// 数据库还原
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int Rollback(string path)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["WriteConnectionString"].ConnectionString);
            try
            {
                string DataBaseName = ConfigurationManager.AppSettings["DataBaseName"];
                string strSql2 = string.Format("use master;restore database [{0}] from disk='{1}'", DataBaseName, path);
                SqlCommand command = new SqlCommand(strSql2, conn);
                conn.Open();
                command.Parameters.AddWithValue("@path", path);
                command.ExecuteNonQuery();
                return 1;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
    }
}
