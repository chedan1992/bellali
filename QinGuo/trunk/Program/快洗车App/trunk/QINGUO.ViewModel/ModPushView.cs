using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aspx.ViewModel
{
    /// <summary>
    /// 推送类
    /// </summary>
    public class ModPushView
    {
        public string Id { get; set; }//主键
        public int PushType { get; set; }//推送类型，依次往后推(0:考勤通知)
        public DateTime? createTime { get; set; }//创建时间
        public string Infomation { get; set; }//需求内容
    }
}
