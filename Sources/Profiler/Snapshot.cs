namespace Debugging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class Snapshot
	{
		public class ReferenceSnapshot
		{
			public ReferenceSnapshot(IReference reference)
			{
				this.Key = reference.Key;
				this.Name = reference.Name;
				this.Type = reference.Type;
				this.Creation = reference.Creation;
				this.Properties = new Dictionary<string, string>();

				foreach (var item in reference.Properties)
				{
					this.Properties[item.Key] = item.Value;
				}
			}

			public string Key { get; private set; }

			public string Name { get; private set; }

			public Type Type { get; private set; }

			public DateTime Creation { get; private set; }

			public Dictionary<string, string> Properties { get; private set; }
		}

		public Snapshot(IEnumerable<IReference> references)
		{
			this.Time = DateTime.Now;
			this.Add(references);
		}

		private List<ReferenceSnapshot> references = new List<ReferenceSnapshot>();

		public DateTime Time { get; set; }

		public IEnumerable<ReferenceSnapshot> References => this.references.ToArray();

		public IEnumerable<ReferenceSnapshot> ReferencesOfType<T>()
		{
			var info = typeof(T).GetTypeInfo();
			return this.references.Where(t => info.IsAssignableFrom(t.GetType().GetTypeInfo())).ToArray();
		}

		public void Add(IReference instance) => this.references.Add(new ReferenceSnapshot(instance));

		public void Add(IEnumerable<IReference> instances)
		{
			foreach (var instance in instances)
			{
				this.Add(instance);
			}
		}

		public bool HasSameElements(Snapshot other)
		{
			var names = this.References.Select(r => r.Key).OrderBy(r => r);
			var othernames = other.References.Select(r => r.Key).OrderBy(r => r);
			return names.SequenceEqual(othernames);

		}
	}
}
