using System.Collections.Generic;

namespace NAlgo.Graphs
{
	/// <summary>
	/// Undirected graph node.
	/// </summary>
	public class GraphNode<T> : Node<T>
	{
		internal List<T> Edges { get; private set; }

		/// <summary>
		/// Creates a new node.
		/// </summary>
		internal GraphNode(T id) : base(id)
		{
			Edges = new List<T>();
		}

		public override string ToString()
		{
			return "[" + Id + ": +(" + string.Join(",", Edges) + ")]";
		}
	}
}
