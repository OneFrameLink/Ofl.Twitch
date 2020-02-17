using System;

namespace Ofl.Twitch
{
    internal static class ReadOnlySpanExtensions
    {
        public static TimeSpan? ParseDuration(this ReadOnlySpan<byte> span)
        {
            // The weeks, days, hours, minutes, seconds.
            int? weeks = null, days = null, hours = null, minutes = null, seconds = null;

            // The current value.
            int value = 0;

            // Cycle through the span items.
            foreach (byte b in span)
            {
                // Is this a number?
                if (b >= '0' && b <= '9')
                    // Update the value.
                    value = (value * 10) + (b - '0');
                else if (GetPart(b, value, ref weeks, ref days, ref hours, ref minutes, ref seconds))
                    // Reset the value.
                    value = 0;
                else
                    return null;
            }

            // Construct a timespan.
            return new TimeSpan((weeks ?? 0) * 7 + (days ?? 0), hours ?? 0, minutes ?? 0, seconds ?? 0);
        }

        private static bool GetPart(
            in byte c,
            int value,
            ref int? weeks,
            ref int? days,
            ref int? hours,
            ref int? minutes,
            ref int? seconds
        )
        {
            // Switch on the byte.
            switch (c)
            {
                case (byte)'w':
                case (byte)'W':
                    // Set if not set.
                    if (weeks == null) { weeks = value; return true; }
                    return false;

                case (byte)'d':
                case (byte)'D':
                    // Set if not set.
                    if (days == null) { days = value; return true; }
                    return false;

                case (byte)'h':
                case (byte)'H':
                    // Set if not set.
                    if (hours == null) { hours = value; return true; }
                    return false;

                case (byte)'m':
                case (byte)'M':
                    // Set if not set.
                    if (minutes == null) { minutes = value; return true; }
                    return false;

                case (byte)'s':
                case (byte)'S':
                    // Set if not set.
                    if (seconds == null) { seconds = value; return true; }
                    return false;

                default:
                    return false;
            }
        }
    }
}
