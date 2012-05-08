using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NAlgo.Graphs
{
	/// <summary>
	/// Directed graph node.
	/// </summary>
	public class DigraphNode<TId, TValue> : Node<TId, TValue>
	{
		internal List<TId> Outgoing { get; private set; }
		internal List<TId> Incoming { get; private set; }
		
		/// <summary>
		/// Creates a new node.
		/// </summary>
		internal DigraphNode(TId id, IDictionary graph) 
			: base(id, graph)
		{
			Outgoing = new List<TId>();
			Incoming = new List<TId>();
		}

		public override IEnumerable<Node<TId, TValue>> GetUnexploredChildren()
		{
			return Outgoing.Select(x => ((Dictionary<TId, DigraphNode<TId, TValue>>)Graph)[x]).Where(x => !x.IsExplored);
		}

		public override string ToString()
		{
			return "[" + Id + ": +(" + string.Join(",", Outgoing) + ") -(" + string.Join(",", Incoming) + ")]";
		}
	}
}