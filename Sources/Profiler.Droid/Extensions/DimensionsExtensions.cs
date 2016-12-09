using System;
using Android.Content;
using Android.Util;

namespace Debugging.Extensions
{
	public static class DimensionsExtensions
	{
		public static int FromDip(this float dip, Context context)
		{
			return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, context.Resources.DisplayMetrics);
		}
	}
}
