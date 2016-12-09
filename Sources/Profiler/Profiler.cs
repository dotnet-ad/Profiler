namespace Debugging
{
	using System.Collections.Generic;
	using System;
	using System.Threading.Tasks;
	using System.Linq;

	public class Profiler
	{
		#region Instance

		private static Lazy<Profiler> defaultInstance = new Lazy<Profiler>(() => new Profiler());

		public static Profiler Default => defaultInstance.Value;

		#endregion

		#region Fields

		private List<Snapshot> snapshots = new List<Snapshot>();

		private List<IReference> references = new List<IReference>();

		#endregion

		#region Properties

		public TimeSpan TimeFrame { get; set; } = TimeSpan.FromSeconds(2);

		public bool IsProfiling { get; private set; }

		public IEnumerable<Snapshot> Snapshots => this.snapshots.ToArray();

		#endregion

		#region Methods

		public void Register<T>(T instance, params string[] properties) where T : class => this.Register<T>(instance, properties, null);

		public void Register<T>(T instance, IEnumerable<string> properties, string name = null) where T : class
		{
			if (instance != null)
			{
				this.references.Add(new Reference<T>(instance, properties, name));
			}
		}

		public void Register<T>(T instance, Dictionary<string, Func<T, string>> properties, string name = null) where T : class
		{
			if (instance != null)
			{
				this.references.Add(new Reference<T>(instance,properties,name));
			}
		}

		public async void StartProfiling()
		{
			if (!this.IsProfiling)
			{
				this.IsProfiling = true;
				while (this.IsProfiling)
				{
					this.TakeSnapshot();
					await Task.Delay(this.TimeFrame);
				}
			}
		}

		public void StopProfiling()
		{
			this.IsProfiling = false;
		}

		public void Clean()
		{
			GC.Collect();
			this.references.RemoveAll((r) => !r.IsAlive);
		}

		#endregion

		#region Private methods

		private void TakeSnapshot()
		{
			this.Clean();

			var snapshot = new Snapshot(this.references);
			var last = this.snapshots.LastOrDefault();

			if (last == null || !last.HasSameElements(snapshot))
			{
				this.snapshots.Add(new Snapshot(this.references));
			}
		}

		#endregion
	}
}
