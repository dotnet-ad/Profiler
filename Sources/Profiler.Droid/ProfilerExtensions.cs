using Android.App;

namespace Debugging
{
	public static class ProfilerExtensions
	{
		public static void Show(this Profiler profiler, Activity activity)
		{
			activity.StartActivity(typeof(ProfilerActivity));
		}
	}
}
