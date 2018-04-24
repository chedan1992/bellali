using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_PushUser")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysPushUser
    {
        /// <summary>
        /// 编号
        /// </summary>		
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        /// <summary>
        /// 推送编号
        /// </summary>		
        private string _propelid;
        public string PropelId
        {
            get { return _propelid; }
            set { _propelid = value; }
        }
        /// <summary>
        /// 用户编号(当接受对象为手机的时候是手机号码，为机器码的时候是机器码)
        /// </summary>		
        private string _userid;
        public string UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// 状态(1:未读，2：已读)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime _createtime;
        public DateTime Createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// 0为未发短信，1为已发送。
        /// </summary>		
        private int _msgstate;
        public int Msgstate
        {
            get { return _msgstate; }
            set { _msgstate = value; }
        }        
    }
}
