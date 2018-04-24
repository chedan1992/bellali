using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using Quartz;

namespace QINGUO.QuartService
{
    /// <summary>
    /// 定时任务框架
    /// </summary>
    public class QuartConfig
    {
        //获取稳健名称
        public static string ConfigFileName = ConfigurationManager.AppSettings["ConfigFileName"];

        /// <summary>
        /// 当前xml文档
        /// </summary>
        public static XDocument xml = null;

        #region 构造函数xml

        static QuartConfig()
        {
            //查看文件是否存在
            if (!File.Exists(ConfigFileName))
                Console.WriteLine("文件不存在");
            else
                xml = XDocument.Load(ConfigFileName);
        }

        #endregion

        #region 获取根节点
        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <returns></returns>
        public static XElement GetRootElement()
        {
            if (xml != null)
                return xml.Root;
            return null;
        }
        #endregion

        #region 获取任务列表
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<XElement> GetTaskList()
        {
            //获取根节点
            XElement root = GetRootElement();
            if (root != null)
            {
                //获取任务列表
                XElement TaskList = root.Element("TaskList");
                //获取具体的每一个任务项
                IEnumerable<XElement> taksItems = TaskList.Elements("TaksItem");
                return taksItems;
            }
            return null;
        }

        /// <summary>
        /// 获取任务中的工作项
        /// </summary>
        /// <param name="taskXElementk">该任务的元素节点</param>
        /// <returns></returns>
        public static IEnumerable<XElement> GetTaskListJob(XElement taskXElementk)
        {
            if (taskXElementk != null)
            {
                List<XElement> jobXElemntList = new List<XElement>();
                var JobsList = taskXElementk.Element("Jobs");
                var jobList = JobsList.Elements("Job");

                foreach (XElement item in jobList)
                {
                    XAttribute xAttribute = item.Attribute("idValue");
                    if (xAttribute != null)
                    {
                        if (!string.IsNullOrEmpty(xAttribute.Value))
                        {
                            XElement currentTrigger = GetJobByID(xAttribute.Value);
                            if (currentTrigger != null)
                                jobXElemntList.Add(currentTrigger);
                        }
                        else
                        {
                            Console.WriteLine("该工作项没有对应的ID");
                        }
                    }
                    else
                    {
                        Console.WriteLine("该工作项没有对应的ID");
                    }
                }
                return jobXElemntList;
            }
            return null;
        }


        #endregion

        #region 获取作业项
        /// <summary>
        /// 获取工作项
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<XElement> GetJobsList()
        {
            //获取根节点
            XElement root = GetRootElement();
            if (root != null)
            {
                //获取任务列表
                XElement JobsList = root.Element("Jobs");
                //获取具体的每一个任务项
                IEnumerable<XElement> jobList = JobsList.Elements("Job");
                return jobList;
            }
            return null;

        }
        /// <summary>
        /// 根据ID获取单一的工作项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static XElement GetJobByID(string id)
        {
            IEnumerable<XElement> jobsList = GetJobsList();
            if (jobsList != null && jobsList.Count() > 0)
            {
                foreach (XElement item in jobsList)
                {
                    XAttribute xattr = item.Attribute("id");
                    if (xattr != null)
                    {
                        string valueString = xattr.Value;
                        if (!string.IsNullOrEmpty(valueString))
                        {
                            if (valueString.Trim() == id)
                                return item;
                        }
                        else
                        {
                            Console.WriteLine("请指定ID");
                        }
                    }
                    else
                    {
                        Console.WriteLine("没有ID属性");
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取某个工作项的触发器
        /// </summary>
        /// <param name="jobXElement"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> GetTriggersByJob(XElement jobXElement)
        {
            if (jobXElement != null)
            {
                List<XElement> triggersList = new List<XElement>();

                //获取工作项的触发器集合
                XElement jobtriggerXElelemt = jobXElement.Element("Triggers");
                if (jobtriggerXElelemt != null)
                {
                    //获取当前工作项所有触发器
                    IEnumerable<XElement> jobTrigger = jobtriggerXElelemt.Elements("Trigger");
                    if (jobTrigger != null && jobTrigger.Count() > 0)
                    {
                        foreach (XElement item in jobTrigger)
                        {
                            XAttribute xAttribute = item.Attribute("idValue");
                            if (xAttribute != null)
                            {
                                if (!string.IsNullOrEmpty(xAttribute.Value))
                                {
                                    XElement currentTrigger = GetTriggerById(xAttribute.Value);
                                    if (currentTrigger != null)
                                        triggersList.Add(currentTrigger);
                                }
                                else
                                {
                                    Console.WriteLine("该触发器没有对应的ID");
                                }
                            }
                            else
                            {
                                Console.WriteLine("该触发器没有对应的ID");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("请设置该工作项的具体触发器.");
                    }
                }
                else
                {
                    Console.WriteLine("请设置该工作项的触发器集合.");
                }
                return triggersList;
            }
            return null;
        }

        #endregion

        #region 获取触发器
        /// <summary>
        /// 获取触发时间
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<XElement> GetTriggers()
        {
            XElement xml = GetRootElement();
            if (xml != null)
            {
                XElement Triggers = xml.Element("Triggers");
                IEnumerable<XElement> triggersList = Triggers.Elements("Trigger");
                return triggersList;
            }
            return null;
        }
        /// <summary>
        /// 根据ID获取触发器
        /// </summary>
        /// <returns></returns>
        public static XElement GetTriggerById(string id)
        {
            IEnumerable<XElement> triggerList = GetTriggers();
            if (triggerList != null && triggerList.Count() > 0)
            {
                foreach (var item in triggerList)
                {
                    XAttribute xattr = item.Attribute("id");
                    if (xattr != null)
                    {
                        string valueString = xattr.Value;
                        if (!string.IsNullOrEmpty(valueString))
                        {
                            if (valueString.Trim() == id)
                                return item;
                        }
                        else
                        {
                            Console.WriteLine("请指定ID");
                        }
                    }
                    else
                    {
                        Console.WriteLine("没有ID属性");
                    }
                }
            }
            return null;
        }
        #endregion


        #region 事件触发器时间获取

        /// <summary>
        /// 获取触发器
        /// </summary>
        /// <returns></returns>
        public static Trigger GetTrigger(string selectNodeName, string GroupName, string taskName)
        {
            Trigger trigger = null;

            XElement rootEle = GetRootElement();
            XElement TriggerTimeElement = rootEle.Element("TriggerTime");
            XElement selectElement = TriggerTimeElement.Element(selectNodeName);

            //通过分组拿
            XElement groupElement = selectElement.Element(GroupName);
            if (selectNodeName == "EveryDay")//每天
            {
                int hours = Convert.ToInt32(groupElement.Element("Hours").Value);//小时
                int Minutes = Convert.ToInt32(groupElement.Element("Minutes").Value);//分钟
                trigger = TriggerUtils.MakeDailyTrigger("tigger1", hours, Minutes);
                PrintTaskTriigerTime(selectNodeName, "【每天" + hours + ":" + Minutes + "】");
            }
            else if (selectNodeName == "EveryMonth")//每月)
            {
                int day = Convert.ToInt32(groupElement.Element("Day").Value);
                int hours = Convert.ToInt32(groupElement.Element("Hours").Value);//小时
                int Minutes = Convert.ToInt32(groupElement.Element("Minutes").Value);//分钟
                trigger = TriggerUtils.MakeMonthlyTrigger("tigger1", day, hours, Minutes);

                PrintTaskTriigerTime(selectNodeName, "【每月第" + day + "天, " + hours + ":" + Minutes + "】");
            }
            else if (selectNodeName == "EveryWeekly")//每周
            {
                int day = Convert.ToInt32(groupElement.Element("Day").Value);
                int hours = Convert.ToInt32(groupElement.Element("Hours").Value);//小时
                int Minutes = Convert.ToInt32(groupElement.Element("Minutes").Value);//分钟
                trigger = TriggerUtils.MakeWeeklyTrigger("tigger1", (DayOfWeek)day, hours, Minutes);
                PrintTaskTriigerTime(selectNodeName, "【每周星期" + day + ", " + hours + ":" + Minutes + "】");
            }
            else if (selectNodeName == "EveryHours")//间隔小时数
            {
                int value = Convert.ToInt32(groupElement.Element("Value").Value);

                XElement elemtRepeatCount = groupElement.Element("RepeatCount");
                bool isResult = SetTrigger(elemtRepeatCount, trigger, value);

                if (isResult == false)
                {
                    trigger = TriggerUtils.MakeHourlyTrigger(value);
                    trigger.Name = "trigger1";
                }
                PrintTaskTriigerTime(selectNodeName, "每隔:【" + value + "】小时");
            }
            else if (selectNodeName == "EveryMinutes")//间隔分钟数
            {
                int value = Convert.ToInt32(groupElement.Element("Value").Value);
                XElement elemtRepeatCount = groupElement.Element("RepeatCount");
                bool isResult = SetTrigger(elemtRepeatCount, trigger, value);
                if (isResult == false)
                {
                    trigger = TriggerUtils.MakeMinutelyTrigger(value);
                    trigger.Name = "trigger1";
                }
                PrintTaskTriigerTime(selectNodeName, "每隔:【" + value + "】分钟");
            }
            else if (selectNodeName == "EverySeconds")//间隔秒数
            {
                int value = Convert.ToInt32(groupElement.Element("Value").Value);
                XElement elemtRepeatCount = groupElement.Element("RepeatCount");
                bool isResult = SetTrigger(elemtRepeatCount, trigger, value);
                if (isResult == false)
                {
                    trigger = TriggerUtils.MakeSecondlyTrigger(value);
                    trigger.Name = "trigger1";
                }
                PrintTaskTriigerTime(selectNodeName, "每隔:【" + value + "】秒");
            }
            return trigger;
        }
        /// <summary>
        /// 打印触发器的时间
        /// </summary>
        public static void PrintTaskTriigerTime(string selectNodeName, string timeString)
        {
            if (selectNodeName == "EveryDay")//每天
            {
                Console.WriteLine("【" + selectNodeName + "】每天:" + timeString);
            }
            else if (selectNodeName == "EveryMonth")//每月)
            {
                Console.WriteLine("【" + selectNodeName + "】每月:" + timeString);
            }
            else if (selectNodeName == "EveryWeekly")//每周
            {
                Console.WriteLine("【" + selectNodeName + "】每周:" + timeString);
            }
            else if (selectNodeName == "EveryHours")//间隔小时数
            {
                Console.WriteLine("【" + selectNodeName + "】每小时,:" + timeString);
            }
            else if (selectNodeName == "EveryMinutes")//间隔分钟数
            {
                Console.WriteLine("【" + selectNodeName + "】每分钟:" + timeString);
            }
            else if (selectNodeName == "EverySeconds")//间隔秒数
            {
                Console.WriteLine("【" + selectNodeName + "】每秒钟:" + timeString);
            }
        }


        /// <summary>
        /// 设置触发器
        /// </summary>
        /// <param name="elemtRepeatCount"></param>
        /// <param name="trigger"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetTrigger(XElement elemtRepeatCount, Trigger trigger, int value)
        {
            if (elemtRepeatCount != null)
            {
                int intRepearCount = 0;
                string strRepeatrCount = elemtRepeatCount.Value;
                if (int.TryParse(strRepeatrCount, out intRepearCount))
                {
                    trigger = TriggerUtils.MakeHourlyTrigger(value, intRepearCount);
                    trigger.Name = "trigger1";
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }



        #endregion


        #region 打印流程
        /// <summary>
        /// 是否打印日志记录
        /// </summary>
        /// <param name="isPrint">是否打印</param>
        /// <param name="msg">打印的消息</param>
        public static void Print(bool isPrint, string msg)
        {
            if (isPrint)
                Console.WriteLine(msg);
        }
        #endregion
    }
}
