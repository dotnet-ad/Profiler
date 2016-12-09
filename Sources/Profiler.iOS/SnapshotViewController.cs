namespace Debugging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
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
			var reference = references.ElementAt(indexPath.Section).ElementAt(indexPath.Row);

			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			cell.TextLabel.Text = reference.Name;
			cell.DetailTextLabel.Text = $"{reference.Creation.ToString("T")} - {reference.Type.ToString()}";

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


	}
}
