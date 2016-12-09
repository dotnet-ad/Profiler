using System;
using Debugging.Memory;
using Foundation;
using UIKit;
using Debugging;

namespace Profiler.iOS
{
	public partial class ReferenceCell : UITableViewCell
	{
		public static string Key => nameof(ReferenceCell);
		public static nfloat Height = 60;
		public static readonly UINib Nib;

		static ReferenceCell()
		{
			Nib = UINib.FromName(nameof(ReferenceCell), NSBundle.MainBundle);
		}

		protected ReferenceCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public IReference Reference
		{
			set
			{
				var time = $"+{value.Creation.FromNowLabel()}";
				if (!value.IsAlive) time += $" -{value.Destruction.Value.FromNowLabel()}";

				this.icon.Text = value.IsAlive ? "🔷" : "🔶";
				this.name.Text = value.Name;
				this.type.Text = value.Type.ToString();
				this.time.Text = time;
			}
		}
	}
}
