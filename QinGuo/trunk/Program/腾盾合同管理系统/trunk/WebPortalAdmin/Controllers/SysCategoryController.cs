using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Business;
using QINGUO.Model;
using System.Data;
using QINGUO.ViewModel;
using WebPortalAdmin.Code;
using QINGUO.Common;
using System.Collections;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 树形分类
    /// </summary>
    public class SysCategoryController : BaseController<ModSysCategory>
    {
        /// <summary>
        /// 分类页
        /// </summary>
        /// <returns></returns>
        public ActionResult SysCategory()
        {
            return View();
        }

        #region==页面列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        [HttpPost]
        public void SearchData()
        {
            string id = Request["id"].ToString();

            if (!CurrentMaster.IsMain)
            {
                //获取用户查看类型
                string className = GetRequestQueryString("className");//页面类名
                if (!String.IsNullOrEmpty(className))
                {
                    if (!String.IsNullOrEmpty(CurrentMaster.RoleIdList))
                    {
                        CurrentMaster.LookPower = new BllSysMaster().GetLookPower(className, CurrentMaster.RoleIdList, CurrentMaster.Company.Attribute);
                    }
                }

                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    _where = "CreaterId='" + CurrentMaster.Id + "'";//自己查看自己的
                }
            }

            DataSet ds = new BllSysCategory().SearchData(id, _where);
            List<jsonSysCategory> list = new FunTreeCommon().GetTreeBySysCategory(ds);
            WriteJsonToPage(JsonHelper.ToJson(list));
        }
        #endregion

        #region --- Comboboxtree

        /// <summary>
        ///
        /// </summary>
        [HttpPost]
        public void Comboboxtree()
        {
            try
            {

                string key = Request["key"].ToString();//编辑的Id
                if (!string.IsNullOrEmpty(key))
                {
                    _where += " and Id!='" + key + "'";
                }
                if (!CurrentMaster.IsMain)
                {
                    //获取用户查看类型
                    string className = GetRequestQueryString("className");//页面类名
                    if (!String.IsNullOrEmpty(className))
                    {
                        if (!String.IsNullOrEmpty(CurrentMaster.RoleIdList))
                        {
                            CurrentMaster.LookPower = new BllSysMaster().GetLookPower(className, CurrentMaster.RoleIdList, CurrentMaster.Company.Attribute);
                        }
                    }

                    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                    {
                        _where += " and CreaterId='" + CurrentMaster.Id + "'";//自己查看自己的
                    }
                }

                DataSet ds = new BllSysCategory().GetTreeList(_where);

                Output = JsonHelper.ToJson(new FunTreeCommon().CategoryTreeNodes(ds));
            }
            catch
            {
                Output = "{{'msg':'','success':false}}";
            }
            WriteJsonToPage(Output);
        }
        #endregion

        #region==编辑页 SearchDataEdit
        /// <summary>
        /// 编辑页
        /// </summary>
         [HttpPost]
        public void SearchDataEdit()
        {
            string ParentCategoryId = Request["ParentCategoryId"] == null ? "0" : Request["ParentCategoryId"];
            List<ModSysCategory> list = new BllSysCategory().QueryToAll().Where(p => p.ParentCategoryId == ParentCategoryId && p.Status==(int)StatusEnum.正常).ToList();
            //增加一个默认节点(全部)
            var debugger = (Request["debugger"] != null ? false : true);
            if (debugger)
            {
                if (list.Count > 0)
                {
                    ModSysCategory model = new ModSysCategory();
                    model.Id = "";
                    model.Name = "全部";
                    model.ParentCategoryId = "0";
                    list.Insert(0, model);
                }
            }
            WriteJsonToPage(JsonHelper.ToJson(list));
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
            int result = new BllSysCategory().UpdateIsStatus(1, key);
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

        //#region ==停用状态 DisableUse

        ///// <summary>
        ///// DisableUse
        ///// </summary>
        //public void DisableUse()
        //{
        //    var msg = new ModJsonResult();

        //    string key = Request["id"];
        //    BllBsTestStore TestStore = new BllBsTestStore();
        //    int result = TestStore.CanStop(key);
        //    if (result > 0)
        //    {
        //        msg.success = true;
        //        msg.msg = "试卷正在使用该类型,不能禁用";
        //        msg.errorCode = (int)SystemError.提示错误;
        //    }
        //    else
        //    {
        //        result = new BllSysCategory().UpdateIsStatus(0, key);
        //        if (result > 0)
        //        {
        //            msg.success = true;
        //        }
        //        else
        //        {
        //            msg.success = false;
        //            msg.msg = "操作失败";
        //        }
        //    }
        //    WriteJsonToPage(msg.ToString());
        //}

        //#endregion

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysCategory t)
        {
            BllSysCategory bll = new BllSysCategory();
            ModJsonResult json = new ModJsonResult();

            t.Status = (int)StatusEnum.正常;
            t.Path = "";
            t.IsSystem = false;
            t.CreaterId = CurrentMaster.Id;
            t.CreaterName = CurrentMaster.UserName;
            t.ParentCategoryId = Request["parentId"].ToString();
            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                var Category = bll.LoadData(t.Id);
                int result = bll.UpdateDate(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
            }
            else
            {
                t.PicUrl = t.PicUrl == null ? "/Resource/ShopCategory/default.png" : t.PicUrl;
                t.Id = Guid.NewGuid().ToString();
                t.CompanyId = CurrentMaster.Cid;

                if (!string.IsNullOrEmpty(t.ParentCategoryId))
                {
                    var Category = bll.LoadData(t.ParentCategoryId);
                    if (Category != null)
                    {
                        t.Depth = Category.Depth + 1;
                    }
                    else
                    {
                        t.Depth = 0;
                    }
                }
                int result = bll.InsertDate(t);
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
                int result = new BllSysCategory().DeleteDate(id);
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
    }
}
