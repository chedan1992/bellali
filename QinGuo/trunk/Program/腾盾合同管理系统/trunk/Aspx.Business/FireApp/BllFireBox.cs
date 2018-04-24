using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.Common;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllFireBox : BllBase<ModFireBox>
    {
        IFireBox dal = CreateDalFactory.CreateDalFireBox();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"View_FireBox";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public Dapper.Page<ModFireBox> GetFireBoxList(Search search)
        {
            search.TableName = @"Fire_FireBox";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";
            return dal.GetFireBoxList(search);

        }
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="QRCode"></param>
        /// <returns></returns>
        public ModFireBox GetFireBoxDetailQRCode(string QRCode)
        {
            return dal.GetFireBoxDetailQRCode(QRCode);
        }

        /// <summary>
        /// 删除箱子
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteStatus(string Id)
        {
            return dal.DeleteStatus(Id);
        }

        /// <summary>
        /// 修改箱子数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool UpdateEquipmentCount(string id, int num)
        {
            return dal.UpdateEquipmentCount(id, num);
        }
    }
}
