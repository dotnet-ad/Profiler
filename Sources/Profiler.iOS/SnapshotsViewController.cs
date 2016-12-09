namespace Debugging
{
	using System;
	using System.Linq;
	using Foundation;
	using UIKit;

	public class SnapshotsViewController : UITableViewController
	{
		public SnapshotsViewController()
		{
			this.Title = "Snapshots";

			this.snapshots = Profiler.Default.Snapshots.OrderByDescending(s => s.Time);

			var thiis = this;
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("Close", UIBarButtonItemStyle.Done, (s,e) =>
			{
				thiis.DismissModalViewController(true);
				thiis = null;
			});
		}

		void OnClick(object sender, EventArgs e)
		{
			this.DismissModalViewController(true);
		}

		readonly IOrderedEnumerable<Snapshot> snapshots;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var identifier = nameof(UITableViewCell);
			var cell = tableView.DequeueReusableCell(nameof(UITableViewCell)) ?? new UITableViewCell(UITableViewCellStyle.Value1, identifier);
			var snapshot = snapshots.ElementAt(indexPath.Row);

			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			cell.TextLabel.Text = snapshot.Time.ToString("T");
			cell.DetailTextLabel.Text = $"{snapshot.References.Count().ToString()} instances";

			return cell;
		}

		public override UIView GetViewForHeader(UITableView tableView, nint section)
		{
			return base.GetViewForHeader(tableView, section);
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return snapshots.Count();
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var snapshot = snapshots.ElementAt(indexPath.Row);
			this.NavigationController.PushViewController(new SnapshotViewController(snapshot), true);
		}
	}
}
