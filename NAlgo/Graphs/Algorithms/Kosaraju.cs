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
	/// * the second pass is done against an unreversed graph, iterating 
	///   over the nodes accordingly to the previously found ordering.
	/// </summary>
	/// <typeparam name="TId">Type of node identifier.</typeparam>
	public class Kosaraju<TId>
	{
		private readonly Dictionary<TId, KosarajuNode> _graph;
		private int _currentTraversalOrder;
		private KosarajuNode _currentLeader;

		/// <summary>
		/// Creates a new algorithm instance.
		/// </summary>
		public Kosaraju(Dictionary<TId, DigraphNode<TId, object>> graph)
		{
			_graph = new Dictionary<TId, KosarajuNode>();
			foreach (var pair in graph) {
				_graph.Add(pair.Key, new KosarajuNode(pair.Value, _graph));
			}
		}

		/// <summary>
		/// Computes the strongly connected components, and returns a map
		/// where the keys are the leader nodes, and the values are the nodes
		/// in the corresponding connected component.
		/// </summary>
		public Dictionary<DigraphNode<TId, object>, List<DigraphNode<TId, object>>> GetConnectedComponents()
		{
			_currentTraversalOrder = _graph.Count;
			
			TraverseGraph(Direction.Reverse);
			TraverseGraph(Direction.Forward);

			return GroupNodesByLeader();
		}

		/// <summary>
		/// Traverses the graph in the given direction, running DFS on each node.
		/// </summary>
		private void TraverseGraph(Direction direction)
		{
			foreach (var node in _graph.Values) {
				node.IsExplored = false;
				node.TraversalDirection = direction;
			}

			var dfs = new DepthFirstSearch<TId, object>();
			foreach (var node in _graph.Values.OrderBy(x => x.TraversalOrder).Where(x => !x.IsExplored)) {
				_currentLeader = node;
				dfs.Run(node, x => {
					((KosarajuNode)x).Leader = _currentLeader;
					((KosarajuNode)x).TraversalOrder = _currentTraversalOrder--;
				});
			}
		}

		private Dictionary<DigraphNode<TId, object>, List<DigraphNode<TId, object>>> GroupNodesByLeader()
		{
			var result = new Dictionary<DigraphNode<TId, object>, List<DigraphNode<TId, object>>>();
			foreach (var node in _graph.Values) {
				var leader = node.Leader;
				if (!result.ContainsKey(leader)) {
					result.Add(leader, new List<DigraphNode<TId, object>>());
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

		private class KosarajuNode : DigraphNode<TId, object>
		{
			internal int TraversalOrder { get; set; }
			internal Direction TraversalDirection { get; set; }
			internal KosarajuNode Leader { get; set; }

			internal KosarajuNode(DigraphNode<TId, object> node, Dictionary<TId, KosarajuNode> graph)
				: base(node.Id, graph)
			{
				Leader = null;
				TraversalOrder = GetInitialTraversalOrder(node.Id);
				Incoming.AddRange(node.Incoming);
				Outgoing.AddRange(node.Outgoing);
			}

			public override IEnumerable<Node<TId, object>> GetUnexploredChildren()
			{
				var source = (TraversalDirection == Direction.Reverse) ? Incoming : Outgoing;
				return source.Select(x => ((Dictionary<TId, KosarajuNode>)Graph)[x]).Where(x => !x.IsExplored);
			}

			private static int GetInitialTraversalOrder(TId id)
			{
				return (typeof(TId) == typeof(int)) ? Convert.ToInt32(id) : id.GetHashCode();
			}
		}
	}
}
