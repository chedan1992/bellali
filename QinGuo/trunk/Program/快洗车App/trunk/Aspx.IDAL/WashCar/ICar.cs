using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ICar : IBaseDAL<ModCar>
    {
        List<ModCar> getUserId(string userid);
        bool Exit(string licensePlate);
        ModCar getLicensePlate(string licensePlate);
    }
}
