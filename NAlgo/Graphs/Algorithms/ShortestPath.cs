using System.Collections.Generic;
using System.Linq;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Simple shortest path algorithm based on BFS.
	/// 
	/// Breaks the graph into layers and calculate distance as the
	/// number of layers from the [Start] to the [End] node.
	/// </summary>
	internal class ShortestPath<T>
	{
		private readonly Dictionary<T, LayeredNode> _graph;

		/// <summary>
		/// Creates a new algorithm instance.
		/// </summary>
		internal ShortestPath(Dictionary<T, GraphNode<T>> graph)
		{
			_graph = graph.ToDictionary(x => x.Key, x => new LayeredNode(x.Value));
		}

		internal int ComputeDistance(GraphNode<T> from, GraphNode<T> to)
		{
			var algo = new BreadthFirstSearch<T>();

			_graph[from.Id].Layer = 0;
			algo.Run(_graph[from.Id], GetUnexploredChildren, (node) => {
				foreach (LayeredNode child in GetUnexploredChildren(node)) {
					child.Layer = ((LayeredNode)node).Layer + 1;
					if (child.Id.Equals(to.Id)) {
						algo.Stop = true;
					}
				}
			});

			return _graph[to.Id].Layer;
		}

		private IEnumerable<Node<T>> GetUnexploredChildren(Node<T> node)
		{
			var nodes = ((GraphNode<T>)node).Adjacent;
			return nodes.Select(x => _graph[x]).Where(xx => !xx.IsExplored);
		}

		private class LayeredNode : GraphNode<T>
		{
			internal int Layer { get; set; }

			internal LayeredNode(GraphNode<T> node, int layer = int.MaxValue)
				: base(node.Id)
			{
				Layer = layer;
				Adjacent.AddRange(node.Adjacent);
			}
		}
	}
}
