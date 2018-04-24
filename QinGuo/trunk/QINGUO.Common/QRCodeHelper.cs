using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using ThoughtWorks.QRCode.Codec;
using System.Configuration;
using System.Web;
using System.Drawing.Imaging;
using System.IO;

namespace QINGUO.Common
{
    /// <summary>
    /// 二维码生成类
    /// </summary>
    public class QRCodeHelper
    {

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="LinkCode">二维码内容</param>
        /// <param name="isTest">是否测试</param>
        /// <param name="merchantLogo">商户Logo  默认木有值</param>
        /// <returns></returns>
        public static string CreateQRCode(string LinkCode, string merchantLogo = null)
        {

            string resultPath = string.Empty;


            if (!string.IsNullOrEmpty(LinkCode))
            {
                //创建二维码编码器
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 4;

                qrCodeEncoder.QRCodeVersion = 8;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

                //根据传递的字符串 生成一个二维码
                System.Drawing.Image image = qrCodeEncoder.Encode(LinkCode, Encoding.UTF8);

                //将二维码保存到内存流中
                System.IO.MemoryStream MStream = new System.IO.MemoryStream();

                image.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);


                //内存组合图片
                System.IO.MemoryStream MStream1 = new System.IO.MemoryStream();


                string MasterDefaultImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MasterDefaultImageName"]);
                Image imageResult = null;
                if (string.IsNullOrEmpty(merchantLogo))
                {
                    imageResult = CombinImage(image, MasterDefaultImagePath);
                }
                else
                {
                    //组合两张图片
                    imageResult = CombinImage(image, MasterDefaultImagePath, merchantLogo);
                }
                //保存到内存流
                imageResult.Save(MStream1, System.Drawing.Imaging.ImageFormat.Png);


                //HttpContext.Current.Response.ClearContent();
                //HttpContext.Current.Response.ContentType = "image/png";
                //HttpContext.Current.Response.BinaryWrite(MStream1.ToArray());

                //image.Dispose();

                //声明画布
                using (Bitmap bitmap = new Bitmap(200, 200))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, 200, 200));
                        g.DrawImage(image, new Rectangle(10, 10, 180, 180));

                        string path = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";

                        //没有logo到默认文件文件夹
                        string savePath = string.Empty;
                        if (string.IsNullOrEmpty(merchantLogo))
                        {
                            savePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["defaultImage"]);

                            //数据库保存路径
                            resultPath = ConfigurationManager.AppSettings["defaultImage"].Trim('~');
                            resultPath = resultPath + path;

                        }
                        else//到logo文件夹
                        {
                            savePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["merchantImage"]);

                            //数据库保存路径
                            resultPath = ConfigurationManager.AppSettings["merchantImage"].Trim('~');
                            resultPath = resultPath + path;
                        }
                        //最终保存路径

                        savePath = savePath + path;
                        if (!string.IsNullOrEmpty(savePath))
                        {
                            if (!File.Exists(savePath))
                            {
                                Directory.CreateDirectory(savePath);
                            }
                            //路径加上文件名字
                            string fileName = Guid.NewGuid().ToString();
                            savePath = savePath + fileName + ".png";

                            //数据库保存文件路径
                            resultPath = resultPath + fileName + ".png";

                            //保存路径修改
                            bitmap.Save(savePath, ImageFormat.Png);
                        }
                    }
                }

                MStream.Dispose();
                MStream1.Dispose();
            }
            //HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.End();
            return resultPath;
        }


        ///
        /// 调用此函数后使此两种图片合并，类似相册，有个
        /// 背景图，中间贴自己的目标图片
        ///
        /// 粘贴的源图片
        /// 粘贴的目标图片
        public static Image CombinImage(Image imgBack, string destImg)
        {
            Image img = Image.FromFile(destImg); //照片图片
            if (img.Height != 65 || img.Width != 65)
            {
                //两张图片
                img = KiResizeImage(img, 65, 65);
            }
            Graphics g = Graphics.FromImage(imgBack);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
            //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);
            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }

        ///
        /// 调用此函数后使此两种图片合并，类似相册，有个
        /// 背景图，中间贴自己的目标图片
        ///
        /// 粘贴的源图片
        /// 粘贴的目标图片
        public static Image CombinImage(Image imgBack, string officeImage, string merchImagePath)
        {
            Image img = CombinMerchImage(officeImage, merchImagePath); //照片图片

            Graphics g = Graphics.FromImage(imgBack);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);

            //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);
            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);


            GC.Collect();

            return imgBack;
        }
        ///
        /// Resize图片
        ///
        /// 原始Bitmap
        /// 新的宽度
        /// 新的高度
        /// 保留着，暂时未用
        /// 处理以后的图片                商家图片         65         65
        public static Image KiResizeImage(Image bmp, int newW, int newH)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        //合并两张图片
        public static Image CombinMerchImage(string officeImagePath, string merchimagePath)
        {
            Image imgOffice = Image.FromFile(officeImagePath); //官网照片图片
            Image merchImage = Image.FromFile(merchimagePath);//商家图片

            Image b = new Bitmap(65, 65);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.DrawImage(imgOffice, new Rectangle(0, 0, 65, 65), new Rectangle(0, 0, imgOffice.Width, imgOffice.Height), GraphicsUnit.Pixel);
            g.DrawImage(imgOffice, new Rectangle(0, 0, 32, 65));
            g.DrawImage(merchImage, new Rectangle(31, 0, 32, 65));
            //g.DrawImage(merchImage, new Rectangle(0, 0, 65, 65), new Rectangle(0, 0, merchImage.Width, merchImage.Height), GraphicsUnit.Pixel);
            return b;
        }
    }
}
