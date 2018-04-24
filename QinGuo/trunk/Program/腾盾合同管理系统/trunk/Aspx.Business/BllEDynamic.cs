using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Common;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.Factory;
using Dapper;
using QINGUO.ViewModel;

namespace QINGUO.Business
{
    public class BllEDynamic : BllBase<ModEDynamic>
    {
        IEDynamic dal = CreateDalFactory.CreateDalEDynamic();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }

        /// <summary>
        /// 查询动态列表
        /// </summary>
        /// <param name="UId"></param>
        /// <returns></returns>
        public List<ModEDynamic> getListAll(int? top)
        {
            return dal.getListAll(top);
        }
        

        /// <summary>
        /// 获取文章分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public Page<ModEDynamic> GetDynamic(Search search)
        {

            search.TableName = @"E_Dynamic";//表名
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "IsTop desc,Sort asc,CreateTime desc";//排序
            search.SelectedColums = "Id,Name,ReadNum,Img,Content,Template,CreateTime,Author";
            return dal.GetDynamic(search);
        }
        


        /// <summary>
        /// 获取文章分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"View_Dynamic";//表名
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status!=" + (int)StatusEnum.删除);//过滤条件
            search.StatusDefaultCondition = "";
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateStatus(int flag, string key)
        {
            return dal.UpdateStatus(flag,key);
        }

         /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModViewDynamic GetModelByWhere(string where)
        {
            var model= dal.GetModelByWhere(where);
            if (model != null) {
                model.ImageList = new BllSysImgPic().GetList(model.Id);
            }
            return model;
        }
    }
}
