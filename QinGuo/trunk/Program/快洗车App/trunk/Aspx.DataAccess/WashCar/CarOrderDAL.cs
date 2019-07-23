using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.DataAccessBase;
using QINGUO.ViewModel;
using QINGUO.Common;
using Dapper;
using System.Data;

namespace QINGUO.DAL
{
    public class CarOrderDAL : BaseDAL<ModCarOrder>, ICarOrder
    {

        /// <summary>
        /// 分页查询审核列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Page<CarOrderView> SearchData(Search search)
        {
            return dabase.ReadDataBase.Page<CarOrderView>(search.CurrentPageIndex, search.PageSize, search.SqlString2);
        }

        public CarOrderView GetDetail(string orderid)
        {
            return dabase.ReadDataBase.FirstOrDefault<CarOrderView>("select a.name,a.Status,a.CreateTime,a.Phone,a.CId as carid,a.auditorId,a.auditor,a.CUserId as cUserId,a.CreateId,a.TermOfValidity,a.remarks,a.auditorRemarks,c.LicensePlate as carno,c.typename as cartTypeName,mc.RoleName as createname  from W_Order a LEFT JOIN W_Car c ON a.CId=c.Id LEFT JOIN Sys_Master mc ON mc.Id=a.CreateId where a.Id=@0 ", orderid);
        }
    }
}
