using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;
using Debugging;

namespace Profiler.Sample.iOS
{
	public partial class ViewController : UIViewController
	{
		public ViewController(bool forceLeak)
		{
			this.Title = "Example " + this.GetHashCode();
				
			//Start profiling this instance
			Debugging.Profiler.Default.Memory.Register(this, nameof(Title));

			this.forceLeak = forceLeak;
		}

		readonly bool forceLeak;

		private UIButton b1, b2;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			b1 = new UIButton(new CGRect(50, 50, 200, 50));
			b1.SetTitle("Next", UIControlState.Normal);
			this.View.Add(b1);

			b2 = new UIButton(new CGRect(50, 150, 200, 50));
			b2.SetTitle("Next with leak", UIControlState.Normal);
			this.View.Add(b2);
		}
		 
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			b1.TouchUpInside += OnNext;
			b2.TouchUpInside += OnNextWithLeak;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			if (!forceLeak)
			{
				b1.TouchUpInside -= OnNext;
				b2.TouchUpInside -= OnNextWithLeak;
			}
		}

		void OnNext(object sender, EventArgs e)
		{
			this.NavigationController.PushViewController(new ViewController(false), true);
		}

		void OnNextWithLeak(object sender, EventArgs e)
		{
			this.NavigationController.PushViewController(new ViewController(true), true);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void MotionEnded(UIEventSubtype motion, UIEvent evt)
		{
			if (motion == UIEventSubtype.MotionShake)
			{
				Debugging.Profiler.Default.Show();
			}
		}
	}
}
