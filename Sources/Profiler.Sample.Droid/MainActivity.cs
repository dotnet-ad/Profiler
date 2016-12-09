using Android.App;
using Android.Widget;
using Android.OS;
using Debugging;
using Android.Content;
using System.IO;

namespace Profiler.Sample.Droid
{
	[Activity(Label = "Profiler.Sample.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		public static bool written;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			if (!written)
			{
				var folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
				System.IO.File.WriteAllText(Path.Combine(folder, "test.txt"), "This is an example file");
				written = true;
			}

			this.leak = Intent.GetBooleanExtra(nameof(leak), false);

			Debugging.Profiler.Default.Start();

			Debugging.Profiler.Default.Memory.Register(this);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			this.button = FindViewById<Button>(Resource.Id.myButton);
			this.button2 = FindViewById<Button>(Resource.Id.myButton2);
		}

		void OnClick(object sender, System.EventArgs e)
		{
			var intent = new Intent(this, typeof(MainActivity));
			intent.PutExtra(nameof(leak), false);
			this.StartActivity(intent);
		}

		void OnLeakClick(object sender, System.EventArgs e)
		{
			var intent = new Intent(this, typeof(MainActivity));
			intent.PutExtra(nameof(leak), true);
			this.StartActivity(intent);
		}

		private ShakeListener shake;

		Button button, button2;

		private bool leak;

		protected override void OnResume()
		{
			base.OnResume();

			button.Click += OnClick;
			button2.Click += OnLeakClick;

			shake = ShakeListener.Register(this);
			shake.Shaked += OnShaked;
		}

		protected override void OnPause()
		{
			base.OnPause();

			if (!leak)
			{
				button.Click -= OnClick;
				button2.Click -= OnLeakClick;
			}

			shake.Unregister();
		}

		private void OnShaked(object sender, System.EventArgs e)
		{
			Debugging.Profiler.Default.Show(this);
		}
	}
}

