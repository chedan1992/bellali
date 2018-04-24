using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using System.Data;
using System.Collections;
using System.Web.Script.Serialization;
using QINGUO.Common;
using WebPortalAdmin.Code;
using QINGUO.ViewModel;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 分类字典表
    /// </summary>
    public class SysDircController : BaseController<ModSysDirc>
    {
        //
        // GET: /SysDirc/

        public ActionResult Index()
        {
            return View();
        }


        #region==分类字典列表 SearchData
        /// <summary>
        /// 分类字典列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            try
            {
                search.AddCondition("Type=" + Request["Type"]);//分类
                if (!CurrentMaster.IsMain)
                {
                    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                    {
                        search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                    }
                }
                LogInsert(OperationTypeEnum.访问, "分类字典模块", "访问页面成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "分类字典模块", "查询列表错误消息:" + ex.Message.ToString());
            }
            var jsonResult = new BllSysDirc().SearchData(search);
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
        public void SaveData(ModSysDirc t)
        {
            BllSysDirc bll = new BllSysDirc();
            ModJsonResult json = new ModJsonResult();
            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                var model = bll.LoadData(t.Id);
                model.Name = t.Name;
                model.OrderNum = t.OrderNum;
                //判断名称是否存在
                int count = 0;
                bool flag = bll.Exists("Sys_Group", " and CompanyId='" + t.CompanyId + "'  and Name='" + t.Name + "' and Type=" + t.Type + " and Id<>'" + t.Id + "' and Status!=" + (int)StatusEnum.删除, out count);
                if (flag)
                {
                    json.success = false;
                    json.msg = " 保存失败,该名称已经存在!";
                }
                else
                {
                    int result = bll.Update(model);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "修改失败,请稍后再操作!";
                    }
                }
            }
            else
            {
                //判断名称是否存在
                int count = 0;
                bool flag = bll.Exists("Sys_Dirc", " and Name='" + t.Name + "' and Type=" + t.Type + " and Status!=" + (int)StatusEnum.删除, out count);
                if (flag)
                {
                    json.success = false;
                    json.msg = " 保存失败,该名称已经存在!";
                }
                else
                {
                    t.Id = Guid.NewGuid().ToString();
                    t.Status = (int)StatusEnum.正常;
                    t.CreaterId = CurrentMaster.Id;
                    t.CreateTime = DateTime.Now;
                    t.CompanyId = CurrentMaster.Cid;
                    t.ParentId = "0";
                    t.Type =int.Parse(Request["Type"]);
                    int result = bll.Insert(t);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = " 保存失败,请稍后再操作!";
                    }
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
                int result = new BllSysDirc().DeleteStatus(id);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                    msg.msg = "删除失败.";
                }
                LogInsert(OperationTypeEnum.访问, "字典设备类型删除操作", "删除操作成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "字典设备类型删除操作", "删除操作异常消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ===字典分类选择树 GetTree
        [HttpPost]
        public void GetTree()
        {
            string where = "";
            string name = (Request["MasterName"] == null ? "" : Request["MasterName"]); //查询过滤条件
            if (name != "")
            {
                where += " Name like '%" + name + "%'";
            }
            string CompanyId = string.IsNullOrEmpty(Request["CompanyId"]) ? CurrentMaster.Cid : Request["CompanyId"];
            DataTable mytab = new BllSysDirc().GetTreeList(where).Tables[0]; //获取所有树
            GetTreeNode(mytab, CompanyId);
            Output = Output.Replace("check", "checked");
            WriteJsonToPage(Output);
        }

        /// <summary>
        /// 查询父节点
        /// </summary>
        public void GetTreeNode(DataTable dt,string CompanyId)
        {
            List<Hashtable> hashList = new List<Hashtable>();
            //获取当前单位选择的类别信息
            DataSet ds = new BllSysDirc().GetList("Sys_Dirc", " and Id in(select ParentId from Sys_Group where CompanyId='" + CompanyId + "')","*",0);
            foreach (DataRow myrow in dt.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", myrow["id"].ToString());
                ht.Add("text", myrow["text"].ToString());
                ht.Add("expanded", true);
                ht.Add("leaf", true);

                bool Has = false;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow[] dRow = ds.Tables[0].Select("Id='" + myrow["id"].ToString() + "'");
                    if (dRow.Count() > 0)
                    {
                        Has = true;
                        break;
                    }
                }
                if (Has == true)
                {
                    ht.Add("check", true);
                }
                else {
                    ht.Add("check", false);
                }
                hashList.Add(ht);
            }
            Output = new JavaScriptSerializer().Serialize(hashList);
        }
        #endregion


        #region ==根据编码获取字典类型 GetSysDicByType
        /// <summary>
        /// 查询所属区
        /// </summary>
        [HttpPost]
        public void GetSysDicByType(int Type)
        {
            List<ModSysDirc> jsonlist = new BllSysDirc().QueryToAll().Where(p => p.Type == Type && p.Status == (int)StatusEnum.正常).ToList();

            List<JsonFunTree> list = new List<JsonFunTree>();
            for (int i = 0; i < jsonlist.Count; i++)
            {
                JsonFunTree model = new JsonFunTree();
                model.id = jsonlist[i].Id;
                model.parentId = "0";
                model.text = jsonlist[i].Name;
                model.Depth = 0;
                model.expanded = false;
                model.leaf = true;
                model.leaf = true;
                list.Add(model);
            }
            WriteJsonToPage(JsonHelper.ToJson(list));
        }
        #endregion
       
    }
}
