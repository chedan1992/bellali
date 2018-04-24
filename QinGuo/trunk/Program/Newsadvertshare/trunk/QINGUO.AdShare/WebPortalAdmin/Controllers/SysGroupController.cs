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
using System.Web.Script.Serialization;
using QINGUO.ViewModel;
using WebPortalAdmin.Code;
using System.Text;

namespace WebPortalAdmin.Controllers
{
    public class SysGroupController : BaseController<ModSysGroup>
    {

        /// <summary>
        /// 超级管理员类型
        /// </summary>
        /// <returns></returns>
        public ActionResult SuperManin()
        {
            return View();
        }

        /// <summary>
        /// 题型
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionsType()
        {
            return View();
        }

        /// <summary>
        /// 公告类别
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassManage()
        {
            return View();
        }

        /// <summary>
        /// 证书类型
        /// </summary>
        /// <returns></returns>
        public ActionResult BookType()
        {
            return View();
        }

        /// <summary>
        /// 产品类型
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductType()
        {
            return View();
        }

        #region==设备类型 SearchDataQuestions  0
        /// <summary>
        /// 设备类型
        /// </summary>
        public void SearchDataQuestions()
        {
            var search = base.GetSearch();

            try
            {
                search.AddCondition("Type=0");//分类
                if (!CurrentMaster.IsMain)
                {
                    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                    {
                        search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                    }
                }
                string CompanyId = CurrentMaster.Cid;//
                if (!string.IsNullOrEmpty(Request["Id"]))
                {
                    string key = Request["Id"].ToString().Trim();
                    if (key != "-1")//查询全部数据
                    {
                        CompanyId = key;
                        search.AddCondition("CompanyId='" + CompanyId + "'");
                    }
                }
                else
                {
                    search.AddCondition("CompanyId='" + CompanyId + "'");
                }
                LogInsert(OperationTypeEnum.访问, "设备类型模块", "访问页面成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备类型模块", "查询列表错误消息:" + ex.Message.ToString());
            }
            var jsonResult = new BllSysGroup().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region==班级管理 SearchDataClass  1
        /// <summary>
        /// 班级管理
        /// </summary>
        public void SearchDataClass()
        {
            var search = base.GetSearch();
            search.AddCondition("Type=1");//班级
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }
            var jsonResult = new BllSysGroup().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region==产品类型 SearchDataProduct  2
        /// <summary>
        /// 产品类型
        /// </summary>
        public void SearchDataProduct()
        {
            var search = base.GetSearch();
            search.AddCondition("Type=2");//产品类型
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }
            var jsonResult = new BllSysGroup().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region==证书类型 SearchDataBook  3
        /// <summary>
        /// 证书类型
        /// </summary>
        public void SearchDataBook()
        {
            var search = base.GetSearch();
            search.AddCondition("Type=3");//证书类型
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }
            var jsonResult = new BllSysGroup().SearchData(search);
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
        public void SaveData(ModSysGroup t)
        {
            BllSysGroup bll = new BllSysGroup();
            ModJsonResult json = new ModJsonResult();
            string type=(Request["Type"]==null?"":Request["Type"].ToString());
            switch (type)
            {
                case "ProductType":
                    t.Type = 2;
                    break;
                case "BookType":
                    t.Type = 3;
                    break;
                case "QuestionsType":
                    t.Type = 0;
                    break;
                case "ClassManage"://公告类型
                    t.Type = 1;
                    break;
            }
            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                var model = bll.LoadData(t.Id);
                model.Name = t.Name;
                model.ParentId = "0";//如果引用字典库的类别,修改后变成自己类别
                 //判断名称是否存在
                int count=0;
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
                string CompanyId = Request["CompanyId"].ToString().Trim();//
                if (string.IsNullOrEmpty(CompanyId))
                {
                    CompanyId = CurrentMaster.Cid;
                }
                //判断名称是否存在
                int count=0;
                bool flag = bll.Exists("Sys_Group", " and CompanyId='" + CompanyId + "' and Name='" + t.Name + "' and Type=" + t.Type + " and Status!=" + (int)StatusEnum.删除, out count);
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
                    t.CompanyId = CompanyId;
                    t.OrderNum = 0;
                    t.ParentId = "0";

                    int result = bll.Insert(t);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = " 保存失败,请稍后再操作!";
                    }
                    new BllSysMaster().ClearCache();
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
                //判断类型是否还在使用
                var mode = new BllSysAppointed().Exists(" Gid='" + id + "'");
                if (mode == true)
                {
                    msg.success = false;
                    msg.msg = "此类别正在使用,暂不能删除.";
                }
                else
                {
                    int result = new BllSysGroup().DeleteStatus(id);
                    if (result > 0)
                    {
                        msg.success = true;
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "删除失败.";
                    }

                    LogInsert(OperationTypeEnum.操作, "类型删除操作", "删除操作成功.");
                }
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "类型删除操作", "删除操作异常消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==GetGroup combox
        /// <summary>
        /// 查询所属区
        /// </summary>
        [HttpPost]
        public void GetGroup(int Type)
        {
           List<ModSysGroup> jsonlist=new BllSysGroup().QueryToAll().Where(p=>p.Type==Type).ToList();

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

        #region ===班级选择树 GetTreeByClass
        [HttpPost]
        public void GetTreeByClass()
        {
            string where = " Type=1 and Status=" + (int) StatusEnum.正常;
            string name = (Request["name"] == null ? "" : Request["name"]); //查询过滤条件
            if (name != "")
            {
                where += " and Name like '%" + name + "%'";
            }

            DataTable mytab = new BllSysGroup().GetTreeList(where).Tables[0]; //获取所有树
            GetTreeNode(mytab);
            Output = Output.Replace("check", "checked");
            WriteJsonToPage(Output);
        }

        /// <summary>
        /// 查询父节点
        /// </summary>
        public void GetTreeNode(DataTable dt)
        {
            string selectUserkey = (Request["select"] == null ? "" : Request["select"]);//选中的人员编号
            List<Hashtable> hashList = new List<Hashtable>();

            foreach (DataRow myrow in dt.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", myrow["id"].ToString());
                ht.Add("text", myrow["text"].ToString());
                ht.Add("expanded", true);
                ht.Add("leaf", true);
                if (selectUserkey.Contains(myrow["id"].ToString()))
                {
                    ht.Add("check", true);
                }
                else
                {
                    ht.Add("check", false);
                }
                hashList.Add(ht);
            }
            Output = new JavaScriptSerializer().Serialize(hashList);
        }
        #endregion

        #region ===获取类型 下拉树 GetSysGroup
        /// <summary>
        /// 获取物业
        /// </summary>
        [HttpPost]
        public void GetSysGroup()
        {
            string CompanyId = Request["CompanyId"].ToString().Trim();//
            if (string.IsNullOrEmpty(CompanyId)|| CompanyId=="0")
            {
                CompanyId=CurrentMaster.Cid;
            }
            List<ModSysGroup> list = new BllSysGroup().QueryToAll().Where(p => p.CompanyId == CompanyId && p.Status == (int)StatusEnum.正常).ToList();
            WriteJsonToPage(JsonHelper.ToJson(list));
        }
        #endregion


        #region ==保存选择 SaveSelect
        /// <summary>
        /// 保存选择
        /// </summary>
        public void SaveSelect()
        {
            var msg = new ModJsonResult();
            try
            {
                string checkid = Request["checkid"].ToString();//获取选中的Id
                string CId = Request["CId"].ToString().Trim()==""?CurrentMaster.Cid:Request["CId"].ToString();//公司编号
                
                StringBuilder sb = new StringBuilder();
                sb.Append("delete from Sys_Group where ParentId !='0';");
                sb.AppendLine();
                sb.Append("insert into Sys_Group(Id,Name,ParentId,CompanyId,CreaterId) select newId() as Id,Name,Id  as ParentId,'" + CId + "' as CompanyId,'" + CurrentMaster.Id + "' as CreaterId from Sys_Dirc where Id in(" + checkid + ")");
                int result = new BllSysGroup().ExecuteNonQueryByText(sb.ToString());
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败.";
                }
                LogInsert(OperationTypeEnum.操作, "设备分类引用操作", "删除操作成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备分类引用操作", "删除操作异常消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion
    }
}
