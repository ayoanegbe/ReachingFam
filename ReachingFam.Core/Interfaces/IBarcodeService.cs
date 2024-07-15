using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IBarcodeService
    {
        byte[] GenerateBarcode(string barcodeText);
        string ReadBarcode(byte[] barcodeImage);
    }
}
