namespace Debugging
{
	using System;
	using System.Reflection;
	using System.Collections.Generic;

	public class Reference<T> : IReference where T : class
	{
		public Reference(T instance, IEnumerable<string> properties, string name = null) : this(instance,new Dictionary<string,Func<T,string>>(), name)
		{
			this.PropertyGetters = new Dictionary<string, Func<T, string>>();

			foreach (var pname in properties)
			{
				var prop = typeof(T).GetRuntimeProperty(pname);
				this.PropertyGetters.Add(pname,(o) => o == null ? "?" : prop.GetValue(o)?.ToString());
			}
		}

		public Reference(T instance, Dictionary<string, Func<T, string>> properties, string name = null)
		{
			this.Key = instance.GetHashCode().ToString();
			this.Name = name ?? this.Key;
			this.Type = instance.GetType();
			this.reference = new WeakReference<T>(instance);
			this.Creation = DateTime.Now;
			this.PropertyGetters = properties;
		}

		#region Fields

		private WeakReference<T> reference;

		private Dictionary<string, Func<T, string>> PropertyGetters;

		#endregion

		#region Properties

		public DateTime Creation { get; private set; }

		public Type Type { get; private set; }

		public string Key { get; private set; }

		public string Name { get; private set; } 

		public bool IsAlive
		{
			get
			{
				T instance;
				return this.reference.TryGetTarget(out instance);
			}
		}

		public IDictionary<string, string> Properties
		{
			get
			{
				T instance;
				if (this.reference.TryGetTarget(out instance))
				{
					var result = new Dictionary<string, string>();
					foreach (var prop in this.PropertyGetters)
					{
						result[prop.Key] = prop.Value(instance);
					}
					return result;
				}

				throw new InvalidOperationException("Instance must be alive");
			}
		}

		#endregion
	}
}
