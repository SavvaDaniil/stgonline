using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Component
{
    public class ResizeImageComponent
    {
        public void ResizeTmpImageAndSaveFinally_16_9(string imageUrl, string destPath)
        {
            ResizeTmpImageAndSaveFinally(imageUrl, destPath, 9, 16);
        }
        public void ResizeTmpImageAndSaveFinally_9_16(string imageUrl, string destPath)
        {
            ResizeTmpImageAndSaveFinally(imageUrl, destPath, 16, 9);
        }
        public void ResizeTmpImageAndSaveFinally_box(string imageUrl, string destPath)
        {
            ResizeTmpImageAndSaveFinally(imageUrl, destPath, 1, 1);
        }

        private void ResizeTmpImageAndSaveFinally(string imageUrl, string destPath, int xProportion, int yProportion)
        {
            System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(imageUrl);

            int newWidth = 0;
            int newHeight = 0;
            if (xProportion > yProportion)
            {
                newWidth = fullSizeImg.Width;
                newHeight = (int)Math.Round((double)(fullSizeImg.Width * xProportion / yProportion));
            } else
            {
                newHeight = fullSizeImg.Height;
                newWidth = (int)Math.Round((double)(fullSizeImg.Height * yProportion / xProportion));
            }

            /*
            System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(newWidth, newHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            System.Drawing.Imaging.ImageCodecInfo[] info = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.EncoderParameters param = new System.Drawing.Imaging.EncoderParameters(1);
            param.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            */

            byte[] result;
            try
            {
                using (Image thumbnail = new Bitmap(newWidth, newHeight))
                {
                    using (Bitmap source = new Bitmap(imageUrl))
                    {
                        using (Graphics g = Graphics.FromImage(thumbnail))
                        {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(source, 0, 0, newWidth, newHeight);
                            g.Dispose();
                        }
                        source.Dispose();
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        thumbnail.Save(ms, ImageFormat.Png);
                        thumbnail.Save(destPath, ImageFormat.Png);
                        result = ms.ToArray();
                        ms.Dispose();
                    }
                    thumbnail.Dispose();
                }
            }
            catch (Exception)
            {

            }
            fullSizeImg.Dispose();

            /*
            //thumbNailImg.Save(destPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            thumbNailImg.Save(destPath, info[1], param);
            thumbNailImg.Dispose();
            fullSizeImg.Dispose();
            */
        }

        private bool ThumbnailCallback()
        {
            return true;
        }
    }
}
