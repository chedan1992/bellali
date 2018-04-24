using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using System.Data;
using WebPortalAdmin.Code;
using System.Web.Script.Serialization;
using System.Text;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 资讯文章
    /// </summary>
    public class EDynamicController : BaseController<ModEDynamic>
    {

        BllEDynamic Bll = new BllEDynamic();
        ModJsonResult msg = new ModJsonResult();

        /// <summary>
        /// 文章
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 文档浏览
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentView()
        {
            string key = "";
            if (!String.IsNullOrEmpty(Request["Id"]))
            {
                key = Request["Id"].ToString();
            }
            var model = Bll.GetModelByWhere(" and Id='" + key + "'");

            ViewData["Name"] = model.Name;
            ViewData["ReadNum"] = model.ReadNum;
            ViewData["Author"] = model.Author;
            ViewData["CreateTime"] = Convert.ToDateTime(model.CreateTime).ToString("yyyy-MM-dd HH:mm");
            ViewData["Content"] = model.Content;
            ViewData["Img"] = model.Img;
            return View();
        }



        /// <summary>
        /// 编辑副本2
        /// </summary>
        /// <returns></returns>
        public ActionResult AddEdit()
        {
            ViewData["GroupId"] = "";
            ViewData["Id"] = "";//编辑选中记录ID
            if (!String.IsNullOrEmpty(Request["Id"]))
                ViewData["Id"] = Request["Id"];

            if (!String.IsNullOrEmpty(Request["GroupId"]))
                ViewData["GroupId"] = Request["GroupId"];

            ViewData["LookView"] = "";
            if (!String.IsNullOrEmpty(Request["LookView"]))
                ViewData["LookView"] = Request["LookView"];

            return View();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            ViewData["Id"] = "";//编辑选中记录ID
            if (!String.IsNullOrEmpty(Request["Id"]))
                ViewData["Id"] = Request["Id"];
            ViewData["LookView"] = "";
            if (!String.IsNullOrEmpty(Request["LookView"]))
                ViewData["LookView"] = Request["LookView"];

            return View();
        }

        /// <summary>
        ///获取商品类别新闻分类
        /// </summary>
        /// <param name="childId"></param>
        [HttpPost]
        public void GetTree(string childId)
        {
            string output = "";
            string _where = "1=1";
            childId = (childId == "-1" ? "0" : childId);
            //获取
            try
            {
                _where += " and Status=" + (int)StatusEnum.正常;
                DataSet ds = new BllSysDirc().GetTreeList(_where);
                output = JsonHelper.ToJson(new FunTreeCommon().GetCategoryNodes(ds));
            }
            catch
            {
                var json = new ModJsonResult();
                json.success = false;
                json.errorCode = (int)SystemError.正常错误;
                json.msg = "菜单树异常,无法进行操作";
                output = JsonHelper.ToJson(json);
            }
            Response.Write(output);
            Response.End();
        }

        /// <summary>
        /// 树形分类过滤条件查询
        /// </summary>
        [HttpPost]
        public void SearchProvinceByTree()
        {
            string name = (Request["name"] == null ? "" : Request["name"]); //查询过滤条件
            string where = "Status=" + (int)StatusEnum.正常 + " and Type=" + Request["Type"];
            if (!string.IsNullOrEmpty(name.Trim()))
            {
                where += "(Name like '%" + name.Trim() + "%')";
            }
            DataTable mytab = new BllSysDirc().GetTreeList(where).Tables[0]; //获取所有树

            List<jsonSysBusinessCircle> list = new List<jsonSysBusinessCircle>();
            if (mytab != null)
            {
                for (int b = 0; b < mytab.Rows.Count; b++)
                {
                    jsonSysBusinessCircle model = new jsonSysBusinessCircle();
                    model.id = mytab.Rows[b]["id"].ToString();
                    model.text = mytab.Rows[b]["text"].ToString();
                    model.Code = "";
                    model.expanded = true;
                    model.leaf = true;
                    model.children = null;
                    list.Add(model);
                }
            }

            list.Insert(0, new jsonSysBusinessCircle()
            {
                id = "0",
                text = "查询全部",
                expanded = true,
                leaf = true,
                children = null,
            });
            WriteJsonToPage(new JavaScriptSerializer().Serialize(list));
        }


        #region==页面列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            string GroupId = Request["GroupId"].ToString();
            if (GroupId != "-1")
            {
                search.AddCondition("GroupId='" + GroupId + "'");
            }
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");//自己查看自己的
                }
            }
            search.SortField = "IsTop desc,Sort ASC,CreateTime desc";//默认排序方式
            var jsonResult = Bll.SearchData(search);
            WriteJsonToPage(jsonResult);
        }
        #endregion

        #region ==禁用状态 EnableUse
        /// <summary>
        /// 禁用状态
        /// </summary>
        public void EnableUse()
        {
            var msg = new ModJsonResult();
            string key = Request["id"];
            int result = Bll.UpdateStatus(1, key);
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
            string key = Request["id"];
            int result = Bll.UpdateStatus(0, key);
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

        //#region ==保存表单 SaveData
        ///// <summary>
        ///// 保存表单
        ///// </summary>
        ///// <param name="t"></param>
        //[HttpPost]
        //[ValidateInput(false)]
        //public void SaveData(ModEDynamic t)
        //{
        //    //获取文件

        //    t.CreaterId = CurrentMaster.Id;//添加人
        //    t.Content = t.Content == null ? "" : t.Content;//内容

        //    string newMark="";
        //    string StripHtml = new Html().StripHtml(t.Content);
        //    if (StripHtml.Length > 40)
        //    {
        //        newMark = StripHtml.Substring(0,40);
        //    }
        //    else
        //    {
        //        newMark = StripHtml;
        //    }
        //    string str = "";
        //    bool succ = true;

        //    #region ===保存修改数据
        //    if (succ)//判断广告图是否上传正确
        //    {
        //        if (!string.IsNullOrEmpty(Request["modify"])) //修改
        //        {
        //            var Category = Bll.LoadData(t.Id);
        //            if (String.IsNullOrEmpty(t.Img))
        //            {
        //                t.Img = Category.Img;//图片
        //            }
        //            t.CreateTime = Category.CreateTime;
        //            t.Status = Category.Status;//数据状态
        //            t.SysId = Category.SysId;
        //            t.Mark = newMark;
        //            int result = Bll.Update(t);
        //            if (result <= 0)
        //            {
        //                msg.success = false;
        //                msg.msg = "修改失败,请稍后再操作!";
        //            }
        //            else
        //            {
        //                msg.msg = "/EDynamic/SaveData?Id='" + t.Id + "'&modify=1";
        //            }
        //        }
        //        else
        //        {
        //            t.Id = Guid.NewGuid().ToString();
        //            t.Status = (int)StatusEnum.正常;//状态
        //            t.CreateTime = DateTime.Now;
        //            t.SysId = CurrentMaster.Cid;
        //            if (t.Img == null)
        //            {
        //                t.Img = "/Resource/img/null.jpg";
        //            }
        //            t.Mark = newMark;
        //            int result = Bll.Insert(t);
        //            if (result <= 0)
        //            {
        //                msg.success = false;
        //                msg.msg = " 保存失败,请稍后再操作!";
        //            }
        //            else
        //            {
        //                msg.msg = "/EDynamic/SaveData?ID='" + t.Id + "'&modify=1";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        msg.success = false;
        //        msg.msg = str;
        //    }
        //    #endregion

        //    WriteJsonToPage(msg.ToString());
        //}

        //#region ===删除添加的广告,以便同步数据 Delete
        ///// <summary>
        ///// 删除添加的广告,以便同步数据
        ///// </summary>
        ///// <param name="t"></param>
        //public void Delete(ModAdActive t)
        //{
        //    //删除数据
        //    new BllAdActive().Delete(t.Id);
        //    //删除图片
        //    if (!string.IsNullOrEmpty(t.Img))
        //    {
        //        string picPath = Server.MapPath("~" + t.Img.Replace("\\", "/"));//获取服务端文件的绝对路径，同时注意拼接成相对与网站根目录的字符串。     
        //        if (System.IO.File.Exists(picPath))
        //        {
        //            try
        //            {
        //                System.IO.File.Delete(picPath);
        //            }
        //            catch
        //            {
        //                //错误处理：         
        //            }
        //        }

        //    }
        //}

        //#endregion

        //#endregion

        #region ==删除 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                int result = Bll.DeleteStatus(id);
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

        #region ==编辑加载数据 LoadData()
        /// <summary>
        /// 根据id 加载数据
        /// </summary>
        public void LoadData()
        {
            string Id = Request["Id"].ToString();
            var list = Bll.GetModelByWhere(" and Id='" + Id + "'");
            WriteJsonToPage(JsonHelper.ToJson(list));
        }
        #endregion

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModEDynamic t)
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                t.CreaterId = CurrentMaster.Id;//添加人
                t.Content = t.Content == null ? "" : t.Content;//广告内容
                t.ActionType = int.Parse(Request["ActionType"]);
                t.ShowType = int.Parse(Request["ShowType"]);
                t.SysId = CurrentMaster.Cid;

                if (t.ActionType == (int)NewsTypeEnum.内部新闻 || t.ActionType == (int)NewsTypeEnum.外部新闻)
                {
                    if (t.ActionType == (int)NewsTypeEnum.内部新闻)//外部广告
                    {
                        t.Content = Request["Content"];
                    }
                    else if (t.ActionType == (int)NewsTypeEnum.外部新闻)//外部广告
                    {
                        t.Content = Request["Link"];
                    }
                }
                if (t.ShowType == 2)//自动下架
                {
                    t.ActiveStartTime = Convert.ToDateTime(Request["ActiveStartTime"]);//开始时间
                    t.ActiveEndTime = Convert.ToDateTime(Request["ActiveEndTime"]);//结束时间
                }
                #region ===保存修改数据

                if (!string.IsNullOrEmpty(Request["modify"])) //修改
                {
                    var Category = Bll.LoadData(t.Id);
                    t.CreateTime = Category.CreateTime;
                    t.Status = Category.Status;//数据状态
                    if (t.ActiveEndTime > DateTime.Now && t.Status != (int)StatusEnum.禁用)
                    {
                        t.Status = (int)StatusEnum.正常;
                    }
                    int result = Bll.Update(t);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "修改失败,请稍后再操作!";
                    }
                    else
                    {
                        //保存图片集合内容
                        SavaCombin(t, ref json, "modify");
                        LogInsert(OperationTypeEnum.操作, "新闻修改操作", "新闻修改操作成功");
                        json.msg = "/EDynamic/SaveData?Id='" + t.Id + "'&modify=1";
                    }
                }
                else
                {
                    t.Id = Guid.NewGuid().ToString();
                    t.Status = (int)StatusEnum.正常;//状态
                    t.CreateTime = DateTime.Now;
                    if (t.Img == null)
                    {
                        t.Img = "/Resource/img/null.jpg";
                    }
                    int result = Bll.Insert(t);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = " 保存失败,请稍后再操作!";
                    }
                    else
                    {
                        //保存图片集合内容
                        SavaCombin(t, ref json, "Add");
                        json.msg = "/EDynamic/SaveData?ID='" + t.Id + "'&modify=1";
                    }
                    LogInsert(OperationTypeEnum.操作, "新闻保存操作", "新闻保存操作成功");
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "新闻保存/修改操作", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
        }
        #region  ===组合广告内容详细信息类  AdTest
        /// <summary>
        /// 序列化组合广告内容数组
        /// </summary>
        [Serializable]
        public class AdTest
        {
            public string picValue { get; set; }//标记修改时直接获取图片名称
            public int Sort { get; set; }
            public int Modify { get; set; }// 1:新增 0:修改 -1:删除
            public string Key { get; set; }//主键
        }
        #endregion


        #region ===保存组合广告方法 SavaCombin

        /// <summary>
        /// 保存组合广告方法
        /// </summary>
        /// <param name="t">实体类</param>
        /// <param name="result">操作后返回结果</param>
        /// <param name="type">操作类型modify:修改 Add:新增</param>
        public void SavaCombin(ModEDynamic t, ref ModJsonResult result, string type)
        {
            string AdModel = Request["AdModel"];

            List<AdTest> _Test = new JavaScriptSerializer().Deserialize<List<AdTest>>(AdModel);
            string configPath = System.Configuration.ConfigurationManager.AppSettings["NewsPath"];//图片保存路径
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _Test.Count; i++)//循环模块数
            {
                bool succ = true;
                string str = "";//图片上传失败原因
                string path = "";//文件路径

                if (_Test[i].Modify == 1 || _Test[i].Modify == 0)
                {
                    //新增图片
                    if (_Test[i].Key == "" && (_Test[i].Modify == 1 || _Test[i].Modify == 0))
                    {
                        HttpPostedFileBase ModelFile = Request.Files["winImg" + _Test[i].Sort];
                        string filename = ModelFile.FileName;//获取上传的文件名称
                        #region ===上传广告图
                        if (!String.IsNullOrEmpty(filename))//判断是否修改图片
                        {
                            if (new PicFileUpLoad().UpLoad("News", ModelFile, configPath, filename, out path, out str))
                            {
                                _Test[i].picValue = path;
                            }
                            else
                            {
                                succ = false;
                            }
                        }
                        #endregion

                        if (succ == true)
                        {
                            sb.Append("insert into Sys_ImgPic(Id,OtherKeyId,PicName,PicUrl,Sort,ImgLength,PicType,CreateTime,CreaterId) values(");
                            sb.Append(" newid(),");
                            sb.Append(" '" + t.Id + "',");
                            sb.Append(" '',");
                            sb.Append(" '" + path + "',");
                            sb.Append(" '" + _Test[i].Sort + "',");
                            sb.Append(" '',");
                            sb.Append(" '',");
                            sb.Append(" getdate(),");
                            sb.Append(" '" + CurrentMaster.CreaterId + "'");
                            sb.Append(");");
                            sb.AppendLine();
                        }
                    }
                    else
                    {
                        if (_Test[i].Modify == 0)
                        {
                            HttpPostedFileBase ModelFile = Request.Files["winImg" + _Test[i].Sort];
                            string filename = ModelFile.FileName;//获取上传的文件名称
                            #region ===上传广告图
                            if (!String.IsNullOrEmpty(filename))//判断是否修改图片
                            {
                                if (new PicFileUpLoad().UpLoad("News", ModelFile, configPath, filename, out path, out str))
                                {
                                    _Test[i].picValue = path;
                                }
                                else
                                {
                                    succ = false;
                                }
                            }
                            #endregion

                            if (succ == true)
                            {
                                sb.Append("update Sys_ImgPic set PicUrl='" + _Test[i].picValue + "' where Id='" + _Test[i].Key + "';");
                            }
                        }
                    }
                }
                else
                {
                    sb.Append("delete from Sys_ImgPic where Id='" + _Test[i].Key + "';");
                }
            }
            if (sb.Length > 0)
            {
                int resultNum = Bll.ExecuteNonQueryByText(sb.ToString());
                if (resultNum > 0)
                {
                    result.success = true;
                    result.msg = "操作成功!";
                }
                else
                {
                    //删除添加的广告,以便同步数据
                    if (type == "Add")
                    {
                        Bll.Delete(t.Id);
                    }
                    result.success = false;
                    result.msg = "组合内容添加失败!";
                }
            }
            else
            {
                result.success = true;
            }
        }

        #endregion

        #endregion

        #region ===图片上传 FileUpload
        /// <summary>
        /// 图片上传
        /// </summary>
        public void FileUpload()
        {
            ModJsonResult json = new ModJsonResult();
            string str = "";//图片上传失败原因
            string path = "";//文件路径
            try
            {
                string id = Request["id"].ToString();
                HttpPostedFileBase ModelFile = Request.Files["winImg" + id];
                string filename = ModelFile.FileName;//获取上传的文件名称
                string configPath = System.Configuration.ConfigurationManager.AppSettings["NewsPath"];//新闻图片保存路径
                if (!string.IsNullOrEmpty(filename))
                {
                    if (new PicFileUpLoad().UpLoad("News", ModelFile, configPath, filename, out path, out str))
                    {
                        //获取图片宽度和高度
                        int width = 0; int height = 0;
                        UploadFile.GetWidthHeight(out width, out height, Server.MapPath(path));
                        json.success = true;
                        json.msg = path;
                        json.data = "[{width:" + width + ",height:" + height + "}]";
                    }
                    else
                    {
                        json.success = false;
                        json.msg = str;
                    }
                }
            }
            catch (Exception a)
            {
                json.success = false;
                json.msg = a.Message;
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion


        #region ==排序 Sort
        /// <summary>
        /// 删除
        /// </summary>
        public void Sort(string id, int num)
        {
            var msg = new ModJsonResult();
            try
            {
                var r = Bll.LoadData(id);
                if (r != null)
                {
                    if (num == 2)
                    {
                        r.IsTop = !r.IsTop;
                    }
                    else
                    {
                        r.Sort += num;
                    }
                    if (Bll.Update(r) > 0)
                    {
                        msg.success = true;
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "操作失败";
                    }
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
