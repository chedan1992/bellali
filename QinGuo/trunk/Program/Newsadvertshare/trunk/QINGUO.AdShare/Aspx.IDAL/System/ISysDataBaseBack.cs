using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysDataBaseBack : IBaseDAL<ModSysDataBaseBack>
    {
          /// <summary>
        /// 数据库备份
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        int BackDataBase(string path);

          /// <summary>
        /// 数据库还原
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        int Rollback(string path);
    }
}
