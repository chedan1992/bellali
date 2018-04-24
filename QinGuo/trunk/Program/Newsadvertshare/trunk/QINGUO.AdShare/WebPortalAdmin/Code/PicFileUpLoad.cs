using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using QINGUO.Common;

namespace WebPortalAdmin
{
    /// <summary>
    /// 文件上传操作
    /// </summary>
    public class PicFileUpLoad
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="postedFile">文件类</param>
        /// <param name="configPath">保存路径</param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        public bool UpLoad(string type, HttpPostedFileBase postedFile, string configPath, string filename, out string path,out string str)
        {
            bool flag = true;
            string savepath = System.Web.HttpContext.Current.Server.MapPath(configPath);//获取保存路径
            string sExtension = filename.Substring(filename.LastIndexOf('.'));//获取拓展名
            savepath += "/"+ DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM");
            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);
            string sNewFileName = DateTime.Now.ToString("yyyyMMdd-"+Guid.NewGuid().ToString());
            path = savepath + @"/" + sNewFileName + sExtension;//保存路径
            string message = "";

            switch (type)
            {
                case "EDynamic"://文章资讯
                    if (UploadFileHelper.UploadImg(postedFile, path, 500, out message))
                    {
                        path = configPath + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + @"/" + sNewFileName + sExtension;
                    }
                    else
                    {
                        flag = false;
                    }
                    break;
                case "EDocument"://文档列表
                    if (UploadFileHelper.UploadImg(postedFile, path, 500, out message))
                    {
                        path = configPath + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + @"/" + sNewFileName + sExtension;
                    }
                    else
                    {
                        flag = false;
                    }
                    break;
                case "Advertise"://品牌
                    if (UploadFileHelper.UploadImg(postedFile, path, 500, out message))
                    {
                        path = configPath + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + @"/" + sNewFileName + sExtension;
                    }
                    else
                    {
                        flag = false;
                    }
                    break;
                case "ShopCategory"://商品类型上传
                    string SmallPath = savepath + "/SmallPath/";
                    if (!Directory.Exists(SmallPath))
                        Directory.CreateDirectory(SmallPath);
                    SmallPath =SmallPath + @"/" + sNewFileName + sExtension;
                    if (UploadFileHelper.UploadSmallImg(postedFile, path, SmallPath, 150, 150, 500, out message))
                    {
                        path = configPath + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + @"/" + sNewFileName + sExtension;
                    }
                    else
                    {
                        flag = false;
                    }
                    break;
                case "Company"://公司和商家图片上传
                    if (UploadFileHelper.UploadImg(postedFile, path, 500, out message))
                    {
                        path = configPath + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + @"/" + sNewFileName + sExtension;
                    }
                    else
                    {
                        flag = false;
                    }
                    break;
                case "News"://新闻图片
                    if (UploadFileHelper.UploadImg(postedFile, path, 500, out message))
                    {
                        path = configPath + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + @"/" + sNewFileName + sExtension;
                    }
                    else
                    {
                        flag = false;
                    }
                    break;
            }
            str = message;//返回错误信息
            return flag;
        }
    }
}