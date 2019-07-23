using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.DataAccessBase;
using QINGUO.ViewModel;
using QINGUO.Common;
using Dapper;
using System.Data;

namespace QINGUO.DAL
{
    public class CarProjectDAL : BaseDAL<ModCarProject>, ICarProject
    {
        public List<ModCarProject> GetList()
        {
            return dabase.ReadDataBase.Query<ModCarProject>("select * from W_Project where Status=1 order by CreateTime desc").ToList();
        }
    }
}
