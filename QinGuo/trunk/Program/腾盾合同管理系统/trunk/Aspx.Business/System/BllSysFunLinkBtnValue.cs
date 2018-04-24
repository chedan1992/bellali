using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.IDAL;

namespace QINGUO.Business
{
    public class BllSysFunLinkBtnValue : BllBase<ModSysFunLinkBtnValue>
    {
        private ISysFunLinkBtnValue dal = CreateDalFactory.CreateDalSysFunLinkBtnValue();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }

        /// <summary>
        /// 批量添加按钮信息
        /// </summary>
        /// <param name="btnId">按钮列表</param>
        /// <param name="funId">页面主键</param>
        /// <returns></returns>
        public void BatchInsert(string btnId, string funId)
        {
            dal.BatchInsert(btnId, funId);
        }
    }
}
