using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using System.Data;

namespace WebPortalAdmin.Controllers
{
    public class ChannelManagerController : BaseController<ModSysQRCode>
    {
        /// <summary>
        /// 渠道图片管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ChannelManager()
        {
            return View();
        }

        #region ==根据加载数据 SearchData()

        /// <summary>
        /// 根据id 加载数据
        /// </summary>
        public JsonResult SearchData()
        {
            var search = base.GetSearch();
            if (!string.IsNullOrEmpty(Request["BrandName"]))
            {
                search.AddCondition(" and (Name like '%" + Request["BrandName"].ToString() + "%' or Jianpin like '%" + Request["BrandName"].ToString() + "%')");
            }
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition(" and CreaterId='" + CurrentMaster.Id + "'");
                }
            }
            IList<ModSysQRCode> list = new BllSysQRCode().GetSysIdList(CurrentMaster.Cid, search.GetConditon());
            var json = new
            {
                rows = list
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysQRCode t)
        {
            BllSysQRCode bll = new BllSysQRCode();
            ModJsonResult json = new ModJsonResult();

            t.Status = (int)StatusEnum.正常;

            //获取文件
            HttpPostedFileBase postedFile = Request.Files["ImageUrl"];
            string filename = postedFile.FileName;//获取上传的文件路径
            string configPath = System.Configuration.ConfigurationManager.AppSettings["ChannelManager"];

            string str = "";
            string path = "";//文件路径
            bool succ = true;
            if (!String.IsNullOrEmpty(filename) || (!string.IsNullOrEmpty(Request["modify"]) && !string.IsNullOrEmpty(Request["isUpLoad"])))
            {
                if (new PicFileUpLoad().UpLoad("Advertise", postedFile, configPath, filename, out path, out str))
                {
                    t.Img = path;
                }
                else
                {
                    succ = false;
                }
            }
            if (succ)
            {
                if (!string.IsNullOrEmpty(Request["modify"])) //修改
                {
                    var Category = bll.LoadData(t.Id);
                    if (!String.IsNullOrEmpty(t.Img))
                    {
                        Category.Img= t.Img;
                    }
                    Category.Name = t.Name;
                    int result = bll.EditData(Category);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "修改失败,请稍后再操作!";
                    }
                }
                else
                {
                    t.Id = Guid.NewGuid().ToString();
                    t.SysId = CurrentMaster.Cid;
                    t.CreateTime = DateTime.Now;//创建时间
                    t.CreaterId = CurrentMaster.Id;//创建人编号

                    int result = bll.InsertData(t);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = " 保存失败,请稍后再操作!";
                    }
                }
            }
            else
            {
                json.success = false;
                json.msg = "请上传正确的图片";
            }

            WriteJsonToPage(json.ToString());
        }

        #endregion

        #region ==加载数据 LoadData
        /// <summary>
        /// 删除
        /// </summary>
        public void LoadData()
        {
            string id = Request["id"].ToString();
            var msg = new ModJsonResult();
            var model = new BllSysQRCode().LoadData(id);
            WriteJsonToPage(new JavaScriptSerializer().Serialize(model));
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
                //判断该品牌是否还在使用
                BllSysQRCode Bll = new BllSysQRCode();
                DataSet ds = Bll.CanDelete(id);
                int can = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    can = int.Parse(ds.Tables[0].Rows[0]["DocumentCount"].ToString());
                    can += int.Parse(ds.Tables[0].Rows[0]["ElevatorCount"].ToString());
                    can += int.Parse(ds.Tables[0].Rows[0]["CompanyCount"].ToString());
                }
                if (can > 0)
                {
                    msg.success = false;
                    msg.msg = "该品牌正在使用,不能删除";
                }
                else
                {
                    int result = new BllSysQRCode().DeleteStatus(id);
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

        #region 查询型号列表 SearchModelData

        /// <summary>
        /// 查询型号列表
        /// </summary>
        public void SearchModelData()
        {
            Search search = this.GetSearch();
            if (!string.IsNullOrEmpty(Request["key"]))
            {
                search.AddCondition("LinkId='"+Request["key"].ToString()+"'");
            }
            WriteJsonToPage(new BllSysModel().SearchData(search));
        }
        #endregion

        #region 保存表单 SaveModelData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveModelData(ModSysModel mod)
        {
            BllSysModel Bll = new BllSysModel();
            ModJsonResult json = new ModJsonResult();

            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                ModSysModel modLod = Bll.LoadData(mod.Id);
                modLod.ModelName = mod.ModelName;
                modLod.Sort = mod.Sort;
                modLod.Remark = mod.Remark;
                int result = Bll.Update(modLod);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
            }
            else {
                mod.CreateTime = DateTime.Now;
                mod.CompanyId = CurrentMaster.Cid;
                mod.Id = Guid.NewGuid().ToString();
                mod.Status = (int)StatusEnum.正常;
                int result = Bll.Insert(mod);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "新增失败,请稍后再操作!";
                }
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region ==删除 DeleteModelData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteModelData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                BllSysModel Bll = new BllSysModel();
                if (Bll.DeleteStatus(id) > 0)
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
