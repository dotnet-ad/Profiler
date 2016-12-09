namespace Debugging
{
	using System;
	using System.Linq;
	using Android.App;
	using Android.Content;
	using Android.Hardware;
	using Android.OS;
	using Android.Runtime;
	using Android.Views;
	using Android.Widget;
	using Debugging.Views;

	[Activity(Label = "Snapshots")]
	public class SnapshotsActivity : ListActivity
	{
		public class Adapter : BaseAdapter<Snapshot>
		{
			public Adapter()
			{
				this.Snapshots = Profiler.Default.Snapshots.OrderByDescending(s => s.Time);
			}

			public IOrderedEnumerable<Snapshot> Snapshots { get; private set; }

			public override Snapshot this[int position] => this.Snapshots.ElementAt(position);

			public override int Count => this.Snapshots.Count();

			public override long GetItemId(int position) => this[position].GetHashCode();

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				var cell = (convertView as ValueCell) ?? new ValueCell(parent.Context);

				var snapshot = Snapshots.ElementAt(position);

				cell.Title.Text = snapshot.Time.ToString("T");
				cell.Value.Text = $"{snapshot.References.Count().ToString()} instances";

				return cell;
			}
		}
	

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			ListAdapter = new Adapter();
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			var snapshot = (this.ListAdapter as Adapter).Snapshots.ElementAt(position);

			var intent = new Intent(this, typeof(SnapshotActivity));
			intent.PutExtra(SnapshotActivity.ExtraSnaphotTicks, snapshot.Time.Ticks);
			this.StartActivity(intent);
		}
	}
}
