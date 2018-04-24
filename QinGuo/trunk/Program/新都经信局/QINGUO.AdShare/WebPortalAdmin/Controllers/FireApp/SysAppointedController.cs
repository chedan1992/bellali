using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Common;
using QINGUO.Business;
using System.Data;
using WebPortalAdmin.Code;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Text;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.Text.RegularExpressions;
using AppLibrary.WriteExcel;
using System.Diagnostics;
using AppLibrary.ReadExcel;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 设备管理
    /// </summary>
    public class SysAppointedController : BaseController<ModSysAppointed>
    {
        string[] title;  //导出的标题
        string[] field;  //导出对应字段

        BllSysAppointed Bll = new BllSysAppointed();

        /// <summary>
        /// 超级管理员查看设备
        /// </summary>
        /// <returns></returns>
        public ActionResult SuperManIndex()
        {
            return View();
        }

        /// <summary>
        /// 单位管理设备
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyIndex()
        {
            return View();
        }
        /// <summary>
        /// 设备导入
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            return View();
        }

         /// <summary>
        /// 上级单位查看设备信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Roles()
        {
            return View();
        }

        
         /// <summary>
        /// 普通用户查看设备信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserIndex()
        {
            return View();
        }
        

        #region ===查询超级管理设备 SuperManIndexData
        /// <summary>
        /// 查询超级管理设备
        /// </summary>
        public void SuperManIndexData()
        {
            try
            {
                Search search = this.GetSearch();
                if (Request["Id"].ToString() != "-1")
                {
                    search.AddCondition("Cid='" + Request["Id"].ToString() + "'");
                }
                LogInsert(OperationTypeEnum.访问, "设备管理模块", "访问页面成功.");
                WriteJsonToPage(Bll.SearchData(search));
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备管理模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion

        #region ===查询单位管理设备 SearchCompanyData
        /// <summary>
        /// 查询单位管理设备
        /// </summary>
        public void SearchCompanyData()
        {
            try
            {
                Search search = this.GetSearch();
                search.AddCondition("Cid='" + CurrentMaster.Cid + "'");

                LogInsert(OperationTypeEnum.访问, "设备管理模块", "访问页面成功.");
                WriteJsonToPage(Bll.SearchData(search));
            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
                LogInsert(OperationTypeEnum.异常, "设备管理模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion

        #region ===查询设备巡检记录 SearchAppointCheckNotes
        /// <summary>
        /// 查询设备巡检记录
        /// </summary>
        public void SearchAppointCheckNotes()
        {
            try
            {
                Search search = this.GetSearch();
                search.AddCondition("AppointId='" + Request["CEId"].ToString() + "'");

                LogInsert(OperationTypeEnum.访问, "设备详情-设备巡检模块", "访问页面成功.");
                WriteJsonToPage(new BllSysAppointCheckNotes().SearchData(search));

            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
                LogInsert(OperationTypeEnum.异常, "设备详情-设备巡检模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion


        #region ===查询设备巡检记录 SearchChartAppointCheckNotes
        /// <summary>
        /// 查询设备巡检记录
        /// </summary>
        public void SearchChartAppointCheckNotes()
        {
            try
            {
                string month = Request["month"].ToString();
                string Attribute = Request["Attribute"].ToString();

                Search search = this.GetSearch();
                switch (int.Parse(Attribute))
                { 
                    case 1://超级管理员
                        break;
                    case 2://单位管理员
                        search.AddCondition("Cid='" +CurrentMaster.Cid + "'");
                        break;
                    case 3:
                    case 4:
                    case 5:
                        search.AddCondition("Cid in (select EmployerId from Sys_CompanyCognate where Cid='" + CurrentMaster.Cid + "' and Status=1 and SelectType=2)");
                        break;
                }
                search.AddCondition("MONTH(CreateTime)=" + month + "");
                WriteJsonToPage(new BllSysAppointCheckNotes().SearchData(search));
            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
            }
        }
        #endregion

        #region ===上级单位根据权限查询设备列表 SuperParentData
        /// <summary>
        /// 上级单位查询设备列表
        /// </summary>
        public void SuperParentData()
        {
            try
            {
                Search search = this.GetSearch();
                if (Request["Id"].ToString() != "-1")
                {
                    search.AddCondition("Cid='" + Request["Id"].ToString() + "'");
                }
                else
                {
                    search.AddCondition("Cid in (select EmployerId from Sys_CompanyCognate where Cid='" + CurrentMaster.Cid + "' and Status=1 and SelectType=2)");
                }

                LogInsert(OperationTypeEnum.访问, "设备管理模块", "访问页面成功.");
                WriteJsonToPage(Bll.SearchData(search));
            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
                LogInsert(OperationTypeEnum.异常, "设备管理模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion

        #region ===普通用户登录查看自己增删改的设备 SearchMyData
        /// <summary>
        /// 普通用户登录查看自己增删改的设备
        /// </summary>
        public void SearchMyData()
        {
            try
            {
                Search search = this.GetSearch();
                search.AddCondition("(CreaterId='" + CurrentMaster.Id + "' or ResponsibleId='" + CurrentMaster.Id + "')");

                LogInsert(OperationTypeEnum.访问, "设备管理模块", "访问页面成功.");

                WriteJsonToPage(Bll.SearchData(search));
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备管理模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion
        

        #region ==导入数据 ImportDate
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void ImportDate()
        {
            ModJsonResult json = new ModJsonResult();

            try
            {
                //获取文件
                HttpPostedFileBase excel = Request.Files["excel"];
                DataTable errortab = null;
                int rowAffected = ExportTest(excel.InputStream,
                    "insert into Sys_Appointed(Id,QRCode,Name,Places,Model,Specifications,StoreNum,ProductionDate,ResponsibleId,Mark,MaintenanceDate,MaintenanceDay,Gid,Img,Status,Cid,CreateTime,CreaterId,LostTime,QrName,MaintenanceStatus)", out errortab, CurrentMaster);
                json.msg = "成功导入" + rowAffected + "条数据！";
                json.success = true;
                if (errortab != null)
                {
                    ExcelRender.RenderToExcel(errortab, Request.MapPath("/Project/Template/Error/" + CurrentMaster.LoginName + "Error.xls"));
                    json.data = "\"" + "/Project/Template/Error/" + CurrentMaster.LoginName + "Error.xls\"";
                    json.msg = "成功导入" + rowAffected + "条数据！错误" + errortab.Rows.Count + "条数据.";
                }
                LogInsert(OperationTypeEnum.操作, "设备导入", "导入操作成功.");

                WriteJsonToPage(json.ToString());
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备导入", "操作异常信息:" + ex);
            }
        }

        #endregion

        #region ===导入数据入口
        /// <summary>
        /// Excel文档导入到数据库
        /// 默认取Excel的第一个表
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="insertSql">插入语句</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <returns></returns>
        public int ExportTest(Stream excelFileStream, string insertSql, out DataTable tab, ModSysMaster CurrentMaster)
        {
            return RenderToDbCompany(excelFileStream, insertSql, 0, 0, out tab, CurrentMaster);
        }

        #region ===数据校验 RowDataVis
        public static BllSysCategory bllCategory = new BllSysCategory();
        /// <summary>
        /// 箱子列表
        /// </summary>
        public static List<ModFireBox> FireBox = new List<ModFireBox>();
        /// <summary>
        /// 设备列表
        /// </summary>
        public static List<ModSysAppointed> SysCategory = new List<ModSysAppointed>();
        /// <summary>
        /// 部门列表
        /// </summary>
        public static List<ModSysCompany> SysCompany = new List<ModSysCompany>();
        /// <summary>
        /// 人员列表
        /// </summary>
        public static List<ModSysMaster> SysMaster = new List<ModSysMaster>();
         /// <summary>
        /// 设备分类
        /// </summary>
        public static List<ModSysGroup> SysGroup = new List<ModSysGroup>();
        
      /// <summary>
        /// 数据校验空
      /// </summary>
      /// <param name="CurrentMaster"></param>
        /// <param name="val1">二维码编码</param>
        /// <param name="val2">地址查询简码</param>
        /// <param name="val3">设备名称</param>
        /// <param name="val4">设备位置</param>
        /// <param name="val5">设备型号</param>
        /// <param name="val6">设备规格</param>
        /// <param name="val7">数量</param>
        /// <param name="val8">生产日期</param>
        /// <param name="val9">维修日期</param>
        /// <param name="val10">过期年限</param>
        /// <param name="val11">责任部门</param>
        /// <param name="val12">责任人</param>
        /// <param name="val13">联系电话</param>
        /// <param name="val14">设备分类</param>
        /// <param name="val15">备注信息</param>
      /// <param name="CompanyId"></param>
        /// <param name="AddressId">箱子地址</param>
      /// <param name="DutyUserId"></param>
      /// <param name="CategoryId"></param>
      /// <param name="errorStr"></param>
      /// <returns></returns>
        public bool CheckData(ModSysMaster CurrentMaster, string val1, string val2, string val3, string val4, string val5, string val6, string val7, string val8, string val9, string val10, string val11, string val12, string val13, string val14, string val15, string CompanyId,ref string AddressId, ref string DutyUserId, ref string CategoryId, ref string errorStr)
        {
            bool flag = false;

            string DeptId = "";
            bool AddAddressId = false;
            bool AddDeptId = false;
            bool AddMasterId = false;
            bool AddCategory = false;

            #region ===校验二维码编码
            if (val1.Length == 0 || string.IsNullOrEmpty(val1.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "二维码编码不能为空,为必填项.";
                return flag;
            }
            else
            {
                if (val1.Length > 100) //验证长度是否合理
                {
                    flag = true;
                    errorStr += "二维码编码长度过长,有效范围1-100字符.";
                    return flag;
                }
                else
                {
                    //判断设备编号是否存在
                    List<ModFireBox> newCategory = FireBox.Where(p => (p.QrCode == val1)).ToList();
                    if (newCategory.Count > 0)
                    {
                        flag = true;
                        errorStr += "二维码编码已经存在.";
                        return flag;
                    }
                }
            }
            #endregion

            #region ===校验地址查询简码和名称
            if (val2.Length == 0 || string.IsNullOrEmpty(val2.Trim()) || val4.Length == 0 || string.IsNullOrEmpty(val4.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "地址简码或地址名称不能为空,为必填项";
                return flag;
            }
            else
            {
                if (val2.Length > 50 || val4.Length>50) //验证长度是否合理
                {
                    flag = true;
                    errorStr += "地址简码或地址名称长度过长,有效范围1-100字符.";
                    return flag;
                }
                else
                {
                    AddAddressId = true;
                }
            }
            #endregion

            #region ===校验设备名称
            if (val3.Length == 0 || string.IsNullOrEmpty(val3.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "设备名称不能为空,为必填项";
                return flag;
            }
            else
            {
                if (val3.Length > 50) //验证长度是否合理
                {
                    flag = true;
                    errorStr += "设备名称长度过长,有效范围1-50字符.";
                    return flag;
                }
            }
            #endregion

            #region ===校验设备位置
            if (val4.Length == 0 || string.IsNullOrEmpty(val4.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "设备位置不能为空,为必填项.";
                return flag;
            }
            else
            {
                if (val4.Length > 50) //验证长度是否合理
                {
                    flag = true;
                    errorStr += "设备位置长度过长,有效范围1-50字符.";
                    return flag;
                }
            }
            #endregion

            #region ===校验设备型号
            if (val5.Length == 0 || string.IsNullOrEmpty(val5.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "设备型号不能为空,为必填项.";
                return flag;
            }
            #endregion

            #region ===校验设备规格
            if (val6.Length == 0 || string.IsNullOrEmpty(val6.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "设备规格不能为空,为必填项.";
                return flag;
            }
            #endregion

            #region ===校验数量
            if (val7.Length != 0) //验证是否是正确数字 0-100
            {
                if (ValidateHelper.IsNumber(val7))//是否数字字符串
                {

                }
                else
                {
                    flag = true;
                    errorStr += "数量数字错误,只能是整形数字.";
                    return flag;
                }
            }
            #endregion

            #region ===校验生产日期
            if (val8.Length == 0 || string.IsNullOrEmpty(val8.Trim())) //验证长度是否合理
            {
                //生产日期为空 ,维修日期必定不能为空
                if (val9.Length == 0 || string.IsNullOrEmpty(val9.Trim()))
                {
                    flag = true;
                    errorStr += "生产日期和维修日期不能同时为空.";
                    return flag;
                }
            }
            else
            {
                try
                {
                    if (val8.IndexOf("年") > 0 || val8.IndexOf("月") > 0)
                    {
                        var time = Convert.ToDateTime(val8);
                    }
                    else
                    {
                        var time = Convert.ToDateTime(ToDateTimeValue(val8));
                    }
                }
                catch
                {
                    flag = true;
                    errorStr += "生产日期格式错误.请输入有效的日期时间.";
                    return flag;
                }
            }
            #endregion

            #region ===校验维修日期
            if (val9.Length == 0 || string.IsNullOrEmpty(val9.Trim())) //验证长度是否合理
            {
              
            }
            else
            {
                try
                {
                    if (val9.IndexOf("年") > 0 || val9.IndexOf("月") > 0)
                    {
                        var time = Convert.ToDateTime(val9);
                    }
                    else
                    {
                        var time = Convert.ToDateTime(ToDateTimeValue(val9));
                    }
                }
                catch
                {
                    flag = true;
                    errorStr += "维修日期格式错误.请输入有效的维修日期";
                    return flag;
                }
            }
            #endregion

            #region ===校验过期年限
            if (val10.Length != 0) //验证是否是正确数字 0-100
            {
                if (ValidateHelper.IsNumber(val10))//是否数字字符串
                {

                }
                else
                {
                    flag = true;
                    errorStr += "过期年限数字错误,只能是整形数字.";
                    return flag;
                }
            }
            #endregion

            #region ===校验部门
            if (val11.Length == 0 || string.IsNullOrEmpty(val11.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "部门不能为空,为必填项.";
                return flag;
            }
            else {
                AddDeptId = true;
            }
            #endregion

            #region ===校验联系电话
            if (val12.Length == 0 || string.IsNullOrEmpty(val12.Trim()) || val13.Length == 0 || string.IsNullOrEmpty(val13.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "联系电话或责任人不能为空，为必填项.";
                return flag;
            }
            else
            { 
              //判断电话号码是否是有效号码
                Regex regex = new Regex("^1\\d{10}$");
                bool res = regex.IsMatch(val13);
                if (res == false)
                {
                    flag = true;
                    errorStr += "请输入有效的11位手机号码.";
                    return flag;
                }
            }
            #endregion

            #region ===校验责任人 和联系电话
            if (val13.Trim() != "") //验证长度是否合理
            {
                //查询登录帐号是否已经存在
                List<ModSysMaster> Master = SysMaster.Where(p => (p.LoginName == val13)).ToList();
                if (Master.Count <= 0)
                {
                    AddMasterId = true;
                }
                else
                {
                    if (Master[0].Cid != CompanyId)
                    {
                        flag = true;
                        errorStr += "联系电话已被其他公司注册使用.";
                        return flag;
                    }
                    else
                    {
                        DutyUserId = Master[0].Id;
                    }
                }
            }
            #endregion

            #region ===校验设备分类
            if (val14.Trim() != "") //验证长度是否合理
            {
                //查询登录帐号是否已经存在
                List<ModSysGroup> Group = SysGroup.Where(p => (p.Name == val14)).ToList();
                if (Group.Count <= 0)
                {
                    AddCategory = true;
                }
                else
                {
                     CategoryId = Group[0].Id;
                }
            }
            else {
                flag = true;
                errorStr += "设备分类不能为空,为必填项.";
                return flag;
            }
            #endregion

            #region ===校验备注信息
            if (val15.Length == 0 || string.IsNullOrEmpty(val15.Trim())) //验证长度是否合理
            {
            }
            else {
                if (val15.Length > 50) //验证长度是否合理
                {
                    flag = true;
                    errorStr += "备注信息长度过长,有效范围1-50字符.";
                    return flag;
                }
            }
            #endregion

            //添加设备位置
            if (AddAddressId == true)
            {
                //判断设备位置和查询简码是否存在
                List<ModFireBox> newCategory = FireBox.Where(p => p.Name == val2.Trim() && p.Address == val4.Trim()).ToList();
                if (newCategory.Count > 0)
                {
                    AddressId = newCategory[0].Id;
                }
                else
                {
                    AddressId = AddFireBox(val2, val4, CurrentMaster, CompanyId);
                }
            }
            //添加部门
            if (AddDeptId == true)
            {
                List<ModSysCompany> newCategory = SysCompany.Where(p => (p.Name == val11)).ToList();
                if (newCategory.Count <= 0)
                {
                    DeptId = AddCompany(val11, CurrentMaster, CompanyId);
                }
                else
                {
                    DeptId = newCategory[0].Id;
                }
            }
            //添加部门人员
            if (AddMasterId == true)
            {
                //添加人员
                DutyUserId = AddMaster(val12, val13, DeptId, CurrentMaster, CompanyId);
            }
            if (AddCategory == true)
            {
                //添加设备分类
                CategoryId = AddCategoryId(val14, CurrentMaster, CompanyId);
            }
            return flag;
        }


        /// <summary>
        /// 添加箱子信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="CurrentMaster"></param>
        public string AddFireBox(string Name,string Address, ModSysMaster CurrentMaster, string companyId)
        {
            ModFireBox t = new ModFireBox();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.Img = QrCode(t.Id + "X");//箱子二维码最后大写X
            t.QrCode = t.Id;
            t.SysId = companyId;
            t.CreateTime = DateTime.Now;
            t.CreaterId = CurrentMaster.Id;//创建人编号
            t.Name = Name.Trim();
            t.Address = Address.Trim();
            t.EquipmentCount = 0;
            new BllFireBox().Insert(t);
            FireBox.Add(t);
            return t.Id;
        }


        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="name"></param>
        /// <param name="CurrentMaster"></param>
        public string AddCompany(string name, ModSysMaster CurrentMaster, string companyId)
        {
            ModSysCompany t = new ModSysCompany();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.CreaterUserId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.Attribute = (int)CompanyType.部门;
            t.Name = name;
            if (companyId != "0")
            {
                var model = new BllSysCompany().LoadData(companyId);
                t.Path = model.Path + "/" + t.Id;
            }
            else
            {
                t.Path = t.Id;
            }
            t.CreateCompanyId = companyId;
            new BllSysCompany().Insert(t);
            SysCompany.Add(t);
            return t.Id;
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="CurrentMaster"></param>
        public string AddMaster(string name,string LoginName,string DeptId, ModSysMaster CurrentMaster, string companyId)
        {
            ModSysMaster t = new ModSysMaster();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.Cid = companyId;
            t.OrganizaId = DeptId;//部门编号
            t.IsSystem = false;
            t.IsMain = true;
            t.CreaterId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.HeadImg ="";
            t.CardNum ="";
            t.Attribute = (int)AdminTypeEnum.单位用户;//用户类型
            t.LoginName = LoginName;
            t.UserName=name;
            t.Pwd = DESEncrypt.Encrypt("666666");//默认密码

            new BllSysMaster().Insert(t);
            SysMaster.Add(t);
            return t.Id;
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="CurrentMaster"></param>
        public string AddCategoryId(string name, ModSysMaster CurrentMaster, string companyId)
        {
            ModSysGroup t = new ModSysGroup();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.CreaterId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.ParentId ="0";
            t.Name = name;
            t.CompanyId = companyId;
            new BllSysGroup().Insert(t);
            SysGroup.Add(t);
            return t.Id;
        }

        #endregion

        #region Excel文档导入到数据库
        /// <summary>
        /// Excel文档导入到数据库
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="insertSql">插入语句</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <param name="shopTypeid">商家分类</param>
        /// <returns></returns>
        public int RenderToDbCompany(Stream excelFileStream, string insertSql, int sheetIndex, int headerRowIndex, out DataTable tab, ModSysMaster CurrentMaster)
        {
            #region ===构建错误表头
            /// <param name="CurrentMaster"></param>
            /// <param name="val1">二维码编码</param>
            /// <param name="val2">地址查询简码</param>
            /// <param name="val3">设备名称</param>
            /// <param name="val4">设备位置</param>
            /// <param name="val5">设备型号</param>
            /// <param name="val6">设备规格</param>
            /// <param name="val7">数量</param>
            /// <param name="val8">生产日期</param>
            /// <param name="val9">维修日期</param>
            /// <param name="val10">过期年限</param>
            /// <param name="val11">责任部门</param>
            /// <param name="val12">责任人</param>
            /// <param name="val13">联系电话</param>
            /// <param name="val14">设备分类</param>
            /// <param name="val15">备注信息</param>
            DataTable ErrorTab = new DataTable();
            DataColumn dc1 = new DataColumn("二维码编码", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("地址简码", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("设备名称", Type.GetType("System.String"));
            DataColumn dc4 = new DataColumn("设备位置", Type.GetType("System.String"));
            DataColumn dc5 = new DataColumn("设备型号", Type.GetType("System.String"));
            DataColumn dc6 = new DataColumn("设备规格", Type.GetType("System.String"));
            DataColumn dc7 = new DataColumn("数量", Type.GetType("System.String"));
            DataColumn dc8 = new DataColumn("生产日期", Type.GetType("System.String"));
            DataColumn dc9 = new DataColumn("维修日期", Type.GetType("System.String"));
            DataColumn dc10 = new DataColumn("过期年限", Type.GetType("System.String"));
            DataColumn dc11 = new DataColumn("责任部门", Type.GetType("System.String"));
            DataColumn dc12 = new DataColumn("责任人", Type.GetType("System.String"));
            DataColumn dc13 = new DataColumn("联系电话", Type.GetType("System.String"));
            DataColumn dc14 = new DataColumn("设备分类", Type.GetType("System.String"));
            DataColumn dc15 = new DataColumn("备注信息", Type.GetType("System.String"));
            DataColumn dc16 = new DataColumn("错误原因", Type.GetType("System.String"));
            ErrorTab.Columns.Add(dc1);
            ErrorTab.Columns.Add(dc2);
            ErrorTab.Columns.Add(dc3);
            ErrorTab.Columns.Add(dc4);
            ErrorTab.Columns.Add(dc5);
            ErrorTab.Columns.Add(dc6);
            ErrorTab.Columns.Add(dc7);
            ErrorTab.Columns.Add(dc8);
            ErrorTab.Columns.Add(dc9);
            ErrorTab.Columns.Add(dc10);
            ErrorTab.Columns.Add(dc11);
            ErrorTab.Columns.Add(dc12);
            ErrorTab.Columns.Add(dc13);
            ErrorTab.Columns.Add(dc14);
            ErrorTab.Columns.Add(dc15);
            ErrorTab.Columns.Add(dc16);
            #endregion

            //当前用户公司编号
            string CompanyId = Request["CompanyId"].ToString().Trim();//
            if (CompanyId == "")
            {
                CompanyId = CurrentMaster.Cid;
            }
            //获取所有列表
            FireBox = new BllFireBox().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.SysId == "" + CompanyId + "").ToList();
            SysCategory = new BllSysAppointed().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Cid == "" + CompanyId + "").ToList();
            SysCompany = new BllSysCompany().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Attribute == (int)CompanyType.部门 && p.CreateCompanyId == "" + CompanyId + "").ToList();
            SysMaster = new BllSysMaster().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除).ToList();
            SysGroup = new BllSysGroup().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.CompanyId == "" + CompanyId + "").ToList();

            int rowAffected = 0;
            using (excelFileStream)//读取流
            {
                //根据类型查询题库信息
                List<ModSysAppointed> list = new BllSysAppointed().GetListByWhere(" and Status!=" + (int)StatusEnum.删除);
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                StringBuilder builder = new StringBuilder();
                StringBuilder builderValue = new StringBuilder();
                IRow headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;//
                int rowCount = sheet.LastRowNum;//
                int Insertcount = 0;  //插入总行数
                //循环表格
                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    //判断空行
                    if (row != null)
                    {
                        #region  数据验证判断
                        var column0 = ExcelRender.GetCellValue(row.GetCell(0)).Replace("'", "''");//二维码编码
                        var column1 = ExcelRender.GetCellValue(row.GetCell(1)).Replace("'", "''");//地址简码
                        var column2 = ExcelRender.GetCellValue(row.GetCell(2)).Replace("'", "''");//设备名称
                        var column3 = ExcelRender.GetCellValue(row.GetCell(3)).Replace("'", "''");//设备位置
                        var column4 = ExcelRender.GetCellValue(row.GetCell(4)).Replace("'", "''");//设备型号
                        var column5 = ExcelRender.GetCellValue(row.GetCell(5)).Replace("'", "''");//设备规格
                        var column6 = ExcelRender.GetCellValue(row.GetCell(6)).Replace("'", "''");//数量
                        var column7 = ExcelRender.GetCellValue(row.GetCell(7)).Replace("'", "''");//生产日期
                        var column8 = ExcelRender.GetCellValue(row.GetCell(8)).Replace("'", "''");//维修日期
                        var column9 = ExcelRender.GetCellValue(row.GetCell(9)).Replace("'", "''");//过期年限
                        var column10 = ExcelRender.GetCellValue(row.GetCell(10)).Replace("'", "''");//责任部门
                        var column11 = ExcelRender.GetCellValue(row.GetCell(11)).Replace("'", "''");//责任人
                        var column12 = ExcelRender.GetCellValue(row.GetCell(12)).Replace("'", "''");//联系电话
                        var column13 = ExcelRender.GetCellValue(row.GetCell(13)).Replace("'", "''");//设备分类
                        var column14 = ExcelRender.GetCellValue(row.GetCell(14)).Replace("'", "''");//备注信息

                        string errorStr = "";
                        string DutyId = "";//责任人编号
                        string CategoryId="";//设备分类
                        string AddressId = "";//箱子位置
                        //校验信息是否完整
                        if (CheckData(CurrentMaster, column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10, column11, column12, column13, column14, CompanyId, ref AddressId,ref DutyId, ref CategoryId, ref errorStr) == true)
                        {
                            //校验不通过
                            if (column1 == "" && column2 == "" && column3 == "" && column4 == "" && column5 == "" && column6 == "" && column7 == "" && column8 == "" && column9 == "" && column10 == "")
                            {
                                break;//遇到空行,直接跳转
                            }
                            else
                            {
                                #region
                                //添加生产错误信息到excel
                                string errorTime = DateTime.Now.ToString("yyyy年MM月");
                                if (column7.Trim() != "")
                                {
                                    try
                                    {
                                        if (column7.IndexOf("年") > 0 || column7.IndexOf("月") > 0)
                                        {
                                            errorTime = Convert.ToDateTime(column7).ToString("yyyy年MM月");
                                        }
                                        else
                                        {
                                            string time = ToDateTimeValue(column7);
                                            errorTime = Convert.ToDateTime(time).ToString("yyyy年MM月");
                                        }
                                    }
                                    catch
                                    {
                                        errorTime = column7;
                                    }
                                }
                                else {
                                    errorTime = column7;
                                }

                                //添加维修日期错误信息到excel
                                string column8Text = DateTime.Now.ToString("yyyy年MM月");
                                if (column8.Trim() != "")
                                {
                                    try
                                    {
                                        if (column8.IndexOf("年") > 0 || column8.IndexOf("月") > 0)
                                        {
                                            column8Text = Convert.ToDateTime(column8).ToString("yyyy年MM月");
                                        }
                                        else
                                        {
                                            string time = ToDateTimeValue(column8);
                                            column8Text = Convert.ToDateTime(time).ToString("yyyy年MM月");
                                        }
                                    }
                                    catch
                                    {
                                        column8Text = column8;
                                    }
                                }
                                else
                                {
                                    column8Text = column8;
                                }
                                ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, errorTime, column8Text, column9, column10, column11, column12, column13, column14, errorStr);
                                #endregion
                            }
                        }
                        else { 
                            //数据验证通过,封装模版数据
                            int StoreCount = int.Parse(column6);
                            for (int a = 0; a< StoreCount; a++)
                            {
                                string newId = Guid.NewGuid().ToString();//设备主键编号
                                DateTime LostTime = new DateTime();
                                builderValue.Length = 0;

                                //添加设备编号 QRCode
                                builderValue.AppendFormat("'{0}',", newId);//生成二维码
                                //设备名称
                                builderValue.AppendFormat("'{0}',", column2);
                                //设备位置
                                builderValue.AppendFormat("'{0}',", AddressId);
                                //设备型号
                                builderValue.AppendFormat("'{0}',", column4);
                                //设备规格
                                builderValue.AppendFormat("'{0}',", column5);
                                //数量
                                builderValue.AppendFormat("'{0}',", column6);
                                //生产日期
                                if (column7.Trim() != "" && column8.Trim() == "")
                                {
                                    string time="";
                                    if (column7.IndexOf("年") > 0 || column7.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column7).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column7);
                                    }
                                    builderValue.AppendFormat("'{0}',", time);
                                    if (column9 != "")
                                    {
                                        LostTime = Convert.ToDateTime(time).AddYears(int.Parse(column9));
                                    }
                                    else
                                    {
                                        LostTime = Convert.ToDateTime(time).AddYears(int.Parse(ConfigurationManager.AppSettings["LostYear"]));//默认5年过期
                                    }
                                }
                                //维修日期
                                if (column8.Trim() != "")
                                {
                                    string time="";
                                    if (column8.IndexOf("年") > 0 || column8.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column8).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column8);
                                    }
                                    builderValue.AppendFormat("'{0}',", time);
                                    if (column9 != "")
                                    {
                                        LostTime = Convert.ToDateTime(time).AddYears(int.Parse(column9));
                                    }
                                    else
                                    {
                                        LostTime = Convert.ToDateTime(time).AddYears(2);//
                                    }
                                }

                                //责任人
                                builderValue.AppendFormat("'{0}',", DutyId);
                                //介绍 Mark
                                builderValue.AppendFormat("'{0}',", column14);

                                #region ==构建sql语句
                                if (builderValue.ToString() != "")
                                {
                                    #region ==添加信息
                                    builder.Append(insertSql);
                                    builder.Append(" values (");
                                    //主键
                                    string ID = Guid.NewGuid().ToString();
                                    builder.Append("'" + ID + "',");
                                    //添加value赋值
                                    builder.Append(builderValue.ToString());

                                    //巡检日期
                                    DateTime dtNow = DateTime.Now.AddMonths(1);
                                    int days = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);//最后一天的前一天

                                    //巡检日期 MaintenanceDate
                                    builder.AppendFormat("'{0}',", DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(2).AddDays(-2).ToString("yyyy-MM-dd"));
                                    //巡检间隔日期 MaintenanceDay
                                    builder.AppendFormat("'{0}',", days);
                                    //设备类型编号 Gid
                                    builder.AppendFormat("'{0}',", CategoryId);
                                    //二维码图片地址 Img
                                    builder.AppendFormat("'{0}',", QrCode(newId + "S"));// 
                                    //状态 Status
                                    builder.AppendFormat("'{0}',", (int)StatusEnum.正常);
                                    //公司编号 Cid
                                    builder.AppendFormat("'{0}',", CompanyId);
                                    //创建时间 CreateTime
                                    builder.AppendFormat("'{0}',", DateTime.Now.ToString("yyyy-MM-dd"));//
                                    //创建人编号 CreaterId
                                    builder.AppendFormat("'{0}',", CurrentMaster.Id);
                                    //过期时间 LostTime
                                    builder.AppendFormat("'{0}',", LostTime.ToString("yyyy-MM-dd"));
                                    //二维码名称 QrName
                                    builder.AppendFormat("'{0}',", newId);
                                    int MaintenanceStatus = 0;
                                    if (LostTime < DateTime.Now)
                                    {
                                        MaintenanceStatus = -1;
                                    }
                                    //设备状态 MaintenanceStatus -1:设备过期  0，设备正常,1外观正常，2状态正常，3有效期内（自动判断），4其他
                                    builder.AppendFormat("'{0}',", MaintenanceStatus);
                                    //去掉最后一个“,”
                                    builder.Length = builder.Length - 1;
                                    builder.Append(");");
                                    #endregion
                                    Insertcount++;
                                }
                                #endregion
                            }
                        }

                        if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
                        {
                            rowAffected += Bll.ExecuteNonQueryByText(builder.ToString());
                            builder.Clear();
                            builder.Length = 0;
                            LogErrorRecord.Debug("导入设备执行返回记录:" + rowAffected);
                        }
                        #endregion
                    }
                }

                if ( builder.Length > 0)
                {
                    rowAffected += Bll.ExecuteNonQueryByText(builder.ToString());
                    builder.Clear();
                    builder.Length = 0;
                    LogErrorRecord.Debug("导入设备最后一次执行返回记录:" + rowAffected);
                }
                if (ErrorTab.Rows.Count >= 1)
                {
                    tab = ErrorTab;
                }
                else
                {
                    tab = null;
                }
                return Insertcount;
            }
        }

        /// <summary>
        /// 数字转换时间格式
        /// </summary>
        /// <param name="timeStr">数字,如:42095.7069444444/0.650694444444444</param>
        /// <returns>日期/时间格式</returns>
        private string ToDateTimeValue(string strNumber)
        {
            if (!string.IsNullOrWhiteSpace(strNumber))
            {
                Decimal tempValue;
                //先检查 是不是数字;
                if (Decimal.TryParse(strNumber, out tempValue))
                {
                    //天数,取整
                    int day = Convert.ToInt32(Math.Truncate(tempValue));
                    //这里也不知道为什么. 如果是小于32,则减1,否则减2
                    //日期从1900-01-01开始累加 
                    // day = day < 32 ? day - 1 : day - 2;
                    DateTime dt = new DateTime(1900, 1, 1).AddDays(day < 32 ? (day - 1) : (day - 2));

                    //小时:减掉天数,这个数字转换小时:(* 24) 
                    Decimal hourTemp = (tempValue - day) * 24;//获取小时数
                    //取整.小时数
                    int hour = Convert.ToInt32(Math.Truncate(hourTemp));
                    //分钟:减掉小时,( * 60)
                    //这里舍入,否则取值会有1分钟误差.
                    Decimal minuteTemp = Math.Round((hourTemp - hour) * 60, 2);//获取分钟数
                    int minute = Convert.ToInt32(Math.Truncate(minuteTemp));
                    //秒:减掉分钟,( * 60)
                    //这里舍入,否则取值会有1秒误差.
                    Decimal secondTemp = Math.Round((minuteTemp - minute) * 60, 2);//获取秒数
                    int second = Convert.ToInt32(Math.Truncate(secondTemp));
                    //时间格式:00:00:00
                    string resultTimes = string.Format("{0}:{1}:{2}",
                            (hour < 10 ? ("0" + hour) : hour.ToString()),
                            (minute < 10 ? ("0" + minute) : minute.ToString()),
                            (second < 10 ? ("0" + second) : second.ToString()));

                    if (day > 0)
                        return string.Format("{0} {1}", dt.ToString("yyyy-MM-dd"), resultTimes);
                    else
                        return resultTimes;
                }
            }
            return string.Empty;
        }
        #endregion

        

        #endregion

        #region ===保存表单
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysAppointed mod)
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                string Gid = Request["Gid"].ToString();
                string ResponsibleId = Request["ResponsibleId"].ToString();
                string LostYear = Request["LostYear"].ToString();//过期年限
                
                if (!string.IsNullOrEmpty(Request["modify"])) //修改
                {
                    string sql = "";
                    ModSysAppointed model = Bll.LoadData(mod.Id);
                    if (model.Places != mod.Places)
                    { 
                        //地址更换,变更数量
                         sql = " update Fire_FireBox set EquipmentCount=(select count(Id) from Sys_Appointed where Places='" + model.Places + "' and Status!=-1) where Id='" + model.Places + "';";
                      
                    }
                    model.Name = mod.Name;
                    model.ResponsibleId = ResponsibleId;
                    model.Gid = Gid;
                    model.ProductionDate = Convert.ToDateTime(Request["Time"]);
                    model.Places = mod.Places;
                    model.Specifications = mod.Specifications;
                    model.StoreNum = mod.StoreNum;
                    model.Mark = mod.Mark;
                    model.Model = mod.Model;
                    model.Placesed = mod.Placesed;
                    model.LostTime = Convert.ToDateTime(Convert.ToDateTime(Request["Time"])).AddYears(int.Parse(LostYear));//过期
                    //设置设备状态
                    if (model.LostTime < DateTime.Now)
                    {
                        model.MaintenanceStatus = -1;
                    }
                    else
                    {
                        model.MaintenanceStatus = 0;
                    }

                    if (model.QrName != mod.QrName)
                    {
                        //变更Code
                        var tt = new BllSysQRCode().LoadData(mod.QRCode);
                        if (tt != null)
                        {
                            model.QrName = tt.Name;
                            model.QRCode = tt.QrCode;
                            model.Img = tt.Img;
                        }
                        tt.Status = (int)StatusEnum.正常;
                        new BllSysQRCode().Update(tt);//更改状态

                        var cc = new BllSysQRCode().GetQRCodeOrName(mod.QRCode);
                        cc.Status = (int)StatusEnum.禁用;
                        new BllSysQRCode().Update(cc);//更改状态
                    }
                    int result = Bll.Update(model);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "修改失败,请稍后再操作!";
                        LogInsert(OperationTypeEnum.操作, "设备修改", "设备修改失败.");
                    }
                    else {
                        if (sql != "")
                        {
                            Bll.ExecuteNonQueryByText(sql);
                        }
                        LogInsert(OperationTypeEnum.操作, "设备修改", "设备修改成功.");
                    }
                }
                else
                {
                    mod.Cid = CurrentMaster.Cid;
                    if (!string.IsNullOrEmpty(Request["CreateCompanyId"]))
                    {
                        if (Request["CreateCompanyId"].ToString().Trim() != "0")
                        {
                            //单位管理员创建的设备.
                            mod.Cid = Request["CreateCompanyId"].ToString();
                        }
                    }
                    mod.Id = Guid.NewGuid().ToString();
                    mod.CreateTime = DateTime.Now;
                    mod.Status = (int)StatusEnum.正常;
                    mod.CreaterId = CurrentMaster.Id;
                    mod.Gid = Gid;//类别
                    mod.ResponsibleId = ResponsibleId;//责任人编号
                    if (string.IsNullOrEmpty(mod.QRCode))
                    {
                        string code = Guid.NewGuid().ToString();
                        mod.QrName = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString();//时间戳
                        mod.QRCode = code;//二维码编码
                        mod.Img = QrCode(code+"S");//二维码地址 //设备二维码最后大写S,箱子二维码最后大写X
                    }
                    else { 
                        //获取二维码信息
                        var tt = new BllSysQRCode().LoadData(mod.QRCode);
                        if (tt != null)
                        {
                            mod.QrName = tt.Name;
                            mod.QRCode = tt.QrCode;
                            mod.Img = tt.Img;
                        }
                        tt.Status = (int)StatusEnum.正常;
                        new BllSysQRCode().Update(tt);//更改状态
                    }
                    mod.ProductionDate = Convert.ToDateTime(Request["Time"]);
                    //过去日期
                    mod.LostTime = Convert.ToDateTime(mod.ProductionDate).AddYears(int.Parse(LostYear));//过期
                    //巡检日期
                   
                    DateTime dtNow = DateTime.Now.AddMonths(1);
                    int days = DateTime.DaysInMonth(dtNow.Year ,dtNow.Month);//最后一天的前一天
                    mod.MaintenanceDate = Convert.ToDateTime(DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-2).ToString("yyyy-MM-dd"));//下一个月的最后一天的前一天
                    mod.MaintenanceDay = days;
                    //设置设备状态
                    if (mod.LostTime < DateTime.Now)
                    {
                        mod.MaintenanceStatus = -1;
                    }
                    else {
                        mod.MaintenanceStatus = 0;
                    }
                    int result = Bll.Insert(mod);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = " 保存失败,请稍后再操作!";
                        LogInsert(OperationTypeEnum.操作, "设备保存", "设备保存失败.");
                    }
                    else
                    {
                        LogInsert(OperationTypeEnum.操作, "设备保存", "设备保存成功.");
                    }
                }
            }
            catch (Exception ex)
            {
                json.msg = "保存失败！";
                json.success = false;
                LogInsert(OperationTypeEnum.异常, "设备保存", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region ===根据id查询实体 GetData
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetData(string id)
        {
            var mod = Bll.GetModelByWhere(" and Id='" + id + "'");
            return Json(new { data = mod }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ===删除 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                string sql = "update Sys_Appointed set Status="+(int)StatusEnum.删除+" where Id in(" + id + ")";
                if (Bll.ExecuteNonQueryByText(sql) > 0)
                {
                    msg.success = true;
                    LogInsert(OperationTypeEnum.操作, "设备删除", "设备删除成功.");
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "设备删除", "设备删除失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "设备删除", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion

        #region ===清空 DeleteAll
        /// <summary>
        /// 清空
        /// </summary>
        public void DeleteAll()
        {
            var msg = new ModJsonResult();
            try
            {
                string Cid = CurrentMaster.Cid;
                if (!string.IsNullOrEmpty(Request["CreateCompanyId"]))
                {
                    if (Request["CreateCompanyId"].ToString().Trim() != "0")
                    {
                        Cid = Request["CreateCompanyId"].ToString();
                    }
                }
                string sql = "delete from Sys_Appointed where Cid='" + Cid + "';";
                Bll.ExecuteNonQueryByText(sql);
                msg.success = true;
                LogInsert(OperationTypeEnum.操作, "设备清空", "设备清空成功.");
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "设备清空", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion
        
        #region ===禁用和启用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DisableUse(string id)
        {
            var msg = new ModJsonResult();
            if (!Bll.UpdateStatue((int)StatusEnum.禁用, id))
            {
                msg.success = false;
                msg.msg = "操作失败";
                LogInsert(OperationTypeEnum.操作, "设备禁用", "操作失败.");
            }
            else {
                LogInsert(OperationTypeEnum.操作, "设备禁用", "操作成功.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void EnableUse(string id)
        {
            var msg = new ModJsonResult();
            if (!Bll.UpdateStatue((int)StatusEnum.正常, id))
            {
                msg.success = false;
                msg.msg = "操作失败";
                LogInsert(OperationTypeEnum.操作, "设备启用", "操作失败.");
            }
            else
            {
                LogInsert(OperationTypeEnum.操作, "设备启用", "操作成功.");
            }
        }

        #endregion

        #region ===查询是否存在 SearchNameExits
        /// <summary>
        /// name查询是否存在
        /// </summary>
        public void SearchNameExits(string name, string key, string SysId)
        {
            var msg = new ModJsonResult();
            int count = 0;
            if (key != "" && key != null)
            {
                msg.success = Bll.Exists("Sys_Appointed", " and Cid='" + CurrentMaster.Cid + "' and Name='" + name + "' and Id<>'" + key + "' and Status<>" + (int)StatusEnum.删除, out count);
            }
            else
            {
                msg.success = Bll.Exists("Sys_Appointed", " and Cid='" + CurrentMaster.Cid + "' and Name='" + name + "' and Status<>" + (int)StatusEnum.删除, out count);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ===数量统计 ChartOrder
        /// <summary>
        /// 数量统计
        /// </summary>
        public void ChartOrder()
        {
            var msg = new ModJsonResult();
            Hashtable info = new Hashtable();
            string where = "";

            if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
            {
                
            }
            else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.单位用户)
            {
                where += " and Cid='" + CurrentMaster.Cid + "'";
            }
            else if (CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防用户)
            {
                where += " and Cid in (select EmployerId from Sys_CompanyCognate where Cid='" + CurrentMaster.Cid + "' and Status=1 and SelectType=2)";
            }

            DataSet ds = Bll.ChartTotla(where);
            if (ds.Tables[0].Rows.Count > 0)
            {
                info.Add("Year", ds.Tables[0].Rows[0]["年份"].ToString());
                info.Add("1", ds.Tables[0].Rows[0]["1月"].ToString());
                info.Add("2", ds.Tables[0].Rows[0]["2月"].ToString());
                info.Add("3", ds.Tables[0].Rows[0]["3月"].ToString());
                info.Add("4", ds.Tables[0].Rows[0]["4月"].ToString());
                info.Add("5", ds.Tables[0].Rows[0]["5月"].ToString());
                info.Add("6", ds.Tables[0].Rows[0]["6月"].ToString());
                info.Add("7", ds.Tables[0].Rows[0]["7月"].ToString());
                info.Add("8", ds.Tables[0].Rows[0]["8月"].ToString());
                info.Add("9", ds.Tables[0].Rows[0]["9月"].ToString());
                info.Add("10", ds.Tables[0].Rows[0]["10月"].ToString());
                info.Add("11", ds.Tables[0].Rows[0]["11月"].ToString());
                info.Add("12", ds.Tables[0].Rows[0]["12月"].ToString());
                msg.success = true;
            }
            else
            {
                info.Add("Year", 0);
                info.Add("1", 0);
                info.Add("2", 0);
                info.Add("3", 0);
                info.Add("4", 0);
                info.Add("5", 0);
                info.Add("6", 0);
                info.Add("7", 0);
                info.Add("8", 0);
                info.Add("9", 0);
                info.Add("10", 0);
                info.Add("11", 0);
                info.Add("12", 0);
                msg.success = true;
            }
            msg.data = JsonHelper.ToJson(info);
            WriteJsonToPage(msg.ToString());
        }
        #endregion

        #region ==统计每个单位的设备过期数量 ChartPart
        /// <summary>
        /// 数量统计
        /// </summary>
        public void ChartPart()
        {
            var msg = new ModJsonResult();
            List<Hashtable> info = new List<Hashtable>();
            DataSet ds = Bll.ChartPart();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Hashtable table = new Hashtable();
                    table.Add("name", ds.Tables[0].Rows[i]["Name"].ToString());
                    table.Add("value", ds.Tables[0].Rows[i]["NumCount"].ToString());
                    info.Add(table);
                }
            }
            msg.data = JsonHelper.ToJson(info);
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==导出数据ImportOut
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="mod"></param>
        public void ImportOut()
        {
            string hearder = "二维码编码,图片,设备名称,设备规格,设备型号,设备位置,生产日期,过期日期,责任人,责任部门,电话,所属分类,添加时间";
            string column = "QRCode,Img,Name,Specifications,Model,PlacesName,ProductionDate,LostTime,Responsible,DeptName,LoginName,GroupName,CreateTime";
            title = hearder.Split(',');  //导出的标题
            field = column.Split(',');  //导出对应字段
            var search = base.GetSearch();
            try
            {
                string IdList = Request["IdList"].ToString();
                string Cid = Request["Cid"].ToString();
                if (string.IsNullOrEmpty(Cid))
                {
                    Cid = CurrentMaster.Cid;
                }
                search.AddCondition("Id in(" + IdList + ")");//过滤选中的记录
                search.AddCondition("Status!=" + (int)StatusEnum.删除);
                //search.AddCondition("Cid='" + Cid + "'");
                DataTable ds = new BllSysAppointed().GetList("View_Appointed", " and " + search.GetConditon(), "", 0).Tables[0];
                if (ds.Rows.Count > 0)
                {
                    ToExcel(ds);
                }
                LogInsert(OperationTypeEnum.操作, "设备批量导出", "设备导出成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备批量导出", "操作异常信息:" + ex);
            }
        }

        // <summary>
        /// 图片转换成字节流
        /// </summary>
        /// <param name="img">要转换的Image对象</param>
        /// <returns>转换后返回的字节流</returns>
        public byte[] ImgToByt(Image img)
        {
            MemoryStream ms = new MemoryStream();
            byte[] imagedata = null;
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imagedata = ms.GetBuffer();
            return imagedata;
        }
        /// <summary>
        /// 字节流转换成图片
        /// </summary>
        /// <param name="byt">要转换的字节流</param>
        /// <returns>转换得到的Image对象</returns>
        public Image BytToImg(byte[] byt)
        {
            MemoryStream ms = new MemoryStream(byt);
            Image img = Image.FromStream(ms);
            return img;
        }
        //
        /// <summary>
        /// 根据图片路径返回图片的字节流byte[]
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>返回的字节流</returns>
        public byte[] getImageByte(string imagePath)
        {
            FileStream files = new FileStream(imagePath, FileMode.Open);
            byte[] imgByte = new byte[files.Length];
            files.Read(imgByte, 0, imgByte.Length);
            files.Close();
            return imgByte;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dt"></param>
        public void ToExcel1(DataTable dt)
        {
            AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
            doc.FileName = DateTime.Now.ToString("yyyyMMdd") + ".xls";
            string SheetName = "Sheet1";
            //记录条数
            int mCount = dt.Rows.Count;
            Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
            Cells cells = sheet.Cells;

            //第一行表头
            for (int i = 0; i < title.Length; i++)
            {
                cells.Add(1, i + 1, title[i].Trim());
            }
            for (int m = 0; m < mCount; m++)
            {
                for (int j = 0; j < title.Length; j++)
                {
                    //可以直接取图片的地址
                    string filename = Server.MapPath("~/" + dt.Rows[m]["Img"].ToString());
                    byte[] filedata = getImageByte(filename); ;
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(filedata);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    img.Save(filename);

                    // cells.Add(m + 2, j + 1, dt.Rows[m][j].ToString());

                    cells.Add(m + 2, j + 1, img);
                }
            }
            doc.Send();
            Response.End();
        }

        #region 导出
        protected void ToExcel(DataTable dt)
        {
            if (dt != null)
            {
                #region 操作excel
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                xlWorkBook = new Microsoft.Office.Interop.Excel.Application().Workbooks.Add(Type.Missing);
                xlWorkBook.Application.Visible = false;
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets[1];
                //设置标题
                xlWorkSheet.Cells[1, 1] = "二维码编号";
                xlWorkSheet.Cells[1, 2] = "图片";
                xlWorkSheet.Cells[1, 3] = "设备名称";
                xlWorkSheet.Cells[1, 4] = "设备规格";
                xlWorkSheet.Cells[1, 5] = "设备型号";
                xlWorkSheet.Cells[1, 6] = "设备位置";
                xlWorkSheet.Cells[1, 7] = "生产日期";
                xlWorkSheet.Cells[1, 8] = "过期日期";
                xlWorkSheet.Cells[1, 9] = "责任人";
                xlWorkSheet.Cells[1, 10] = "责任部门";
                xlWorkSheet.Cells[1, 11] = "电话";
                xlWorkSheet.Cells[1, 12] = "所属分类";
                xlWorkSheet.Cells[1, 13] = "添加时间";
                //设置宽度            
                ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1]).ColumnWidth =35;//图片的宽度
                ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 2]).ColumnWidth =15;//图片的宽度
                ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 7]).ColumnWidth =20;//图片的宽度
                ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 8]).ColumnWidth = 20;//图片的宽度
                ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 11]).ColumnWidth = 20;//图片的宽度
                ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 13]).ColumnWidth = 20;//图片的宽度
                //设置字体
                xlWorkSheet.Cells.Font.Size = 12;
                xlWorkSheet.Cells.Rows.RowHeight = 100;

                #region 为excel赋值

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //为单元格赋值。
                    xlWorkSheet.Cells[i + 2, 1] = dt.Rows[i]["QRCode"].ToString();
                    xlWorkSheet.Cells[i + 2, 2] = "";
                    xlWorkSheet.Cells[i + 2, 3] = dt.Rows[i]["Name"].ToString();
                    xlWorkSheet.Cells[i + 2, 4] = dt.Rows[i]["Specifications"].ToString();
                    xlWorkSheet.Cells[i + 2, 5] = dt.Rows[i]["Model"].ToString();
                    xlWorkSheet.Cells[i + 2, 6] = dt.Rows[i]["PlacesName"].ToString();
                    xlWorkSheet.Cells[i + 2, 7] = Convert.ToDateTime(dt.Rows[i]["ProductionDate"].ToString()).ToString("yyyy-MM-dd");
                    xlWorkSheet.Cells[i + 2, 8] = Convert.ToDateTime(dt.Rows[i]["LostTime"].ToString()).ToString("yyyy-MM-dd");
                    xlWorkSheet.Cells[i + 2, 9] = dt.Rows[i]["Responsible"].ToString();
                    xlWorkSheet.Cells[i + 2, 10] = dt.Rows[i]["DeptName"].ToString();
                    xlWorkSheet.Cells[i + 2, 11] = dt.Rows[i]["LoginName"].ToString();
                    xlWorkSheet.Cells[i + 2, 12] = dt.Rows[i]["GroupName"].ToString();
                    xlWorkSheet.Cells[i + 2, 13] = Convert.ToDateTime(dt.Rows[i]["CreateTime"].ToString()).ToString("yyyy-MM-dd");
                    string filename = Server.MapPath("~/" + dt.Rows[i]["Img"].ToString());
                    if (System.IO.File.Exists(filename))
                    {
                        //声明一个pictures对象,用来存放sheet的图片
                        //xlWorkSheet.Shapes.AddPicture(filename, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue,220,100+ i * 100, 100, 100);
                    }
                }
                #endregion

                #region 保存excel文件
                string filePath = Server.MapPath("/UploadFile/QrExport/");
                new FileHelper().CreateDirectory(filePath);
                filePath += DateTime.Now.ToString("yyyymmddHHmmss") + "设备批量导出.xls";
                try
                {
                    xlWorkBook.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);
                }
                catch { }
                xlWorkBook.Application.Quit();
                xlWorkSheet = null;
                xlWorkBook = null;
                GC.Collect();
                System.GC.WaitForPendingFinalizers();
                #endregion
                #endregion
                #region 导出到客户端
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.AppendHeader("content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("设备批量导出", System.Text.Encoding.UTF8) + ".xls");
                Response.ContentType = "Application/excel";
                Response.WriteFile(filePath);
                Response.End();
                #endregion
                KillProcessexcel("EXCEL");
            }
        }
        #endregion

        #region 杀死进程
        private void KillProcessexcel(string processName)
        { //获得进程对象，以用来操作
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程
            try
            {
                //获得需要杀死的进程名
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                { //立即杀死进程
                    thisproc.Kill();
                }
            }
            catch (Exception Exc)
            {
                throw new Exception("", Exc);
            }
        }
        #endregion


        #endregion

        #region ===复制添加 CopyAll
        /// <summary>
        /// 复制添加
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void CopyAll()
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                string id = Request["id"].ToString();
                ModSysAppointed model = Bll.LoadData(id);
                model.Id = Guid.NewGuid().ToString();
                model.CreateTime = DateTime.Now;
                model.CreaterId = CurrentMaster.Id;
                string code = Guid.NewGuid().ToString();
                model.QrName = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString();//时间戳
                model.QRCode = code;//二维码编码
                model.Img = QrCode(code+"S");//二维码地址
                int result = Bll.Insert(model);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "复制失败,请稍后再操作!";
                    LogInsert(OperationTypeEnum.操作, "设备复制添加", "设备保存失败.");
                }
                else
                {
                    LogInsert(OperationTypeEnum.操作, "设备复制添加", "设备保存成功.");
                }
            }
            catch (Exception ex)
            {
                json.msg = "保存失败！";
                json.success = false;
                LogInsert(OperationTypeEnum.异常, "设备复制添加", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region ===恢复设备正常状态 RightStatus
        /// <summary>
        /// 恢复设备正常状态
        /// </summary>
        /// <param name="id"></param>
        public void RightStatus(string id)
        {
            var msg = new ModJsonResult();
            var model = Bll.LoadData(id);
            model.MaintenanceStatus = 0;
            if (Bll.Update(model)>0)
            {
                msg.success = false;
                msg.msg = "操作失败";
                LogInsert(OperationTypeEnum.操作, "恢复设备状态", "操作失败.");
            }
            else
            {
                LogInsert(OperationTypeEnum.操作, "恢复设备状态", "操作成功.");
            }
        }
        #endregion

    }
}
