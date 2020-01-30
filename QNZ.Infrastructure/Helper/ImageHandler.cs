//using FreeImageAPI;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.Processing.Filters;
//using SixLabors.ImageSharp.Processing.Transforms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIG.Infrastructure.Helper
{
    public sealed class ImageHandler
    {
        static readonly Random rand = new Random();
        private static int RandomNumber(int next)
        {
            return rand.Next(next);
        }

        /// <summary>
        /// 使用随机数获取一个随机的文件名称
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <returns></returns>
        public static string GetRandomFileName(string sourceFileName)
        {
            string fileExtension = Path.GetExtension(sourceFileName).ToLower();
            return GetRandomFileName(fileExtension, 100000);
        }

        /// <summary>
        /// 截取时间获取一个随机的文件名称
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="randomNum"></param>
        /// <returns></returns>
        public static string GetRandomFileName(string extension, int randomNum)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff") + (randomNum > 0 ? "_" + RandomNumber(randomNum).ToString() : string.Empty) + extension;
        }

        /// <summary>
        /// 获取后缀，检查图片的格式
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CheckImageType(string fileName)
        {
            string fileExtend = Path.GetExtension(fileName).ToLower();
            if (fileExtend == ".jpg" || fileExtend == ".bmp" || fileExtend == ".gif" || fileExtend == ".png" || fileExtend == ".jpeg")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加载文件，获取文件真实格式
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static bool CheckImageType(Stream sr)
        {
            BinaryReader reader = new BinaryReader(sr);

            byte buffer;
            buffer = reader.ReadByte();
            string fileClass = buffer.ToString();
            buffer = reader.ReadByte();
            fileClass += buffer.ToString();

            sr.Position = 0;
            reader = null;

            //jpg || gif ||bmp ||png
            if (fileClass == "255216" || fileClass == "7173" || fileClass == "6677" || fileClass == "13780")
                return true;

            return false;
        }

        //public void ResizeImage(IFormFile uploadedFile, string desiredThumbPath, int desiredWidth = 0, int desiredHeight = 0)
        //{
        //    string webroot = host.WebRootPath;

        //    if (uploadedFile.Length > 0)
        //    {
        //        using (var stream = uploadedFile.OpenReadStream())
        //        {
        //            var uploadedImage = System.Drawing.Image.FromStream(stream);

        //            //decide how to scale dimensions
        //            if (desiredHeight == 0 && desiredWidth > 0)
        //            {
        //                var img = ImageResize.ScaleByWidth(uploadedImage, desiredWidth); // returns System.Drawing.Image file
        //                img.SaveAs(Path.Combine(webroot, desiredThumbPath));
        //            }
        //            else if (desiredWidth == 0 && desiredHeight > 0)
        //            {
        //                var img = ImageResize.ScaleByHeight(uploadedImage, desiredHeight); // returns System.Drawing.Image file
        //                img.SaveAs(Path.Combine(webroot, desiredThumbPath));
        //            }
        //            else
        //            {
        //                var img = ImageResize.Scale(uploadedImage, desiredWidth, desiredHeight); // returns System.Drawing.Image file
        //                img.SaveAs(Path.Combine(webroot, desiredThumbPath));
        //            }
        //        }
        //    }
        //    return;
        //}

        public static ImageCodecInfo GetImgCodecInf(string mimeType)
        {
            ImageCodecInfo[] imgCodecInfo = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo infoItem in imgCodecInfo)
            {
                if (infoItem.MimeType.ToString().ToLower() == mimeType.ToLower())
                {
                    return infoItem;
                }
            }

            return null;
        }

        public static void ResizeAndSaveImage(string originalImagePath, string showPath, string thumbnailPath)
        {
            //  using (Bitmap uploadedBmp = new Bitmap(FileUpload1.FileContent))
            using (Bitmap uploadedBmp = new Bitmap(originalImagePath))
            {
                decimal origHeight = uploadedBmp.Height;
                decimal origWidth = uploadedBmp.Width;
                int newHeight = 112;
                int newWidth = Convert.ToInt32(newHeight / (origHeight / origWidth));

                using (Graphics resizedGr = Graphics.FromImage(uploadedBmp))
                {
                    // Optional. These properties are set for the best possible quality
                    resizedGr.CompositingMode = CompositingMode.SourceCopy;
                    resizedGr.CompositingQuality = CompositingQuality.HighQuality;
                    resizedGr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    resizedGr.SmoothingMode = SmoothingMode.HighQuality;
                    resizedGr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (Bitmap resizedBmp = new Bitmap(uploadedBmp, newWidth, newHeight))
                    {
                        resizedGr.DrawImage(resizedBmp, 0, 0);

                        using (MemoryStream resizedMs = new MemoryStream())
                        {
                            System.Drawing.Imaging.EncoderParameters encParms = new System.Drawing.Imaging.EncoderParameters(1);

                            // This allows jpeg compression to be set to 90
                            encParms.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

                            resizedBmp.Save(resizedMs, GetImgCodecInf("image/jpeg"), encParms);
                            long msLen = resizedMs.Length;
                            byte[] resizedData = new byte[msLen];
                            resizedData = resizedMs.ToArray();

                            using (System.IO.FileStream fStream = new System.IO.FileStream(showPath, System.IO.FileMode.Create))
                            {
                                fStream.Write(resizedData, 0, resizedData.Length);
                            }

                            // Repeat process to create a thumbnail image, reusing resizedBmp
                            // This approach does not use the 'using' statement or the high quality graphics properties

                            origHeight = resizedBmp.Height;
                            origWidth = resizedBmp.Width;
                            int thumbHeight = 100;
                            int thumbWidth = Convert.ToInt32(thumbHeight / (origHeight / origWidth));

                            Bitmap thumbBmp = new Bitmap(resizedBmp, thumbWidth, thumbHeight);
                            Graphics thumbGr = Graphics.FromImage(thumbBmp);
                            thumbGr.DrawImage(thumbBmp, 0, 0);

                            MemoryStream thumbMs = new MemoryStream();
                            thumbBmp.Save(thumbMs, System.Drawing.Imaging.ImageFormat.Jpeg);
                            long thumbmsLen = thumbMs.Length;
                            byte[] thumbData = new byte[thumbmsLen];
                            thumbData = thumbMs.ToArray();

                            System.IO.FileStream tStream = new System.IO.FileStream(thumbnailPath, System.IO.FileMode.Create);
                            tStream.Write(thumbData, 0, thumbData.Length);
                            tStream.Close();

                            thumbGr.Dispose();
                            thumbBmp.Dispose();
                            thumbMs.Dispose();
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param> 
        /// <param name="mode1">HW/指定高宽缩放(可能变形)</param> 
        /// <param name="mode2">W/指定宽，高按比例</param> 
        /// <param name="mode3">H/指定高，宽按比例</param> 
        /// <param name="mode4">Cut/指定高宽裁减(不变形)</param> 
        /// <param name="mode5">DB/等比缩放(不变形，如果高大按高，宽大按宽缩放)</param> 
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode, string type)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW":    //指定高宽缩放（可能变形） 
                    break;
                case "W":     //指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":     //指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":   //指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                case "DB":      //等比缩放（不变形，如果高大按高，宽大按宽缩放） 
                    if ((double)originalImage.Width / (double)towidth < (double)originalImage.Height / (double)toheight)
                    {
                        toheight = height;
                        towidth = originalImage.Width * height / originalImage.Height;
                    }
                    else
                    {
                        towidth = width;
                        toheight = originalImage.Height * width / originalImage.Width;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new Bitmap(towidth, toheight);
            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);


            g.CompositingMode = CompositingMode.SourceCopy;
            g.CompositingQuality = CompositingQuality.HighQuality;
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);

            try
            {
                //保存缩略图
                if (type == ".jpg")
                {
                    bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
                }
                if (type == ".bmp")
                {
                    bitmap.Save(thumbnailPath, ImageFormat.Bmp);
                }
                if (type == ".gif")
                {
                    bitmap.Save(thumbnailPath, ImageFormat.Gif);
                }
                if (type == ".png")
                {
                    bitmap.Save(thumbnailPath, ImageFormat.Png);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        public static void MakeThumbnail2(string originalImagePath, string thumbnailPath, int width, int height)
        {
            if (Path.GetExtension(originalImagePath) != ".svg")
            {
                Image originalImage = Image.FromFile(originalImagePath);

                var orgWidth = originalImage.Width;
                int orgHeight = originalImage.Height;

                double tempHeight = orgHeight / (orgWidth / width);
                double tempWidth;
                if (tempHeight > height)
                {
                    tempWidth = orgWidth / (orgHeight / height);
                    tempHeight = height;
                }
                else
                {
                    tempWidth = width;
                }



                FileInfo fi = new FileInfo(thumbnailPath);
                if (!fi.Directory.Exists)
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
                Image thumb = originalImage.GetThumbnailImage((int)tempWidth, (int)tempHeight, () => false, IntPtr.Zero);
                thumb.Save(thumbnailPath);
                originalImage.Dispose();
            }

            
        }



    }
}
