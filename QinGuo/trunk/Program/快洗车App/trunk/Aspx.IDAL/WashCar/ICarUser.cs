using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ICarUser : IBaseDAL<ModCarUser>
    {
        ModCarUser GetCarUser(string uid, string cid);
    }
}
