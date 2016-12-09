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
	public class ReferenceActivity : ListActivity
	{
		public class Adapter : BaseAdapter<KeyValuePair<string, string>>
		{
			public Adapter(string referenceKey)
			{
				var reference = Profiler.Default.Memory.References.First(s => s.Key == referenceKey);
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
				cell.SubTitle1.Text = property.Value;

				return cell;
			}
		}

		public const string ExtraReferenceKey = nameof(ExtraReferenceKey);

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			var referenceKey = Intent.GetStringExtra(ExtraReferenceKey);

			if (referenceKey != null)
				ListAdapter = new Adapter(referenceKey);
		}
	}
}
