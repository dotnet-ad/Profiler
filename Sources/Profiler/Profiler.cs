namespace Debugging
{
	using System.Collections.Generic;
	using System;
	using System.Threading.Tasks;
	using System.Linq;
	using Debugging.Memory;

	public class Profiler
	{
		#region Instance

		private static Lazy<Profiler> defaultInstance = new Lazy<Profiler>(() => new Profiler());

		public static Profiler Default => defaultInstance.Value;

		#endregion

		public Profiler()
		{
			this.Memory = new Memory.Memory();
		}

		public IMemory Memory { get; private set; }

		public void Start()
		{
			this.Memory.Start();
		}

		public void Stop()
		{
			this.Memory.Stop();
		}
	}
}
