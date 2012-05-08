using System.Collections.Generic;
using System.Linq;

namespace NAlgo.Graphs
{
	/// <summary>
	/// Undirected graph node.
	/// </summary>
	public class GraphNode<TId, TValue> : Node<TId, TValue>
	{
		internal List<TId> Adjacent { get; private set; }

		/// <summary>
		/// Creates a new node.
		/// </summary>
		internal GraphNode(TId id, Dictionary<TId, GraphNode<TId, TValue>> graph) 
			: base(id, graph)
		{
			Adjacent = new List<TId>();
		}

		public override IEnumerable<Node<TId, TValue>> GetUnexploredChildren()
		{
			return Adjacent.Select(x => ((Dictionary<TId, GraphNode<TId, TValue>>)Graph)[x]).Where(x => !x.IsExplored);
		}

		public override string ToString()
		{
			return "[" + Id + ": +(" + string.Join(",", Adjacent) + ")]";
		}
	}
}
