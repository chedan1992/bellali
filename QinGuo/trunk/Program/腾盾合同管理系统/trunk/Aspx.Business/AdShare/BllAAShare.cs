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

namespace QINGUO.Business
{
    public class BllAAShare : BllBase<ModAAShare>
    {
        private IAAShare DAL = CreateDalFactory.CreateAAShare();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        public List<ModAAShare> getUserId(string userid)
        {
            return DAL.getUserId(userid);
        }

        public string SearchData(Search search)
        {
            search.TableName = @"dbo.Ad_AShare a LEFT JOIN dbo.Sys_Master m ON m.Id=a.CreaterId LEFT JOIN dbo.Sys_Master mc ON mc.id=a.Auditor";//表名
            search.SelectedColums = "a.*,m.UserName as PUserName,m.LoginName as PLoginName,mc.UserName as MUserName,mc.LoginName as MLoginName";//查询列
            search.KeyFiled = "a.Id";//主键
            search.AddCondition("1=1");//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "a.Status asc,a.CreateTime desc";
            return base.QueryPageToJson(search);
        }
    }
}
