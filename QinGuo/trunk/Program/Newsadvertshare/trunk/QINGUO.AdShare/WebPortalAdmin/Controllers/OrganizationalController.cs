using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Common;
using QINGUO.Business;
using WebPortalAdmin.Code;
using QINGUO.ViewModel;
using System.Text;

namespace WebPortalAdmin.Controllers
{
    public class OrganizationalController : BaseController<ModSysCompany>
    {
        BllSysCompany bll = new BllSysCompany();
        /// <summary>
        ///组织架构
        /// </summary>
        /// <returns></returns>
        public ActionResult Organizational()
        {
            return View();
        }
        /// <summary>
        /// 部门管理
        /// </summary>
        /// <returns></returns>
        public ActionResult DeptManage()
        {
            return View();
        }
        /// <summary>
        ///职位管理
        /// </summary>
        /// <returns></returns>
         public ActionResult PostManage()
        {
            return View();
        }


         #region ===上级单位查询自己所在权限下的公司列表 SearchRolesData
         /// <summary>
        /// 查询公司列表
        /// </summary>
        [HttpPost]
         public void SearchRolesData()
        {
            try
            {
                _where += " and Attribute=" + (int)CompanyType.单位 + " and Status=" + (int)StatusEnum.正常 + " and Id in (select EmployerId from Sys_CompanyCognate where Cid='"+ CurrentMaster.Cid+"' and Status=1)";
                _mySet = bll.GetTreeList(_where);
                List<JsonCompanyTree> list = new FunTreeCommon().GetCompanyTreeNodes(_mySet);
                Output = JsonHelper.ToJson(list);
            }
            catch
            {
                Output = "";
            }
            WriteJsonToPage(Output);
        }

        #endregion


        #region ===查询公司列表 SearchData
        /// <summary>
        /// 查询公司列表
        /// </summary>
        [HttpPost]
        public void SearchData()
        {
            try
            {
                if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
                {
                    //超级管理员查看所有单位
                    _where += " and Attribute=" + (int)CompanyType.单位 + " and Status>" + (int)StatusEnum.删除;
                }
                else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                {
                    //查看自己单位
                    _where += " and Attribute=" + (int)CompanyType.单位 + " and Status>" + (int)StatusEnum.删除 + " and Id='"+CurrentMaster.Cid+"'";
                }
               
                _mySet = bll.GetTreeList(_where);
                List<JsonCompanyTree> list = new FunTreeCommon().GetCompanyTreeNodes(_mySet);
                Output = JsonHelper.ToJson(list);
            }
            catch
            {
                Output = "";
            }
            WriteJsonToPage(Output);
        }

        #endregion

        #region ===查询部门列表 SearchList
        /// <summary>
        /// 查询部门列表
        /// </summary>
        public void SearchList()
        {
            Search search = this.GetSearch();
            string id = Request["Id"].ToString();
            search.AddCondition("Attribute=" + (int)CompanyType.部门);
            if (id != "-1")
            {
                search.AddCondition("CreateCompanyId='" + id + "'");
            }
            else
            {
                if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
                {
                    //查看全部
                }
                else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                {
                    //查看自己单位
                    search.AddCondition("CreateCompanyId='" + CurrentMaster.Cid + "'");
                }
            }
            WriteJsonToPage(new BllSysCompany().SearchData(search));
        }
        #endregion

        #region ===查询岗位列表 SearchPostList
        /// <summary>
        /// 查询岗位列表
        /// </summary>
        public void SearchPostList()
        {
            Search search = this.GetSearch();
            string id = Request["Id"].ToString();
            search.AddCondition("Attribute=" + (int)CompanyType.部门);
            if (id != "-1")
            {
                search.AddCondition("CreateCompanyId='" + id + "'");
            }
            WriteJsonToPage(new BllSysCompany().SearchData(search));
        }
        #endregion

        #region ===查询树形公司,岗位部门列表 SearchPostData
        /// <summary>
        /// 查询公司列表
        /// </summary>
        [HttpPost]
        public void SearchPostData()
        {
            try
            {
                if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
                {
                    //超级管理员查看所有单位
                    _where += " and (Attribute>=" + (int)CompanyType.单位 + " and Attribute<=" + (int)CompanyType.部门 + ") and Status=" + (int)StatusEnum.正常;
                }
                else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                {
                    //查看自己单位
                    _where += " and (Attribute>=" + (int)CompanyType.单位 + " and Attribute<=" + (int)CompanyType.部门 + ") and Status=" + (int)StatusEnum.正常 + " and (Id='" + CurrentMaster.Cid + "' or CreateCompanyId='" + CurrentMaster.Cid + "')";
                }
                if (!string.IsNullOrEmpty(Request["BrandName"]))
                {
                    _where += " and (Name like '%" + Request["BrandName"].ToString() + "%')";
                }
                _mySet = bll.GetTreeList(_where);
                List<JsonCompanyTree> list = new FunTreeCommon().GetCompanyTreeNodes(_mySet);
                if (_mySet.Tables[0].Rows.Count > 0 && list.Count == 0)
                {
                    for (int i = 0; i < _mySet.Tables[0].Rows.Count; i++)
                    {
                        JsonCompanyTree model = new JsonCompanyTree();
                        model.id = _mySet.Tables[0].Rows[i]["Id"].ToString();
                        model.parentId = "0";
                        model.text = _mySet.Tables[0].Rows[i]["Name"].ToString();
                        model.expanded = false;
                        model.leaf = true;
                        model.leaf = true;
                        list.Add(model);
                    }
                }
                Output = JsonHelper.ToJson(list);
            }
            catch
            {
                Output = "";
            }
            WriteJsonToPage(Output);
        }

        #endregion

        

        #region 保存公司表单 SaveData

        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysCompany t)
        {
            ModJsonResult json = new ModJsonResult();

            t.LinkUser = (t.LinkUser == null ? "" : t.LinkUser);
            t.LegalPerson = (t.LegalPerson == null ? "" : t.LegalPerson);
            t.CompLat = (t.CompLat == null ? "" : t.CompLat);
            t.ComPLon = (t.ComPLon == null ? "" : t.ComPLon);

            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                ModSysCompany model = bll.LoadData(t.Id);
                model.Name = t.Name;
                model.LegalPerson = t.LegalPerson;
                model.LinkUser = t.LinkUser;
                model.Address = t.Address;
                model.CompLat = t.CompLat;
                model.ComPLon = t.ComPLon;
                model.Code = t.Code;
                model.ReegistMoney = t.ReegistMoney;
                model.Phone = t.Phone;
                model.Email = t.Email;
                model.Introduction = t.Introduction;

                int result = bll.Update(model);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
            }
            else
            {
                t.Id = Guid.NewGuid().ToString();
                t.Status = (int)StatusEnum.正常;
                t.CreaterUserId = CurrentMaster.Id;
                t.CreateTime = DateTime.Now;
                if (t.Attribute == (int)CompanyType.维保公司)//集团
                {
                    t.Path = t.Id;
                }
                else {
                    t.Path = t.Path + "," + t.Id;
                }
                int result = bll.Insert(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region ===保存部门表单 SaveDeptData

        /// <summary>
        /// 保存部门表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveDeptData(ModSysCompany t)
        {
            ModJsonResult json = new ModJsonResult();

            t.LinkUser = (t.LinkUser == null ? "" : t.LinkUser);
            t.LegalPerson = (t.LegalPerson == null ? "" : t.LegalPerson);
            t.CompLat = (t.CompLat == null ? "" : t.CompLat);
            t.ComPLon = (t.ComPLon == null ? "" : t.ComPLon);

            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                ModSysCompany model = bll.LoadData(t.Id);
                model.Name = t.Name;
                model.NameTitle = t.NameTitle;
                model.LinkUser = t.LinkUser;
                model.Order = t.Order;
                model.Code = t.Code;
                model.Phone = t.Phone;
                model.Email = t.Email;
                model.Introduction = t.Introduction;
                int result = bll.Update(model);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
            }
            else
            {
                if (t.CreateCompanyId != "0")
                {
                    var model = bll.LoadData(t.CreateCompanyId);
                    t.Path = model.Path + "/" + t.Id;
                }
                else
                {
                    t.Path = t.Id;
                }
                t.Id = Guid.NewGuid().ToString();
                t.Status = (int)StatusEnum.正常;
                t.CreaterUserId = CurrentMaster.Id;
                t.CreateTime = DateTime.Now;
                t.Attribute = (int)CompanyType.部门;
                int result = bll.Insert(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region ===保存职位表单 SavePostData

        /// <summary>
        /// 保存职位表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SavePostData(ModSysCompany t)
        {
            ModJsonResult json = new ModJsonResult();

            t.LinkUser = (t.LinkUser == null ? "" : t.LinkUser);
            t.LegalPerson = (t.LegalPerson == null ? "" : t.LegalPerson);
            t.CompLat = (t.CompLat == null ? "" : t.CompLat);
            t.ComPLon = (t.ComPLon == null ? "" : t.ComPLon);

            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                ModSysCompany model = bll.LoadData(t.Id);
                model.Name = t.Name;
                model.NameTitle = t.NameTitle;
                model.LinkUser = t.LinkUser;
                model.Order = t.Order;
                model.Code = t.Code;
                model.Phone = t.Phone;
                model.Email = t.Email;
                model.Introduction = t.Introduction;
                int result = bll.Update(model);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
            }
            else
            {
                if (t.CreateCompanyId != "0")
                {
                    var model = bll.LoadData(t.CreateCompanyId);
                    t.Path = model.Path + "/" + t.Id;
                }
                else
                {
                    t.Path = t.Id;
                }
                t.Id = Guid.NewGuid().ToString();
                t.Status = (int)StatusEnum.正常;
                t.CreaterUserId = CurrentMaster.Id;
                t.CreateTime = DateTime.Now;
                t.Attribute = (int)CompanyType.部门;
                int result = bll.Insert(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region ==删除 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                int result =bll.DeleteStatus(id);
                if (result > 0)
                {
                    msg.success = true;
                    //清空用户部门信息
                    string sql = "update Sys_Master set OrganizaId='' where OrganizaId='" + id + "';";
                    bll.ExecuteNonQueryByText(sql);
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==启用状态 EnableUse
        /// <summary>
        /// 启用状态
        /// </summary>
        public void EnableUse()
        {
            var msg = new ModJsonResult();

            string key = Request["id"];
            bool result = new BllSysCompany().UpdateStatue(1, key);
            if (result)
            {
                msg.success = true;
            }
            else
            {
                msg.success = false;
                msg.msg = "操作失败";
            }

            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==停用状态 DisableUse

        /// <summary>
        /// DisableUse
        /// </summary>
        public void DisableUse()
        {
            var msg = new ModJsonResult();

            string key = Request["id"];
            bool result = new BllSysCompany().UpdateStatue(0, key);
            if (result)
            {
                msg.success = true;
            }
            else
            {
                msg.success = false;
                msg.msg = "操作失败";
            }

            WriteJsonToPage(msg.ToString());
        }

        #endregion


        #region 批量保存部门信息 SaveDeptListData
        /// <summary>
        /// 批量保存部门信息
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveDeptListData()
        {
            ModJsonResult json = new ModJsonResult();
            string NameList = Request["NameList"].ToString();
            string CreateCompanyId = Request["CreateCompanyId"].ToString();//单位编号
            
            string[] str = NameList.Split(',');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                sb.Append("insert into Sys_Company(Id,Name,Level,Attribute,Status,CreateCompanyId)");
                sb.Append(" values(");
                sb.Append("'" + Guid.NewGuid().ToString() + "',");
                sb.Append("'" + str[i] + "',");
                sb.Append("'" + 0 + "',");
                sb.Append("'" + (int)CompanyType.部门 + "',");
                sb.Append("'" + (int)StatusEnum.正常 + "',");
                sb.Append("'" + CreateCompanyId + "'");
                sb.Append(")");
                sb.AppendLine();
            }
            int result = bll.ExecuteNonQueryByText(sb.ToString());
            if (result <= 0)
            {
                json.success = false;
                json.msg = " 保存失败,请稍后再操作!";
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion
    }
}
