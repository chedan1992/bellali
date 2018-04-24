using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysBtnValue : IBaseDAL<ModSysBtnValue>
    {
        /// <summary>
        /// 
        /// 获得按钮选择左边列表
        /// </summary>
        /// <param name="Id">页面导航id</param>
        /// <returns></returns>
        DataSet GetBtnLeftSelect(string Id);

        /// <summary>
        /// 获得按钮选择右边列表
        /// </summary>
        /// <param name="Id">页面导航id</param>
        /// <returns></returns>
        DataSet GetBtnRightSelect(string Id);

        /// <summary>
        /// 根据页面主键,获取页面的按钮
        /// </summary>
        /// <param name="key">页面主键</param>
        /// <returns></returns>
        DataSet GetBtnByPage(string key);

        /// <summary>
        /// 根据主键软删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int DeleteDate(string key);
    }
}
