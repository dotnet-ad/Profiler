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
			this.padding = 10.0f.FromDip(context);

			this.Orientation = Orientation.Horizontal;
			this.SetPadding(padding, padding, padding, padding);
			this.LayoutParameters = new ListView.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

			// Icon
			this.Icon = new TextView(this.Context);
			this.Icon.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.MatchParent, 0.0f);
			this.Icon.Gravity = GravityFlags.Left;
			this.Icon.SetPadding(0, 0, padding, 0);
			this.AddView(this.Icon);

			// Right part
			var vertical = new LinearLayout(this.Context);
			vertical.Orientation = Orientation.Vertical;
			vertical.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent, 1.0f);

			this.Title = CreateTitle(13,true);
			vertical.AddView(this.Title);

			this.SubTitle1 = CreateTitle(10,false);
			vertical.AddView(this.SubTitle1);

			this.SubTitle2 = CreateTitle(10,false);
			vertical.AddView(this.SubTitle2);

			this.AddView(vertical);

		}

		readonly int padding;

		private TextView CreateTitle(float fontsize, bool bold)
		{
			var result = new TextView(this.Context)
			{
				TextSize = fontsize.FromDip(this.Context),

				TextAlignment = TextAlignment.TextStart,
				LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent, 0.0f),
				Gravity = GravityFlags.Left,
			};

			if(bold)
				result.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);

			return result;
		}

		public TextView Icon
		{
			get;
			private set;
		}

		public TextView Title
		{
			get;
			private set;
		}

		public TextView SubTitle1
		{
			get;
			private set;
		}

		public TextView SubTitle2
		{
			get;
			private set;
		}
	}
}
