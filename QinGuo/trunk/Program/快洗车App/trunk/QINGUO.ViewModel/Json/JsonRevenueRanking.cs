using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 抢单json
    /// </summary>
    public class JsonRevenueRanking
    {
        public string Id { get; set; }
        public string Cid { get; set; }
        public decimal Money { get; set; }
        public string UserName { get; set; }
        public string HeadImg { get; set; }
        public string CName { get; set; }
        public int OrderCount { get; set; }
        public string Grade { get; set; }
    }
}
