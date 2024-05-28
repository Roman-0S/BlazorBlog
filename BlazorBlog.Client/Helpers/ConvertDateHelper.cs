using System.Xml.Linq;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorBlog.Client.Helpers
{
    public static class ConvertDateHelper
    {

        public static string ConvertDate(DateTimeOffset date)
        {
            string convertedDate = string.Empty;

            TimeSpan timeDiff = DateTimeOffset.Now - date;

            if (timeDiff < TimeSpan.FromHours(1))
            {
                convertedDate = timeDiff.Seconds.ToString() + " minutes ago";
            }
            else if (timeDiff < TimeSpan.FromDays(1))
            {
                convertedDate = timeDiff.Hours.ToString() + " hours ago";
            }
            else if (timeDiff < TimeSpan.FromDays(30))
            {
                convertedDate = timeDiff.Days.ToString() + " days ago";
            }
            else
            {
                convertedDate = date.ToLocalTime().ToString("d");
            }

            return convertedDate;
        }

    }
}
