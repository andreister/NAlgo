using System.Collections;
using System.Collections.Generic;

namespace NAlgo.Graphs
{
	/// <summary>
	/// Generic node in a graph.
	/// </summary>
	/// <typeparam name="TId">Type of the node identifier (int, string, etc)</typeparam>
	/// <typeparam name="TValue">Type of the value, associated with the node.</typeparam>
	public abstract class Node<TId, TValue>
	{
		protected IDictionary Graph;

		/// <summary>
		/// Unique identifier of the node (number, name, etc).
		/// </summary>
		public TId Id { get; set; }

		/// <summary>
		/// Value associated with the node.
		/// </summary>
		public TValue Value { get; set; }

		/// <summary>
		/// Flag used by traversal algorithms to set or determine whether the node had been already visited.
		/// </summary>
		internal bool IsExplored { get; set; }

		/// <summary>
		/// Creates a new node.
		/// </summary>
		/// <param name="id">Unique identifier of the node (number, name, etc).</param>
		/// <param name="graph">Graph containing the node. Cannot pass here the generic
		/// type, but it should be Dictionary{TId, SomeNode{TId, TValue}}</param>
		protected Node(TId id, IDictionary graph)
		{
			Id = id;
			IsExplored = false;
			Graph = graph;
		}

		/// <summary>
		/// Returns adjacent or child nodes which haven't been explored yet.
		/// </summary>
		public abstract IEnumerable<Node<TId, TValue>> GetUnexploredChildren();
	}
}
