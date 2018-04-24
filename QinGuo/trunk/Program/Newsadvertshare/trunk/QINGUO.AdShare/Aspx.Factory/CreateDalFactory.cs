using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using QINGUO.IDAL;

namespace QINGUO.Factory
{
    public class CreateDalFactory : IDataAccess
    {

        /************************************系统类**************************************************/

        #region ==系统类
        /// <summary>
        /// 系统按钮表
        /// </summary>
        /// <returns></returns>
        public static ISysBtnValue CreateDalSysBtnValue()
        {
            string className = path + ".SysBtnDAL";
            return (ISysBtnValue)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 系统栏目链接表
        /// </summary>
        /// <returns></returns>
        public static ISysFun CreateDalSysFun()
        {
            string className = path + ".SysFunDAL";
            return (ISysFun)Assembly.Load(path).CreateInstance(className);
        }
        /// <summary>
        /// 模块链接按钮表
        /// </summary>
        /// <returns></returns>
        public static ISysFunLinkBtnValue CreateDalSysFunLinkBtnValue()
        {
            string className = path + ".SysFunLinkBtnValueDAL";
            return (ISysFunLinkBtnValue)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 用户角色表
        /// </summary>
        /// <returns></returns>
        public static ISysMasterRole CreateDalSysMasterRole()
        {
            string className = path + ".SysMasterRoleDAL";
            return (ISysMasterRole)Assembly.Load(path).CreateInstance(className);
        }
        /// <summary>
        /// 角色表
        /// </summary>
        /// <returns></returns>
        public static ISysRole CreateDalSysRole()
        {
            string className = path + ".SysRoleDAL";
            return (ISysRole)Assembly.Load(path).CreateInstance(className);
        }
        /// <summary>
        /// 角色功能表
        /// </summary>
        /// <returns></returns>
        public static ISysRoleFun CreateDalSysRoleFun()
        {
            string className = path + ".SysRoleFunDAL";
            return (ISysRoleFun)Assembly.Load(path).CreateInstance(className);
        }
        /// <summary>
        /// 角色权限范围表
        /// </summary>
        /// <returns></returns>
        public static ISysRoleRangeData CreateDalSysRoleRangeData()
        {
            string className = path + ".SysRoleRangeDataDAL";

            return (ISysRoleRangeData)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 分组表
        /// </summary>
        /// <returns></returns>
        public static ISysGroup CreateSysGroup()
        {
            string className = path + ".SysGroupDAL";
            return (ISysGroup)Assembly.Load(path).CreateInstance(className);
        }
         /// <summary>
        /// 字典分类表
        /// </summary>
        /// <returns></returns>
        public static ISysDirc CreateSysDirc()
        {
            string className = path + ".SysDircDAL";

            return (ISysDirc)Assembly.Load(path).CreateInstance(className);
        }
        
          /// <summary>
        /// 型号表
        /// </summary>
        /// <returns></returns>
        public static ISysModel CreateSysModel()
        {
            string className = path + ".SysModelDAL";

            return (ISysModel)Assembly.Load(path).CreateInstance(className);
        }
        

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        public static ISysUser CreateDalSysUser()
        {
            string className = path + ".SysUserDAL";
            return (ISysUser)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 短信验证码
        /// </summary>
        /// <returns></returns>
        public static ISysMessageAuthCode CreateSysMessageAuthCode()
        {
            string className = path + ".SysMessageAuthCodeDAL";
            return (ISysMessageAuthCode)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 商户管理员
        /// </summary>
        /// <returns></returns>
        public static ISysMaster CreateSysMaster()
        {
            string className = path + ".SysMasterDAL";
            return (ISysMaster)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <returns></returns>
        public static ISysFeedback CreateSysFeedback()
        {
            string className = path + ".SysFeedbackDAL";
            return (ISysFeedback)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 省份信息
        /// </summary>
        /// <returns></returns>
        public static ISysHatProvince CreateSysHatProvince()
        {
            string className = path + ".SysHatProvinceDAL";
            return (ISysHatProvince)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 城市信息
        /// </summary>
        /// <returns></returns>
        public static ISysHatcity CreateSysHatcity()
        {
            string className = path + ".SysHatcityDAL";
            return (ISysHatcity)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 区信息
        /// </summary>
        /// <returns></returns>
        public static ISysHatarea CreateSysHatarea()
        {
            string className = path + ".SysHatareaDAL";
            return (ISysHatarea)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 推送信息类
        /// </summary>
        /// <returns></returns>
        public static ISysPushInfo CreateSysPushInfo()
        {
            string className = path + ".SysPushInfoDAL";
            return (ISysPushInfo)Assembly.Load(path).CreateInstance(className);
        }
        /// <summary>
        /// 推送用户类
        /// </summary>
        /// <returns></returns>
        public static ISysPushUser CreateSysPushUser()
        {
            string className = path + ".SysPushUserDAL";
            return (ISysPushUser)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 公司
        /// </summary>
        /// <returns></returns>
        public static ISysCompany CreateSysCompany()
        {
            string className = path + ".SysCompanyDAL";
            return (ISysCompany)Assembly.Load(path).CreateInstance(className);
        }
        /// <summary>
        /// 数据库备份表
        /// </summary>
        /// <returns></returns>
        public static ISysDataBaseBack CreateSysDataBaseBack()
        {
            string className = path + ".SysDataBaseBackDAL";
            return (ISysDataBaseBack)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 系统日志类
        /// </summary>
        /// <returns></returns>
        public static ISysOperateLog CreateSysOperateLog()
        {
            string className = path + ".SysOperateLogDAL";
            return (ISysOperateLog)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 层级分类
        /// </summary>
        /// <returns></returns>
        public static ISysCategory CreateSysCategory()
        {
            string className = path + ".SysCategoryDAL";
            return (ISysCategory)Assembly.Load(path).CreateInstance(className);
        }

          /// <summary>
        /// 商品分类
        /// </summary>
        /// <returns></returns>
        public static IShopCategory CreateShopCategoryDAL()
        {
            string className = path + ".ShopCategoryDAL";
            return (IShopCategory)Assembly.Load(path).CreateInstance(className);
        }
        /// <summary>
        /// 商品
        /// </summary>
        /// <returns></returns>
        public static IShopGoods CreateShopGoodsDAL()
        {
            string className = path + ".ShopGoodsDAL";
            return (IShopGoods)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 系统图片
        /// </summary>
        /// <returns></returns>
        public static ISysImgPic CreateSysImgPicDAL()
        {
            string className = path + ".SysImgPicDAL";
            return (ISysImgPic)Assembly.Load(path).CreateInstance(className);
        }
        

        #endregion

        /*********************************第三方接口类*********************************************/




        /********************************* 业务类*********************************************/

        /// <summary>
        /// 广告操作类
        /// </summary>
        /// <returns></returns>
        public static IAdActive CreateAdActive()
        {
            string className = path + ".AdActiveDAL";
            return (IAdActive)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 订单操作类
        /// </summary>
        /// <returns></returns>
        public static IOrderRunOrder CreateOrderRunOrder()
        {
            string className = path + ".OrderRunOrderDAL";
            return (IOrderRunOrder)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 用户金额类
        /// </summary>
        /// <returns></returns>
        public static IOrderUserExtended CreateOrderUserExtended()
        {
            string className = path + ".OrderUserExtendedDAL";
            return (IOrderUserExtended)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 用户提现记录
        /// </summary>
        /// <returns></returns>
        public static IOrderUserMoneyRecord CreateOrderUserMoneyRecord()
        {
            string className = path + ".OrderUserMoneyRecordDAL";
            return (IOrderUserMoneyRecord)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// 订单评价类
        /// </summary>
        /// <returns></returns>
        public static IOrderEvaluation CreateOrderEvaluation()
        {
            string className = path + ".OrderEvaluationDAL";
            return (IOrderEvaluation)Assembly.Load(path).CreateInstance(className);
        }

        public static ISysCompanyPaySet CreateSysCompanyPaySet()
        {
            string className = path + ".SysCompanyPaySetDAL";
            return (ISysCompanyPaySet)Assembly.Load(path).CreateInstance(className);
        }
        public static ISysUserFriends CreateDalSysUserFriends()
        {
            string className = path + ".SysUserFriendsDAL";
            return (ISysUserFriends)Assembly.Load(path).CreateInstance(className);
        }
        public static ISysRemarkSetting CreateSysRemarkSetting()
        {
            string className = path + ".SysRemarkSettingDAL";
            return (ISysRemarkSetting)Assembly.Load(path).CreateInstance(className);
        }
        public static IEDynamic CreateDalEDynamic()
        {
            string className = path + ".EDynamicDAL";
            return (IEDynamic)Assembly.Load(path).CreateInstance(className);
        }
        public static ISysFlow CreateDalSysFlow()
        {
            string className = path + ".SysFlowDAL";
            return (ISysFlow)Assembly.Load(path).CreateInstance(className);
        }
        public static ISysQRCode CreateDalSysQRCode()
        {
            string className = path + ".SysQRCodeDAL";
            return (ISysQRCode)Assembly.Load(path).CreateInstance(className);
        }
        /// <summary>
        /// 设备箱子 管理
        /// </summary>
        /// <returns></returns>
        public static IFireBox CreateDalFireBox()
        {
            string className = path + ".FireBoxDAL";
            return (IFireBox)Assembly.Load(path).CreateInstance(className);
        }
        
        public static ISysAppointed CreateDalSysAppointed()
        {
            string className = path + ".SysAppointedDAL";
            return (ISysAppointed)Assembly.Load(path).CreateInstance(className);
        }

        public static ISysAppointCheckNotes CreateDalSysAppointCheckNotes()
        {
            string className = path + ".SysAppointCheckNotesDAL";
            return (ISysAppointCheckNotes)Assembly.Load(path).CreateInstance(className);
        }

        public static ISysPushMessage CreateSysPushMessage()
        {
            string className = path + ".SysPushMessageDAL";
            return (ISysPushMessage)Assembly.Load(path).CreateInstance(className);
        }

        public static ISysCompanyCognate CreateSysCompanyCognate()
        {
            string className = path + ".SysCompanyCognateDAL";
            return (ISysCompanyCognate)Assembly.Load(path).CreateInstance(className);
        }

        public static IAAShare CreateAAShare()
        {
            string className = path + ".AAShareDAL";
            return (IAAShare)Assembly.Load(path).CreateInstance(className);
        }

        public static ISysCollection CreateSysCollection()
        {
            string className = path + ".SysCollectionDAL";
            return (ISysCollection)Assembly.Load(path).CreateInstance(className);
        }
    }
}
