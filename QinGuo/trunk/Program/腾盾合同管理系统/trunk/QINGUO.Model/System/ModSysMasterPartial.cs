using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Common;

namespace QINGUO.Model
{
    /// <summary>
    /// 用户功能权限类
    /// </summary>
    public partial class ModSysMaster
    {
        private List<ModSysFun> _frameSysFun = new List<ModSysFun>();

        private ModSysCompany _company = null;
        /// <summary>
        /// 所属公司
        /// </summary>
        [Property(IsDataBaseField = false)]
        [Dapper.Ignore]
        public ModSysCompany Company
        {
            get { return _company; }
            set { _company = value; }
        }

        private string _roleIdList = string.Empty;
        /// <summary>
        /// 用户角色主键
        /// </summary>
        [Property(IsDataBaseField = false)]
        [Dapper.Ignore]
        public string RoleIdList
        {
            get { return _roleIdList; }
            set { _roleIdList = value; }
        }

        private int _lookPower = (int)LookPowerEnum.查看自建;
        /// <summary>
        /// 用户查看类型
        /// </summary>
        [Property(IsDataBaseField = false)]
        [Dapper.Ignore]
        public int LookPower
        {
            get { return _lookPower; }
            set { _lookPower = value; }
        }

        private int _comStatus = 1;
        /// <summary>
        /// 公司状态
        /// </summary>
        [Property(IsDataBaseField = false)]
        [Dapper.Ignore]
        public int ComStatus
        {
            get { return _comStatus; }
            set { _comStatus = value; }
        }
    }
}
