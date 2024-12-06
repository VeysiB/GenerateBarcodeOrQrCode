using BarcodeLib;
using GenerateBarcodeOrQrCode.Models;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace GenerateBarcodeOrQrCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //barcode
        public IActionResult GenerateBarcode( string code="112233")
        {
            Barcode barcode=new Barcode();
            Image img = barcode.Encode(TYPE.CODE39, code, 250, 100);
            var data = ConvertImageToByte(img);
            return File(data, "image/jpeg");
        }
        private byte[] ConvertImageToByte(Image img)
        {
            using(MemoryStream memoryStream = new MemoryStream())
            {
                img.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }

        //QR Code
        public IActionResult GenerateQrCode(string code)
        {
            if (code == null) code = "Veysi Aren Rodin";
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap bitmap = qRCode.GetGraphic(15);
            var bitmapBytes = ConvertBitmapToBytes(bitmap);
            return File(bitmapBytes, "image/jpeg");
        }
        private byte[] ConvertBitmapToBytes(Bitmap bitmap)
        {
            using(var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
    }
}