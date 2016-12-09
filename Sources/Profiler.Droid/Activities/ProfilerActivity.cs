namespace Debugging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Android.App;
	using Android.Content;
	using Android.OS;
	using Android.Views;
	using Android.Widget;
	using Views;

	[Activity(Label = "Profiler")]
	public class ProfilerActivity : ListActivity
	{
		public class Adapter : BaseAdapter<Tuple<string, string, Type>>
		{
			public Adapter()
			{
			}

			public static readonly Tuple<string, string, Type>[] Menu =
			{
				new Tuple<string,string, Type>("Memory", "🚨", typeof(MemoryActivity)),
				new Tuple<string,string, Type>("Files", "📂", typeof(FolderActivity)),
			};

			public override Tuple<string, string, Type> this[int position] => Menu.ElementAt(position);

			public override int Count => Menu.Count();

			public override long GetItemId(int position) => this[position].GetHashCode();

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				var cell = (convertView as ValueCell) ?? new ValueCell(parent.Context);

				var item = this[position];

				cell.Title.Text = $"{item.Item2} {item.Item1}";

				return cell;
			}
		}

		public const string ExtraReferenceKey = nameof(ExtraReferenceKey);

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
            ListAdapter = new Adapter();
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			var reference = (this.ListAdapter as Adapter)[position];

			var intent = new Intent(this, reference.Item3);
			this.StartActivity(intent);
		}
	}
}
