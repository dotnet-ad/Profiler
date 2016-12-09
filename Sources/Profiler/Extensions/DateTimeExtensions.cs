
namespace Debugging
{
	using System;

	public static class DateTimeExtensions
	{
		public static string FromNowLabel(this DateTime time)
		{
			var interval = (DateTime.Now - time);
			if (interval >= TimeSpan.FromHours(2)) return (int)(interval.TotalHours) + "h";
			if (interval >= TimeSpan.FromMinutes(2)) return (int)(interval.TotalMinutes) + "min";
			return (int)(interval.TotalSeconds) + "s";
		}
	}
}
