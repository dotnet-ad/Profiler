using System;
using System.IO;
using UIKit;

namespace Profiler.iOS
{
	public class PreviewViewController : UIViewController
	{
		public PreviewViewController(string path)
		{
			this.path = path;
			this.Title = Path.GetFileName(path);
		}

		private string path;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var image = new UIImageView(UIImage.FromFile(path))
			{
				Frame = this.View.Frame,
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
			};

			this.View.AddSubview(image);
		}

	}
}
