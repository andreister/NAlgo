using System;
using System.Collections.Generic;
using System.Linq;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Implements Kosaraju's two-pass algorithm to compute the
	/// strongly connected components in a directed graph.
	/// 
	/// * the first pass is done on a reversed graph, with the goal to
	///   calculate the "magic" traversal ordering for the second pass
	/// * the second pass is done on a "normal" (unreversed) graph,
	///   iterating over the nodes accordingly to the above ordering.
	/// </summary>
	internal class Kosaraju<T>
	{
		private readonly Dictionary<T, KosarajuNode> _nodes;
		private int _currentTraversalOrder;
		private KosarajuNode _currentLeader;

		/// <summary>
		/// Creates a new algorithm instance.
		/// </summary>
		internal Kosaraju(Dictionary<T, DirectedGraphNode<T>> graph)
		{
			_nodes = graph.ToDictionary(x => x.Key, x => new KosarajuNode(x.Value));
		}

		/// <summary>
		/// Computes the strongly connected components, and returns a map 
		/// where the keys are the leader nodes, and the values are the nodes 
		/// in the corresponding connected component.
		/// </summary>
		internal Dictionary<DirectedGraphNode<T>, List<DirectedGraphNode<T>>> GetConnectedComponents()
		{
			_currentTraversalOrder = _nodes.Count;
			
			TraverseGraph(Direction.Reverse);
			TraverseGraph(Direction.Forward);

			return GroupNodesByLeader();
		}

		/// <summary>
		/// Traverses the graph in the given direction, running DFS on each node.
		/// </summary>
		private void TraverseGraph(Direction direction)
		{
			foreach (var node in _nodes.Values) {
				node.IsExplored = false;
			}

			var nodes = _nodes.Values.OrderBy(x => x.TraversalOrder).Where(x => !x.IsExplored);
			foreach (var node in nodes) {
				_currentLeader = node;
				DepthFirstSearch(node, direction);
			}
		}

		private void DepthFirstSearch(KosarajuNode root, Direction direction)
		{
			root.IsExplored = true;

			var search = new DepthFirstSearch<T>();
			search.Run(root, (stack, x) => {
				var node = (KosarajuNode)x; 
				var unexploredChild = node.GetUnexploredChildren(_nodes, direction).FirstOrDefault();
				if (unexploredChild != null) {
					unexploredChild.IsExplored = true;
					stack.Push(node);				//push the node because there may be more unexplored children
					stack.Push(unexploredChild);	//push the unexplored child to DFS it on the next step
					return;
				}
				node.Leader = _currentLeader;
				node.TraversalOrder = _currentTraversalOrder--;
			});
		}

		private Dictionary<DirectedGraphNode<T>, List<DirectedGraphNode<T>>> GroupNodesByLeader()
		{
			var result = new Dictionary<DirectedGraphNode<T>, List<DirectedGraphNode<T>>>();
			foreach (var node in _nodes.Values) {
				var leader = node.Leader;
				if (!result.ContainsKey(leader)) {
					result.Add(leader, new List<DirectedGraphNode<T>>());
				}
				result[leader].Add(node);
			}
			return result;
		}

		private enum Direction
		{
			Reverse = 0,
			Forward = 1
		}

		private class KosarajuNode : DirectedGraphNode<T>
		{
			internal bool IsExplored { get; set; }
			internal int TraversalOrder { get; set; }
			internal KosarajuNode Leader { get; set; }

			internal KosarajuNode(DirectedGraphNode<T> node)
				: base(node.Id)
			{
				TraversalOrder = ConvertToInt(node.Id);
				IsExplored = false;
				Leader = null;
				Incoming.AddRange(node.Incoming);
				Outgoing.AddRange(node.Outgoing);
			}

			internal IEnumerable<KosarajuNode> GetUnexploredChildren(Dictionary<T, KosarajuNode> nodes, Direction direction)
			{
				return (direction == Direction.Reverse ? Incoming : Outgoing).Select(x => nodes[x]).Where(x => !x.IsExplored);
			}

			private static int ConvertToInt(T id)
			{
				if (typeof(T) == typeof(int)) {
					return Convert.ToInt32(id);
				}
				return id.GetHashCode();
			}
		}
	}
}
