using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Services
{
    public static class FriendlyNumberHelper
    {
        public static string FormatNumber(double num)
        {
            // Ensure number has max 3 significant digits (no rounding up can happen)
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;

            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + "B";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "M";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "K";

            return num.ToString("#,0");
        }

        public static string FormatNumber(long num)
        {
            double lngNum = Convert.ToDouble(num);
            // Ensure number has max 3 significant digits (no rounding up can happen)
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(lngNum) - 2));
            lngNum = lngNum / i * i;

            if (lngNum >= 1000000000)
                return (lngNum / 1000000000D).ToString("0.##") + "B";
            if (lngNum >= 1000000)
                return (lngNum / 1000000D).ToString("0.##") + "M";
            if (lngNum >= 1000)
                return (lngNum / 1000D).ToString("0.##") + "K";

            return lngNum.ToString("#,0");
        }

        public static string FormatNumber(decimal num)
        {
            double decNum = decimal.ToDouble(num);
            // Ensure number has max 3 significant digits (no rounding up can happen)
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(decNum) - 2));
            decNum = decNum / i * i;

            if (decNum >= 1000000000)
                return (decNum / 1000000000D).ToString("0.##") + "B";
            if (decNum >= 1000000)
                return (decNum / 1000000D).ToString("0.##") + "M";
            if (decNum >= 1000)
                return (decNum / 1000D).ToString("0.##") + "K";

            return num.ToString("#,0");
        }
    }
}
