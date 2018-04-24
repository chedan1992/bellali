using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using QINGUO.Business;
using QINGUO.Model;
using System.Collections;
using QINGUO.Common;
using QINGUO.ViewModel;

namespace QINGUO.QuartService
{
    public class Expired : IJob
    {
        //定义一个私有成员变量，用于Lock  
        private static object lockobj = new object();
        public void Execute(JobExecutionContext context)
        {
            //开始循环作业调度
            lock (lockobj)
            {
                StartJobScheduling();
            }
        }

        /// <summary>
        /// 开始作业调度
        /// </summary>
        public void StartJobScheduling()
        {
            try
            {
                DateTime now = DateTime.Now;
                //查询设备
                BllEDynamic bll = new BllEDynamic();
                var ds = bll.GetList("E_Dynamic", " and ShowType=2 and Status=1 and ActiveEndTime<getdate()","",10);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sql = "update E_Dynamic set Status=2 where Id='" + ds.Tables[0].Rows[i]["Id"].ToString()+ "'";
                        bll.ExecuteNonQueryByText(sql);
                        Console.WriteLine("新闻ID:" + ds.Tables[0].Rows[i]["Id"] + "自动过期.已下架");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常了,异常信息:" + ex);
            }
        }
    }

}

