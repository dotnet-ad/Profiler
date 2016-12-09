namespace Debugging
{
	using System;
	using Android.App;
	using Android.Hardware;
	using Android.Runtime;

	public class ShakeListener : Java.Lang.Object, ISensorEventListener
	{
		private DateTime lastUpdate;
		private double gravity = SensorManager.GravityEarth;
		private double acceleration = 0;

		private static readonly TimeSpan ShakeDetectionTimeLapse = TimeSpan.FromMilliseconds(250);
		public double ShakeThreshold { get; set; } = 2f;
		private const double LowPassFilter = 0.8f;

		public event EventHandler Shaked;

		private Activity activity;

		private ShakeListener(Activity activity)
		{
			this.activity = activity;
		}

		public static ShakeListener Register(Activity activity)
		{
			var manager = activity.GetSystemService(Activity.SensorService) as SensorManager;
			var sensor = manager.GetDefaultSensor(SensorType.Accelerometer);
			var gesture = new ShakeListener(activity);
			manager.RegisterListener(gesture, sensor, SensorDelay.Ui);
			return gesture;
		}

		public void Unregister()
		{
			var manager = activity.GetSystemService(Activity.SensorService) as SensorManager;
			manager.UnregisterListener(this);
			Shaked = null;
		}

		public void OnSensorChanged(Android.Hardware.SensorEvent e)
		{
			if (e.Sensor.Type == SensorType.Accelerometer)
			{
				float x = e.Values[0];
				float y = e.Values[1];
				float z = e.Values[2];

				var previous = this.gravity;
				this.gravity = Math.Sqrt(x * x + y * y + z * z);
				var delta = gravity - previous;

				this.acceleration = this.acceleration * LowPassFilter + delta;

				if ((acceleration > ShakeThreshold) && (lastUpdate + ShakeDetectionTimeLapse < DateTime.Now))
				{
					lastUpdate = DateTime.Now;
					this.Shaked?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy) {}
	}
}
