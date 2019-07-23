using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Ninject;
using QINGUO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class AliyunMsg
    {
        private IKernel ninjectKernel = new StandardKernel();
        public AliyunMsg()
        {
            ninjectKernel.Bind<ILogAction>().To<SharePresentationLog>();
        }
        //错误日志记录器
        public ILogAction LogErrorRecord
        {
            get
            {
                return ninjectKernel.Get<ILogAction>();
            }
        }
        public bool sendMsg(string msg, string PhoneNumbers, string TemplateCode, string code)
        {

            IClientProfile profile = DefaultProfile.GetProfile("default", "LTAILhQTlW0BVBrh", "VeiVpu7lhKjE1bjzKDMzZ3QaCv06k4");
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";

            request.AddQueryParameters("PhoneNumbers", PhoneNumbers);
            request.AddQueryParameters("SignName", "快洗车app");
            request.AddQueryParameters("TemplateCode", TemplateCode);
            request.AddQueryParameters("TemplateParam", "{\"code\":\"" + code + "\"}");
            request.Protocol = ProtocolType.HTTP;

            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
                return true;
            }
            catch (ServerException ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                return false;
            }
            catch (ClientException ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                return false;

            }
        }
    }
}