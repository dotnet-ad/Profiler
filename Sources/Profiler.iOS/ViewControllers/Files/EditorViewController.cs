using System;
using System.IO;
using UIKit;

namespace Profiler.iOS
{
	public class EditorViewController: UIViewController
	{
		public EditorViewController(string path)
		{
			this.path = path;
			this.Title = Path.GetFileName(path);

			var thiis = this;
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("Save", UIBarButtonItemStyle.Done, (s, e) =>
			{
				File.WriteAllText(path, this.editor.Text);
				thiis.NavigationController.PopViewController(true);
				thiis = null;
			});
		}

		private string path;
		private UITextView editor;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.editor = new UITextView(this.View.Frame)
			{
				Editable = true,
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
				Text = File.ReadAllText(path),
			};

			this.View.AddSubview(this.editor);
		}

	}
}
