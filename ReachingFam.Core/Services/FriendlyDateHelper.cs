using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Services
{
    public static class FriendlyDateHelper
    {
        public static string GetPrettyDate(DateTime d)
        {
            TimeSpan elapsedTime = DateTime.Now.Subtract(d);
            int dayDiff = (int)elapsedTime.TotalDays;

            if (dayDiff < 1)
            {
                int minDiff = (int)elapsedTime.TotalMinutes;

                if (minDiff <= 1)
                {
                    return "just now";
                }
                if (minDiff < 60)
                {
                    return $"{minDiff} mins ago";
                }
                if (minDiff >= 60)
                {
                    return $"{Math.Ceiling((double)minDiff / 60)} hours ago";
                }
            }
            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return $"{dayDiff} days ago";
            }
            if (dayDiff < 31)
            {
                return $"{Math.Ceiling((double)dayDiff / 7)} weeks ago";
            }
            return $"{Math.Ceiling((double)dayDiff / 31)} months ago";
        }
    }
}
