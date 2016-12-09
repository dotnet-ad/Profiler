namespace Debugging
{
	using System;
	using System.Collections.Generic;
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

	[Activity(Label = "Snapshots")]
	public class MemoryActivity : ListActivity
	{
		public class Adapter : BaseAdapter<IReference>
		{
			public Adapter()
			{
				this.References = Profiler.Default.Memory.References.OrderByDescending((arg) => arg.Creation).ToArray();
			}

			public IEnumerable<IReference> References { get; private set; }

			public override IReference this[int position] => this.References.ElementAt(position);

			public override int Count => this.References.Count();

			public override long GetItemId(int position) => this[position].GetHashCode();

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				var cell = (convertView as ValueCell) ?? new ValueCell(parent.Context);

				var reference = this[position];

				var time = $"+{reference.Creation.FromNowLabel()}";
				if (!reference.IsAlive) time += $" -{reference.Destruction.Value.FromNowLabel()}";

				cell.Icon.Text = reference.IsAlive ? "🔷" : "🔶";
				cell.Title.Text = reference.Name;
				cell.SubTitle1.Text = reference.Type.ToString();
				cell.SubTitle2.Text = time;

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
			var reference = (this.ListAdapter as Adapter)[position];

			var intent = new Intent(this, typeof(ReferenceActivity));
			intent.PutExtra(ReferenceActivity.ExtraReferenceKey, reference.Key);
			this.StartActivity(intent);
		}
	}
}
