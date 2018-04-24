using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 试卷题库视图
    /// </summary>
    public class jsonPapersTestView
    {
        public string Id { get; set; }
        public string TestType { get; set; }
        public string Content { get; set; }
        public string Answer { get; set; }
        public string AnalyticQuestions { get; set; }
        public string MyAnswer { get; set; }
        public string MyGrade { get; set; }
        public string PapersId { get; set; }
        public string TestScore { get; set; }

    }
}
