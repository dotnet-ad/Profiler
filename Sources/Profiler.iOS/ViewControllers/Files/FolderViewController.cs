namespace Debugging
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Foundation;
	using global::Profiler.iOS;
	using UIKit;

	public class FolderViewController : UITableViewController
	{
		public FolderViewController(): this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".."))
		{
			this.Title = "Files";
		}

		public FolderViewController(string path)
		{
			this.Title = Path.GetFileName(path);
			this.files = Directory.EnumerateFileSystemEntries(path).OrderBy(p => p).ToArray();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			TableView.RegisterNibForCellReuse(FileCell.Nib, FileCell.Key);
		}

		readonly IEnumerable<string> files;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(FileCell.Key) as FileCell;
			cell.Path = this.files.ElementAt(indexPath.Row);
			return cell;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => ReferenceCell.Height;

		public override nint NumberOfSections(UITableView tableView) => 1;

		public override nint RowsInSection(UITableView tableView, nint section) => this.files.Count();

		private readonly string[] PreviewExtensions = { ".png", ".jpg", ".gif" };

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var path = this.files.ElementAt(indexPath.Row);
			var attr = File.GetAttributes(path);
			var isDir = attr.HasFlag(FileAttributes.Directory);
			var ext = Path.GetExtension(path);

			if (isDir)
			{
				this.NavigationController.PushViewController(new FolderViewController(path), true);
			}
			else if(PreviewExtensions.Contains(ext))
			{
				this.NavigationController.PushViewController(new PreviewViewController(path), true);
			}
			else
			{
				this.NavigationController.PushViewController(new EditorViewController(path), true);
			}
		}
	}
}
