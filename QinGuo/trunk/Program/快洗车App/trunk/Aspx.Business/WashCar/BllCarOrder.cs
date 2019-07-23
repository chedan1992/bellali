using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;
using QINGUO.ViewModel;
using System.Data;
using Dapper;

namespace QINGUO.Business
{
    public class BllCarOrder : BllBase<ModCarOrder>
    {
        private ICarOrder DAL = CreateDalFactory.CreateCarOrder();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        public Page<CarOrderView> SearchData(Search search)
        {
            search.TableName = @" W_Order a LEFT JOIN W_Car c ON a.CId=c.Id LEFT JOIN Sys_Master mc ON mc.Id=a.CreateId ";//表名
            search.SelectedColums = @"a.Id,a.name,a.Status,a.CreateTime,a.Phone,a.CId as carid,a.auditorId,a.auditor,a.CUserId as cUserId,a.CreateId,a.TermOfValidity,a.remarks,a.auditorRemarks,c.LicensePlate as carno,c.typename as cartTypeName,mc.RoleName as createname";//查询列

            search.KeyFiled = "a.Id";//主键
            search.AddCondition("1=1");//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "a.Status asc,a.CreateTime desc";
            return DAL.SearchData(search);
        }

        public CarOrderView GetDetail(string orderid)
        {
            return DAL.GetDetail(orderid);
        }
    }
}
