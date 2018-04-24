using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.Common;

namespace QINGUO.DAL
{
    public class FireBoxDAL : BaseDAL<ModFireBox>, IFireBox
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public Dapper.Page<ModFireBox> GetFireBoxList(Search search)
        {
            return dabase.ReadDataBase.Page<ModFireBox>(search.CurrentPageIndex, search.PageSize, search.SqlString2);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="QRCode"></param>
        /// <returns></returns>
        public ModFireBox GetFireBoxDetailQRCode(string QRCode)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModFireBox>("select * from Fire_FireBox where 'XF'+QrCode+'X'=@0 ", QRCode);
        }


        /// <summary>
        /// 删除箱子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteStatus(string Id)
        {
            dabase.BeginTransaction();
            StringBuilder sb = new StringBuilder();
            sb.Append("update Fire_FireBox set Status=" + (int)StatusEnum.删除 + " where Id in(" + Id + ");");
            sb.AppendLine();
            sb.Append("update Sys_Appointed set Status=" + (int)StatusEnum.删除 + " where Places in(" + Id + ");");

            try
            {
                int r = dabase.WriteDataBase.Execute(sb.ToString());
                dabase.CommitTransaction();
                return r;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }


        /// <summary>
        /// 修改箱子数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool UpdateEquipmentCount(string id, int num)
        {

            dabase.BeginTransaction();
            StringBuilder sb = new StringBuilder();
            sb.Append("update Fire_FireBox set EquipmentCount=EquipmentCount + @1 where id=@0");

            try
            {
                int r = dabase.WriteDataBase.Execute(sb.ToString(), id, num);
                dabase.CommitTransaction();
                return r > 0;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return false;
            }
        }

    }
}
