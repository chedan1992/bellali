using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using Dapper;
using QINGUO.Common;

namespace QINGUO.DAL
{
    public class SysAppointCheckNotesDAL : BaseDAL<ModSysAppointCheckNotes>, ISysAppointCheckNotes
    {

        public Page<ModSysAppointCheckNotes> GetAppointCheckNotesList(Search search)
        {
            return dabase.ReadDataBase.Page<ModSysAppointCheckNotes>(search.CurrentPageIndex, search.PageSize, search.SqlString2);
        }
    }
}
