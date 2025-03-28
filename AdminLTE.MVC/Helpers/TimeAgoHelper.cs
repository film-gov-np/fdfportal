using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.MVC.Helpers
{
    public class TimeAgoHelper
    {
        public static string GetTimeAgo(DateTime dateTime)
    {
        var timeSpan = DateTime.Now - dateTime;

        if (timeSpan.TotalMinutes < 1)
            return "just now";
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} mins ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hours ago";
        if (timeSpan.TotalDays < 30)
            return $"{(int)timeSpan.TotalDays} days ago";
        if (timeSpan.TotalDays < 365)
            return $"{(int)(timeSpan.TotalDays / 30)} months ago";

        return $"{(int)(timeSpan.TotalDays / 365)} years ago";
    }
    }
}