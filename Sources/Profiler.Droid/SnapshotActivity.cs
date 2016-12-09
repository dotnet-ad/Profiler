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

	[Activity(Label = "Snapshots")]
	public class SnapshotActivity : ListActivity
	{
		public class Adapter : BaseAdapter<Snapshot.ReferenceSnapshot>
		{
			public Adapter(long snapshotTicks)
			{
				var snapshot = Profiler.Default.Snapshots.First(s => s.Time.Ticks == snapshotTicks);
				this.references = snapshot.References.OrderBy((arg) => arg.Type);
			}

			readonly IEnumerable<Snapshot.ReferenceSnapshot> references;

			public override Snapshot.ReferenceSnapshot this[int position] => this.references.ElementAt(position);

			public override int Count => this.references.Count();

			public override long GetItemId(int position) => this[position].GetHashCode();

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				var cell = (convertView as ValueCell) ?? new ValueCell(parent.Context);

				var reference = references.ElementAt(position);

				cell.Title.Text = reference.Name;
				cell.Value.Text = $"{reference.Type}";

				return cell;
			}
		}

		public const string ExtraSnaphotTicks = nameof(ExtraSnaphotTicks);

		private long ticks;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			ticks = Intent.GetLongExtra(ExtraSnaphotTicks, -1);

			if(ticks >= 0)
				ListAdapter = new Adapter(ticks);
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			var key = (this.ListAdapter as Adapter)[position].Key;

			var intent = new Intent(this, typeof(ReferenceActivity));
			intent.PutExtra(ReferenceActivity.ExtraSnaphotTicks, ticks);
			intent.PutExtra(ReferenceActivity.ExtraReferenceKey, key);
			this.StartActivity(intent);
		}
	}
}
