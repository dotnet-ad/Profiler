using UIKit;

namespace Debugging
{
	public static class ProfilerExtensions
	{
		public static void Show(this Profiler profiler)
		{
			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentModalViewController(new ProfilerViewController(),true);
		}
	}
}
