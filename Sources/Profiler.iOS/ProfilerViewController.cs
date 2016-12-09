namespace Debugging
{
	using UIKit;

	public class ProfilerViewController : UINavigationController
	{
		public ProfilerViewController() : base(new SnapshotsViewController())
		{
		}
	}
}
