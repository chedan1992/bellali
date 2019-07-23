using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using System.Data;

namespace QINGUO.DAL
{
    public class SysOperateLogDAL : BaseDAL<ModSysOperateLog>, ISysOperateLog
    {
        /// <summary>
        /// 导出日志数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet ExportOut(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT UserName,LinkUrl,Remark,IPAddress,CreateTime FROM Sys_OperateLog where 1=1");
            if (!string.IsNullOrEmpty(where))
            {
                sb.Append(where);
            }
            return dabase.ExecuteDataSet(sb.ToString());
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int DeleteAll(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("delete FROM Sys_OperateLog where 1=1");
            if (!string.IsNullOrEmpty(where))
            {
                sb.Append(where);
            }
            return dabase.ExecuteNonQuery(sb.ToString());
        }
    }
}
