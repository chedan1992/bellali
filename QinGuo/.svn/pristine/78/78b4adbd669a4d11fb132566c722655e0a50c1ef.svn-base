using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysFlow : IBaseDAL<ModSysFlow>
    {
         /// <summary>
        /// 
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        List<ModSysFlow> getListAll(int? top, string where);

        bool Exists(int FlowType, int FlowStatus, string AppointId);

        Dapper.Page<ModSysFlow> GetFlowList(Common.Search search);

        int flowmsg(ModSysMaster sysMaster);
    }
}
