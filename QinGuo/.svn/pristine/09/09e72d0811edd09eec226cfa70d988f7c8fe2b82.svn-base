using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;
using System.Data;

namespace QINGUO.Business
{
    public class BllOrderUserMoneyRecord : BllBase<ModOrderUserMoneyRecord>
    {
        private IOrderUserMoneyRecord DAL = CreateDalFactory.CreateOrderUserMoneyRecord();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 后台查询等待审批的取现操作
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModCompanyOrder</returns>
        public string SearchData(Search search)
        {
            //查询条件
            search.TableName = @"OrderUserMoneyRecordView";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return base.QueryPageToJson(search);
        }


        /// <summary>
        /// 获取用户提现记录
        /// </summary>
        /// <param name="IwillId"></param>
        /// <returns></returns>
        public List<ModOrderUserMoneyRecord> QueryAll(string UserId)
        {
            return DAL.QueryAll(UserId);
        }

        /// <summary>
        /// 统计信息
        /// </summary>
        /// <returns></returns>
        public DataSet Total()
        {
            return DAL.Total();
        }
        /// <summary>
        /// 查询提现详细记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModOrderUserMoneyRecord> GetInId(string id)
        {
            return DAL.GetInId(id);
        }
        /// <summary>
        /// 修改批次号
        /// </summary>
        /// <param name="batch_no"></param>
        /// <returns></returns>
        public bool Updatebatch_no(string batch_no, string id)
        {
            return DAL.Updatebatch_no(batch_no, id);
        }
        /// <summary>
        /// 根据批次号id查询
        /// </summary>
        /// <param name="batch_no"></param>
        /// <returns></returns>
        public List<ModOrderUserMoneyRecord> GetByBatch_no(string batch_no)
        {
            return DAL.GetByBatch_no(batch_no);
        }
        /// <summary>
        /// 根据批次号修改状态
        /// </summary>
        /// <param name="batch_no"></param>
        /// <returns></returns>
        public bool UpdateByBatch_no(string batch_no)
        {
            return DAL.UpdateByBatch_no(batch_no);
        }
    }
}
