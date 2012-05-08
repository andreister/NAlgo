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
	/// <typeparam name="TId">Type of node identifier.</typeparam>
	internal class ShortestPath<TId>
	{
		private readonly Dictionary<TId, GraphNode<TId, int>> _graph;

		/// <summary>
		/// Creates a new algorithm instance.
		/// </summary>
		internal ShortestPath(Dictionary<TId, GraphNode<TId, int>> graph)
		{
			_graph = graph.ToDictionary(x => x.Key, x => x.Value);
		}

		internal int ComputeDistance(GraphNode<TId, int> from, GraphNode<TId, int> to)
		{
			var algo = new BreadthFirstSearch<TId, int>();

			var startNode = _graph[from.Id];
			startNode.Value = 0;

			algo.Run(startNode, x => {
				foreach (GraphNode<TId, int> child in x.GetUnexploredChildren()) {
					child.Value = x.Value + 1;
					if (child.Id.Equals(to.Id)) {
						algo.Stop = true;
					}
				}
			});

			return _graph[to.Id].Value;
		}
	}
}
