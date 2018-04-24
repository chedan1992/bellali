using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysFunLinkBtnValue : IBaseDAL<ModSysFunLinkBtnValue>
    {
        /// <summary>
        /// 批量添加按钮信息
        /// </summary>
        /// <param name="BtnId">按钮列表</param>
        /// <param name="FunId">页面主键</param>
        /// <returns></returns>
        void BatchInsert(string BtnId, string FunId);
    }
}
