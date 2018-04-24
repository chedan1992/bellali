using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_BtnValue")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysBtnValue
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
        /// 按钮名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 按钮名称tip
        /// </summary>		
        private string _nameTip;
        public string NameTip
        {
            get { return _nameTip; }
            set { _nameTip = value; }
        }

        /// <summary>
        /// 按钮值
        /// </summary>		
        private long _value;
        public long Value
        {
            get { return _value; }
            set { _value = value; }
        }
        /// <summary>
        /// 按钮操作方法
        /// </summary>		
        private string _actionname;
        public string ActionName
        {
            get { return _actionname; }
            set { _actionname = value; }
        }
        /// <summary>
        /// 按钮图标名称
        /// </summary>		
        private string _iconname;
        public string IConName
        {
            get { return _iconname; }
            set { _iconname = value; }
        }
        /// <summary>
        /// 按钮状态(1为启用，0为禁用)
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
        public DateTime CreateTime
        {
            get
            {

                if (_createtime.ToShortDateString().IndexOf("0001") > -1)
                    return DateTime.Now;
                else
                    return _createtime;
            }
            set
            {
                    _createtime = value;
            }
        }

      
    }
}
