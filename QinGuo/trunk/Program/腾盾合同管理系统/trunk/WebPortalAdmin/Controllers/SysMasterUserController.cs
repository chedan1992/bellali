using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using System.Data;
using System.Collections;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 管理用户类
    /// </summary>
    public class SysMasterUserController : BaseController<ModSysMaster>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SysMasterUser()
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
            search.AddCondition("CID='" + CurrentMaster.Cid + "'");
            search.AddCondition("Attribute=" + (int)AdminTypeEnum.手机用户);
            search.AddCondition("Status!=" + (int)StatusEnum.删除);
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }

            var jsonResult = new BllSysMaster().SearchData(search);
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
            t.Attribute = (int)AdminTypeEnum.普通员工;//栏目类型
            t.IsMain = true;//普通用户
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

            WriteJsonToPage(json.ToString());
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
            string TypeId = Request["TypeId"].ToString();
            if (key != "" && key != null)
            {
                json.success = new BllSysMaster().Exists("Sys_Master", " and OrganizaId='" + TypeId + "'  and Attribute=" + (int)AdminTypeEnum.普通员工 + " and CID='" + CurrentMaster.Company.Id + "' and LoginName='" + code + "'  and Id<>'" + key + "' and Status!=" + (int)StatusEnum.删除, out count);
            }
            else
            {
                json.success = new BllSysMaster().Exists("Sys_Master", " and OrganizaId='" + TypeId + "'   and Attribute=" + (int)AdminTypeEnum.普通员工 + " and CID='" + CurrentMaster.Company.Id + "' and LoginName='" + code + "'  and Status!=" + (int)StatusEnum.删除, out count);
            }
            WriteJsonToPage(json.ToString());
        }

        #endregion
    }
}
