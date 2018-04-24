using System;
namespace QINGUO.Model{

	//表Sys_Fun和表Sys_BtnValue的关联
    //存放每个导航链接页面所需要的操作按钮信息
    [Serializable]
    [Dapper.TableNameAttribute("Sys_FunLinkBtnValue")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
	public class ModSysFunLinkBtnValue
	{
        /// <summary>
        /// 编号
        /// </summary>		
        public string Id{get; set; }
      	/// <summary>
		/// 系统功能主键
        /// </summary>		
        public string FunId{get; set;}        
		/// <summary>
		/// 按钮主键
        /// </summary>		
        public string BtnId{get; set;}

        public int FunSort { get; set; }
	}
}
