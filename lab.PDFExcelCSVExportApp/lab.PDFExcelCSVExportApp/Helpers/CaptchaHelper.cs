using lab.PDFExcelCSVExportApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab.PDFExcelCSVExportApp.Helpers
{
    public static class CaptchaHelper
    {
        private static int _width = 400;
        private static int _height = 100;
        private static Random _random = new Random();

        public static CaptchaViewModel GenerateCaptcha(int charCount = 5)
        {
            CaptchaViewModel captchaViewModel = new CaptchaViewModel();
            var captchaCode = GenerateCaptchaCode(charCount);
            var captchaImage = GenerateCaptchaImage(captchaCode);
            captchaViewModel.CaptchaId = Guid.NewGuid().ToString();
            captchaViewModel.CaptchaCode = captchaCode;
            captchaViewModel.CaptchaImage = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapToBytes(captchaImage)));
            captchaViewModel.CaptchaToken = GetEncryptString($"{captchaViewModel.CaptchaId}_{captchaViewModel.CaptchaCode}");
            return captchaViewModel;
        }

        public static bool IsValidCaptcha(string captchaToken, string captchaId, string captchaText)
        {
            var captchaTokenDectrypt = GetDectryptString(captchaToken);
            if (!string.IsNullOrEmpty(captchaToken) || !string.IsNullOrEmpty(captchaText))
            {
                string[] captchaTokenDectryptArr = captchaTokenDectrypt.Split('_').ToArray();
                var captchaIdDectrypt = captchaTokenDectryptArr[0];
                var captchaCodeDectrypt = captchaTokenDectryptArr[1];
                if (captchaIdDectrypt == captchaId && captchaCodeDectrypt == captchaText)
                {
                    return true;
                }
            }
            return false;
        }

        private static string GenerateCaptchaCode(int charCount)
        {
            Random random = new Random();
            string s = "";
            for (int i = 0; i < charCount; i++)
            {
                int a = random.Next(3);
                int chr;
                switch (a)
                {
                    case 0:
                        chr = random.Next(0, 9);
                        s = s + chr.ToString();
                        break;
                    case 1:
                        chr = random.Next(65, 90);
                        s = s + Convert.ToChar(chr).ToString();
                        break;
                    case 2:
                        chr = random.Next(97, 122);
                        s = s + Convert.ToChar(chr).ToString();
                        break;
                }
            }
            return s;
        }

        private static Bitmap GenerateCaptchaImage(string captchaCode)
        {
            //First declare a bitmap and declare graphic from this bitmap
            Bitmap bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            //And create a rectangle to delegete this image graphic 
            Rectangle rectangle = new Rectangle(0, 0, _width, _height);
            //And create a brush to make some drawings
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.DottedGrid, Color.Aqua, Color.White);
            graphics.FillRectangle(hatchBrush, rectangle);

            //here we make the text configurations
            GraphicsPath graphicPath = new GraphicsPath();
            //add this string to image with the rectangle delegate
            graphicPath.AddString(captchaCode, FontFamily.GenericSansSerif, (int)FontStyle.Bold, 90, rectangle, null);
            //And the brush that you will write the text
            hatchBrush = new HatchBrush(HatchStyle.Percent20, Color.Black, Color.GreenYellow);
            graphics.FillPath(hatchBrush, graphicPath);
            //We are adding the dots to the image
            for (int i = 0; i < (int)(rectangle.Width * rectangle.Height / 50F); i++)
            {
                int x = _random.Next(_width);
                int y = _random.Next(_height);
                int w = _random.Next(10);
                int h = _random.Next(10);
                graphics.FillEllipse(hatchBrush, x, y, w, h);
            }
            //Remove all of variables from the memory to save resource
            hatchBrush.Dispose();
            graphics.Dispose();
            //return the image to the related component
            return bitmap;
        }

        // This method is for converting bitmap into a byte array
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private static string GetEncryptString(string plainObject)
        {
            if (plainObject == null || Convert.ToString(plainObject) == string.Empty)
            {
                return "";
            }
            return CryptographyHelper.EncryptString(plainObject.ToString(), CryptographySettings.KeyPassword,
                CryptographySettings.SaltPassword);
        }

        private static string GetEncryptString(string plainObject, string defaultValue)
        {
            if (plainObject == null || Convert.ToString(plainObject) == string.Empty)
            {
                return defaultValue?.ToString();
            }
            return CryptographyHelper.EncryptString(plainObject.ToString(), CryptographySettings.KeyPassword,
                CryptographySettings.SaltPassword);
        }

        private static string GetDectryptString(string encryptValue)
        {
            if (encryptValue == null || Convert.ToString(encryptValue) == string.Empty)
            {
                return "";
            }
            return CryptographyHelper.DecryptString(encryptValue, CryptographySettings.KeyPassword,
                CryptographySettings.SaltPassword);
        }
    }
}
