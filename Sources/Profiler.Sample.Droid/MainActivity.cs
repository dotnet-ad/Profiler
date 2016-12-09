using Android.App;
using Android.Widget;
using Android.OS;
using Debugging;

namespace Profiler.Sample.Droid
{
	[Activity(Label = "Profiler.Sample.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Debugging.Profiler.Default.StartProfiling();

			Debugging.Profiler.Default.Register(this);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
		}

		private ShakeListener shake;

		protected override void OnResume()
		{
			base.OnResume();

			shake = ShakeListener.Register(this);
			shake.Shaked += OnShaked;
		}

		protected override void OnPause()
		{
			base.OnPause();

			shake.Unregister();
		}

		private void OnShaked(object sender, System.EventArgs e)
		{
			shake.Shaked -= OnShaked;
			Debugging.Profiler.Default.Show(this);
		}
	}
}

