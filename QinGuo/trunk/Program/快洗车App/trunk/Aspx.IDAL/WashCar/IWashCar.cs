using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.IDAL
{
    public interface IWashCar : IBaseDAL<ModWashCar>
    {
        ModWashCar GetWashCar(string uid, string cid);
    }
}
