
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using QINGUO.ViewModel;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 角色信息操作类
    /// </summary>
    public class RoleManageController : BaseController<ModSysRole>
    {
        ModJsonResult json = new ModJsonResult();
        public string _key;


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleManage()
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
            search.AddCondition("CompanyId='" + CurrentMaster.Cid + "'");
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");//自己查看自己的
                }
            }
            var jsonResult = new BllSysRole().SearchData(search);

            LogInsert(OperationTypeEnum.访问, "角色配置", CurrentMaster.UserName + "页面访问正常.");

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
        public void SaveData(ModSysRole t)
        {
            BllSysRole bll = new BllSysRole();
            ModJsonResult json = new ModJsonResult();

            t.Status = (int)StatusEnum.正常;
            t.CreaterId = CurrentMaster.Id;//创建人编号
            t.RoleType = CurrentMaster.Attribute;//角色类型
            if (string.IsNullOrEmpty(t.CompanyID))
            {
                t.CompanyID = CurrentMaster.Cid;
            }
            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                var model = bll.LoadData(t.Id);
                model.Name = t.Name;
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
                int result = bll.Insert(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
            }

            LogInsert(OperationTypeEnum.操作, "角色配置", CurrentMaster.UserName + "角色配置新增或修改操作正常.");

            WriteJsonToPage(json.ToString());
        }

        #endregion

        #region ===权限管理 funTree,GetOtherfunTree
        /// <summary>
        /// 查看他人权限设置
        /// </summary>
        public void GetOtherfunTree()
        {
            if (CurrentMaster != null)
            {
                string _typeId = Request["TypeID"]; _key = Request["key"];
                string output; DataTable mytab = null;
                BllSysMaster mybase = new BllSysMaster();

                //查询角色创建人
                BllSysRole role = new BllSysRole();
                var roleInfo = role.LoadData(_key);
                var other = mybase.LoadData(roleInfo.CreaterId);//角色用户
                if (other.Attribute != (int)AdminTypeEnum.普通员工) //超级管理员,不用控权
                {
                    _where += " and TypeID=" + _typeId + " and Status=" + (int)StatusEnum.正常;
                    mytab = new BllSysFun().GetTreeList(_where).Tables[0]; //获取所有树
                }
                else
                {
                    string roleIdList = new BllSysMasterRole().GetMasterRole(other.Id);
                    if (!String.IsNullOrEmpty(roleIdList))
                    {
                        _where += " and TypeID=" + _typeId + "and Status=" + (int)StatusEnum.正常;
                        mytab = mybase.GetAuthByPage(roleIdList, _where).Tables[0];
                    }
                    else
                    {
                        WriteJsonToPage(Output);
                        return;
                    }
                }
                GetTreeNode(mytab);//功能权限
            }
            Output = Output.Replace("check", "checked");
            WriteJsonToPage(Output);
        }

        /// <summary>
        /// 功能权限控制
        /// </summary>
        public void funTree()
        {
            if (CurrentMaster != null)
            {
                string _typeId = Request["TypeID"]; _key = Request["key"];
                string output; DataTable mytab;
                string name = (Request["TreeName"] == null ? "" : Request["TreeName"]); //查询过滤条件
                if (CurrentMaster.Attribute != (int)AdminTypeEnum.普通员工) //超级管理员
                {
                    string where = "";
                    if (name != "")
                    {
                        where += " FunName like '%" + name + "%' and TypeID=" + _typeId + " and Status=" + (int)StatusEnum.正常;
                    }
                    else
                    {
                        where += " TypeID=" + _typeId + " and Status=" + (int)StatusEnum.正常;
                    }
                    mytab = new BllSysFun().GetTreeList(where).Tables[0]; //获取所有树

                }
                else
                {
                    BllSysMaster mybase = new BllSysMaster();
                    _where += (name == "" ? " and TypeID=" + _typeId + " and Status=" + (int)StatusEnum.正常 : " and TypeID=" + _typeId + "  and Status=" + (int)StatusEnum.正常 + " and FunName like '%" + name + "%'");
                    mytab = mybase.GetAuthByPage(CurrentMaster.RoleIdList, _where).Tables[0];
                }
                GetTreeNode(mytab);//功能权限
            }
            Output = Output.Replace("check", "checked");
            WriteJsonToPage(Output);
        }

        /// <summary>
        /// 范围权限控制
        /// </summary>
        public void roleTree()
        {
            if (CurrentMaster != null)
            {
                string _typeId = Request["TypeID"]; _key = Request["key"];
                string output; DataTable mytab;
                BllSysMaster mybase = new BllSysMaster();

                _where += " and TypeID=" + _typeId + " and Status=" + (int)StatusEnum.正常;
                DataSet ds = mybase.GetAuthByPage("'" + _key + "'", _where);
                if (ds.Tables.Count > 0)
                {
                    mytab = ds.Tables[0];
                    GetRoleTreeNode(mytab);
                }
                else
                {
                    WriteJsonToPage(Output);
                    return;
                }
            }
            Output = Output.Replace("check", "checked");
            WriteJsonToPage(Output);
        }
        #endregion

        #region ===功能选择模块树 GetTreeNode，GetChildNodes，getBtns

        /// <summary>
        /// 查询父节点
        /// </summary>
        public void GetTreeNode(DataTable dt)
        {
            List<Hashtable> hashList = new List<Hashtable>();

            DataView dv = dt.DefaultView;
            dv.RowFilter = "ParentID='0'"; //树形默认根节点0
            DataTable ds = dv.ToTable();

            foreach (DataRow myrow in ds.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", myrow["id"].ToString());
                ht.Add("text", myrow["text"].ToString());
                ht.Add("iconCls", myrow["iconCls"].ToString());
                ht.Add("expanded", true);
                //查询子节点
                dv.RowFilter = "ParentID='" + myrow["id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    ht.Add("leaf", false);
                    ht.Add("children", GetChildNodes(dt, childSet));
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
        /// 获取子节点
        /// </summary>
        /// <param name="parDs"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public List<Hashtable> GetChildNodes(DataTable parDs, DataTable ds)
        {
            //表明该节点已经没有子节点呢
            if (ds.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                DataView dv = parDs.DefaultView;

                List<Hashtable> hashList = new List<Hashtable>();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("id", ds.Rows[i]["id"].ToString());
                    ht.Add("text", ds.Rows[i]["text"].ToString());
                    ht.Add("iconCls", ds.Rows[i]["iconCls"].ToString());
                    ht.Add("expanded", true);

                    //查询子节点
                    dv.RowFilter = "ParentID='" + ds.Rows[i]["id"].ToString() + "'";
                    DataTable childSet = dv.ToTable();
                    if (childSet.Rows.Count > 0)
                    {
                        ht.Add("leaf", false);
                        ht.Add("children", GetChildNodes(parDs, childSet));
                    }
                    else
                    {
                        if (Convert.ToBoolean(ds.Rows[i]["isChild"].ToString()))//叶子节点需加上复选框
                        {
                            //根据用户角色id查询使用权限
                            DataTable mySelectFun = new BllSysRoleFun().GetSelectFun(ds.Rows[i]["id"].ToString(), _key);
                            int count = 0;
                            foreach (DataRow prow in mySelectFun.Rows)
                            {
                                if (ds.Rows[i]["id"].ToString() == prow["FunId"].ToString())
                                {
                                    count++;
                                    ht.Add("check", true);
                                    break;
                                }
                            }
                            if (count <= 0)
                            {
                                ht.Add("check", false);
                            }
                            bool HasBtns = true;//该链接是否存在按钮
                            ht.Add("children", getBtns(ds.Rows[i]["id"].ToString(), mySelectFun, ref HasBtns)); //获取页面下的按钮信息
                            if (HasBtns)
                            {
                                ht.Add("leaf", false);
                            }
                            else
                            {
                                ht.Add("leaf", true);
                            }
                        }

                    }
                    hashList.Add(ht);
                }
                return hashList;
            }
        }

        /// <summary>
        /// 获取页面下的操作按钮
        /// </summary>
        /// <param name="pid">菜单funid</param>
        /// <param name="Btn">菜单funid对应的按钮信息</param>
        /// <returns></returns>
        private List<jsonFunTreeByChk> getBtns(string pid, DataTable Btn, ref bool Btns)
        {
            DataTable btnTable = new BllSysBtn().GetBtnByPage(pid).Tables[0];

            //表明该节点已经没有子节点呢
            if (btnTable.Rows.Count == 0)
            {
                Btns = false;//没有按钮
                return null;
            }
            else
            {
                List<jsonFunTreeByChk> myts = new List<jsonFunTreeByChk>();
                for (int i = 0; i < btnTable.Rows.Count; i++)
                {
                    jsonFunTreeByChk myrow = new jsonFunTreeByChk();
                    myrow.id = btnTable.Rows[i]["Id"].ToString();
                    myrow.text = btnTable.Rows[i]["Name"].ToString();
                    myrow.check = false;
                    myrow.children = null;
                    if (Btn.Rows.Count > 0)
                    {
                        foreach (DataRow prow in Btn.Rows)
                        {
                            if (myrow.id == prow["UniteId"].ToString())
                            {
                                myrow.check = true;
                                break;
                            }
                        }
                    }
                    myrow.leaf = myrow.children == null ? true : false;
                    myts.Add(myrow);
                }
                return myts;
            }
        }

        #endregion

        #region ===范围权限模块树  GetRoleTreeNode，GetRoleChildNodes

        /// <summary>
        /// 查询父节点
        /// </summary>
        public void GetRoleTreeNode(DataTable dt)
        {

            string TreeIdList = Request["funid"].ToString();//获取用户先前选择的节点

            List<Hashtable> hashList = new List<Hashtable>();

            DataView dv = dt.DefaultView;
            dv.RowFilter = "ParentID='0'"; //树形默认根节点0
            DataTable ds = dv.ToTable();

            foreach (DataRow myrow in ds.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", myrow["id"].ToString());
                ht.Add("text", myrow["text"].ToString());
                ht.Add("iconCls", myrow["iconCls"].ToString());
                ht.Add("expanded", true);
                //查询子节点
                dv.RowFilter = "ParentID='" + myrow["id"].ToString() + "'";
                DataTable childSet = dv.ToTable();
                if (childSet.Rows.Count > 0)
                {
                    ht.Add("leaf", false);
                    ht.Add("children", GetRoleChildNodes(dt, childSet, TreeIdList));
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
        /// 获取子节点
        /// </summary>
        /// <param name="parDs"></param>
        /// <param name="ds"></param>
        /// <param name="TreeIdList">先前选择的节点</param>
        /// <returns></returns>
        public List<Hashtable> GetRoleChildNodes(DataTable parDs, DataTable ds, string TreeIdList)
        {
            //表明该节点已经没有子节点呢
            if (ds.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                string[] PID = TreeIdList.Split(',');

                DataView dv = parDs.DefaultView;
                List<Hashtable> hashList = new List<Hashtable>();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("id", ds.Rows[i]["id"].ToString());
                    ht.Add("text", ds.Rows[i]["text"].ToString());
                    ht.Add("iconCls", ds.Rows[i]["iconCls"].ToString());


                    //查询子节点
                    dv.RowFilter = "ParentID='" + ds.Rows[i]["id"].ToString() + "'";
                    DataTable childSet = dv.ToTable();
                    if (childSet.Rows.Count > 0)
                    {
                        ht.Add("expanded", true);
                        ht.Add("leaf", false);
                        ht.Add("children", GetRoleChildNodes(parDs, childSet, TreeIdList));
                    }
                    else
                    {
                        ht.Add("leaf", true);
                        ht.Add("expanded", false);
                        int count = 0;
                        foreach (string prow in PID)
                        {
                            if (ds.Rows[i]["id"].ToString() == prow.ToString())
                            {
                                count++;
                                ht.Add("check", true);
                                break;
                            }
                        }
                        if (count <= 0)
                        {
                            ht.Add("check", false);
                        }
                        if (!Convert.ToBoolean(ds.Rows[i]["isCheckRole"].ToString()))
                        {
                            ht.Add("disabled", true);
                        }
                    }
                    hashList.Add(ht);
                }
                return hashList;
            }
        }

        #endregion

        #region ===获取数据范围数据集合 GetAllData
        /// <summary>
        /// 获取数据范围数据集合
        /// </summary>
        [HttpPost]
        public void GetAllData()
        {
            string type = Request["ea"].ToString();//获取用户类型
            string where = " CompanyId='" + CurrentMaster.Company.Id + "'";
            DataSet ds = new BllSysRoleRangeData().GetAllData(type, where);
            var jsonResult = JsonHelper.DataTableToJsonForGridDataSuorce(ds.Tables[0]);
            WriteJsonToPage(jsonResult);
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
                int result = new BllSysRole().DeleteRoleInMaster(id);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                }
                LogInsert(OperationTypeEnum.操作, "角色配置", CurrentMaster.UserName + "角色配置删除操作正常.");
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "角色配置", CurrentMaster.UserName + "角色配置删除操作异常." + msg.msg);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ===保存范围权限 DateRoleSave
        /// <summary>
        /// 保存范围权限
        /// </summary>
        [HttpPost]
        public void DateRoleSave()
        {
            var msg = new ModJsonResult();
            string json = Request["json"].ToString();//实体字符串
            string opeaterType = Request["opeaterType"].ToString();//操作类型

            ModSysRoleRangeData dsobj = (ModSysRoleRangeData)new JavaScriptSerializer().Deserialize(json, typeof(ModSysRoleRangeData));

            string Id = "";//先前选择的模块树编号
            string Name = "";

            try
            {
                if (opeaterType == "add")//新增
                {
                    DataSet ds = new BllSysRoleRangeData().GetAllData(dsobj.RoleId, "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Id += ds.Tables[0].Rows[i]["FunId"].ToString() + ",";
                            Name += ds.Tables[0].Rows[i]["FunName"].ToString() + ",";
                        }
                        Id = Id.Substring(0, Id.Length - 1);
                        Name = Name.Substring(0, Name.Length - 1);
                    }

                    dsobj.Id = Guid.NewGuid().ToString();
                    dsobj.CompanyId = CurrentMaster.Company.Id;
                    dsobj.CreaterId = CurrentMaster.Id;

                    //新增时先看是否该类型已经添加
                    if (string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Name))//还没有注册这类型
                    {
                        new BllSysRoleRangeData().Insert(dsobj);
                        msg.success = true;
                    }
                    else
                    {
                        string[] treeNew = dsobj.FunId.Split(',');//选择的模块树
                        string[] treeOld = Id.Split(',');
                        string[] treeNewName = dsobj.FunName.Split(',');

                        bool flag = false;//没有遇到相同的id
                        for (int i = 0; i < treeNew.Length; i++)
                        {
                            for (int j = 0; j < treeOld.Length; j++)
                            {
                                if (treeNew[i] == treeOld[j])//说明同一个模块下注册相同类型了
                                {
                                    flag = true;
                                    msg.success = false;
                                    msg.msg = "[" + treeNewName[i] + "]模块已经设置,请重新选择！";
                                    break;
                                }
                            }
                            if (flag)
                            {
                                break;
                            }
                        }
                        if (!flag)//没有相同的id
                        {
                            new BllSysRoleRangeData().Insert(dsobj);
                            msg.success = true;
                        }
                    }
                }
                else//修改
                {
                    DataSet ds = new BllSysRoleRangeData().GetAllData(dsobj.RoleId, "Id!='" + dsobj.Id + "'");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Id += ds.Tables[0].Rows[i]["FunId"].ToString() + ",";
                            Name += ds.Tables[0].Rows[i]["FunName"].ToString() + ",";
                        }
                        Id = Id.Substring(0, Id.Length - 1);
                        Name = Name.Substring(0, Name.Length - 1);
                    }


                    string[] treeNew = dsobj.FunId.Split(',');//选择的模块树
                    string[] treeOld = Id.Split(',');
                    string[] treeNewName = dsobj.FunName.Split(',');

                    bool flag = false;//没有遇到相同的id
                    for (int i = 0; i < treeNew.Length; i++)
                    {
                        for (int j = 0; j < treeOld.Length; j++)
                        {
                            if (treeNew[i] == treeOld[j])//说明同一个模块下注册相同类型了
                            {
                                flag = true;
                                msg.success = false;
                                msg.msg = "[" + treeNewName[i] + "]模块已经设置,请重新选择！";
                                break;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                    }
                    if (!flag)//没有相同的id
                    {
                        new BllSysRoleRangeData().UpdateData(dsobj);
                        msg.success = true;
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

        #region ===模块功能分配 SaveRoles

        /// <summary>
        /// 保存模块功能分配
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="key"></param>
        public void SaveRoles()
        {
            try
            {
                string jsonStr = Request["json"];
                string key = Request["key"];
                BllSysRoleFun bll = new BllSysRoleFun();

                //保存用户角色菜单信息
                string[] _str = jsonStr.Split(',');
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < _str.Length; i++)
                {
                    string[] str = _str[i].Split('|');
                    string funId = "";
                    string btnId = "";
                    if (str.Length > 1) //存在按钮
                    {
                        funId = str[1];
                        btnId = str[0];
                    }
                    else if (str.Length == 1) //模块链接没有按钮
                    {
                        funId = str[0];
                    }
                    var model = new ModSysRoleFun()
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = key,
                        FunId = funId,
                        BtnId = btnId,
                        UniteId = _str[i], //组合键
                        CreaterId = CurrentMaster.Id,
                        CreaterName = CurrentMaster.UserName,
                        CId = CurrentMaster.Cid
                    };
                    sb.Append(bll.InsertRole(model));
                    sb.AppendLine();
                }

                if (bll.InsertRoleByRoleId(key, sb.ToString()) > 0)
                {
                    json.success = true;
                    SearchClidRole(key);
                }
                else
                {
                    json.success = false;
                }

            }
            catch (Exception ex)
            {
                json.success = false;
                json.msg = ex.Message;
            }
            WriteJsonToPage(json.ToString());
        }


        /// <summary>
        /// 角色下创建的其它角色
        /// </summary>
        /// <param name="roleId"></param>
        public void SearchClidRole(string roleId)
        {
            DataSet ds = new BllSysRole().SearchClidRole(roleId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//循环匹配子角色与父角色
                {
                    string role = ds.Tables[0].Rows[i]["Id"].ToString();
                    new BllSysRole().DelteClidRole(roleId, role);
                    if (role != roleId)
                    {
                        SearchClidRole(role);
                    }
                }
            }
        }
        #endregion


        #region ===保存用户自定义grid显示列
        /// <summary>
        /// 保存用户自定义grid显示列
        /// </summary>
        [AcceptVerbs(HttpVerbs.Post)]
        public void UpdateGridColumn()
        {
            try
            {
                string ColumnIndex = Request["ColumnIndex"];
                string moudel = Request["moudel"];
                var CurrentMaster = new MasterContext().Master;
                if (!string.IsNullOrEmpty(CurrentMaster.Id))
                {
                    StringBuilder sb = new StringBuilder();
                    //删除用户先前配置的信息
                    sb.Append("delete from Sys_GridColumn where UserId='" + CurrentMaster.Id + "' and ModelId='" + moudel + "';");
                    sb.AppendLine();
                    //插入新的设置信息
                    sb.Append("insert into Sys_GridColumn(Id,UserId,ModelId,ColumnId) values(NEWID(),'" + CurrentMaster.Id + "','" + moudel + "','" + ColumnIndex + "')");

                    int resut = new BllSysRole().ExecuteNonQueryByText(sb.ToString());
                    if (resut > 0)
                    {
                        json.success = true;
                        json.msg = "设置成功!";
                    }
                    else
                    {
                        json.success = false;
                        json.msg = "设置失败!";
                    }
                }
                else
                {
                    json.success = false;
                    json.msg = "用户信息已过期，请重新登陆!";
                }

            }
            catch (Exception ex)
            {
                json.success = false;
                json.msg = "操作失败!" + ex.Message;
            }
            Response.Write(new JavaScriptSerializer().Serialize(json).ToString());
            Response.End();
        }
        #endregion

        #region ===获取用户自定义grid显示列
        /// <summary>
        ///获取用户自定义grid显示列
        /// </summary>
        [AcceptVerbs(HttpVerbs.Post)]
        public void GetGridColumn()
        {
            try
            {
                string moudel = Request["moudel"];
                var CurrentMaster = new MasterContext().Master;
                if (!string.IsNullOrEmpty(CurrentMaster.Id))
                {
                    DataSet ds = new BllSysRole().GetGridColumn(moudel, CurrentMaster.Id);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("UserId", ds.Tables[0].Rows[0]["UserId"].ToString());
                        ht.Add("ModelId", ds.Tables[0].Rows[0]["ModelId"].ToString());
                        ht.Add("ColumnId", ds.Tables[0].Rows[0]["ColumnId"].ToString());
                        json.success = true;
                        json.msg = "设置成功!";
                        json.data = JsonHelper.ToJson(ht);
                    }
                }
                else
                {
                    json.success = false;
                    json.msg = "用户信息已过期，请重新登陆!";
                }

            }
            catch (Exception ex)
            {
                json.success = false;
                json.msg = "操作失败!" + ex.Message;
            }
            Response.Write(new JavaScriptSerializer().Serialize(json).ToString());
            Response.End();
        }
        #endregion

    }
}
