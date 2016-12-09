namespace Debugging.Memory
{
	using System;
	using System.Collections.Generic;
	using Debugging.Memory;

	public interface IMemory
	{
		IEnumerable<IReference> References { get; }

		void Register<T>(T instance, params string[] properties) where T : class;

		void Register<T>(T instance, IEnumerable<string> properties, string name = null) where T : class;

		void Register<T>(T instance, Dictionary<string, Func<T, string>> properties, string name = null) where T : class;

		void Start();

		void Stop();
	}
}
