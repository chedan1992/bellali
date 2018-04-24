using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ISysOperateLog : IBaseDAL<ModSysOperateLog>
    {
         /// <summary>
        /// 导出日志数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataSet ExportOut(string where);

         /// <summary>
        /// 清空
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int DeleteAll(string where);
    }
}
