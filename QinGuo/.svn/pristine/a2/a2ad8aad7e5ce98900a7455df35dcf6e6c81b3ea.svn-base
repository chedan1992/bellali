using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Common
{
    /// <summary>
    /// 查询类
    /// </summary>
    public class Search
    {
        private List<string> conditions = new List<string>();

        private int _pageSize = 10;
        public int PageSize { set { _pageSize = value; } get { return _pageSize; } }

        private int _currentPageIndex = 1;
        public int CurrentPageIndex { set { _currentPageIndex = value; } get { return _currentPageIndex; } }

        private string _selectedColums = "*";
        public string SelectedColums { set { _selectedColums = value; } get { return _selectedColums; } }

        private string _keyFiled = string.Empty;
        public string KeyFiled { set { _keyFiled = value; } get { return _keyFiled; } }

        private string _tableName = string.Empty;
        public string TableName { set { _tableName = value; } get { return _tableName; } }

        private string _sortField = string.Empty;
        public string SortField { set { _sortField = value; } get { return _sortField; } }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        private string _searchKeyWord = string.Empty;
        public string SearchKeyWord
        {
            get { return _searchKeyWord; }
            set { _searchKeyWord = value; }
        }

        private string _statusDefaultCondition = string.Empty;
        /// <summary>
        /// 状态默认查询条件：STATUS=-1
        /// </summary>
        public string StatusDefaultCondition { set { _statusDefaultCondition = value; } get { return _statusDefaultCondition; } }

        public void AddCondition(string conditionField, string condition, string conditionValue)
        {
            conditionValue = conditionValue.Replace("'", "''");
            switch (condition)
            {
                case "like":
                    conditions.Add(string.Format("{0} like '%{1}%'", conditionField, conditionValue));
                    break;
                case "=":
                    conditions.Add(string.Format("{0} = '{1}'", conditionField, conditionValue));
                    break;
                case "<>":
                    conditions.Add(string.Format("{0} <> '{1}'", conditionField, conditionValue));
                    break;
                case ">":
                    conditions.Add(string.Format("{0} > '{1}'", conditionField, conditionValue));
                    break;
                case ">=":
                    conditions.Add(string.Format("{0} >= '{1}'", conditionField, conditionValue));
                    break;
                case "<":
                    conditions.Add(string.Format("{0} < '{1}'", conditionField, conditionValue));
                    break;
                case "<=":
                    conditions.Add(string.Format("{0} <= '{1}'", conditionField, conditionValue));
                    break;
            }
        }

        public void AddCondition(string condition)
        {
            conditions.Add(condition);
        }

        public string GetConditon()
        {
            StringBuilder sb = new StringBuilder();
            int index = 1;
            foreach (var item in conditions)
            {
                sb.Append((index++) == 1 ? item : string.Format(" AND {0} ", item));
            }

            if (sb.Length > 0)
            {
                if (_statusDefaultCondition.Length > 0) sb.Append(string.Format(" AND {0} ", _statusDefaultCondition));
            }
            else
            {
                if (_statusDefaultCondition.Length > 0) sb.Append(string.Format(" {0} ", _statusDefaultCondition));
            }
            return sb.ToString();
        }

        public string SqlString
        {
            get
            {
                var sqlString = "SELECT {0} FROM {1} WHERE {2} ";
                sqlString = string.Format(sqlString, _selectedColums, _tableName, GetConditon());
                return sqlString;
            }
        }

        public string SqlString2
        {
            get
            {
                if (string.IsNullOrEmpty(SortField))
                {
                    return SqlString;
                }
                else
                {
                    var sqlString = "SELECT {0} FROM {1} WHERE {2} Order by {3}";
                    sqlString = string.Format(sqlString, _selectedColums, _tableName, GetConditon(), SortField);
                    return sqlString;
                }
            }
        }
    }
}
