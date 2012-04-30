using System;
using System.Collections.Generic;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Implements a queue based BFS algorithm.
	/// </summary>
	public class BreadthFirstSearch<T>
	{
		private readonly Dictionary<T, GraphNode<T>> _graph;

		public BreadthFirstSearch(Dictionary<T, GraphNode<T>> graph)
		{
			_graph = graph;
		}

		public void Run(GraphNode<T> root, Action<Node<T>> process)
		{
			var queue = new Queue<GraphNode<T>>();
			queue.Enqueue(root);

			while (queue.Count != 0) {
				var node = queue.Dequeue();
				if (node.IsExplored) {
					continue;
				}

				process(node);
				node.IsExplored = true;
				foreach (var nodeId in node.Edges) {
					var adjacentNode = _graph[nodeId];
					if (!adjacentNode.IsExplored) {
						queue.Enqueue(adjacentNode);
					}
				}
			}
		}
	}
}
