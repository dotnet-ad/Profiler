namespace Debugging
{
	using System;
	using UIKit;

	public class ProfilerViewController :  UITableViewController
	{
		public ProfilerViewController() 
		{
			this.Title = "Profiler";

			var thiis = this;
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("Close", UIBarButtonItemStyle.Done, (s, e) =>
			{
				thiis.DismissModalViewController(true);
				thiis = null;
			});
		}

		private Tuple<string, string, Type>[] menu =
		{
			new Tuple<string,string, Type>("Memory", "🌡", typeof(MemoryViewController)),
			new Tuple<string,string, Type>("Files", "📂", typeof(FolderViewController)),
		};

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.TableView.RegisterClassForCellReuse(typeof(UITableViewCell), nameof(UITableViewCell));
		}

		public override nint NumberOfSections(UITableView tableView) => 1;

		public override nint RowsInSection(UITableView tableView, nint section) => menu.Length;

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var item = menu[indexPath.Row];
			var cell = tableView.DequeueReusableCell(nameof(UITableViewCell));
			cell.TextLabel.Text = $"{item.Item2} {item.Item1}";
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
	
			return cell;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var vc = Activator.CreateInstance(menu[indexPath.Row].Item3) as UIViewController;	
			this.NavigationController.PushViewController(vc, true);
		}
	}
}
