namespace Debugging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Debugging.Memory;
	using Foundation;
	using UIKit;

	public class ReferenceViewController : UITableViewController
	{
		public ReferenceViewController(IReference reference)
		{
			this.Title = reference.Name;
			this.properties = reference.Properties.OrderBy((arg) => arg.Key);
		}

		readonly IEnumerable<KeyValuePair<string, string>> properties;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var identifier = nameof(UITableViewCell);
			var cell = tableView.DequeueReusableCell(nameof(UITableViewCell)) ?? new UITableViewCell(UITableViewCellStyle.Value1, identifier);
			var property = properties.ElementAt(indexPath.Section);

			cell.DetailTextLabel.Text = property.Value;
			cell.TextLabel.Text = property.Key;

			return cell;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return properties.Count();
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}
	}
}
