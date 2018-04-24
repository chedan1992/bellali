using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Common;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.ViewModel;
using Dapper;

namespace QINGUO.Business
{
    public class BllSysAppointCheckNotes : BllBase<ModSysAppointCheckNotes>
    {
        ISysAppointCheckNotes dal = CreateDalFactory.CreateDalSysAppointCheckNotes();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }

        /// <summary>
        /// 分页查询设备巡检记录
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Page<ModSysAppointCheckNotes> GetAppointCheckNotesList(Search search)
        {
            search.TableName = @"View_AppointCheckNotes";//视图
            search.KeyFiled = "Id";//主键
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return dal.GetAppointCheckNotesList(search);
        }
        /// <summary>
        /// 后台分页查询设备
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"View_AppointCheckNotes";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.AddCondition("1=1");//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";
            return base.QueryPageToJson(search);
        }
       
    }
}
