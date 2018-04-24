using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Model;
using QINGUO.ViewModel;
using QINGUO.Common;
using System.Data;
using Dapper;

namespace QINGUO.Business
{
    public class BllSysCompany : BllBase<ModSysCompany>
    {
        private ISysCompany DAL = CreateDalFactory.CreateSysCompany();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
        /// <summary>
        /// 获得公司树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            return DAL.GetTreeList(strWhere);
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"View_SysCompany";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.AddCondition("Status>" + (int)StatusEnum.删除);//过滤条件,没有审批过的单位不能显示
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCompany(string userId, string Column)
        {
            return DAL.UpdateCompany(userId, Column);
        }

        /// <summary>
        /// 修改公司状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateStatue(int status, string Id)
        {
            return DAL.UpdateStatue(status, Id);
        }

        /// <summary>
        /// 更改公司介绍或协议
        /// </summary>
        /// <param name="key">1:公司 2:协议</param>
        /// <param name="content">更改内容</param>
        /// <returns></returns>
        public int UpdateSetting(int key, string content)
        {
            return DAL.UpdateSetting(key, content);
        }

        /// <summary>
        /// 查询文本配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSetting(int key)
        {
            return DAL.GetSetting(key);
        }

        /// <summary>
        /// 根据类型获取列表
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListByAttribute(int Attribute)
        {
            return DAL.GetListByAttribute(Attribute);
        }


        /// <summary>
        /// 根据区域获取列表
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListByAreaId(int AreaId, int Attribute)
        {
            return DAL.GetListByAreaId(AreaId, Attribute);
        }

        /// <summary>
        /// 根据物业编号查询
        /// </summary>
        /// <returns></returns>
        public ModSysCompany GetCode(string code)
        {
            return DAL.GetCode(code);
        }

        /// <summary>
        /// 根据监管部门获取
        /// </summary>
        /// <param name="super"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListBySuper(ModSysCompany super, int Attribute)
        {
            return DAL.GetListBySuper(super, Attribute);
        }

        /// <summary>
        /// 根据省获取列表
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListByProvinceId(int ProvinceId, int Attribute)
        {
            return DAL.GetListByProvinceId(ProvinceId, Attribute);
        }
        /// <summary>
        /// 根据市获取列表
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListByCityId(int CityId, int Attribute)
        {
            return DAL.GetListByCityId(CityId, Attribute);
        }

        /// <summary>
        /// 获取单位部门
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetDepartments(string cid)
        {
            return DAL.GetDepartments(cid);
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public Page<ModSysCompany> SearchDataList(Search search)
        {
            search.TableName = @"View_SysCompany";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";
            return DAL.SearchData(search);
        }
    }
}
