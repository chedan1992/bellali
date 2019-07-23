/*---------------------------------------------------------------- 
// @青果科技. All Rights Reserved. 版权所有。  
// 
// 文件名：品牌管理
// 文件功能描述：  
// 
// 创建人： zj
// 创建日期： 2016-2-7
// 
// 修改标识： 
// 修改描述： 
// 
// 修改标识： 
// 修改描述： 
//----------------------------------------------------------------*/
using System;
namespace QINGUO.Model
{

    //E_Elevator
    [Serializable]
    [Dapper.TableNameAttribute("E_ElevatorBrand")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModEElevatorBrand
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
        /// 名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Img
        /// </summary>			
        private string _img;
        public string Img
        {
            get { return _img; }
            set { _img = value; }
        }


        /// <summary>
        /// Status
        /// </summary>			
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// 总平台管理
        /// </summary>			
        private string _sysId;
        public string SysId
        {
            get { return _sysId; }
            set { _sysId = value; }
        }

        /// <summary>
        /// CreateTime
        /// </summary>			
        private DateTime _createTime;
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 创建者
        /// </summary>		
        private string _createrId;
        public string CreaterId
        {
            get { return _createrId; }
            set { _createrId = value; }
        }
    }
}