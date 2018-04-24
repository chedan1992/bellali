using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface IFireBox : IBaseDAL<ModFireBox>
    {
        Dapper.Page<ModFireBox> GetFireBoxList(Common.Search searchtemp);

        ModFireBox GetFireBoxDetailQRCode(string QRCode);

        int DeleteStatus(string Id);


        bool UpdateEquipmentCount(string id, int num);
    }
}
