using System.Collections.Generic;

namespace NAlgo.Graphs
{
	/// <summary>
	/// Directed graph node.
	/// </summary>
	public class DigraphNode<T> : Node<T>
	{
		internal List<T> Outgoing { get; private set; }
		internal List<T> Incoming { get; private set; }
		
		/// <summary>
		/// Creates a new node.
		/// </summary>
		internal DigraphNode(T id) : base(id)
		{
			Outgoing = new List<T>();
			Incoming = new List<T>();
		}

		public override string ToString()
		{
			return "[" + Id + ": +(" + string.Join(",", Outgoing) + ") -(" + string.Join(",", Incoming) + ")]";
		}
	}
}