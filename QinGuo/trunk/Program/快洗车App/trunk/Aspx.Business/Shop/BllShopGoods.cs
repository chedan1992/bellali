#region Version Info
/* ======================================================================== 
* 【本类功能概述】 商品业务逻辑层
* 
* 作者：张建 时间：2014/1/2 15:54:08 
* 文件名：BllShopGoods 
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
using QINGUO.Factory;
using QINGUO.Common;
using QINGUO.Model;

namespace QINGUO.Business
{
    public class BllShopGoods : BllBase<ModShopGoods>
    {
        IShopGoods DAL = CreateDalFactory.CreateShopGoodsDAL();
        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
      
    }
}
