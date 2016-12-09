namespace Debugging.Memory
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
			this.IsAlive = true;
			this.PropertyGetters = properties;
		}

		#region Fields

		private WeakReference<T> reference;

		private Dictionary<string, string> lastProperties;

		private Dictionary<string, Func<T, string>> PropertyGetters;

		#endregion

		#region Properties

		public IDictionary<string, string> Properties => this.lastProperties;

		public DateTime Creation { get; private set; }

		public DateTime? Destruction { get; set; }

		public Type Type { get; private set; }

		public string Key { get; private set; }

		public string Name { get; private set; } 

		public bool IsAlive { get; private set; }

		#endregion

		public void Update()
		{
			var wasAlive = this.IsAlive;

			if (wasAlive)
			{
				T instance;
				this.IsAlive = this.reference.TryGetTarget(out instance);

				if (this.IsAlive)
				{
					this.lastProperties = new Dictionary<string, string>();
					foreach (var prop in this.PropertyGetters)
					{
						this.lastProperties[prop.Key] = prop.Value(instance);
					}
				}
				else
				{
					this.Destruction = DateTime.Now;
				}
			}
		}
			
	}
}
