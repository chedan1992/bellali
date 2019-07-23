using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;
using Dapper;

namespace QINGUO.Business
{
    public class BllSysCollection : BllBase<ModSysCollection>
    {

        ISysCollection DAL = CreateDalFactory.CreateSysCollection();
        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Page<ModEDynamic> getEDynamicSearch(Search search)
        {
            search.TableName = @"E_Dynamic e INNER JOIN Sys_Collection c ON c.CollId=e.Id ";//表名
            search.KeyFiled = "e.Id";//主键
            search.AddCondition("e.Status=" + (int)StatusEnum.正常);//过滤条件
            search.AddCondition("c.Type=" + (int)CollectionEnum.新闻收藏);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "c.CreateTime desc";//排序
            search.SelectedColums = "e.Id,e.Name,e.ReadNum,Img,e.Content,e.Template,e.CreateTime,e.Author";

            return DAL.getEDynamicSearch(search);
        }
        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="collid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ModSysCollection Exit(string userid, string collid, CollectionEnum type)
        {
            return DAL.Exit(userid, collid, type);
        }

        /// <summary>
        /// 删除用户收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="collid"></param>
        /// <returns></returns>
        public int DeleteCollid(string userid, string collid,CollectionEnum type)
        {
            return DAL.DeleteCollid(userid, collid, type);
        }

        /// <summary>
        /// 统计用户收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="collid"></param>
        /// <returns></returns>
        public int getNum(string userid, CollectionEnum type)
        {
            return DAL.getNum(userid, type);
        }
    }
}
