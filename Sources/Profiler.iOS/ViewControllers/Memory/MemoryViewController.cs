namespace Debugging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CoreGraphics;
	using Debugging.Memory;
	using Foundation;
	using global::Profiler.iOS;
	using UIKit;

	public class MemoryViewController : UITableViewController
	{
		public MemoryViewController()
		{
			this.Title = "Memory";

			this.references = Profiler.Default.Memory.References.OrderByDescending((arg) => arg.Creation).ToArray();
			this.SortReferences();
		}



		private UISegmentedControl segments;

		private UIView CreateHeader()
		{
			var header = new UIView(new CGRect(0, 0, 100, 50));

			segments = new UISegmentedControl();
			segments.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			segments.Frame = new CGRect(10, 10, 80, 30);
			segments.InsertSegment("Created", 0, false);
			segments.InsertSegment("Destroyed", 1, false);
			segments.InsertSegment("Type", 2, false);
			segments.InsertSegment("Name", 3, false);
			segments.SelectedSegment = 0;

			header.AddSubview(segments);

			return header;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			TableView.TableHeaderView = CreateHeader();
			TableView.RegisterNibForCellReuse(ReferenceCell.Nib, ReferenceCell.Key);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			segments.ValueChanged += OnSegmentsValueChanged;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			segments.ValueChanged -= OnSegmentsValueChanged;
		}

		private void SortReferences()
		{
			switch (this.segments?.SelectedSegment ?? 0)
			{
				case 3:
					this.sortedReferences = Profiler.Default.Memory.References.OrderBy((arg) => arg.Name).ThenByDescending(a => a.Creation).ToArray();
					break;
				case 2:
					this.sortedReferences = Profiler.Default.Memory.References.OrderBy((arg) => arg.Type).ThenByDescending(a => a.Creation).ToArray();
					break;
				case 1:
					this.sortedReferences = Profiler.Default.Memory.References.OrderByDescending((arg) => arg.Destruction).ToArray();
					break;
				default:
					this.sortedReferences = references;
					break;
			}
		}

		private void OnSegmentsValueChanged(object sender, EventArgs e)
		{
			this.SortReferences();
			this.TableView.ReloadData();
		}

		readonly IEnumerable<IReference> references;
		private IEnumerable<IReference> sortedReferences;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(ReferenceCell.Key) as ReferenceCell;
			cell.Reference = sortedReferences.ElementAt(indexPath.Row);
			return cell;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => ReferenceCell.Height;

		public override nint NumberOfSections(UITableView tableView) => 1;

		public override nint RowsInSection(UITableView tableView, nint section) => this.sortedReferences.Count();

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var reference = this.sortedReferences.ElementAt(indexPath.Row);
			this.NavigationController.PushViewController(new ReferenceViewController(reference), true);
		}
	}
}
