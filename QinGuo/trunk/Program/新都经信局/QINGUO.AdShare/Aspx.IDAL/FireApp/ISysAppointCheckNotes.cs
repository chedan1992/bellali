using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using QINGUO.ViewModel;
using QINGUO.Common;
using Dapper;

namespace QINGUO.IDAL
{
    public interface ISysAppointCheckNotes : IBaseDAL<ModSysAppointCheckNotes>
    {

        Page<ModSysAppointCheckNotes> GetAppointCheckNotesList(Search search);
    }
}
