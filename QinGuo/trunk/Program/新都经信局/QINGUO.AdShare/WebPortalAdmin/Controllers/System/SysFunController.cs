using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using QINGUO.Business;
using QINGUO.Model;
using QINGUO.ViewModel;
using WebPortalAdmin.Code;
using QINGUO.Common;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 菜单树导航操作类
    /// </summary>
    public class SysFunController :BaseController<ModSysFun>
    {

        /// <summary>
        /// 页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SysFun()
        {
            return View();
        }

        #region --菜单管理模块 InitMain

        /// <summary>
        /// 操作方法
        /// </summary>
        [HttpPost]
        public void InitMain()
        {
            try
            {
                string typeId = (Request["id"] == null ? "0" : Request["id"]);//左边树形类型
                BllSysFun mybase = new BllSysFun();
                _where += " and Status!="+(int)StatusEnum.删除+" and TypeID=" + typeId;
                _mySet = mybase.GetTreeList(_where);
                List<JsonFunTree> list = new FunTreeCommon().GetFunTreeNodes(_mySet);
                Output =JsonHelper.ToJson(list);
            }
            catch
            {
                Output ="";
            }
            WriteJsonToPage(Output);
        }

        #endregion

        #region ---左侧菜单树导航 Comboboxtree

        /// <summary>
        /// 左侧菜单树导航,获取用户权限链接
        /// </summary>
        [HttpPost]
        public void Comboboxtree()
        {
            try
            {
                string LeftId = Request["LeftId"];//左边菜单选择
                BllSysFun mybase = new BllSysFun();
                _where += " and Status=" + (int)StatusEnum.正常 + "and TypeID=" + LeftId;
                _mySet = mybase.GetTreeList(_where);

                Output = JsonHelper.ToJson(new FunTreeCommon().GetFunTreeNodes(_mySet));
            }
            catch
            {
                Output = "{{'msg':'','success':false}}";
            }
            WriteJsonToPage(Output);
        }
        #endregion

        #region ---按钮选择面板 itemselector

        /// <summary>
        /// 按钮选择面板
        /// </summary>
        public void itemselectorStore()
        {
            try
            {
                string pageName = Request["Action"];
                BllSysBtn bll = new BllSysBtn();
                string id = Request["Id"];
                switch (pageName)
                {
                    case "BtnLeftSelect":
                        Output = bll.GetBtnLeftSelect(id);
                        break;
                    case "BtnRightSelect":
                        Output = bll.GetBtnRightSelect(id);
                        break;
                }
            }
            catch
            {
                Output = "";
            }
            WriteJsonToPage(Output);
        }

        #endregion

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysFun t)
        {
            BllSysFun bll = new BllSysFun();
            ModJsonResult json = new ModJsonResult();

            string pageAction = Request["PageAction"];
            string parentId = "0";//树状根节点统一为0

            t.isChild = false;
            if (Request["parentId"] != null && Request["parentId"] != "")
            {
                parentId = Request["parentId"];
                t.isChild = true;
            }
            t.ParentId = parentId;

            t.TypeId = int.Parse(Request["leftType"]);//左边模块选择的树
           
            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                t.PageUrl=(t.PageUrl == null ? "" : t.PageUrl);
                t.iconCls = (t.iconCls == null ? "" : t.iconCls);
                t.ClassName = (t.ClassName == null ? "" : t.ClassName);
                int result = bll.UpdateData(t);
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
                t.IsSystem = false;
                t.PageUrl = (t.PageUrl == null ? "" : t.PageUrl);
                t.iconCls = (t.iconCls == null ? "" : t.iconCls);
                t.ClassName = (t.ClassName == null ? "" : t.ClassName);

                int result = bll.Insert(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
            }

            //删除原有的按钮信息
            BllSysFunLinkBtnValue sysFunLinkBtnValue = new BllSysFunLinkBtnValue();
            sysFunLinkBtnValue.BatchInsert(pageAction, t.Id);
            
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
                int result = new BllSysFun().DeleteDate(id);
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
            int result = new BllSysFun().UpdateIsStatus(1, key);
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
            int result = new BllSysFun().UpdateIsStatus(0, key);
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
    }
}
