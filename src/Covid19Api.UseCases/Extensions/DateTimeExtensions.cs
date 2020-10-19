using System;

namespace Covid19Api.UseCases.Extensions
{
    internal static class DateTimeExtensions
    {
        /// <summary>
        /// Adds one month and subtracts 1 second from the given <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> for the calculation.</param>
        /// <returns></returns>
        public static DateTime MonthsEnd(this DateTime dateTime)
            => dateTime.AddMonths(1).AddSeconds(-1);
    }
}