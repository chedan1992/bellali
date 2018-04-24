using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_PushInfo")]
    [Dapper.PrimaryKeyAttribute("PropelId", autoIncrement = false)]
    public class ModSysPushInfo
    {
        /// <summary>
        /// 编号
        /// </summary>		
        private string _propelid;
        public string PropelId
        {
            get { return _propelid; }
            set { _propelid = value; }
        }
        /// <summary>
        /// 推送类型（1:直接内容）
        /// </summary>		
        private int _typeid;
        public int TypeId
        {
            get { return _typeid; }
            set { _typeid = value; }
        }
        /// <summary>
        /// 推送标题
        /// </summary>		
        private string _ptitle;
        public string PTitle
        {
            get { return _ptitle; }
            set { _ptitle = value; }
        }
        /// <summary>
        /// 推送内容
        /// </summary>		
        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }
        /// <summary>
        /// 接收对象（0为关联会员，1为手机号码，2为机器码）
        /// </summary>		
        private string _receiverobject;
        public string ReceiverObject
        {
            get { return _receiverobject; }
            set { _receiverobject = value; }
        }
        /// <summary>
        /// 关联编号
        /// </summary>		
        private string _correlativeid;
        public string CorrelativeId
        {
            get { return _correlativeid; }
            set { _correlativeid = value; }
        }
        /// <summary>
        /// 所属公司
        /// </summary>		
        private string _companyid;
        public string CompanyId
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        /// <summary>
        /// 创建者
        /// </summary>		
        private string _creatorid;
        public string CreatorId
        {
            get { return _creatorid; }
            set { _creatorid = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime _addtime;
        public DateTime Addtime
        {
            get { return _addtime; }
            set { _addtime = value; }
        }
        /// <summary>
        /// 推送模式（1为app，2为智能，3为短信）
        /// </summary>		
        private int _model;
        public int Model
        {
            get { return _model; }
            set { _model = value; }
        }
        /// <summary>
        /// 发送状态（发送中、发送完成）
        /// </summary>		
        private string _state;
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// 短信签名
        /// </summary>		
        private string _msgentname;
        public string MsgEntName
        {
            get { return _msgentname; }
            set { _msgentname = value; }
        }
        /// <summary>
        /// 智能模式时间(分钟)
        /// </summary>		
        private int _latetime;
        public int LateTime
        {
            get { return _latetime; }
            set { _latetime = value; }
        }        
    }
}
