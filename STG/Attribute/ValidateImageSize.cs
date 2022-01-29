using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace STG.Attribute
{
    public class ValidateImageSize
    {
        public static System.Drawing.Image checkAndResizeIfNeed_16_9(IFormFile file)
        {
            if (!ValidateImageAttribute.IsValid(file)) return null;

            Image imgToResize = Image.FromStream(file.OpenReadStream(), true, true);
            Bitmap imgBitmap = new Bitmap(imgToResize);

            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            int newImgWidth = sourceWidth;
            int newImgHeight = sourceWidth * 9 / 16;
            Bitmap answerImgBitmap = new Bitmap(sourceWidth, newImgHeight);
            using (Graphics g = Graphics.FromImage(answerImgBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0,0 , newImgWidth, newImgHeight);
                g.Dispose();

                return (System.Drawing.Image)answerImgBitmap;
            }
        }


        public static System.Drawing.Image checkAndResizeIfNeed_box(IFormFile file)
        {
            if (!ValidateImageAttribute.IsValid(file)) return null;

            Image imgToResize = Image.FromStream(file.OpenReadStream(), true, true);
            Bitmap imgBitmap = new Bitmap(imgToResize);

            int basicSize = (imgToResize.Width > imgToResize.Height ? imgToResize.Height : imgToResize.Width);

            int newImgWidth = basicSize;
            int newImgHeight = basicSize;
            Bitmap answerImgBitmap = new Bitmap(basicSize, newImgHeight);
            using (Graphics g = Graphics.FromImage(answerImgBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, newImgWidth, newImgHeight);
                g.Dispose();

                return (System.Drawing.Image)answerImgBitmap;
            }
        }
    }
}
