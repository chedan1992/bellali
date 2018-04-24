using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.ViewModel;
using System.Data;
using QINGUO.Common;

namespace QINGUO.Business
{
    public class BllSysUserFriends : BllBase<ModSysUserFriends>
    {
        private ISysUserFriends DAL = CreateDalFactory.CreateDalSysUserFriends();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        public List<ModSysMaster> GetFriends(string userid)
        {
            throw new NotImplementedException();
        }

        public Dapper.Page<ModSysMaster> GetFriends(Search search)
        {
            search.TableName = @"Cor_View_Friends";//表名
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return DAL.GetFriends(search);
        }
    }
}
