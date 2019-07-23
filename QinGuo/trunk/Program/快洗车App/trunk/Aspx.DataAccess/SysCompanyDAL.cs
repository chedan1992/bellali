using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.ViewModel;
using QINGUO.DataAccessBase;
using QINGUO.Common;
using System.Data;
using Dapper;

namespace QINGUO.DAL
{
    public class SysCompanyDAL : BaseDAL<ModSysCompany>, ISysCompany
    {

        /// <summary>
        /// 获得公司树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" FROM Sys_Company where 1=1");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            strSql.Append(" order by [CreateTime] asc ");
            return dabase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCompany(string userId, string Column)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", userId);

            int result = dabase.ExecuteNonQueryByText("update Sys_Company set " + Column + "  where Id=(select top 1 Cid from Sys_Master where Id=@Id)", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改公司状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateStatue(int status, string Id)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Status", status);
            paras.Add("@Id", Id);
            int result = 0;
            if (status == (int)StatusEnum.禁用)
            {
                result = dabase.ExecuteNonQueryByText("update Sys_Company set Status=@Status where CHARINDEX(@Id,Path)>0 or id=@Id", paras);
            }
            else
            {
                result = dabase.ExecuteNonQueryByText("update Sys_Company set Status=@Status where ID ='" + Id + "'", paras);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更改公司介绍或协议
        /// </summary>
        /// <param name="key">1:公司 2:协议</param>
        /// <param name="content">更改内容</param>
        /// <returns></returns>
        public int UpdateSetting(int key, string content)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", key);
            paras.Add("@Content", content);
            string sql = "update Sys_RemarkSetting set Content=@Content where Id=@Id";
            try
            {
                dabase.ExecuteNonQueryByText(sql, paras);
                dabase.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 查询文本配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSetting(int key)
        {
            string sql = "select * from Sys_RemarkSetting where Id=" + key;
            DataSet ds = dabase.ExecuteDataSet(sql);
            string result = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0]["Content"].ToString();
            }
            return result;
        }


        /// <summary>
        /// 根据类型获取列表
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListByAttribute(int Attribute)
        {
            string sql = "select * from Sys_Company where status=1 and Attribute=@0";

            return dabase.ReadDataBase.Query<ModSysCompany>(sql, Attribute).ToList();
        }

        /// <summary>
        /// 根据省获取列表
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListByProvinceId(int ProvinceId, int Attribute)
        {
            string sql = "select * from Sys_Company where status=1 and Attribute=@0 and Province=@1";
            return dabase.ReadDataBase.Query<ModSysCompany>(sql, Attribute, ProvinceId).ToList();
        }
        /// <summary>
        /// 根据市获取列表
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListByCityId(int CityId, int Attribute)
        {
            string sql = "select * from Sys_Company where status=1 and Attribute=@0 and CityId=@1";
            return dabase.ReadDataBase.Query<ModSysCompany>(sql, Attribute, CityId).ToList();
        }
        /// <summary>
        /// 根据区域获取列表
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListByAreaId(int AreaId, int Attribute)
        {
            string sql = "select * from Sys_Company where status=1 and Attribute=@0 and AreaId=@1";
            return dabase.ReadDataBase.Query<ModSysCompany>(sql, Attribute, AreaId).ToList();
        }


        /// <summary>
        /// 根据物业编号查询
        /// </summary>
        /// <returns></returns>
        public ModSysCompany GetCode(string code)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModSysCompany>("select * from Sys_Company where status=1 and Code=@0 and Attribute=" + (int)CompanyType.部门, code);
        }


        /// <summary>
        /// 根据监管部门获取
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetListBySuper(ModSysCompany super, int Attribute)
        {
            string sql = "select * from Sys_Company where status=1 and Attribute=@0 ";

            string selectid = "";

            if (super.Province == "-1")
            { //全国

            }
            else if (super.CityId == -1)
            { //全省
                selectid = super.Province;
                sql += "and Province=@1";
            }
            else if (super.AreaId == -1)//全市
            {
                selectid = super.CityId.ToString();
                sql += "and CityId=@1";
            }
            else//全区
            {
                selectid = super.AreaId.ToString();
                sql += "and AreaId=@1";
            }


            return dabase.ReadDataBase.Query<ModSysCompany>(sql, Attribute, selectid).ToList();
        }


        /// <summary>
        /// 获取单位部门
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<ModSysCompany> GetDepartments(string cid)
        {
            string sql = "select id,name from Sys_Company where Attribute=@0 and CreateCompanyId=@1 and Status=@2";
            return dabase.ReadDataBase.Query<ModSysCompany>(sql, (int)CompanyType.部门, cid, (int)StatusEnum.正常).ToList();
        }


        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public Page<ModSysCompany> SearchData(Search search)
        {
            return dabase.ReadDataBase.Page<ModSysCompany>(search.CurrentPageIndex, search.PageSize, search.SqlString);
        }
    }
}
