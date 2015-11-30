using System;

namespace CommonQuery.Builder
{
    /// <summary>
    ///     Class UnixTime.
    /// </summary>
    internal class UnixTime
    {
        /// <summary>
        ///     The _base time
        /// </summary>
        private static DateTime baseTime = new DateTime(1970, 1, 1);

        /// <summary>
        ///     Froms the date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>System.Int64.</returns>
        public static long FromDateTime(DateTime dateTime)
        {
            return (dateTime.Ticks - baseTime.Ticks) / 10000000 - 8 * 60 * 60;
        }

        /// <summary>
        ///     Convert unixtime to net DateTime
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <returns>DateTime.</returns>
        public static DateTime FromUnixTime(long timeStamp)
        {
            return new DateTime((timeStamp + 8 * 60 * 60) * 10000000 + baseTime.Ticks);
        }
    }
}