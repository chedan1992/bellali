using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace QINGUO.DataAccessBase
{
    public struct ParamItem
    {
        public string ParameterName;
        public object ParameterValue;
        public DbType DbType;
        public short Direction;
        public int Size;
    }

    /// <summary>
    /// 参数集合
    /// </summary>
    public class DataParameters
    {
        public void Clear()
        {
            _items.Clear();
            _outputItems.Clear();
        }

        public void Add(string parameterName, object parameterValue)
        {
            ParamItem para = new ParamItem();
            para.ParameterName = parameterName;
            para.ParameterValue = parameterValue;
            //para.DbType = dbType;
            para.Direction = 0;
            _items.Add(para);
        }

        public void AddOutput(string parameterName, DbType dbType)
        {
            ParamItem para = new ParamItem();
            para.ParameterName = parameterName;
            para.ParameterValue = null;
            para.DbType = dbType;
            para.Direction = 1;
            _items.Add(para);
        }

        public void AddOutput(string parameterName, DbType dbType, int size)
        {
            ParamItem para = new ParamItem();
            para.ParameterName = parameterName;
            para.ParameterValue = null;
            para.DbType = dbType;
            para.Direction = 1;
            para.Size = size;
            _items.Add(para);
        }

        private IList<ParamItem> _items = new List<ParamItem>();
        public IList<ParamItem> Items { get { return _items; } set { _items = value; } }
        private CommandType _ctype = CommandType.Text;
        public CommandType DataCommandType { get { return _ctype; } set { _ctype = value; } }
        private IList<IDbDataParameter> _outputItems = new List<IDbDataParameter>();
        public IList<IDbDataParameter> OutPutItems { get { return _outputItems; } set { _outputItems = value; } }
        private bool _hasreturnvalue = true;
        public bool HasReturnValue { get { return _hasreturnvalue; } set { _hasreturnvalue = value; } }

    }
}
