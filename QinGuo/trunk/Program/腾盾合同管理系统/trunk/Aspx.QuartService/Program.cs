using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.QuartService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "即刻分享服务控制平台-请勿关闭  Copyright @ 成都青果科技"; //设置控制台窗口的标题 
            Console.ForegroundColor = ConsoleColor.Green; //设置字体颜色为红色 
            QuartNetHelper HELPER = new QuartNetHelper();
            HELPER.StratTaskExecute();
        }
    }
}
