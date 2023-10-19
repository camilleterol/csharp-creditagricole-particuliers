using System.Globalization;

namespace CreditAgricoleSdk.Extensions;

public static class DateTimeExtension
{
    public static string ToUnixTimestamp(this DateTime dateTime) => (Math.Round((dateTime - new DateTime(1970, 1, 1)).TotalSeconds) * 1000).ToString(CultureInfo.InvariantCulture);
}