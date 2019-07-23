using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using System.Data;
using Dapper;
using QINGUO.Common;

namespace QINGUO.IDAL
{
    public interface ICarOrder : IBaseDAL<ModCarOrder>
    {
        Page<CarOrderView> SearchData(Search search);
        CarOrderView GetDetail(string orderid);
    }
}
