namespace Debugging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Android.App;
	using Android.OS;
	using Android.Views;
	using Android.Widget;
	using Views;

	[Activity(Label = "Snapshots")]
	public class ReferenceActivity : ListActivity
	{
		public class Adapter : BaseAdapter<KeyValuePair<string, string>>
		{
			public Adapter(long snapshotTicks, string referenceKey)
			{
				var snapshot = Profiler.Default.Snapshots.First(s => s.Time.Ticks == snapshotTicks);
				var reference = snapshot.References.First(r => r.Key == referenceKey);
				this.properties = reference.Properties.OrderBy((arg) => arg.Key);
			}

			readonly IEnumerable<KeyValuePair<string, string>> properties;

			public override KeyValuePair<string, string> this[int position] => this.properties.ElementAt(position);

			public override int Count => this.properties.Count();

			public override long GetItemId(int position) => this[position].GetHashCode();

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				var cell = (convertView as ValueCell) ?? new ValueCell(parent.Context);

				var property = this[position];

				cell.Title.Text = property.Key;
				cell.Value.Text = property.Value;

				return cell;
			}
		}

		public const string ExtraSnaphotTicks = nameof(ExtraSnaphotTicks);
		public const string ExtraReferenceKey = nameof(ExtraReferenceKey);

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			var ticks = Intent.GetLongExtra(ExtraSnaphotTicks, -1);
			var referenceKey = Intent.GetStringExtra(ExtraReferenceKey);

			if (ticks >= 0 && referenceKey != null)
				ListAdapter = new Adapter(ticks,referenceKey);
		}
	}
}
