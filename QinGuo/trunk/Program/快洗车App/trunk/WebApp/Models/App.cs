using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    /// <summary>
    /// App信息
    /// </summary>
    public class App
    {
        /// <summary>
        /// App的ID号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// App的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// App的说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// App的说明
        /// </summary>
        public DateTime Date { get; set; }
    }
}