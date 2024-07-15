using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using ZXing;
using ZXing.Common;
using SkiaSharp;
using IronBarCode;
using ReachingFam.Core.Interfaces;

namespace ReachingFam.Core.Services
{
    public class BarcodeService : IBarcodeService
    {
        public byte[] GenerateBarcode(string barcodeText)
        {
            var barcode = BarcodeWriter.CreateBarcode(barcodeText, BarcodeEncoding.Code128);
            using var ms = new MemoryStream();
            barcode.SaveAsPng(ms.ToString());
            return ms.ToArray();
        }

        public string ReadBarcode(byte[] barcodeImage)
        {
            using var ms = new MemoryStream(barcodeImage);
            var barcodeResult = BarcodeReader.Read(ms);
            return barcodeResult?.ToString();
        }
    }
}
