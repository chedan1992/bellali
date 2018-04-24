using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using QINGUO.Common;
using System.Data;
using Dapper;

namespace QINGUO.IDAL
{
    public interface ISysCompany : IBaseDAL<ModSysCompany>
    {
         /// <summary>
        /// 获得公司树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        DataSet GetTreeList(string strWhere);
           /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateCompany(string userId, string Column);

        /// <summary>
        /// 修改公司状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        bool UpdateStatue(int status, string Id);

          /// <summary>
        /// 更改公司介绍或协议
        /// </summary>
        /// <param name="key">1:公司 2:协议</param>
        /// <param name="content">更改内容</param>
        /// <returns></returns>
        int UpdateSetting(int key, string content);


        /// <summary>
        /// 查询文本配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetSetting(int key);

        /// <summary>
        /// 根据类型获取列表
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        List<ModSysCompany> GetListByAttribute(int Attribute);

        List<ModSysCompany> GetListByAreaId(int AreaId, int Attribute);

        ModSysCompany GetCode(string code);

        List<ModSysCompany> GetListBySuper(ModSysCompany super, int Attribute);

        /// <summary>
        /// 根据省获取列表
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        List<ModSysCompany> GetListByProvinceId(int ProvinceId, int Attribute);
        /// <summary>
        /// 根据市获取列表
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        List<ModSysCompany> GetListByCityId(int CityId, int Attribute);

        /// <summary>
        /// 获取单位部门
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        List<ModSysCompany> GetDepartments(string cid);

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Page<ModSysCompany> SearchData(Search search);
    }
}
