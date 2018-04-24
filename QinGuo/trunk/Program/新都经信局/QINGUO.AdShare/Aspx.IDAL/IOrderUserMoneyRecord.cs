using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using System.Data;

namespace QINGUO.IDAL
{
    public interface IOrderUserMoneyRecord : IBaseDAL<ModOrderUserMoneyRecord>
    {
           /// <summary>
        /// 获取用户提现记录
        /// </summary>
        /// <param name="IwillId"></param>
        /// <returns></returns>
         List<ModOrderUserMoneyRecord> QueryAll(string UserId);
         /// <summary>
        /// 统计信息
        /// </summary>
        /// <returns></returns>
         DataSet Total();

         bool Updatebatch_no(string batch_no, string id);

         List<ModOrderUserMoneyRecord> GetInId(string id);

         List<ModOrderUserMoneyRecord> GetByBatch_no(string batch_no);

         bool UpdateByBatch_no(string batch_no);
    }
}
