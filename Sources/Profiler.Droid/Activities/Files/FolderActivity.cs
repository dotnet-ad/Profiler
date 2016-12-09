namespace Debugging
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Android.App;
	using Android.Content;
	using Android.Hardware;
	using Android.OS;
	using Android.Runtime;
	using Android.Views;
	using Android.Widget;
	using Debugging.Memory;
	using Debugging.Views;
	using global::Profiler.Droid;

	[Activity(Label = "Files")]
	public class FolderActivity : ListActivity
	{
		public class Adapter : BaseAdapter<string>
		{
			public Adapter(string path)
			{
				this.Files = Directory.EnumerateFileSystemEntries(path).OrderBy(p => p).ToArray();
			}

			public IEnumerable<string> Files { get; private set; }

			public override string this[int position] => this.Files.ElementAt(position);

			public override int Count => this.Files.Count();

			public override long GetItemId(int position) => this[position].GetHashCode();

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				var cell = (convertView as ValueCell) ?? new ValueCell(parent.Context);

				var value = this[position];

				var attr = File.GetAttributes(value);
				var isDir = attr.HasFlag(FileAttributes.Directory);

				if (!isDir)
				{
					var info = new FileInfo(value);

					cell.Icon.Text = "📄";
					cell.Title.Text = System.IO.Path.GetFileName(value);
					cell.SubTitle1.Text = info.LastWriteTime.ToString();
					cell.SubTitle2.Text = info.Length + " bytes";
				}
				else
				{
					var info = new DirectoryInfo(value);

					cell.Icon.Text = "📁";
					cell.Title.Text = System.IO.Path.GetFileName(value);
					cell.SubTitle2.Text = info.LastWriteTime.ToString();
					cell.SubTitle2.Text = (info.GetFiles().Length + info.GetDirectories().Length) + " files";

				}

				return cell;
			}
		}

		public const string ExtraPathKey = nameof(ExtraPathKey);

		private readonly string[] PreviewExtensions = { ".png", ".jpg", ".gif" };

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			ListAdapter = new Adapter(Intent.GetStringExtra(ExtraPathKey) ?? System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			var path = (this.ListAdapter as Adapter)[position];
			var attr = File.GetAttributes(path);
			var isDir = attr.HasFlag(FileAttributes.Directory);
			var ext = Path.GetExtension(path);

			Intent intent;

			if (isDir)
			{
				intent = new Intent(this, typeof(FolderActivity));
			}
			else if (PreviewExtensions.Contains(ext))
			{
				intent = new Intent(this, typeof(EditorActivity)); // TODO PreviewActivity
			}
			else
			{
				intent = new Intent(this, typeof(EditorActivity));
			}

			intent.PutExtra(ExtraPathKey, path);
			this.StartActivity(intent);
		}
	}
}
