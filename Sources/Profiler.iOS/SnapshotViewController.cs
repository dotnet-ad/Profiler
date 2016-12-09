namespace Debugging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CoreGraphics;
	using Foundation;
	using UIKit;

	public class SnapshotViewController : UITableViewController
	{
		public SnapshotViewController(Snapshot snapshot)
		{
			this.Title = snapshot.Time.ToString("T");
			this.references = snapshot.References.GroupBy((arg) => arg.Type);
		}

		readonly IEnumerable<IGrouping<Type, Snapshot.ReferenceSnapshot>> references;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var identifier = nameof(UITableViewCell);
			var cell = tableView.DequeueReusableCell(nameof(UITableViewCell)) ?? new UITableViewCell(UITableViewCellStyle.Subtitle, identifier);
			var reference = references.ElementAt(indexPath.Section).OrderByDescending(r => r.Creation).ElementAt(indexPath.Row);

			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			cell.TextLabel.Text = reference.Name;
			cell.DetailTextLabel.Text = $"Created at {reference.Creation.ToString("T")}";

			return cell;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return references.ElementAt((int)section).Count();
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return references.Count();
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var reference = references.ElementAt(indexPath.Section).ElementAt(indexPath.Row);
			this.NavigationController.PushViewController(new ReferenceViewController(reference), true);
		}

		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			return 32;
		}

		public override UIView GetViewForHeader(UITableView tableView, nint section)
		{
			var type = references.ElementAt((int)section).Key;

			var label = new UILabel(new CGRect(18,6, this.View.Bounds.Width,20))
			{
				Text = $"{type}",
				Font = UIFont.BoldSystemFontOfSize(10),
			};

			var result = new UIView()
			{
				BackgroundColor = UIColor.FromWhiteAlpha(0.94f,1),
			};
			result.AddSubview(label);
			return result;
		}

	}
}
