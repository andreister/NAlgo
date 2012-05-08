using System.Collections.Generic;
using System.Linq;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Simple topological sort algorithm based on DFS.
	/// 
	/// Gives the biggest number to the "lowest sink" node,
	/// marks it as "explored" and repeats until all the nodes
	/// are explored.
	/// </summary>
	/// <typeparam name="TId">Type of node identifier.</typeparam>
	public class TopologicalSort<TId>
	{
		private readonly Dictionary<TId, DigraphNode<TId, int>> _graph;

		/// <summary>
		/// Creates a new algorithm instance.
		/// </summary>
		internal TopologicalSort(Dictionary<TId, DigraphNode<TId, int>> graph)
		{
			_graph = graph.ToDictionary(x => x.Key, x => x.Value);
			foreach (var node in graph.Values) {
				node.Value = int.MaxValue;
			}
		}

		/// <summary>
		/// Traverses the graph and sets correct topological order
		/// into each node's "Value" property.
		/// </summary>
		public void Run()
		{
			int order = _graph.Count;

			var algo = new DepthFirstSearch<TId, int>();
			foreach (var node in _graph.Values.Where(x => !x.IsExplored)) {
				algo.Run(node, x => {
					if (!x.GetUnexploredChildren().Any()) {
						x.Value = order--;
					}
				});
			}
		}
	}
}
