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
	public class Kosaraju<T>
	{
		private readonly Dictionary<T, KosarajuNode> _graph;
		private int _currentTraversalOrder;
		private KosarajuNode _currentLeader;

		/// <summary>
		/// Creates a new algorithm instance.
		/// </summary>
		public Kosaraju(Dictionary<T, DigraphNode<T>> graph)
		{
			_graph = graph.ToDictionary(x => x.Key, x => new KosarajuNode(x.Value));
		}

		/// <summary>
		/// Computes the strongly connected components, and returns a map
		/// where the keys are the leader nodes, and the values are the nodes
		/// in the corresponding connected component.
		/// </summary>
		public Dictionary<DigraphNode<T>, List<DigraphNode<T>>> GetConnectedComponents()
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
			}

			var nodes = _graph.Values.OrderBy(x => x.TraversalOrder).Where(x => !x.IsExplored);
			foreach (var node in nodes) {
				_currentLeader = node;
				DepthFirstSearch(node, direction);
			}
		}

		private void DepthFirstSearch(KosarajuNode root, Direction direction)
		{
			var search = new DepthFirstSearch<T>();
			search.Run(root, x => GetUnexploredChildren(x, direction), (x) => {
				var node = (KosarajuNode)x; 
				node.Leader = _currentLeader;
				node.TraversalOrder = _currentTraversalOrder--;
			});
		}

		private IEnumerable<Node<T>> GetUnexploredChildren(Node<T> node, Direction direction)
		{
			var nodes = (direction == Direction.Reverse) ? ((KosarajuNode)node).Incoming : ((KosarajuNode)node).Outgoing;
			return nodes.Select(x => _graph[x]).Where(x => !x.IsExplored);
		}

		private Dictionary<DigraphNode<T>, List<DigraphNode<T>>> GroupNodesByLeader()
		{
			var result = new Dictionary<DigraphNode<T>, List<DigraphNode<T>>>();
			foreach (var node in _graph.Values) {
				var leader = node.Leader;
				if (!result.ContainsKey(leader)) {
					result.Add(leader, new List<DigraphNode<T>>());
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

		private class KosarajuNode : DigraphNode<T>
		{
			internal int TraversalOrder { get; set; }
			internal KosarajuNode Leader { get; set; }

			internal KosarajuNode(DigraphNode<T> node)
				: base(node.Id)
			{
				Leader = null;
				TraversalOrder = ConvertToInt(node.Id);
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
