using System;
using System.Text;

namespace AmazonDeliveryPlanner
{
    public static class TimespanExtensions
    {
        // from https://stackoverflow.com/a/36191436
        //public static string ToHumanReadableString(this TimeSpan t)
        //{
        //    if (t.TotalSeconds <= 1)
        //    {
        //        return $@"{t:s\.ff} seconds";
        //    }
        //    if (t.TotalMinutes <= 1)
        //    {
        //        return $@"{t:%s} seconds";
        //    }
        //    if (t.TotalHours <= 1)
        //    {
        //        return $@"{t:%m} minutes";
        //    }
        //    if (t.TotalDays <= 1)
        //    {
        //        return $@"{t:%h} hours";
        //    }

        //    return $@"{t:%d} days";
        //}

        // from https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.Core/Dashboard/HtmlHelper.cs        
        // public string ToHumanDuration(TimeSpan? duration, bool displaySign = true)
        public static string ToHumanReadableString(this TimeSpan duration, bool displaySign = false)
        {
            if (duration == null) return null;

            var builder = new StringBuilder();
            if (displaySign)
            {
                builder.Append(duration.TotalMilliseconds < 0 ? "-" : "+");
            }

            duration = duration.Duration();

            if (duration.Days > 0)
            {
                builder.Append($"{duration.Days}d ");
            }

            if (duration.Hours > 0)
            {
                builder.Append($"{duration.Hours}h ");
            }

            if (duration.Minutes > 0)
            {
                builder.Append($"{duration.Minutes}m ");
            }

            if (duration.TotalHours < 1)
            {
                if (duration.Seconds > 0)
                {
                    builder.Append(duration.Seconds);
                    if (duration.Milliseconds > 0)
                    {
                        builder.Append($".{duration.Milliseconds.ToString().PadLeft(3, '0')}");
                    }

                    builder.Append("s ");
                }
                else
                {
                    if (duration.Milliseconds > 0)
                    {
                        builder.Append($"{duration.Milliseconds}ms ");
                    }
                }
            }

            if (builder.Length <= 1)
            {
                builder.Append(" <1ms ");
            }

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }
    }
}
