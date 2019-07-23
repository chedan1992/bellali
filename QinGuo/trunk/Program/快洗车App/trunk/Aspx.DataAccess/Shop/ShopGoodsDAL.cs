#region Version Info
/* ======================================================================== 
* 【本类功能概述】 商品数据层
* 
* 作者：张建 时间：2014/1/2 15:51:43 
* 文件名：DalShopGoods 
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
using QINGUO.IDAL;
using QINGUO.DataAccessBase;
using QINGUO.Model;
using QINGUO.Common;

namespace QINGUO.DAL
{
    public class ShopGoodsDAL : BaseDAL<ModShopGoods>, IShopGoods
    {
       
    }
}
