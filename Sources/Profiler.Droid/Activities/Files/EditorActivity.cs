namespace Profiler.Droid
{
	using System.Threading.Tasks;
	using Android.App;
	using Android.OS;
	using Android.Text.Method;
	using Android.Views;
	using Android.Widget;

	[Activity(Label = "Editor")]
	public class EditorActivity : Activity
	{
		private TextView Edit;
		private string path;

		public const string ExtraPathKey = nameof(ExtraPathKey);

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			this.path = Intent.GetStringExtra(ExtraPathKey);
			this.Edit = new TextView(this)
			{
				VerticalScrollBarEnabled = true,
				MovementMethod = (new ScrollingMovementMethod()),
			};

			this.Edit.SetMaxLines(99999);
			this.Edit.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
			this.SetContentView(this.Edit);

			var path = this.path;
			var edit = this.Edit;

			Task.Run(() =>
			{
				var text = System.IO.File.ReadAllText(path);
				var edit2 = edit;
				edit = null;
				this.RunOnUiThread(() =>
				{
					edit2.Text = text;
					edit2 = null;
				});
			});
		}
	}
}
