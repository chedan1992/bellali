using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using System.Web.Script.Serialization;
using System.Collections;
using System.Data;
using QINGUO.ViewModel;

namespace WebPortalAdmin.Controllers
{
    public class SysUserController : BaseController<ModSysUser>
    {
       /// <summary>
       /// 用户信息
       /// </summary>
       /// <returns></returns>
        public ActionResult SysUser()
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

            search.AddCondition("IsCarer=0");//查询普通用户
            //查询自己所属公司
            //search.AddCondition("CId='" + CurrentMaster.Cid + "'");
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }
            var jsonResult = new BllSysUser().SearchData(search);
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
        public void SaveData(ModSysUser t)
        {
            BllSysUser bll = new BllSysUser();
            ModJsonResult json = new ModJsonResult();
            string TypeId = Request["TypeId"].ToString();

            t.Pwd = (string.IsNullOrEmpty(t.Pwd) ? DESEncrypt.Encrypt("666666") : DESEncrypt.Encrypt(t.Pwd.Trim()));
            t.Name = t.Name.Trim();
            t.Nickname = t.Nickname.Trim();
            t.OrganizaId = TypeId;
            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                var model = bll.LoadData(t.Id);
                model.Pwd = t.Pwd;
                model.Name = t.Name;
                model.Nickname = t.Nickname;
                model.OrganizaId = t.OrganizaId;
                model.Sex = t.Sex;
                model.Tel = t.Tel;
                model.Email = t.Email;

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
                t.CId = CurrentMaster.Cid;
                t.CreaterId = CurrentMaster.Id;
                t.CreateTime = DateTime.Now;
                t.CId = CurrentMaster.Cid;
                t.MobileCode = ""; //机器码
                t.Birthday = DateTime.Now;
                t.BDUserId = "";
                t.BDChannelId = "";
                t.PaltForm =0;//登录平台
                t.Visible = true;
                t.IsCarer = false;
                if (t.Sex==0)
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
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region 查询当前注册用户是否存在 ExitsUser

        /// <summary>
        /// 查询当前注册管理员是否存在
        /// </summary>
        public void ExitsUser(string code, string key)
        {
            ModJsonResult json = new ModJsonResult();
            int count = 0;
            if (key != "" && key != null)
            {
                json.success = new BllSysUser().Exists("Sys_User", "  and CId='" + CurrentMaster.Company.Id + "' and Name='" + code + "'  and Id<>'" + key + "' and Status!=" + (int)StatusEnum.删除, out count);
            }
            else
            {
                json.success = new BllSysUser().Exists("Sys_User", "  and CId='" + CurrentMaster.Company.Id + "' and Name='" + code + "'  and Status!=" + (int)StatusEnum.删除, out count);
            }
            WriteJsonToPage(json.ToString());
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
            int result = new BllSysUser().UpdateStatus(1, key);
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
            int result = new BllSysUser().UpdateStatus(0, key);
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

        #region ==删除 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                BllSysUser User = new BllSysUser();
                var Model = User.LoadData(id);
                if (Model != null)
                {
                    Model.Status = (int)StatusEnum.删除;
                    int result = User.Update(Model);
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
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==密码重置 ReSetPwd
        /// <summary>
        /// 密码重置
        /// </summary>
        [AcceptVerbs(HttpVerbs.Post)]
        public void ReSetPwd()
        {
            var json = new ModJsonResult();
            try
            {
                string id = Request["id"];
                string pwd = DESEncrypt.Encrypt("666666");
                int result = new BllSysUser().ReSetPwd(id, pwd);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "重置失败!";
                }
                else
                {
                    json.success = true;
                    json.msg = "重置成功!";
                }
            }
            catch (Exception ex)
            {
                json.success = false;
                json.msg = "操作失败!";
            }
            Response.Write(new JavaScriptSerializer().Serialize(json).ToString());
            Response.End();
        }

        #endregion

        #region ===用户选择树 GetTreeByMaster
        [HttpPost]
        public void GetTreeByMaster()
        {
            string Attribute = Request["Attribute"];//1:老师 2:学生
            if (Attribute == "1")
            {
                string where = " CId='" + CurrentMaster.Company.Id + "'";
                string name = (Request["name"] == null ? "" : Request["name"]); //查询过滤条件
                if (name != "")
                {
                    where += " and Nickname like '%" + name + "%'";
                }

                DataTable mytab = new BllSysUser().GetTreeList(where).Tables[0]; //获取所有树
                GetTreeNode(mytab);
                Output = Output.Replace("check", "checked");
                WriteJsonToPage(Output);
            }
            else
            {
                //查询班级
                string where = " Type=1 and Status=" + (int)StatusEnum.正常;
                DataTable mytab = new BllSysGroup().GetTreeList(where).Tables[0]; //获取所有树
                string userwhere = " CId='" + CurrentMaster.Company.Id + "'";
                DataTable usermytab = new BllSysUser().GetTreeList(userwhere).Tables[0]; //获取所有树
                GetClassNode(mytab, usermytab);
                Output = Output.Replace("check", "checked");
                WriteJsonToPage(Output);
            }
        }

        /// <summary>
        /// 查询父节点
        /// </summary>
        public void GetTreeNode(DataTable dt)
        {
            List<Hashtable> hashList = new List<Hashtable>();

            foreach (DataRow myrow in dt.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", myrow["id"].ToString());
                ht.Add("text", myrow["text"].ToString());
                ht.Add("expanded", true);
                ht.Add("leaf", true);
                ht.Add("check", false);
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
        public void GetClassNode(DataTable dt, DataTable usertable)
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
                    ht.Add("children", GetChildNodes(childSet));
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
        /// 获取页面下的操作按钮
        /// </summary>
        /// <param name="pid">菜单funid</param>
        /// <param name="Btn">菜单funid对应的按钮信息</param>
        /// <returns></returns>
        private List<jsonFunTreeByChk> GetChildNodes(DataTable btnTable)
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
                    myts.Add(myrow);
                }
                return myts;
            }
        }

        #endregion
    }
}
