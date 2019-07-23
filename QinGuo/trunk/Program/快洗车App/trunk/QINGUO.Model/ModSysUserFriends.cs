using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 朋友列表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Sys_UserFriends")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysUserFriends
    {
        /// <summary>
        /// 编号
        /// </summary>		
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        
        /// <summary>
        /// 用户Id
        /// </summary>		
        private string _UserId;
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }


        /// <summary>
        /// 用户好友Id
        /// </summary>		
        private string _FriendsId;
        public string FriendsId
        {
            get { return _FriendsId; }
            set { _FriendsId = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime? _createtime;
        public DateTime? CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// 0:黑名单 1:正常
        /// </summary>
        /// 字段长度:4
        /// 是否为空:true
        public int Status { get; set; }
    }
}
