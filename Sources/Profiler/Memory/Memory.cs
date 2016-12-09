using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Debugging.Memory
{
	public class Memory : IMemory
	{
		#region Fields

		private List<IReference> references = new List<IReference>();

		#endregion

		#region Properties

		public TimeSpan TimeFrame { get; set; } = TimeSpan.FromSeconds(3);

		public IEnumerable<IReference> References => this.references.ToArray();

		public bool IsProfiling { get; private set; }

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
				this.references.Add(new Reference<T>(instance, properties, name));
			}
		}

		public async void Start()
		{
			if (!this.IsProfiling)
			{
				this.IsProfiling = true;
				while (this.IsProfiling)
				{
					this.Update();
					await Task.Delay(this.TimeFrame);
				}
			}
		}

		public void Stop()
		{
			this.IsProfiling = false;
		}

		#endregion

		#region Private methods

		private void Update()
		{
			GC.Collect(0); // Minor collections
			GC.Collect(); // Major collections

			foreach (var reference in this.references)
			{
				reference.Update();
			}
		}

		#endregion
	}
}
