namespace Debugging.Views
{
	using Android.Content;
	using Android.Views;
	using Android.Widget;
	using Extensions;

	public class ValueCell : LinearLayout
	{
		public ValueCell(Context context) : base(context)
		{
			var padding = 10.0f.FromDip(context);

			this.Orientation = Orientation.Horizontal;
			this.SetPadding(padding, padding, padding, padding);
			this.LayoutParameters = new ListView.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

			this.Title = new TextView(this.Context);
			this.Title.TextAlignment = TextAlignment.TextStart;
			this.Title.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.MatchParent, 0.0f);
			this.Title.Gravity = GravityFlags.Left;
			this.Title.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
			this.Title.SetPadding(0, 0, padding, 0);
			this.AddView(this.Title);

			this.Value = new TextView(this.Context);
			this.Value.TextAlignment = TextAlignment.Gravity;
			this.Value.Gravity = GravityFlags.Right;
			this.Value.SetWidth(0);
			this.Value.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1.0f);
			this.AddView(this.Value);
		}

		public TextView Title
		{
			get;
			private set;
		}

		public TextView Value
		{
			get;
			private set;
		}
	}
}
