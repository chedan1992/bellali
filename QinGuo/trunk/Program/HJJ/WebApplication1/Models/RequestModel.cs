using System;

namespace Commonlication
{
    public class RequestModel
    {
        public int code { get; set; }

        public object data { get; set; }

        public string msg { get; set; }

        public int totalCount { get; set; }
    }
}