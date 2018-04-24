using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Drawing;

namespace QINGUO.Common
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class UploadFile
    {

        /// <summary>
        /// 通用上传
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="tmpPath">文件保存路径</param>
        /// <param name="MaxSize">文件最大值</param>
        /// <param name="msg">返回结果说明</param>
        /// <returns>上传是否成功</returns>
        public static bool Upload(HttpPostedFileBase file, string tmpPath, int MaxSize, out string msg)
        {
            try
            {
                //int _ImgSize = MaxSize * 1024;
                //if (file.ContentLength > _ImgSize)
                //{
                //    msg = "文件大小有误(上传文件最大为：“" + _ImgSize + "KB”)！";
                //    return false;
                //}
                file.SaveAs(tmpPath);
                msg = "文件上传成功！";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 通用上传
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="tmpPath">文件保存路径</param>
        /// <param name="MaxSize">文件最大值</param>
        /// <param name="msg">返回结果说明</param>
        /// <returns>上传是否成功</returns>
        public static bool Upload(HttpPostedFile file, string tmpPath, int MaxSize, out string msg)
        {
            try
            {
                //int _ImgSize = MaxSize * 1024;
                //if (file.ContentLength > _ImgSize)
                //{
                //    msg = "文件大小有误(上传文件最大为：“" + _ImgSize + "KB”)！";
                //    return false;
                //}
                file.SaveAs(tmpPath);
                msg = "文件上传成功！";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }
        /// <summary>
        /// 上传Amr文件
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="tmpPath">文件保存路径</param>
        /// <param name="MaxSize">文件最大值</param>
        /// <param name="msg">返回结果说明</param>
        /// <returns>上传是否成功</returns>
        public static bool UploadAmr(HttpPostedFile file, string tmpPath, int MaxSize, out string msg)
        {
            try
            {
                int _ImgSize = MaxSize * 1024;
                bool bl = true;
                if (file.ContentLength > _ImgSize)
                {
                    msg = "文件大小有误(上传文件最大为：“" + MaxSize + "KB”)！";
                    return false;
                }
                file.SaveAs(tmpPath);
                FileExtension extension = CheckTextFile(tmpPath);
                if (extension == FileExtension.Amr)
                {
                    bl = false;
                }
                if (bl)
                {
                    msg = "文件格式有误(只能上传格式为amr的文件)！";
                    try
                    {
                        FileDelete(tmpPath);
                    }
                    catch (Exception a)
                    { }
                    return false;
                }
                msg = "文件上传成功！";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 上传Amr文件
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="tmpPath">文件保存路径</param>
        /// <param name="MaxSize">文件最大值</param>
        /// <param name="msg">返回结果说明</param>
        /// <returns>上传是否成功</returns>
        public static bool UploadFileRar(HttpPostedFile file, string tmpPath, int MaxSize, out string msg)
        {
            try
            {
                int _ImgSize = MaxSize * 1024;
                bool bl = true;
                if (file.ContentLength > _ImgSize)
                {
                    msg = "文件大小有误(上传文件最大为：“" + MaxSize + "KB”)！";
                    return false;
                }
                file.SaveAs(tmpPath);
                FileExtension extension = CheckTextFile(tmpPath);
                if (extension == FileExtension.Amr || extension == FileExtension.Zip || extension == FileExtension.Office)
                {
                    bl = false;
                }
                if (bl)
                {
                    msg = "文件格式有误(只能上传格式为rar,zip,word,execl,ppt的文件)！";
                    try
                    {
                        FileDelete(tmpPath);
                    }
                    catch
                    { }
                    return false;
                }
                msg = "文件上传成功！";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="tmpPath">文件保存路径</param>
        /// <param name="ImgSize">图片最大值</param>
        /// <param name="msg">返回结果说明</param>
        /// <returns>上传是否成功</returns>
        public static bool UploadImg(HttpPostedFileBase file, string tmpPath, int ImgSize, out string msg)
        {
            try
            {
                int _ImgSize = ImgSize * 1024;
                bool bl = true;
                if (file.ContentLength > _ImgSize)
                {
                    msg = "文件大小有误(上传文件最大为：“" + ImgSize + "KB”)！";
                    return false;
                }
                file.SaveAs(tmpPath);
                FileExtension extension = CheckTextFile(tmpPath);
                if (extension == FileExtension.Bmp || extension == FileExtension.Gif || extension == FileExtension.Jpg || extension == FileExtension.Png)
                {
                    bl = false;
                }
                if (bl)
                {
                    msg = "文件格式有误(只能上传格式为jpg、bmp、gif、png的图片)！";
                    try
                    {
                        FileDelete(tmpPath);
                    }
                    catch
                    { }
                    return false;
                }
                msg = "文件上传成功！";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <param name="imgpath"></param>
        public static void GetWidthHeight(out int _width, out int _height, string imgpath)
        {
            _width = 0;
            _height = 0;
            try
            {
                Bitmap pic = new Bitmap(imgpath);
                _width = pic.Size.Width;   // 图片的宽度
                _height = pic.Size.Height;   // 图片的高度
                pic.Dispose();
            }
            catch
            { }
        }
        /// <summary>
        /// 上传图片生成缩略图
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="tmpPath">文件保存路径</param>
        /// <param name="smallPath">缩略图保存路径</param>
        /// <param name="smallWidth">缩略图宽度</param>
        /// <param name="smallHeight">缩略图高度</param>
        /// <returns>上传是否成功</returns>
        public static bool UploadSmallImg(HttpPostedFileBase file, string tmpPath, string smallPath, int smallWidth, int smallHeight)
        {
            try
            {
                file.SaveAs(tmpPath);
                FileExtension extension = CheckTextFile(tmpPath);
                bool bl = true;
                if (extension == FileExtension.Bmp || extension == FileExtension.Gif || extension == FileExtension.Jpg || extension == FileExtension.Png)
                {
                    bl = false;
                }
                if (bl)
                {
                    try
                    {
                        FileDelete(tmpPath);
                    }
                    catch
                    { }
                    return false;
                }
                System.IO.Stream myStream = file.InputStream;
                System.Drawing.Image myImage = System.Drawing.Image.FromStream(myStream, false);
                //对图像，生成一个“缩略图”图像对象(原图片一半大小)   
                decimal _right = myImage.Size.Height;
                decimal dcs = myImage.Size.Width;
                dcs = dcs / smallWidth;
                _right = _right / smallHeight;
                int intHeight = Convert.ToInt32(_right);
                int dc = Convert.ToInt32(dcs);
                System.Drawing.Image thumbImage;
                if (myImage.Size.Width > smallWidth)
                {
                    if (myImage.Size.Height > smallHeight)
                    {
                        if (intHeight > dc)
                        {
                            thumbImage = myImage.GetThumbnailImage(myImage.Size.Width / intHeight, smallHeight, null, System.IntPtr.Zero);
                        }
                        else
                        {
                            thumbImage = myImage.GetThumbnailImage(smallWidth, myImage.Size.Height / dc, null, System.IntPtr.Zero);
                        }
                    }
                    else
                    {
                        thumbImage = myImage.GetThumbnailImage(smallWidth, myImage.Size.Height / dc, null, System.IntPtr.Zero);
                    }
                }
                else
                {
                    if (myImage.Size.Height > smallHeight)
                    {
                        thumbImage = myImage.GetThumbnailImage(myImage.Size.Width / intHeight, smallHeight, null, System.IntPtr.Zero);
                    }
                    else
                    {
                        thumbImage = myImage.GetThumbnailImage(myImage.Size.Width, myImage.Size.Height, null, System.IntPtr.Zero);
                    }
                }
                ////指定缩略图的保存路径
                string fileXltPath = smallPath;
                //保存缩略图
                thumbImage.Save(fileXltPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                //关闭(释放）缩略图对象
                thumbImage.Dispose();
                //关闭(释放）图片对象
                myImage.Dispose();
                System.Threading.Thread.Sleep(1000);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 上传图片生成缩略图
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="tmpPath">文件保存路径</param>
        /// <param name="SmallPath">缩略图保存路径</param>
        /// <param name="SmallWidth">缩略图宽度</param>
        /// <param name="SmallHeight">缩略图高度</param>
        /// <param name="ImgSize">图片最大值</param>
        /// <param name="msg">返回结果说明</param>
        /// <returns>上传是否成功</returns>
        public static bool UploadSmallImg(HttpPostedFileBase file, string tmpPath, string SmallPath, int SmallWidth, int SmallHeight, int ImgSize, out string msg)
        {
            msg = "";
            try
            {
                bool bl = true;
                int _ImgSize = ImgSize * 1024;
                if (file.ContentLength > _ImgSize)
                {
                    msg = "文件大小有误(上传文件最大为：“" + ImgSize + "KB”)！";
                    return false;
                }
                file.SaveAs(tmpPath);
                FileExtension extension = CheckTextFile(tmpPath);
                if (extension == FileExtension.Bmp || extension == FileExtension.Gif || extension == FileExtension.Jpg || extension == FileExtension.Png)
                {
                    bl = false;
                }
                if (bl)
                {
                    msg = "文件格式有误(只能上传格式为jpg、bmp、gif、png的图片)！";
                    try
                    {
                        FileDelete(tmpPath);
                    }
                    catch
                    { }
                    return false;
                }
                System.IO.Stream myStream = file.InputStream;
                System.Drawing.Image myImage = System.Drawing.Image.FromStream(myStream, false);
                //对图像，生成一个“缩略图”图像对象(原图片一半大小)   
                decimal _right = myImage.Size.Height;
                decimal dcs = myImage.Size.Width;
                dcs = dcs / SmallWidth;
                _right = _right / SmallHeight;
                decimal intHeight = _right;
                decimal dc = dcs;
                System.Drawing.Image thumbImage;
                if (myImage.Size.Width > SmallWidth)
                {
                    if (myImage.Size.Height > SmallHeight)
                    {
                        if (intHeight > dc)
                        {
                            thumbImage = myImage.GetThumbnailImage(Convert.ToInt32(myImage.Size.Width / intHeight), SmallHeight, null, System.IntPtr.Zero);
                        }
                        else
                        {
                            thumbImage = myImage.GetThumbnailImage(SmallWidth, Convert.ToInt32(myImage.Size.Height / dc), null, System.IntPtr.Zero);
                        }
                    }
                    else
                    {
                        thumbImage = myImage.GetThumbnailImage(SmallWidth, Convert.ToInt32(myImage.Size.Height / dc), null, System.IntPtr.Zero);
                    }
                }
                else
                {
                    if (myImage.Size.Height > SmallHeight)
                    {
                        thumbImage = myImage.GetThumbnailImage(Convert.ToInt32(myImage.Size.Width / intHeight), SmallHeight, null, System.IntPtr.Zero);
                    }
                    else
                    {
                        thumbImage = myImage.GetThumbnailImage(myImage.Size.Width, myImage.Size.Height, null, System.IntPtr.Zero);
                    }
                }
                ////指定缩略图的保存路径
                string fileXltPath = SmallPath;
                //保存缩略图
                thumbImage.Save(fileXltPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                //关闭(释放）缩略图对象
                thumbImage.Dispose();
                //关闭(释放）图片对象
                myImage.Dispose();
                System.Threading.Thread.Sleep(1000);
                return true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 上传图片生成缩略图
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="tmpPath">文件保存路径</param>
        /// <param name="SmallPath">缩略图保存路径</param>
        /// <param name="SmallWidth">缩略图宽度</param>
        /// <param name="SmallHeight">缩略图高度</param>
        /// <param name="ImgSize">图片最大值</param>
        /// <param name="msg">返回结果说明</param>
        /// <returns>上传是否成功</returns>
        public static bool UploadSmallImg(HttpPostedFile file, string tmpPath, string SmallPath, int SmallWidth, int SmallHeight, int ImgSize, out string msg)
        {
            msg = "";
            try
            {
                bool bl = true;
                int _ImgSize = ImgSize * 1024;
                if (file.ContentLength > _ImgSize)
                {
                    msg = "文件大小有误(上传文件最大为：“" + ImgSize + "KB”)！";
                    return false;
                }
                file.SaveAs(tmpPath);
                FileExtension extension = CheckTextFile(tmpPath);
                if (extension == FileExtension.Bmp || extension == FileExtension.Gif || extension == FileExtension.Jpg || extension == FileExtension.Png)
                {
                    bl = false;
                }
                if (bl)
                {
                    msg = "文件格式有误(只能上传格式为jpg、bmp、gif、png的图片)！";
                    try
                    {
                        FileDelete(tmpPath);
                    }
                    catch
                    { }
                    return false;
                }
                System.IO.Stream myStream = file.InputStream;
                System.Drawing.Image myImage = System.Drawing.Image.FromStream(myStream, false);
                //对图像，生成一个“缩略图”图像对象(原图片一半大小)   
                decimal _right = myImage.Size.Height;
                decimal dcs = myImage.Size.Width;
                dcs = dcs / SmallWidth;
                _right = _right / SmallHeight;
                decimal intHeight = _right;
                decimal dc = dcs;
                System.Drawing.Image thumbImage;
                if (myImage.Size.Width > SmallWidth)
                {
                    if (myImage.Size.Height > SmallHeight)
                    {
                        if (intHeight > dc)
                        {
                            thumbImage = myImage.GetThumbnailImage(Convert.ToInt32(myImage.Size.Width / intHeight), SmallHeight, null, System.IntPtr.Zero);
                        }
                        else
                        {
                            thumbImage = myImage.GetThumbnailImage(SmallWidth, Convert.ToInt32(myImage.Size.Height / dc), null, System.IntPtr.Zero);
                        }
                    }
                    else
                    {
                        thumbImage = myImage.GetThumbnailImage(SmallWidth, Convert.ToInt32(myImage.Size.Height / dc), null, System.IntPtr.Zero);
                    }
                }
                else
                {
                    if (myImage.Size.Height > SmallHeight)
                    {
                        thumbImage = myImage.GetThumbnailImage(Convert.ToInt32(myImage.Size.Width / intHeight), SmallHeight, null, System.IntPtr.Zero);
                    }
                    else
                    {
                        thumbImage = myImage.GetThumbnailImage(myImage.Size.Width, myImage.Size.Height, null, System.IntPtr.Zero);
                    }
                }
                ////指定缩略图的保存路径
                string fileXltPath = SmallPath;
                //保存缩略图
                thumbImage.Save(fileXltPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                //关闭(释放）缩略图对象
                thumbImage.Dispose();
                //关闭(释放）图片对象
                myImage.Dispose();
                System.Threading.Thread.Sleep(1000);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool FileDelete(string path)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        public static FileExtension CheckTextFile(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            string fileType = string.Empty; ;
            try
            {
                byte data = br.ReadByte();
                fileType += data.ToString();
                data = br.ReadByte();
                fileType += data.ToString();
                FileExtension extension;
                try
                {
                    extension = (FileExtension)Enum.Parse(typeof(FileExtension), fileType);
                }
                catch
                {

                    extension = FileExtension.Validfilf;
                }
                return extension;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    br.Close();
                }
            }
        }
    }
}