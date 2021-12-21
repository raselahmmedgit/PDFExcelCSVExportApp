using iTextSharp.text.pdf;
using QRCodeDecoderLibrary;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace lab.PDFExcelCSVExportApp.Helpers
{
    public static class QrBarCodeHelper
    {
        public static Result QrBarCodeFromPdf(string uploadFolderPath, string fileFullPath)
        {
            try
            {
                //get images from source2.pdf  
                //string barCodeImgFullPath = GetImages(uploadFolderPath, fileFullPath);
                string barCodeImgFullPath = @"C:\Users\placo\OneDrive\Documents\GitHub\PDFExcelCSVExportApp\lab.PDFExcelCSVExportApp\lab.PDFExcelCSVExportApp\wwwroot\upload\Bloom-Richard-QrCode.jpg";

                string originalFileName = "Bloom-Richard-QrCode.jpg";

                string newFileName120x124 = originalFileName.Substring(0, originalFileName.LastIndexOf('.')) + "-120X124" + originalFileName.Substring(originalFileName.LastIndexOf('.'), originalFileName.Length - originalFileName.LastIndexOf('.'));
                //string barCodeImgFullPath120x124 = ImageHelper.CropImageWithFullPath(uploadFolderPath, string.Empty, originalFileName, newFileName120x124, 0, 0, 120, 124);
                string barCodeImgFullPath120x124 = @"C:\Users\placo\OneDrive\Documents\GitHub\PDFExcelCSVExportApp\lab.PDFExcelCSVExportApp\lab.PDFExcelCSVExportApp\wwwroot\upload\Bloom-Richard-QrCode-120X124.jpg";

                if (!string.IsNullOrEmpty(barCodeImgFullPath120x124))
                {
                    //scan the images for barcode  
                    bool imageExist = File.Exists(barCodeImgFullPath120x124);
                    if (imageExist)
                    {
                        // create QR Code decoder object
                        QRDecoder qRDecoder = new QRDecoder();

                        using (var image = Image.FromFile(Path.Combine(barCodeImgFullPath120x124)))
                        {
                            Bitmap bitmapImage = new Bitmap(image);

                            // call image decoder methos with <code>Bitmap</code> image of QRCode barcode
                            byte[][] dataByteArray = qRDecoder.ImageDecoder(bitmapImage);

                            string qrCodeResult = ByteArrayToStr(dataByteArray[0]);

                            return Result.Ok(MessageHelper.BarCodeScanner, qrCodeResult);
                        }
                            
                    }

                    return Result.Fail(MessageHelper.BarCodeScannerFail);

                }
                else
                {
                    return Result.Fail(MessageHelper.BarCodeScannerFail);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // The QRDecoder converts byte array to text string the class using this conversion
        public static string ByteArrayToStr(byte[] dataArray)
        {
            var decoder = Encoding.UTF8.GetDecoder();
            int CharCount = decoder.GetCharCount(dataArray, 0, dataArray.Length);
            char[] CharArray = new char[CharCount];
            decoder.GetChars(dataArray, 0, dataArray.Length, CharArray, 0);
            return new string(CharArray);
        }

        private static string GetImages(string uploadFolderPath, string fileFullPath, int pageNumber = 1)
        {
            string barCodeImgFullPath = string.Empty;

            PdfReader pdfReader = new PdfReader(fileFullPath);
            PdfDictionary pdfDictionary = pdfReader.GetPageN(pageNumber);
            PdfDictionary pdfResources = (PdfDictionary)PdfReader.GetPdfObject(pdfDictionary.Get(PdfName.RESOURCES));
            PdfDictionary pdfXobject = (PdfDictionary)PdfReader.GetPdfObject(pdfResources.Get(PdfName.XOBJECT));
            if (pdfXobject == null)
            {
                return barCodeImgFullPath; 
            }
            
            foreach (PdfName pdfName in pdfXobject.Keys)
            {
                PdfObject pdfObject = pdfXobject.Get(pdfName);
                if (!pdfObject.IsIndirect()) 
                { 
                    continue;
                }

                PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(pdfObject);
                PdfName pdfSubtype = (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
                if (!pdfSubtype.Equals(PdfName.IMAGE)) 
                { 
                    continue;
                }

                int xrefIndex = Convert.ToInt32(((PRIndirectReference)pdfObject).Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                
                PdfObject pdfObj = pdfReader.GetPdfObject(xrefIndex);
                PdfStream pdfStrem = (PdfStream)pdfObj;
                byte[] bytes = PdfReader.GetStreamBytesRaw((PRStream)pdfStrem);
                if (bytes == null) { continue; }
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    memoryStream.Position = 0;
                    Image img = Image.FromStream(memoryStream);

                    //string fileName = Path.Combine(String.Format(@"Bloom-Richard-QrCode-{0}.jpg", pageNumber));
                    string fileName = @"Bloom-Richard-QrCode.jpg";
                    barCodeImgFullPath = Path.Combine(uploadFolderPath, fileName);

                    EncoderParameters parms = new EncoderParameters(1);
                    parms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 0);
                    var jpegEncoder = ImageCodecInfo.GetImageEncoders().ToList().Find(x => x.FormatID == ImageFormat.Jpeg.Guid);

                    img.Save(barCodeImgFullPath, jpegEncoder, parms);

                    return barCodeImgFullPath;
                }
            }

            return barCodeImgFullPath;
        }

    }
}
