using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspx.Business;
using Aspx.Model;
using Aspx.Common;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using InterFaceWeb.Common;
using Dapper;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.Text;

namespace InterFaceWeb.Controllers
{
    public class AppointController : BaseController
    {

        //查询设备（扫描）
        //[HttpGet]
        public JsonResult GetAppointDetailQRCode(string QRCode, string Cid, string Id, bool isInput = false)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：QRCode={0}&Cid={1}&Id={2}&isInput={3}", QRCode, Cid, Id, isInput);
                CheckParams(QRCode, "无效二维码或简码！");
                CheckParams(Cid, "单位Cid参数不能为空！");
                CheckParams(Id, "单位用户Id参数不能为空！");

                if (isInput && QRCode.Length > 50)
                {
                    CheckParams("", "简码超长,请限制50字以内！");
                }

                if (jsonResult.success)
                {
                    ModFireBox tempbox = null;

                    BllSysAppointed bllSysAppointed = new BllSysAppointed();
                    BllFireBox bllFireBox = new BllFireBox();
                    if (!isInput && QRCode.LastIndexOf("X") == QRCode.Length - 1)//箱子二维码
                    {
                        ModFireBox temp = bllFireBox.GetFireBoxDetailQRCode(QRCode);
                        tempbox = temp;
                        ScanBox(Id, temp, true);
                        md(QRCode, Cid);
                    }
                    else if (!isInput && QRCode.LastIndexOf("S") == QRCode.Length - 1)//设备二维码 
                    {
                        ModSysAppointed temp = bllSysAppointed.GetAppointDetailQRCode(QRCode);
                        ScanAppointed(Id, temp, true);
                        md(QRCode, Cid);
                    }
                    else if (isInput)//输入查询处理
                    {
                        ModFireBox temp = bllFireBox.GetFireBoxDetailQRCode(QRCode);
                        tempbox = temp;
                        ScanBox(Id, temp, false);
                        if (!jsonResult.success)
                        {
                            ModSysAppointed temp1 = bllSysAppointed.GetAppointDetailQRCode(QRCode);
                            ScanAppointed(Id, temp1, false);
                            md(QRCode, Cid);
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "没有查询到箱子信息！";
                        jsonResult.errorCode = 2;
                    }
                    if (tempbox != null)
                        jsonResult.otherMsg = new { Id = tempbox.Id, Name = tempbox.Name, Address = tempbox.Address, QrCode = "XF" + tempbox.QrCode + "X", EquipmentCount = tempbox.EquipmentCount };
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //判断二维码
        private void md(string QRCode, string Cid)
        {
            if (jsonResult.errorCode == 1)
            {
                //判断二维码是否失效
                BllSysQRCode bllSysQRCode = new BllSysQRCode();
                ModSysQRCode modSysQRCode = bllSysQRCode.GetQRCodeOrName(QRCode);
                if (modSysQRCode != null && modSysQRCode.SysId != Cid)
                {
                    jsonResult.msg = "没有查询到设备信息！";
                    jsonResult.errorCode = 2;
                    jsonResult.success = false;
                }
            }
        }

        //扫码设备处理
        private void ScanAppointed(string Id, ModSysAppointed temp, bool isAdd = false)
        {
            if (temp == null || (temp != null && temp.Status == (int)StatusEnum.删除))
            {
                BllSysMaster bllSysMaster = new BllSysMaster();
                var master = bllSysMaster.LoadData(Id);
                if (master != null && (master.Attribute == (int)AdminTypeEnum.单位用户 || master.Attribute == (int)AdminTypeEnum.单位管理员))
                {
                    if (isAdd)
                    {
                        jsonResult.msg = "请添加设备信息！";
                        jsonResult.errorCode = 1;
                    }
                    else
                    {
                        jsonResult.msg = "没有查询到设备信息！";
                        jsonResult.errorCode = 2;
                    }
                    jsonResult.success = false;
                }
                else
                {
                    jsonResult.success = false;
                    jsonResult.msg = "没有查询到设备信息！";
                    jsonResult.errorCode = 2;
                }
            }
            else if (temp != null && temp.Status == (int)StatusEnum.禁用)
            {
                jsonResult.success = false;
                jsonResult.msg = "设备被禁用！";
                jsonResult.errorCode = 3;
            }
            else
            {
                var r = new List<object>();
                temp.QRCode = "XF" + temp.QRCode + "S";
                r.Add(temp);
                jsonResult.data = r;
                jsonResult.msg = "获取成功！";
                jsonResult.success = true;
                jsonResult.errorCode = 0;
            }
        }

        //扫码设备处理
        private void ScanBox(string Id, ModFireBox temp, bool isAdd = false)
        {
            if (temp == null || (temp != null && temp.Status == (int)StatusEnum.删除))
            {
                BllSysMaster bllSysMaster = new BllSysMaster();
                var master = bllSysMaster.LoadData(Id);
                if (master != null && (master.Attribute == (int)AdminTypeEnum.单位用户 || master.Attribute == (int)AdminTypeEnum.单位管理员))
                {
                    if (isAdd)
                    {
                        jsonResult.msg = "请添加箱子信息！";
                        jsonResult.errorCode = 1;
                    }
                    else
                    {
                        jsonResult.msg = "没有查询到箱子信息！";
                        jsonResult.errorCode = 2;
                    }
                    jsonResult.success = false;
                }
                else
                {
                    jsonResult.success = false;
                    jsonResult.msg = "没有查询到箱子信息！";
                    jsonResult.errorCode = 2;
                }
            }
            else if (temp != null && temp.Status == (int)StatusEnum.禁用)
            {
                jsonResult.success = false;
                jsonResult.msg = "请添加箱子信息！";
                jsonResult.errorCode = 1;
            }
            else
            {

                BllSysAppointed bllSysAppointed = new BllSysAppointed();
                List<ModSysAppointed> r = bllSysAppointed.GetByPlacesList(temp.Id);
                foreach (var item in r)
                {
                    item.QRCode = "XF" + item.QRCode + "S";
                }
                jsonResult.data = r;
                jsonResult.msg = "获取成功！";
            }
        }


        #region ===二维码生成 QrCode
        /// <summary>
        /// 二维码生成
        /// </summary>
        public string QrCode(string PrimaryKey)
        {
            try
            {
                string Name = "";//二维码编号
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                int scale = Convert.ToInt16(4);
                qrCodeEncoder.QRCodeScale = scale;
                qrCodeEncoder.QRCodeVersion = 5;
                qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;

                System.Drawing.Image myimg = qrCodeEncoder.Encode("XF" + PrimaryKey, System.Text.Encoding.UTF8);

                string rootQROrderPath = ConfigurationManager.AppSettings["QROrder"] ?? "~/UploadFile/QRCode/";
                string path = rootQROrderPath + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";

                if (!string.IsNullOrEmpty(Server.MapPath(path)))
                {
                    if (!System.IO.File.Exists(Server.MapPath(path)))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(path));
                    }
                }
                path = path + PrimaryKey + ".png";//组成图片名称
                using (Bitmap bitmap = new Bitmap(250, 250))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.FillRectangle(new SolidBrush(System.Drawing.Color.White), new Rectangle(0, 0, 250, 250));
                        g.DrawImage(myimg, new Rectangle(10, 10, 230, 230));
                        bitmap.Save(Server.MapPath(path), System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                path = path.Substring(1, path.Length - 1);
                return path;
            }
            catch (Exception ex)
            {
                LogErrorRecord.Debug("二维码生成错误" + ex.Message);
                return "";
            }
        }

        #endregion

        //查询设备id
        //[HttpGet]
        public JsonResult GetAppointDetailId(string id, string msgid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：id={0}&msgid={1}", id, msgid);
                CheckParams(id, "设备id无效！");

                if (jsonResult.success)
                {
                    BllSysAppointed bllSysAppointed = new BllSysAppointed();
                    var Appoint = bllSysAppointed.GetAppointDetailId(id);
                    if (Appoint != null)
                    {
                        Appoint.QRCode = "XF" + Appoint.QRCode + "S";
                        jsonResult.data = Appoint;
                        if (!string.IsNullOrEmpty(msgid))
                        { //修改消息读状态
                            BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                            bllSysPushMessage.UpdateMsgId(msgid);
                        }
                        jsonResult.msg = "获取成功！";
                    }
                    else
                    {
                        jsonResult.msg = "没有查询到设备信息！";
                        jsonResult.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //查询箱子列表
        //[HttpGet]
        public JsonResult GetFireBoxList(string userid, string cid, string search, string PageIndex, string PageSize)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：cid={0}&userid={1}&search={2}&PageIndex={3}&PageSize={4}", cid, userid, search, PageIndex, PageSize);
                CheckParams(cid, "单位id无效！");
                //CheckParams(userid, "用户id无效！");

                if (jsonResult.success)
                {
                    Search searchtemp = GetSearch();
                    if (!string.IsNullOrEmpty(search))
                    {
                        searchtemp.AddCondition("(Name like '%" + search + "%' or Address like '%" + search + "%' or QrCode like '%" + search + "%')");
                    }
                    searchtemp.AddCondition("Status=1");
                    searchtemp.AddCondition("SysId='" + cid + "'");
                    BllFireBox bllFireBox = new BllFireBox();
                    Page<ModFireBox> r = bllFireBox.GetFireBoxList(searchtemp);
                    List<object> temp = new List<object>();
                    foreach (var item in r.Items)
                    {
                        temp.Add(new { Id = item.Id, Name = item.Name, Address = item.Address, QrCode = "XF" + item.QrCode + "X", EquipmentCount = item.EquipmentCount });
                    }
                    jsonResult.data = temp;
                    jsonResult.msg = "获取成功！";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.msg = "常异:" + ex.Message;
                jsonResult.errorCode = -1;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //添加箱子
        //[HttpGet]
        public JsonResult AddFireBox(ModFireBox a)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);

                LogErrorRecord.InfoFormat("参数参数：Name={0}&Address={1}&SysId={2}&CreaterId={3}&QrCode={4}", a.Name, a.Address, a.SysId, a.CreaterId, a.QrCode);

                //CheckParams(a.QrCode, "无效二维码！");
                CheckParams(a.Name, "箱子名称不能为空！");
                CheckParams(a.Address, "箱子位置不能为空！");

                if (string.IsNullOrEmpty(a.QrCode))
                {
                    a.QrCode = Guid.NewGuid().ToString();
                }
                else if (jsonResult.success && a.QrCode.LastIndexOf("S") == a.QrCode.Length - 1)//设备二维码 
                {
                    CheckParams("", "此二维码是设备二维码！");
                }

                BllFireBox bllFireBox = new BllFireBox();
                var r = bllFireBox.GetFireBoxDetailQRCode(a.QrCode);


                //验证单位id
                BllSysCompany bllSysCompany = new BllSysCompany();
                var company = bllSysCompany.LoadData(a.SysId);
                if (jsonResult.success && company != null && company.Attribute != (int)CompanyType.单位)
                {
                    CheckParams("", "对不起您,没有添加箱子权限！");
                }
                else if (jsonResult.success && (company == null || company != null && company.Status != 1))
                {
                    CheckParams("", "该社会单位已经注销或禁用！");
                }

                if (jsonResult.success)
                {
                    LogErrorRecord.InfoFormat("参数参数：Status={0}", a.Status);
                    if (r != null && r.Status != (int)StatusEnum.删除)
                    {
                        r.Name = a.Name;
                        r.Address = a.Address;
                        r.Status = 1;
                        if (bllFireBox.Update(r) > 0)
                        {
                            jsonResult.msg = "新增箱子成功，快去添加设备吧！";
                            jsonResult.data = new { Id = r.Id, Name = r.Name, Address = r.Address, QrCode = "XF" + r.QrCode + "X", EquipmentCount = r.EquipmentCount };
                        }
                        else
                        {
                            jsonResult.msg = "新增箱子失败！";
                            jsonResult.success = false;
                        }
                    }
                    else
                    {
                        a.CreateTime = DateTime.Now;
                        a.Status = (int)StatusEnum.正常;
                        a.Id = Guid.NewGuid().ToString();
                        a.EquipmentCount = 0;

                        a.Img = QrCode(a.QrCode + "X");

                        if (bllFireBox.Insert(a) > 0)
                        {
                            jsonResult.msg = "新增箱子成功，快去添加设备吧！";
                            jsonResult.data = new { Id = a.Id, Name = a.Name, Address = a.Address, QrCode = "XF" + a.QrCode + "X", EquipmentCount = a.EquipmentCount };
                        }
                        else
                        {
                            jsonResult.msg = "新增箱子失败！";
                            jsonResult.success = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.msg = "常异:" + ex.Message;
                jsonResult.errorCode = -1;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }


        //修改箱子
        //[HttpGet]
        public JsonResult UpFireBox(ModFireBox a)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                CheckParams(a.Id, "箱子id不能为空！");
                CheckParams(a.Name, "箱子名称不能为空！");
                CheckParams(a.Address, "箱子位置不能为空！");

                if (string.IsNullOrEmpty(a.QrCode))
                {
                    a.QrCode = Guid.NewGuid().ToString();
                }

                BllFireBox bllFireBox = new BllFireBox();
                var r = bllFireBox.LoadData(a.Id);


                //验证单位id
                BllSysCompany bllSysCompany = new BllSysCompany();
                var company = bllSysCompany.LoadData(r.SysId);
                if (jsonResult.success && company != null && company.Attribute != (int)CompanyType.单位)
                {
                    CheckParams("", "对不起您,没有修改箱子权限！");
                }
                else if (jsonResult.success && (company == null || company != null && company.Status != 1))
                {
                    CheckParams("", "该社会单位已经注销或禁用！");
                }

                if (jsonResult.success)
                {
                    r.Name = a.Name;
                    r.Address = a.Address;

                    if (bllFireBox.Update(r) > 0)
                    {
                        jsonResult.msg = "修改箱子成功！";
                        jsonResult.data = a.Id;
                    }
                    else
                    {
                        jsonResult.msg = "修改箱子失败！";
                        jsonResult.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.msg = "常异:" + ex.Message;
                jsonResult.errorCode = -1;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //删除箱子
        //[HttpGet]
        public JsonResult DeFireBox(string fireBoxId, string userid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：fireBoxId={0}&userid={1}", fireBoxId, userid);
                CheckParams(fireBoxId, "箱子Id不能为空！");
                CheckParams(userid, "用户Id不能为空！");


                BllFireBox bllFireBox = new BllFireBox();
                var temp = bllFireBox.LoadData(fireBoxId);
                if (temp == null)
                {
                    CheckParams("", "箱子已删除！");
                }
                else if (temp != null && temp.Status == (int)StatusEnum.禁用)
                {
                    CheckParams("", "箱子已禁用！");
                }
                else if (temp != null && temp.Status == (int)StatusEnum.删除)
                {
                    CheckParams("", "箱子已删除！");
                }

                if (jsonResult.success)
                {
                    //验证单位id
                    BllSysCompany bllSysCompany = new BllSysCompany();
                    var company = bllSysCompany.LoadData(temp.SysId);
                    //if (jsonResult.success && userid != temp.Responsible)
                    //{
                    //    CheckParams("", "对不起您,没有删除设备权限！");
                    //}
                    //else 
                    if (jsonResult.success && (company == null || company != null && company.Status != 1))
                    {
                        CheckParams("", "该社会单位已经注销或禁用！");
                    }

                    if (jsonResult.success)
                    {
                        if (bllFireBox.DeleteStatus("'" + temp.Id + "'") > 0)
                        {
                            jsonResult.success = true;
                            jsonResult.msg = "删除成功！";
                        }
                        else
                        {
                            jsonResult.success = false;
                            jsonResult.msg = "删除失败！";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.msg = "常异:" + ex.Message;
                jsonResult.errorCode = -1;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //查询设备列表
        //[HttpGet]
        public JsonResult GetAppointList(string userid, string cid, string search, string PageIndex, string PageSize, int type = 0, int maintenanceStatus = -3)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：cid={0}&type={1}&userid={2}&search={3}&maintenanceStatus={4}&PageIndex={5}&PageSize={6}", cid, type, userid, search, maintenanceStatus, PageIndex, PageSize);
                CheckParams(cid, "单位id无效！");
                //CheckParams(userid, "用户id无效！");

                if (jsonResult.success)
                {
                    Search searchtemp = GetSearch();

                    if (!string.IsNullOrEmpty(search))//模糊查询
                    {
                        searchtemp.AddCondition("(Name like '%" + search + "%' or Model like '%" + search + "%' or GroupName like '%" + search + "%')");
                    }
                    if (maintenanceStatus >= -2) //设备状态
                    {
                        if (maintenanceStatus >= -1)
                        {
                            searchtemp.AddCondition("maintenanceStatus=" + maintenanceStatus);
                        }
                        else//超期未巡检
                        {
                            searchtemp.AddCondition("MaintenanceDate < GETDATE()");
                        }
                    }

                    if (!string.IsNullOrEmpty(userid))
                    {
                        if (userid.IndexOf(",") > 0)
                        {
                            userid = userid.Replace(",", "','");
                            searchtemp.AddCondition("ResponsibleId in ('" + userid + "')");//责任人id
                        }
                        else
                        {
                            if (type == (int)AdminTypeEnum.单位管理员)
                            {
                                var master = new BllSysMaster().LoadData(userid);
                                if (!(master != null && master.Attribute == (int)AdminTypeEnum.单位管理员))//单位员管理就查看所以设备（个人中心我的设备管理）
                                {
                                    searchtemp.AddCondition("ResponsibleId='" + userid + "'");//责任人id
                                }
                            }
                            else
                            {
                                searchtemp.AddCondition("ResponsibleId='" + userid + "'");//责任人id
                            }
                        }
                    }
                    switch (type)
                    {
                        case (int)AdminTypeEnum.单位管理员:
                            searchtemp.AddCondition("cid='" + cid + "'");
                            break;
                        case (int)AdminTypeEnum.单位用户:
                            searchtemp.AddCondition("cid='" + cid + "'");
                            break;
                        case (int)AdminTypeEnum.供应商管理员:
                        case (int)AdminTypeEnum.供应商用户:
                        case (int)AdminTypeEnum.维保公司管理员:
                        case (int)AdminTypeEnum.维保用户:
                        case (int)AdminTypeEnum.消防部门管理员:
                        case (int)AdminTypeEnum.消防用户:
                            BllSysCompanyCognate bllSysCompanyCognate = new BllSysCompanyCognate();
                            string sql = bllSysCompanyCognate.GetInCompanyResult(cid, type);
                            if (!string.IsNullOrEmpty(sql))
                            {
                                searchtemp.AddCondition("cid in (" + sql + ")");
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = "获取成功！";
                            }
                            break;
                        default:
                            jsonResult.success = false;
                            jsonResult.msg = "获取成功！";
                            break;
                    }
                    if (jsonResult.success)
                    {
                        BllSysAppointed bllSysAppointed = new BllSysAppointed();
                        Page<ModSysAppointed> r = bllSysAppointed.GetAppointList(searchtemp);
                        //效率问题???
                        foreach (var item in r.Items)
                        {
                            item.QRCode = "XF" + item.QRCode + "S";
                        }
                        jsonResult.data = r.Items;
                        jsonResult.msg = "获取成功！";
                    }
                    else
                    {
                        jsonResult.success = true;
                        jsonResult.data = new List<ModSysAppointed>();
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.msg = "常异:" + ex.Message;
                jsonResult.errorCode = -1;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //添加设备
        //[HttpGet]
        public JsonResult AddAppoint(ModSysAppointed a, int LostYear = 5)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                CheckParams(a.QRCode, "无效二维码！");

                if (jsonResult.success && a.QRCode.LastIndexOf("X") == a.QRCode.Length - 1)//设备二维码 
                {
                    CheckParams("", "此二维码是设备二维码！");
                }

                CheckParams(a.Name, "设备名称不能为空！");

                CheckParams(a.ResponsibleId, "设备责任人不能为空！");
                CheckParams(a.Gid, "设备类型不能为空！");
                //CheckParams(a.Places, "请选择箱子！");

                BllSysAppointed bllSysAppointed = new BllSysAppointed();
                var r = bllSysAppointed.GetAppointDetailQRCode(a.QRCode);

                if (jsonResult.success && r != null && r.Status != (int)StatusEnum.删除)
                {
                    CheckParams("", "设备信息已经录入！");
                }

                //判断二维码是否失效
                BllSysQRCode bllSysQRCode = new BllSysQRCode();
                ModSysQRCode modSysQRCode = bllSysQRCode.GetQRCodeOrName(a.QRCode);

                if (jsonResult.success && modSysQRCode != null && modSysQRCode.SysId != a.Cid)
                {
                    CheckParams("", "设备二维码不是本单位二维码！");
                }

                //验证单位id
                BllSysCompany bllSysCompany = new BllSysCompany();
                var company = bllSysCompany.LoadData(a.Cid);
                if (jsonResult.success && company != null && company.Attribute != (int)CompanyType.单位)
                {
                    CheckParams("", "对不起您,没有添加设备权限！");
                }
                else if (jsonResult.success && (company == null || company != null && company.Status != 1))
                {
                    CheckParams("", "该社会单位已经注销或禁用！");
                }

                if (jsonResult.success)
                {
                    a.CreateTime = DateTime.Now;
                    a.Status = (int)StatusEnum.正常;
                    a.Id = Guid.NewGuid().ToString();
                    a.LostTime = a.ProductionDate.Value.AddYears(LostYear);
                    a.MaintenanceDate =  Convert.ToDateTime(a.CreateTime.Value.AddDays(1 - DateTime.Now.Day).AddMonths(2).AddDays(-2).ToString("yyyy-MM-dd")); ;//下一个月的最后一天的前一天
                    a.MaintenanceDay = 0;

                    int result = 0;
                    if (r != null && r.Status == (int)StatusEnum.删除)
                    {
                        r.Id = a.Id;
                        r.Cid = a.Cid;
                        r.Cname = a.Cname;
                        r.CreaterId = a.CreaterId;
                        r.CreateTime = a.CreateTime;
                        r.Gid = a.Gid;
                        r.GroupName = a.GroupName;
                        r.LostTime = a.LostTime;
                        r.MaintenanceDate = a.MaintenanceDate;
                        r.MaintenanceDay = a.MaintenanceDay;
                        r.MaintenanceStatus = a.MaintenanceStatus;
                        r.Mark = a.Mark;
                        r.Model = a.Model;
                        r.Name = a.Name;
                        r.Places = a.Places;
                        r.PlacesCode = a.PlacesCode;
                        r.Placesed = a.Placesed;
                        r.PlacesName = a.PlacesName;
                        r.ProductionDate = a.ProductionDate;
                        r.ResponsibleId = a.ResponsibleId;
                        r.Specifications = a.Specifications;
                        r.StoreNum = a.StoreNum;
                        r.Status = (int)StatusEnum.正常;

                        result = bllSysAppointed.Insert(r);
                    }
                    else
                    {
                        if (modSysQRCode != null)
                        {
                            a.QRCode = modSysQRCode.QrCode;
                            a.QrName = modSysQRCode.Name;
                            a.Img = modSysQRCode.Img;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(a.QRCode))
                            {
                                a.QRCode = Guid.NewGuid().ToString();
                            }
                            a.QrName = a.QRCode;
                            a.Img = QrCode(a.QrName + "S");
                        }
                        result = bllSysAppointed.Insert(a);
                    }

                    if (result > 0)
                    {
                        if (modSysQRCode != null)//回写二维码状态
                            bllSysQRCode.EditStatus(a.QRCode);

                        //if (!string.IsNullOrEmpty(a.Places))  //修改设备箱子数量
                        //{
                        //    BllFireBox bllFireBox = new BllFireBox();
                        //    var box = bllFireBox.LoadData(a.Places);
                        //    if (box != null)
                        //    {
                        //        box.EquipmentCount--;
                        //        bllFireBox.Update(box);
                        //    }
                        //}
                        jsonResult.msg = "新增设备成功！";
                        jsonResult.data = a.Id;
                    }
                    else
                    {
                        jsonResult.msg = "新增设备失败！";
                        jsonResult.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.msg = "常异:" + ex.Message;
                jsonResult.errorCode = -1;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //修改设备
        //[HttpGet]
        public JsonResult UpAppoint(ModSysAppointed a, int LostYear = 5)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：Places={0}", a.Places);
                CheckParams(a.Name, "设备名称不能为空！");
                //CheckParams(a.QRCode, "无效二维码！");
                CheckParams(a.ResponsibleId, "设备责任人不能为空！");
                CheckParams(a.Gid, "设备类型不能为空！");

                BllSysAppointed bllSysAppointed = new BllSysAppointed();

                var temp = bllSysAppointed.LoadData(a.Id);
                if (temp == null)
                {
                    CheckParams("", "编辑失败,设备已删除！");
                }
                else if (temp != null && temp.Status == (int)StatusEnum.禁用)
                {
                    CheckParams("", "编辑失败,设备已禁用！");
                }
                else if (temp != null && temp.Status == (int)StatusEnum.删除)
                {
                    CheckParams("", "编辑失败,设备已删除！");
                }
                if (jsonResult.success)
                {
                    string tempPlaces = temp.Places;
                    //验证单位id
                    BllSysCompany bllSysCompany = new BllSysCompany();
                    var company = bllSysCompany.LoadData(temp.Cid);
                    if (jsonResult.success && (company == null || company != null && company.Status != 1))
                    {
                        CheckParams("", "该社会单位已经注销或禁用！");
                    }

                    if (jsonResult.success)
                    {
                        temp.ProductionDate = a.ProductionDate;
                        temp.LostTime = a.ProductionDate.Value.AddYears(LostYear);
                        temp.StoreNum = a.StoreNum;
                        temp.Specifications = a.Specifications;
                        temp.ResponsibleId = a.ResponsibleId;
                        temp.Places = a.Places;
                        temp.Name = a.Name;
                        temp.Model = a.Model;
                        temp.Mark = a.Mark;
                        temp.Gid = a.Gid;

                        if (bllSysAppointed.Update(temp) > 0)
                        {
                            jsonResult.success = true;
                            jsonResult.msg = "编辑成功！";
                            if (tempPlaces != a.Places)//修改箱子数量
                            {
                                string sql = " update Fire_FireBox set EquipmentCount=(select count(Id) from Sys_Appointed where Places='" + tempPlaces + "' and Status!=-1) where Id='" + tempPlaces + "';";
                                //sql += " update Fire_FireBox set EquipmentCount=(select count(Id) from Sys_Appointed where Places='" + tempPlaces + "' and Status!=-1) where Id='" + tempPlaces + "';";

                                new BllFireBox().ExecuteNonQueryByText(sql);

                                //new BllFireBox().UpdateEquipmentCount(a.Places, 1);
                                //new BllFireBox().UpdateEquipmentCount(tempPlaces, -1);
                            }
                        }
                        else
                        {
                            jsonResult.success = false;
                            jsonResult.msg = "编辑失败！";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.msg = "常异:" + ex.Message;
                jsonResult.errorCode = -1;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //删除设备
        //[HttpGet]
        public JsonResult DeAppoint(string appointId, string userid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：appointId={0}&userid={1}", appointId, userid);
                CheckParams(appointId, "设备Id不能为空！");
                CheckParams(userid, "用户Id不能为空！");

                BllSysAppointed bllSysAppointed = new BllSysAppointed();

                var temp = bllSysAppointed.LoadData(appointId);
                if (temp == null)
                {
                    CheckParams("", "设备已删除！");
                }
                else if (temp != null && temp.Status == (int)StatusEnum.禁用)
                {
                    CheckParams("", "设备已禁用！");
                }
                else if (temp != null && temp.Status == (int)StatusEnum.删除)
                {
                    CheckParams("", "设备已删除！");
                }

                if (jsonResult.success)
                {
                    //验证单位id
                    BllSysCompany bllSysCompany = new BllSysCompany();
                    var company = bllSysCompany.LoadData(temp.Cid);
                    //if (jsonResult.success && userid != temp.Responsible)
                    //{
                    //    CheckParams("", "对不起您,没有删除设备权限！");
                    //}
                    //else 
                    if (jsonResult.success && (company == null || company != null && company.Status != 1))
                    {
                        CheckParams("", "该社会单位已经注销或禁用！");
                    }

                    if (jsonResult.success)
                    {
                        if (bllSysAppointed.DeleteStatus(temp.Id) > 0)
                        {

                            //if (!string.IsNullOrEmpty(temp.Places))  //修改设备箱子数量
                            //{
                            //    BllFireBox bllFireBox = new BllFireBox();
                            //    var box = bllFireBox.LoadData(temp.Places);
                            //    if (box != null)
                            //    {
                            //        box.EquipmentCount--;
                            //        bllFireBox.Update(box);
                            //    }
                            //}
                            jsonResult.success = true;
                            jsonResult.msg = "删除成功！";

                        }
                        else
                        {
                            jsonResult.success = false;
                            jsonResult.msg = "删除失败！";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.msg = "常异:" + ex.Message;
                jsonResult.errorCode = -1;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //查询设备类型
        //[HttpGet]
        public JsonResult GetGroup(string cid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数：cid={0}", cid);
                if (jsonResult.success)
                {
                    BllSysGroup bllSysGroup = new BllSysGroup();

                    List<jsonCategory> jsonC = new List<jsonCategory>();

                    List<ModSysGroup> r = bllSysGroup.GetGroupList(cid);
                    foreach (var item in r)
                    {
                        jsonC.Add(new jsonCategory
                        {
                            id = item.Id,
                            name = item.Name,
                            //child = tree(r, item.Id)
                        });
                    }

                    jsonResult.data = jsonC;
                    jsonResult.msg = "获取成功！";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.msg = "常异:" + ex.Message;
                jsonResult.errorCode = -1;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }
        public List<jsonCategory> tree(List<ModSysGroup> r, string pid)
        {
            List<jsonCategory> jsonC = new List<jsonCategory>();
            foreach (var item in r.Where(c => c.ParentId == pid))
            {
                jsonC.Add(new jsonCategory
                {
                    id = item.Id,
                    name = item.Name,
                    child = tree(r, item.Id)
                });
            }
            return jsonC;
        }

        //查询设备责任人
        //[HttpGet]
        public JsonResult GetResponsible(string cid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：cid={0}", cid);
                CheckParams(cid, "单位id无效！");

                if (jsonResult.success)
                {
                    BllSysCompany bllsyscompany = new BllSysCompany();
                    BllSysMaster bllmaster = new BllSysMaster();

                    //查询单位部门
                    List<ModSysCompany> list = bllsyscompany.GetDepartments(cid);

                    //查询单位员工
                    List<ModSysMaster> rmaster = bllmaster.GetCIdByList(cid);
                    List<jsonComapny> r = new List<jsonComapny>();
                    foreach (var item in list)
                    {
                        jsonComapny j = new jsonComapny();
                        j.id = item.Id;
                        j.name = item.Name;
                        j.userlist = new List<object>();
                        foreach (var m in rmaster.Where(c => c.OrganizaId == item.Id))
                        {
                            j.userlist.Add(new
                            {
                                name = string.IsNullOrEmpty(m.UserName) ? m.LoginName : m.UserName,
                                loginname = m.LoginName,
                                id = m.Id,
                                headimg = m.HeadImg
                            });
                        }
                        //if (j.userlist.Count > 0)
                        r.Add(j);
                    }
                    jsonResult.data = r;
                    jsonResult.msg = "获取成功！";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        #region 添加设备巡检记录 注释代码
        //添加设备巡检记录
        //[HttpGet]
        public JsonResult AddAppointCheckNotesed(ModSysAppointCheckNotes a, string StatusStr, string ProductionDate, string ApprovalUser)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：ProductionDate={0}&ApprovalUser={1}&Mark={2}", ProductionDate, ApprovalUser, a.Mark);
                CheckParams(a.AppointId, "巡检设备id无效！");
                CheckParams(StatusStr, "巡检状态无效！");

                if (jsonResult.success)
                {

                    BllSysMaster bllSysMaster = new BllSysMaster();
                    var master = bllSysMaster.LoadData(a.ResponsibleId);
                    if (master != null && (master.Attribute == (int)AdminTypeEnum.单位用户 || master.Attribute == (int)AdminTypeEnum.单位管理员))
                    {
                        string[] AppointIds = a.AppointId.Split(',');
                        //string[] Marks = a.Mark.Split(',');

                        List<string> marklist = JsonHelper.serializer.Deserialize<List<string>>(a.Mark);

                        string[] StatusStrs = StatusStr.Split(',');
                        string[] ProductionDates = ProductionDate.Split(',');
                        string[] ApprovalUsers = ApprovalUser.Split(',');

                        if (AppointIds.Length > 0)
                        {
                            //设备验证
                            BllSysAppointed bllSysAppointed = new BllSysAppointed();
                            var temp = bllSysAppointed.LoadData(AppointIds[0]);
                            if (master.Cid != temp.Cid)
                            {
                                jsonResult.msg = "您没有巡检设备权限！";
                                jsonResult.success = false;
                            }

                            if (jsonResult.success)
                            {
                                int result = 0;
                                for (int i = 0; i < AppointIds.Length; i++)
                                {
                                    string AppointId = AppointIds[i];
                                    if (!string.IsNullOrEmpty(AppointId))
                                    {
                                        var Appoint = bllSysAppointed.LoadData(AppointId);
                                        if (Appoint != null && Appoint.Status == (int)StatusEnum.正常)
                                        {
                                            var t = new ModSysAppointCheckNotes();
                                            t.CreateTime = DateTime.Now;
                                            t.Id = Guid.NewGuid().ToString();
                                            t.AppointId = AppointId;
                                            t.CId = a.CId;
                                            t.Img = a.Img;
                                            t.Mark = marklist[i];//Marks[i];
                                            t.ResponsibleId = a.ResponsibleId;
                                            t.Status = Convert.ToInt32(StatusStrs[i]);

                                            BllSysAppointCheckNotes bllSysAppointCheckNotes = new BllSysAppointCheckNotes();
                                            if (bllSysAppointCheckNotes.Insert(t) > 0)
                                            {
                                                //修改下次维保时间
                                                Appoint.MaintenanceDate = Convert.ToDateTime(DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(2).AddDays(-2).ToString("yyyy-MM-dd")); ;//下一个月的最后一天的前一天
                                                Appoint.MaintenanceStatus = t.Status;
                                                if (t.Status == -1)//过期
                                                {
                                                    if (!string.IsNullOrEmpty(ProductionDates[i]))//换新
                                                    {
                                                        Appoint.ProductionDate = Convert.ToDateTime(ProductionDates[i]);
                                                    }
                                                    else//已维修,自动加两年
                                                    {
                                                        Appoint.LostTime.Value.AddYears(2);//保质期
                                                    }
                                                    Appoint.MaintenanceStatus = 0;//设备正常
                                                }

                                                bllSysAppointed.Update(Appoint);

                                                if (!string.IsNullOrEmpty(ApprovalUsers[i]))//责任人变更
                                                {
                                                    if (ApprovalUsers[i] != Appoint.ResponsibleId)//不是当前责任人
                                                    {
                                                        //查询是否正在变更中
                                                        BllSysFlow bllSysFlow = new BllSysFlow();
                                                        if (!bllSysFlow.Exists(4, 0, t.AppointId))
                                                        {
                                                            var ub = bllSysMaster.LoadData(Appoint.ResponsibleId);
                                                            ModSysFlow SysFlow = new ModSysFlow();
                                                            SysFlow.Id = Guid.NewGuid().ToString();
                                                            SysFlow.Title = Appoint.Name + "的设备责任人" + ub.UserName + "申请变更为" + master.UserName;
                                                            SysFlow.FlowType = 4;//(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核,4:设备责任人变更)
                                                            SysFlow.FlowStatus = 0;
                                                            SysFlow.Reamrk = "";
                                                            SysFlow.ApprovalUser = ApprovalUsers[i];
                                                            SysFlow.ApprovalTime = DateTime.Now;
                                                            SysFlow.CompanyId = Appoint.Cid;
                                                            SysFlow.MasterId = t.AppointId;
                                                            if (bllSysFlow.Insert(SysFlow) > 0)
                                                            {
                                                                if (result <= 5)
                                                                    result = 5;
                                                                jsonResult.msg = "新增巡检记录成功,变更责任人成功,等待审核！";
                                                            }
                                                            else
                                                            {
                                                                if (result <= 4)
                                                                    result = 4;
                                                                jsonResult.msg = "新增巡检记录成功,变更责任人失败！";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (result <= 3)
                                                                result = 3;
                                                            jsonResult.msg = "新增巡检记录成功,正在审核中,不能变更责任人！";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (result <= 2)
                                                            result = 2;
                                                        jsonResult.msg = "新增巡检记录成功！变更无效！";
                                                    }
                                                }
                                                else
                                                {
                                                    if (result <= 1)
                                                        result = 1;
                                                    jsonResult.msg = "新增巡检记录成功！";
                                                }
                                            }
                                        }
                                    }
                                }
                                switch (result)
                                {
                                    case 5:
                                        jsonResult.msg = "新增巡检记录成功,变更责任人成功,等待审核！";
                                        break;
                                    case 4:
                                        jsonResult.msg = "新增巡检记录成功,变更责任人失败！";
                                        break;
                                    case 3:
                                        jsonResult.msg = "新增巡检记录成功,正在审核中,不能变更责任人！";
                                        break;
                                    case 2:
                                        jsonResult.msg = "新增巡检记录成功！变更无效！";
                                        break;
                                    case 1:
                                    default:
                                        jsonResult.msg = "新增巡检记录成功！";
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        jsonResult.msg = "您没有巡检设备权限！";
                        jsonResult.success = false;
                    }

                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        #endregion

        //添加设备巡检记录
        //[HttpGet]
        public JsonResult AddAppointCheckNotes(ModSysAppointCheckNotes a, string StatusStr, string ProductionDate, string ApprovalUser)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：ProductionDate={0}&ApprovalUser={1}&Mark={2}&AppointId={3}&StatusStr={4}&ResponsibleId={5}", ProductionDate, ApprovalUser, a.Mark, a.AppointId, StatusStr, a.ResponsibleId);
                CheckParams(a.AppointId, "巡检设备id无效！");
                CheckParams(StatusStr, "巡检状态无效！");

                if (jsonResult.success)
                {

                    BllSysMaster bllSysMaster = new BllSysMaster();
                    var master = bllSysMaster.LoadData(a.ResponsibleId);
                    if (master != null && (master.Attribute == (int)AdminTypeEnum.单位用户 || master.Attribute == (int)AdminTypeEnum.单位管理员))
                    {
                        string[] AppointIds = a.AppointId.Split(',');
                        //string[] Marks = a.Mark.Split(',');

                        List<string> marklist = JsonHelper.serializer.Deserialize<List<string>>(a.Mark);

                        //List<string> imglist = JsonHelper.serializer.Deserialize<List<string>>(a.Img);

                        string[] StatusStrs = StatusStr.Split(',');
                        if (string.IsNullOrEmpty(ProductionDate))
                        {
                            ProductionDate = "";
                        }
                        string[] ProductionDates = ProductionDate.Split(',');
                        //string[] ApprovalUsers = ApprovalUser.Split(',');

                        if (AppointIds.Length > 0)
                        {
                            //设备验证
                            BllSysAppointed bllSysAppointed = new BllSysAppointed();
                            var temp = bllSysAppointed.LoadData(AppointIds[0]);
                            if (temp == null)
                            {
                                temp = bllSysAppointed.GetAppointDetailQRCode(AppointIds[0]);
                            }
                            if (temp != null && master.Cid != temp.Cid)
                            {
                                jsonResult.msg = "您没有巡检设备权限！";
                                jsonResult.success = false;
                            }

                            if (jsonResult.success)
                            {
                                #region --
                                int resultnum = 0;
                                for (int i = 0; i < AppointIds.Length; i++)
                                {
                                    string AppointId = AppointIds[i];
                                    if (!string.IsNullOrEmpty(AppointId))
                                    {
                                        ModSysAppointed Appoint = bllSysAppointed.LoadData(AppointId);
                                        if (Appoint == null)
                                        {
                                            Appoint = bllSysAppointed.GetAppointDetailQRCode(AppointId);
                                        }

                                        if (Appoint != null && Appoint.Status == (int)StatusEnum.正常)
                                        {
                                            var t = new ModSysAppointCheckNotes();
                                            t.CreateTime = DateTime.Now;
                                            t.Id = Guid.NewGuid().ToString();
                                            t.AppointId = Appoint.Id;
                                            t.CId = a.CId;
                                            t.Img = a.Img;//imglist[i];
                                            t.Mark = marklist[i];//Marks[i];
                                            t.ResponsibleId = a.ResponsibleId;
                                            t.Status = Convert.ToInt32(StatusStrs[i]);

                                            BllSysAppointCheckNotes bllSysAppointCheckNotes = new BllSysAppointCheckNotes();
                                            if (bllSysAppointCheckNotes.Insert(t) > 0)
                                            {
                                                //修改下次维保时间
                                                Appoint.MaintenanceDate = Convert.ToDateTime(DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(2).AddDays(-2).ToString("yyyy-MM-dd"));//下一个月的最后一天的前一天
                                                Appoint.MaintenanceStatus = t.Status;
                                                if (t.Status == -1)//过期
                                                {
                                                    if (!string.IsNullOrEmpty(ProductionDates[i]) && string.Compare(ProductionDates[i], "null", true) < 0)//换新
                                                    {
                                                        Appoint.ProductionDate = Convert.ToDateTime(ProductionDates[i]);
                                                    }
                                                    else//已维修,自动加两年
                                                    {
                                                        Appoint.LostTime.Value.AddYears(2);//保质期
                                                    }
                                                    Appoint.MaintenanceStatus = 0;//设备正常
                                                }
                                                bllSysAppointed.Update(Appoint);
                                                resultnum++;
                                            }
                                        }
                                    }
                                }
                                jsonResult.msg = "成功巡检" + resultnum + "个设备！";
                                #endregion
                            }
                        }
                    }
                    else
                    {
                        jsonResult.msg = "您没有巡检设备权限！";
                        jsonResult.success = false;
                    }

                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //设备巡检记录
        //[HttpGet]
        public JsonResult GetAppointCheckNotesList(string AppointId)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：AppointId={0}", AppointId);
                CheckParams(AppointId, "设备id无效！");

                if (jsonResult.success)
                {
                    BllSysAppointCheckNotes bllSysAppointCheckNotes = new BllSysAppointCheckNotes();

                    Search search = GetSearch();
                    search.AddCondition("AppointId='" + AppointId + "'");
                    search.AddCondition("appointedstatus=1");//设备必须正常

                    BllSysAppointed bllSysAppointed = new BllSysAppointed();
                    Page<ModSysAppointCheckNotes> r = bllSysAppointCheckNotes.GetAppointCheckNotesList(search);

                    List<jsonAppointCheckNotes> json = new List<jsonAppointCheckNotes>();
                    foreach (var item in r.Items)
                    {
                        jsonAppointCheckNotes j = new jsonAppointCheckNotes();
                        j.AppointId = item.AppointId;
                        j.CreateTime = item.CreateTime;
                        j.Img = item.Img;

                        //处理大图路径
                        BigImgPath(item, j);

                        j.Mark = item.Mark;
                        j.Responsible = item.Responsible;
                        j.ResponsibleId = item.ResponsibleId;
                        j.Status = item.Status;
                        j.Id = item.Id;

                        var temp = bllSysAppointed.GetAppointDetailId(item.AppointId);
                        if (temp != null)
                        {
                            j.Places = !string.IsNullOrEmpty(temp.PlacesName) ? temp.PlacesName : temp.Placesed;
                            j.StoreNum = temp.StoreNum;
                            j.GroupName = temp.GroupName;
                            j.Model = temp.Model;
                            j.Name = temp.Name;
                            json.Add(j);
                        }

                    }
                    jsonResult.data = json;
                    jsonResult.msg = "获取成功！";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //我的设备巡检记录
        //[HttpGet]
        public JsonResult GetMyAppointCheckNotesList(string userid, string cid, int type = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&cid={1}&type={2}", userid, cid, type);
                CheckParams(userid, "用户id无效！");
                CheckParams(cid, "单位id无效！");

                if (jsonResult.success)
                {

                    BllSysMaster bllSysMaster = new BllSysMaster();
                    var user = bllSysMaster.LoadData(userid);

                    BllSysAppointCheckNotes bllSysAppointCheckNotes = new BllSysAppointCheckNotes();

                    Search search = GetSearch();

                    search.AddCondition("appointedstatus=1");//设备必须正常
                    switch (type)
                    {
                        case (int)AdminTypeEnum.单位管理员:
                            search.AddCondition("cid='" + cid + "'");
                            break;
                        case (int)AdminTypeEnum.单位用户:
                            search.AddCondition("cid='" + cid + "'");
                            search.AddCondition("ResponsibleId='" + userid + "'");//设备巡检人
                            break;
                        case (int)AdminTypeEnum.供应商管理员:
                        case (int)AdminTypeEnum.供应商用户:
                        case (int)AdminTypeEnum.维保公司管理员:
                        case (int)AdminTypeEnum.维保用户:
                        case (int)AdminTypeEnum.消防部门管理员:
                        case (int)AdminTypeEnum.消防用户:
                            BllSysCompanyCognate bllSysCompanyCognate = new BllSysCompanyCognate();
                            string sql = bllSysCompanyCognate.GetInCompanyResult(cid, type);
                            if (!string.IsNullOrEmpty(sql))
                            {
                                search.AddCondition("cid in (" + sql + ")");
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = "获取成功！";
                            }
                            break;
                        default:
                            jsonResult.success = false;
                            jsonResult.msg = "获取成功！";
                            break;
                    }
                    if (jsonResult.success)
                    {

                        Page<ModSysAppointCheckNotes> r = bllSysAppointCheckNotes.GetAppointCheckNotesList(search);

                        List<jsonAppointCheckNotes> json = new List<jsonAppointCheckNotes>();

                        BllSysAppointed bllSysAppointed = new BllSysAppointed();
                        foreach (var item in r.Items)
                        {
                            jsonAppointCheckNotes j = new jsonAppointCheckNotes();
                            j.AppointId = item.AppointId;
                            j.CreateTime = item.CreateTime;
                            j.Img = item.Img;
                            //处理大图路径
                            BigImgPath(item, j);

                            j.Mark = item.Mark;
                            j.Responsible = item.Responsible;
                            j.ResponsibleId = item.ResponsibleId;
                            j.Status = item.Status;
                            j.Id = item.Id;

                            var temp = bllSysAppointed.GetAppointDetailId(item.AppointId);
                            if (temp != null)
                            {
                                j.StoreNum = temp.StoreNum;
                                j.GroupName = temp.GroupName;
                                j.Model = temp.Model;
                                j.Name = temp.Name;
                                j.Places = !string.IsNullOrEmpty(temp.PlacesName) ? temp.PlacesName : temp.Placesed;
                                json.Add(j);
                            }
                        }
                        jsonResult.data = json;
                        jsonResult.msg = "获取成功！";
                    }
                    else
                    {
                        jsonResult.success = true;
                        jsonResult.data = new List<jsonAppointCheckNotes>();
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //我要管理设备
        //[HttpGet]
        public JsonResult MyManagerAppoint(string appointId, string userid, int type = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：appointId={0}&userid={1}&type={2}", appointId, userid, type);
                CheckParams(userid, "用户id无效！");
                CheckParams(appointId, "设备id无效！");
                if (jsonResult.success)
                {
                    if (jsonResult.success && type != (int)AdminTypeEnum.单位用户)
                    {
                        CheckParams("", "只有单位用户才能管理设备！");
                    }

                    BllSysMaster bllSysMaster = new BllSysMaster();
                    var master = bllSysMaster.LoadData(userid);
                    if (jsonResult.success && (master == null || (master != null && master.Status != (int)StatusEnum.正常)))
                    {
                        CheckParams("", "当前用户已禁用或删除！");
                    }

                    BllSysAppointed bllSysAppointed = new BllSysAppointed();
                    var Appoint = bllSysAppointed.LoadData(appointId);
                    if (jsonResult.success && Appoint == null || (Appoint != null && Appoint.Status != (int)StatusEnum.正常))
                    {
                        CheckParams("", "当前设备已禁用或删除！");
                    }

                    if (jsonResult.success && (Appoint != null & master != null && Appoint.Cid != master.Cid))
                    {
                        CheckParams("", "不能申请本单位以外的设备管理！");
                    }

                    if (jsonResult.success)
                    {
                        if (userid != Appoint.ResponsibleId)//不是当前责任人
                        {
                            //查询是否正在变更中
                            BllSysFlow bllSysFlow = new BllSysFlow();
                            if (!bllSysFlow.Exists(4, 0, appointId))
                            {
                                var ub = bllSysMaster.LoadData(Appoint.ResponsibleId);
                                if (ub == null) { ub = new ModSysMaster(); }
                                ModSysFlow SysFlow = new ModSysFlow();
                                SysFlow.Id = Guid.NewGuid().ToString();
                                SysFlow.Title = Appoint.Name + "的设备责任人" + ub.UserName + "申请变更为" + master.UserName;
                                SysFlow.FlowType = 4;//(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核,4:设备责任人变更)
                                SysFlow.FlowStatus = 0;
                                SysFlow.Reamrk = "";
                                SysFlow.ApprovalUser = userid;
                                SysFlow.ApprovalTime = DateTime.Now;
                                SysFlow.CompanyId = Appoint.Cid;
                                SysFlow.MasterId = appointId;
                                if (bllSysFlow.Insert(SysFlow) > 0)
                                {
                                    jsonResult.msg = "变更责任人成功,等待审核！";
                                }
                                else
                                {
                                    jsonResult.success = false;
                                    jsonResult.msg = "变更责任人失败！";
                                }
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = "正在审核中,不能变更责任人！";
                            }
                        }
                        else
                        {
                            jsonResult.success = false;
                            jsonResult.msg = "您已经是该设备管理员了！";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);

        }

        //处理大图路径
        public void BigImgPath(ModSysAppointCheckNotes item, jsonAppointCheckNotes j)
        {
            try
            {
                if (!string.IsNullOrEmpty(item.Img))
                {
                    string[] imgs = item.Img.Split(',');
                    for (int i = 0; i < imgs.Count(); i++)
                    {
                        string str = imgs[i];
                        if (!string.IsNullOrEmpty(str))
                        {
                            string tempstr = str.Substring(0, str.LastIndexOf("/")) + "/BigImg" + str.Substring(str.LastIndexOf("/"), str.Length - str.LastIndexOf("/"));
                            if (i == imgs.Count() - 1)
                            {
                                j.Path += tempstr;
                            }
                            else
                            {
                                j.Path += tempstr + ",";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},处理大图路径", ex.Message);
            }
        }
    }
}
