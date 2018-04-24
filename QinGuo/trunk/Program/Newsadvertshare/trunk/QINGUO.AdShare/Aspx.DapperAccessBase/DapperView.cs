using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Dapper;

namespace QINGUO.DapperAccessBase
{
    public class DapperView
    {
        private string ReadConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ReadConnectionStringName"];
            }
        }

        private string WriteConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["WriteConnectionStringName"];
            }
        }

        /// <summary>
        /// 写数据库(主)
        /// </summary>
        public Database WriteDataBase
        {

            get
            {
                return DapperManager.Instance.GetCurrentDataBase(WriteConnectionString);
            }
        }

        /// <summary>
        /// 读数据库(从)
        /// </summary>
        public Database ReadDataBase
        {
            get
            {
                //return DapperManager.Instance.GetCurrentDataBase(ReadConnectionString);
                return WriteDataBase;
            }
        }
    }
}
