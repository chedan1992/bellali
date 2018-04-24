using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using QINGUO.Business;
using QINGUO.Common;
using QINGUO.Model;
using QINGUO.ViewModel;

namespace WebPortalAdmin.Controllers
{

    /// <summary>
    /// 系统管理员操作类
    /// </summary>
    public class SysMasterController : BaseController<ModSysMaster>
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult SysMaster()
        {
            return View();
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            return View();
        }



        #region==页面列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            //查询自己所属公司
            search.AddCondition("Cid='" + CurrentMaster.Cid + "'");
            search.AddCondition("IsSystem!=" + (int)YesOrNoEnum.是);//是否是系统登录员
            search.AddCondition("Attribute=" + (int)AdminTypeEnum.普通管理员);

            search.AddCondition("Status!=" + (int)StatusEnum.删除);
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }

            var jsonResult = new BllSysMaster().SearchData(search);

            LogInsert(OperationTypeEnum.访问, "人员管理", CurrentMaster.UserName + "页面访问正常.");

            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysMaster t)
        {
            BllSysMaster bll = new BllSysMaster();
            ModJsonResult json = new ModJsonResult();

            string roleId = Request["roleId"] == null ? "" : Request["roleId"];
            string roleName = Request["roleName"] == null ? "" : Request["roleName"];

            t.RoleName = roleName;//角色名称
            t.Pwd = (string.IsNullOrEmpty(t.Pwd) ? DESEncrypt.Encrypt("666666") : DESEncrypt.Encrypt(t.Pwd.Trim()));
            t.LoginName = t.LoginName.Trim();
            t.UserName = t.UserName.Trim();
            t.Attribute = (int)AdminTypeEnum.普通管理员;//栏目类型
            if (string.IsNullOrEmpty(t.Cid))
            {
                t.Cid = CurrentMaster.Cid;
            }
            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                int result = bll.UpdateDate(t);
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
                t.Cid = CurrentMaster.Cid;
                t.IsSystem = false;
                t.CreaterId = CurrentMaster.Id;
                t.CreateTime = DateTime.Now;

                if (t.Sex == 0)
                {
                    t.HeadImg = (t.HeadImg == null ? "/Resource/css/icons/head/GTP_hmale_big.jpg" : "");
                }
                else
                {
                    t.HeadImg = (t.HeadImg == null ? "/Resource/css/icons/head/GTP_hfemale_big.jpg" : "");
                }

                int result = bll.Insert(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
                new BllSysMaster().ClearCache();
            }

            #region ==保存角色信息
            if (new BllSysMasterRole().DeleteAllByMasterId(t.Id) > 0)//删除管理员原有角色信息
            {
                if (!string.IsNullOrEmpty(roleId))
                {
                    string[] _str = roleId.Split(',');
                    for (int i = 0; i < _str.Length; i++)
                    {
                        var model = new ModSysMasterRole()
                        {
                            Id = Guid.NewGuid().ToString(),
                            RoleId = _str[i],
                            MasterId = t.Id
                        };
                        new BllSysMasterRole().Insert(model);
                    }
                }
            }
            #endregion

            LogInsert(OperationTypeEnum.操作, "人员管理", CurrentMaster.UserName + "人员新增或修改操作正常.");

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
                //判断类型是否还在使用
                var mode = new BllSysAppointed().Exists(" ResponsibleId='" + id + "'");
                if (mode == true)
                {
                    msg.success = false;
                    msg.msg = "该用户下还存在管理的设备,请先更改设备责任人再删除.";
                }
                else
                {
                    BllSysMaster Master = new BllSysMaster();
                    var Model = Master.LoadData(id);
                    if (Model != null)
                    {
                        Model.Status = (int)StatusEnum.删除;
                        int result = Master.Update(Model);
                        if (result > 0)
                        {
                            msg.success = true;
                        }
                        else
                        {
                            msg.success = false;
                            msg.msg = "操作失败";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==获取角色列表 GetRoleList
        /// <summary>
        /// GetRoleList
        /// </summary>
        public void GetRoleList()
        {
            string Output = "";
            try
            {
                string masterId = Request["masterId"] == null ? "" : Request["masterId"];

                //查询角色查看
                string className = "SysMaster";//页面类名
                if (!String.IsNullOrEmpty(className))
                {
                    if (!String.IsNullOrEmpty(CurrentMaster.RoleIdList))
                    {
                        CurrentMaster.LookPower = new BllSysMaster().GetLookPower(className, CurrentMaster.RoleIdList, CurrentMaster.Attribute);
                    }
                }
                _where += " and Status=" + (int)StatusEnum.正常 + " and CompanyID='" + CurrentMaster.Cid + "'";
                if (!CurrentMaster.IsMain)
                {
                    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                    {
                        _where += " and CreaterId='" + CurrentMaster.Id + "' order by CreateTime desc,RoleSort asc";
                    }
                }

                DataSet ds = new BllSysRole().GetAllRole(_where);//获取所有角色信息
                DataSet MySelect = new BllSysRole().GetCheckRoleList(masterId);

                List<Hashtable> hashList = new List<Hashtable>();
                foreach (DataRow myrow in ds.Tables[0].Rows)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("Name", myrow["Name"].ToString());
                    ht.Add("Id", myrow["Id"].ToString());
                    int count = 0;
                    foreach (DataRow prow in MySelect.Tables[0].Rows)
                    {
                        if (myrow["Id"].ToString() == prow["RoleId"].ToString())
                        {
                            count++;
                            ht.Add("checked", true);
                            break;
                        }
                    }
                    if (count <= 0)
                    {
                        ht.Add("checked", false);
                    }
                    hashList.Add(ht);
                }
                Output = JsonHelper.ToJson(hashList);
            }
            catch
            {
                Output = "";
            }
            WriteJsonToPage(Output);
        }

        #endregion

        #region ==获取页面按钮权限 GetRoleBtn
        /// <summary>
        /// 获取页面按钮权限
        /// </summary>
        public void GetRoleBtn()
        {
            var msg = new ModJsonResult();

            if (CurrentMaster != null)
            {
                string pageAction = Request["PageAction"];
                //超级管理员不用控权
                if (CurrentMaster.IsMain)
                {
                    DataSet ds = new BllSysMaster().GetPageBtns(pageAction, CurrentMaster.Attribute);
                    if (ds.Tables.Count == 0)
                    {
                        msg.msg = "{}";
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            msg.msg = JsonHelper.DataTableToJson(ds.Tables[0]);
                        }
                        else
                        {
                            msg.success = false;
                        }
                    }
                }
                else
                {
                    DataSet ds = new BllSysMaster().GetAuthByBtn(pageAction, CurrentMaster.RoleIdList, CurrentMaster.Attribute);
                    if (ds.Tables.Count == 0)
                    {
                        msg.msg = "{}";
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            msg.msg = JsonHelper.DataTableToJson(ds.Tables[0]);
                        }
                        else
                        {
                            msg.success = false;
                        }
                    }
                }
            }
            else
            {
                msg.success = false;
                msg.errorCode = (int)SystemError.用户过期错误;
            }
            WriteJsonToPage(JsonHelper.ToJson(msg));
        }
        #endregion

        #region ==获取在线用户人数 online
        /// <summary>
        /// 获取在线用户人数
        /// </summary>
        public void online()
        {
            //获取在线人数
            var msg = new ModJsonResult();
            int online = int.Parse(HttpContext.Application["user_online"].ToString());
            try
            {
                msg.success = true;
                msg.msg = online.ToString();
            }
            catch
            {
                msg.success = false;
            }
            WriteJsonToPage(new JavaScriptSerializer().Serialize(msg).ToString());
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
            int result = new BllSysMaster().UpdateStatus(1, key);
            if (result > 0)
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
            int result = new BllSysMaster().UpdateStatus(0, key);
            if (result > 0)
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

        #region ===用户选择树 GetTreeByMaster
        [HttpPost]
        public void GetTreeByMaster()
        {
            string Attribute = Request["Attribute"];//1:业务员 2:学生
            string selectUserkey = (Request["select"] == null ? "" : Request["select"]);//选中的人员编号

            if (Attribute == "6")
            {
                string where = " Cid='" + CurrentMaster.Company.Id + "' and IsSystem!=1 and Attribute=" + Attribute;
                string name = (Request["MasterName"] == null ? "" : Request["MasterName"]); //查询过滤条件
                if (name != "")
                {
                    where += " and UserName like '%" + name + "%'";
                }

                DataTable mytab = new BllSysMaster().GetTreeList(where).Tables[0]; //获取所有树
                GetTreeNode(mytab, selectUserkey);
                Output = Output.Replace("check", "checked");
                WriteJsonToPage(Output);
            }
            else
            {
                //查询班级
                string where = " Type=1 and Status=" + (int)StatusEnum.正常;
                DataTable mytab = new BllSysGroup().GetTreeList(where).Tables[0]; //获取所有树
                string userwhere = " Cid='" + CurrentMaster.Company.Id + "' and IsSystem!=1 and Attribute=" + Attribute;
                DataTable usermytab = new BllSysMaster().GetTreeList(userwhere).Tables[0]; //获取所有树
                GetClassNode(mytab, usermytab, selectUserkey);
                Output = Output.Replace("check", "checked");
                WriteJsonToPage(Output);
            }
        }

        /// <summary>
        /// 查询父节点
        /// </summary>
        public void GetTreeNode(DataTable dt, string selectlist)
        {
            List<Hashtable> hashList = new List<Hashtable>();

            foreach (DataRow myrow in dt.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", myrow["id"].ToString());
                ht.Add("text", myrow["text"].ToString());
                ht.Add("expanded", true);
                ht.Add("leaf", true);
                if (selectlist.Contains(myrow["id"].ToString()))
                {
                    ht.Add("check", true);
                }
                else
                {
                    ht.Add("check", false);
                }
                //if (myrow["OrganizaId"].ToString().Trim() == "0")//为分配部门人员
                //{
                //    ht.Add("iconCls", "GTP_unknowperson");
                //}
                //else
                //{
                //    ht.Add("iconCls", "GTP_person");
                //}
                hashList.Add(ht);
            }
            Output = new JavaScriptSerializer().Serialize(hashList);
        }
        #endregion

        #region ===绑定班级下拉树 GetClassNode

        /// <summary>
        /// 查询父节点
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="?"></param>
        public void GetClassNode(DataTable dt, DataTable usertable, string selectlist)
        {
            List<Hashtable> hashList = new List<Hashtable>();
            DataView dv = dt.DefaultView;
            DataTable ds = dv.ToTable();

            DataView userdv = usertable.DefaultView;

            foreach (DataRow myrow in ds.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", myrow["id"].ToString());
                ht.Add("text", myrow["text"].ToString());
                ht.Add("iconCls", "GTP_dummy");//班级
                ht.Add("expanded", true);

                //查询子节点
                userdv.RowFilter = "OrganizaId='" + myrow["id"].ToString() + "'";
                DataTable childSet = userdv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    ht.Add("leaf", false);
                    ht.Add("children", GetChildNodes(childSet, selectlist));
                }
                else
                {
                    ht.Add("leaf", true);
                }
                hashList.Add(ht);
            }
            Output = JsonHelper.ToJson(hashList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid">菜单funid</param>
        /// <param name="Btn">菜单funid对应的按钮信息</param>
        /// <returns></returns>
        private List<jsonFunTreeByChk> GetChildNodes(DataTable btnTable, string selectlist)
        {
            //表明该节点已经没有子节点呢
            if (btnTable.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                List<jsonFunTreeByChk> myts = new List<jsonFunTreeByChk>();
                for (int i = 0; i < btnTable.Rows.Count; i++)
                {
                    jsonFunTreeByChk myrow = new jsonFunTreeByChk();
                    myrow.id = btnTable.Rows[i]["Id"].ToString();
                    myrow.text = btnTable.Rows[i]["text"].ToString();
                    myrow.check = false;
                    myrow.children = null;
                    myrow.leaf = myrow.children == null ? true : false;
                    if (selectlist.Contains(btnTable.Rows[i]["Id"].ToString()))
                    {
                        myrow.check = true;
                    }
                    myts.Add(myrow);
                }
                return myts;
            }
        }

        #endregion

        #region 查询当前注册管理员是否存在 ExitsMaster
        /// <summary>
        /// 查询当前注册管理员是否存在
        /// </summary>
        public void ExitsMaster(string code, string key)
        {
            ModJsonResult json = new ModJsonResult();
            int count = 0;
            if (key != "" && key != null)
            {
                json.success = new BllSysMaster().Exists("Sys_Master", " and LoginName='" + code + "'  and Id<>'" + key + "' and Status!=" + (int)StatusEnum.删除, out count);
            }
            else
            {
                json.success = new BllSysMaster().Exists("Sys_Master", " and LoginName='" + code + "'  and Status!=" + (int)StatusEnum.删除, out count);
            }
            WriteJsonToPage(json.ToString());
        }

        #endregion

        #region ===根据id查询实体 GetData
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult MasterInfo(string cid)
        {
            var company = new BllSysCompany().LoadData(cid);
            ModSysMaster mod = new BllSysMaster().LoadData(company.MasterId);
            if (mod != null)
            {
                mod.Pwd = DESEncrypt.Decrypt(mod.Pwd);
            }
            return Json(new { data = mod }, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
