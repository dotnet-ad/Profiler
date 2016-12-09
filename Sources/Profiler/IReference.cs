namespace Debugging
{
	using System;
	using System.Collections.Generic;

	public interface IReference
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Debugging.Instance"/> is alive.
		/// </summary>
		/// <value><c>true</c> if is alive; otherwise, <c>false</c>.</value>
		bool IsAlive { get; }

		/// <summary>
		/// Gets the name of the instance.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		/// <summary>
		/// Gets the creation date and time.
		/// </summary>
		/// <value>The creation.</value>
		DateTime Creation { get;}

		/// <summary>
		/// Gets the type of the referenced instance.
		/// </summary>
		/// <value>The type.</value>
		Type Type { get; }

		/// <summary>
		/// Gets the properties.
		/// </summary>
		/// <value>The properties.</value>
		IDictionary<string, string> Properties { get; }
	}
}
