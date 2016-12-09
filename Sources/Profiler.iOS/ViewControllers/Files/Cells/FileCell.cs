using System;
using System.IO;
using Foundation;
using UIKit;

namespace Profiler.iOS
{
	public partial class FileCell : UITableViewCell
	{
		public static string Key => nameof(FileCell);
		public static nfloat Height => 60;

		public static readonly UINib Nib;

		static FileCell()
		{
			Nib = UINib.FromName("FileCell", NSBundle.MainBundle);
		}

		protected FileCell(IntPtr handle) : base(handle)
		{
		}

		public string Path
		{
			set
			{
				var attr = File.GetAttributes(value);
				var isDir = attr.HasFlag(FileAttributes.Directory);

				if (!isDir)
				{
					var info = new FileInfo(value);

					this.icon.Text = "📄";
					this.name.Text = System.IO.Path.GetFileName(value);
					this.time.Text = info.LastWriteTime.ToString();
					this.size.Text = info.Length + " bytes";
				}
				else
				{
					var info = new DirectoryInfo(value);

					this.icon.Text = "📁";
					this.name.Text = System.IO.Path.GetFileName(value);
					this.time.Text = info.LastWriteTime.ToString();
					this.size.Text = (info.GetFiles().Length + info.GetDirectories().Length) + " files";

				}
			}
		}

		
	}
}
