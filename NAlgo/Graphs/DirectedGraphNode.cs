using System.Collections.Generic;

namespace NAlgo.Graphs
{
	public class DirectedGraphNode<T> : Node<T>
	{
		internal List<T> Outgoing { get; private set; }
		internal List<T> Incoming { get; private set; }
		
		internal DirectedGraphNode(T id) : base(id)
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