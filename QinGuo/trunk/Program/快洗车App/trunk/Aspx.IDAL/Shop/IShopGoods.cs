#region Version Info
/* ======================================================================== 
* 【本类功能概述】 商品接口
* 
* 作者：张建 时间：2014/1/2 15:49:06 
* 文件名：IShopGoods 
* 版本：V1.0.1 
* 
* 修改者： 时间： 
* 修改说明： 
* ======================================================================== 
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using QINGUO.Common;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface IShopGoods : IBaseDAL<ModShopGoods>
    {
      
    }
}
