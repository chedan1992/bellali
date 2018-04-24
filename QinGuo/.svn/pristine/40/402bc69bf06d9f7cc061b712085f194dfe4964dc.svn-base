using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Quartz.Impl;
using Quartz;

namespace QINGUO.QuartService
{
    /// <summary>
    /// 定时任务框架帮助类
    /// </summary>
   public class QuartNetHelper
    {
        /// <summary>
        /// 当前作业工厂
        /// </summary>
        public static ISchedulerFactory sf = null;
        private static IScheduler sched;
        /// <summary>
        /// 当前构造
        /// </summary>
        public QuartNetHelper()
        {
            sf = new StdSchedulerFactory();
            sched = sf.GetScheduler();
        }

        /// <summary>
        /// 开始任务执行
        /// </summary>
        public void StratTaskExecute()
        {
            IEnumerable<XElement> taskElementList = QuartConfig.GetTaskList();
            if (taskElementList != null && taskElementList.Count() > 0)
            {
                int i = 1;
                foreach (XElement item in taskElementList)
                {
                    #region 每一个任务执行

                    #region 获取任务名称
                    XAttribute xAttribute = item.Attribute("name");
                    string TaskName = string.Empty;
                    if (xAttribute != null)
                        TaskName = xAttribute.Value;
                    else
                        TaskName = "第" + i + "个";
                    #endregion

                    #region 判断任务是否被开启
                    //是否启用该定时任务  1:isEnabled
                    XAttribute isEnabledAttr = item.Attribute("isEnabled");
                    if (isEnabledAttr != null && !string.IsNullOrEmpty(isEnabledAttr.Value.Trim()))
                    {
                        //获取设置字符串
                        string boolString = isEnabledAttr.Value.Trim().ToLower();
                        if (boolString == "true" || boolString == "false")
                        {
                            //转换bool
                            if (!Convert.ToBoolean(boolString))
                            {
                                Console.WriteLine(TaskName + "任务没被开启...");
                                continue;
                            }
                        }
                    }
                    #endregion

                    #region 是否可以打印日志
                    bool isPrint = true;
                    XAttribute isPrintdAttr = item.Attribute("Print");
                    if (isPrintdAttr != null && !string.IsNullOrEmpty(isPrintdAttr.Value.Trim()))
                    {
                        //获取设置字符串
                        string boolString = isPrintdAttr.Value.Trim().ToLower();
                        if (boolString == "true" || boolString == "false")
                        {
                            isPrint = Convert.ToBoolean(boolString);
                        }
                    }
                    #endregion

                    #region 获取所有工作项
                    //该工作项的所有工作项
                    var allJobs = QuartConfig.GetTaskListJob(item);
                    JobInfo jobInfo = new JobInfo();
                    TriggerInfo triigerInfo = new TriggerInfo();
                    //该工作项下面有很多触发器
                    foreach (XElement jobItem in allJobs)
                    {
                        #region 创建每一个工作项
                        if (jobItem != null)
                        {
                            bool isSuccess = IsDecideJobElement(jobItem, jobInfo);
                            if (isSuccess)
                            {
                                //4:拿到trigger集合
                                var triggerJob = QuartConfig.GetTriggersByJob(jobItem);
                                if (triggerJob != null && triggerJob.Count() > 0)
                                {
                                    //5:循环trigger创建该trigger
                                    JobDetail job = new JobDetail(jobInfo.JobName, jobInfo.JobGroup, Type.GetType(jobInfo.Type));

                                    sched.AddJob(job, true);
                                    foreach (XElement trigger in triggerJob)
                                    {
                                        #region 为工作项创建多个触发器
                                        bool triggerIsSuccess = IsDecideTriggerElement(trigger, triigerInfo);
                                        if (!triggerIsSuccess) continue;
                                        if (triigerInfo != null)
                                        {
                                            Trigger currentTrigger = QuartConfig.GetTrigger(triigerInfo.TriggerTimeType, triigerInfo.TimeGroupValue, TaskName);
                                            currentTrigger.JobName = job.Name;
                                            currentTrigger.JobGroup = job.Group;
                                            currentTrigger.Group = triigerInfo.Group;
                                            currentTrigger.JobDataMap = new JobDataMap();
                                            currentTrigger.JobDataMap.Add("isPrint", isPrint);
                                            sched.ScheduleJob(currentTrigger);

                                        }
                                        else
                                            continue;
                                        #endregion
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("请配置该工作项的触发器,至少要有一个");
                                    continue;
                                }
                            }
                        }
                        else
                            continue;
                        #endregion
                    }
                    #endregion

                    Console.WriteLine("==="+TaskName + "已启动==");
                    #endregion
                }

                sched.Start();
            }
            else
            {
                Console.WriteLine("没有任何任务可以定时执行");
            }

        }
        /// <summary>
        /// 判断当前触发器
        /// </summary>
        /// <param name="jobNameEle"></param>
        /// <param name="jobGroupEle"></param>
        /// <param name="trigger"></param>
        private static bool IsDecideTriggerElement(XElement trigger, TriggerInfo triigerInfo)
        {

            bool isSuccess = true;
            try
            {
                //1:拿到name节点
                XElement triggerNameEle = trigger.Element("Name");
                //   拿到value
                XAttribute triggerNameValue = triggerNameEle.Attribute("value");

                //2:拿到group节点
                XElement triggerGroupEle = trigger.Element("Group");
                //   拿到value
                XAttribute triggerGroupValue = triggerGroupEle.Attribute("value");

                //3:获取Time节点
                XElement triggerTimeEle = trigger.Element("Time");

                //  拿到触发器类型
                XAttribute TriggerTimeTypeAttr = triggerTimeEle.Attribute("TriggerTimeType");
                //  拿到触发器的组名称
                XAttribute GroupValueAttr = triggerTimeEle.Attribute("GroupValue");

                if (triggerNameEle == null || triggerGroupEle == null || triggerTimeEle == null)
                {
                    Console.WriteLine("请确保触发器都有Name,Group,Time节点");
                    return false;
                }
                if (triggerNameValue == null || triggerGroupValue == null || TriggerTimeTypeAttr == null || GroupValueAttr == null)
                {
                    Console.WriteLine("请确保工作项的Name,Group,TriggerTimeType,GroupValue都有值");
                    return false;
                }
                triigerInfo.Name = triggerNameValue.Value;
                triigerInfo.Group = triggerGroupValue.Value;
                triigerInfo.TriggerTimeType = TriggerTimeTypeAttr.Value;
                triigerInfo.TimeGroupValue = GroupValueAttr.Value;
                if (string.IsNullOrEmpty(triigerInfo.Name) || string.IsNullOrEmpty(triigerInfo.Group) || string.IsNullOrEmpty(triigerInfo.TriggerTimeType) || string.IsNullOrEmpty(triigerInfo.TimeGroupValue))
                {
                    Console.WriteLine("请确保工作项的Name,Group,TriggerTimeType,GroupValue的值不为null");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常了:" + ex);
                isSuccess = false;
            }
            return isSuccess;
        }
        /// <summary>
        /// 判断job工作项
        /// </summary>
        /// <param name="jobItem"></param>
        /// <param name="jobNameEle"></param>
        /// <param name="jobNameValue"></param>
        /// <param name="jobGroupEle"></param>
        /// <param name="jobGroupValue"></param>
        /// <param name="xattributeType"></param>
        /// <returns></returns>
        private static bool IsDecideJobElement(XElement jobItem, JobInfo jobInfo)
        {
            bool isSuccess = true;
            try
            {
                XElement jobNameEle = null;
                XAttribute jobNameValue = null;
                XElement jobGroupEle = null;
                XAttribute jobGroupValue = null;
                XAttribute xattributeType = null;

                //1:拿到name
                jobNameEle = jobItem.Element("Name");
                jobNameValue = jobNameEle.Attribute("value");

                //2:拿到group
                jobGroupEle = jobItem.Element("Group");
                jobGroupValue = jobGroupEle.Attribute("value");

                //3:获取type
                xattributeType = jobItem.Attribute("type");
                if (jobNameEle == null || jobGroupEle == null || xattributeType == null)
                {
                    Console.WriteLine("请确保工作项的都有Name,Group,Triggers节点和当前type的属性的全名称");
                    return false;
                }
                if (string.IsNullOrEmpty(jobNameValue.Value) || string.IsNullOrEmpty(jobGroupValue.Value) || string.IsNullOrEmpty(xattributeType.Value))
                {
                    Console.WriteLine("请确保工作项的Name,Group,和当前类型的全名称都有值");
                    return false;
                }
                jobInfo.JobName = jobNameValue.Value;
                jobInfo.JobGroup = jobGroupValue.Value;
                jobInfo.Type = xattributeType.Value;

            }
            catch (Exception ex)
            {
                Console.WriteLine("异常了:" + ex);
                isSuccess = false;
            }
            return isSuccess;
        }



    }
}
